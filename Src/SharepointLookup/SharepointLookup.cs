using System;
using System.ComponentModel;
using BizTalkComponents.Utils;
using Microsoft.BizTalk.Component.Interop;
using Microsoft.BizTalk.Message.Interop;
using IComponent = Microsoft.BizTalk.Component.Interop.IComponent;
using BizTalkComponents.Utilities.LookupUtility;
using BizTalkComponents.Utilities.LookupUtility.Repository;
using System.ComponentModel.DataAnnotations;

namespace BizTalkComponents.PipelineComponents.SharepointLookup
{
    [ComponentCategory(CategoryTypes.CATID_PipelineComponent)]
    [System.Runtime.InteropServices.Guid("F0D6E4C0-9449-11EA-9A14-51F167B95080")]
    [ComponentCategory(CategoryTypes.CATID_Any)]
    public partial class SharepointLookup : IComponent, IBaseComponent,
                                        IPersistPropertyBag, IComponentUI
    {
        [DisplayName("Disabled")]
        [Description("Disables the component")]
        [RequiredRuntime]
        public bool Disabled { get; set; }

        [DisplayName("Property Path")]
        [Description("The property Path to use as key to query the sharepoint list, i.e. http//:examplenamespace#Myproperty")]
        [RegularExpression(@"^.*#.*$",
         ErrorMessage = "A property path should be formatted as namespace#property.")]
        [RequiredRuntime]
        public string PropertyPath { get; set; }

        [DisplayName("Destination Path")]
        [Description("The property Path to where the returned value will be promoted to, i.e. http//:examplenamespace#Myproperty")]
        [RegularExpression(@"^.*#.*$",
         ErrorMessage = "A property path should be formatted as namespace#property.")]
        [RequiredRuntime]
        public string DestinationPath { get; set; }

        [DisplayName("List Name")]
        [Description("The name of the Sharepoint list to query")]
        [RequiredRuntime]
        public string ListName { get; set; }

        [DisplayName("Throw Exception")]
        [Description("Throw exception if key or sharepoint list were not found")]
        [RequiredRuntime]
        public bool ThrowException { get; set; }

        [DisplayName("Promote Property")]
        [Description("Specifies whether the property should be promoted or just written to the context.")]
        [RequiredRuntime]
        public bool PromoteProperty { get; set; }

        public IBaseMessage Execute(IPipelineContext pContext, IBaseMessage pInMsg)
        {
            string errorMessage;

            if (!Validate(out errorMessage))
            {
                throw new ArgumentException(errorMessage);
            }
            
            //component will not be executed if the property is set to true
            if(Disabled)
            {
                return pInMsg;
            }

            //get the value from the provided property
            IBaseMessageContext context = pInMsg.Context;
            string key = (string)context.Read(new ContextProperty(PropertyPath)); // PropertyPath.Split('#')[1], PropertyPath.Split('#')[0]).ToString();

            //query the sharepoint table
            var lookupService = new LookupUtilityService(new SharepointLookupRepository());
            string result = "";
            try
            {
                result = lookupService.GetValue(ListName, key, true);
            }catch(ArgumentException ex)
            {
                if (ThrowException)
                {
                    throw ex;
                }
                else
                {
                    System.Diagnostics.EventLog.WriteEntry(pContext.PipelineName, ex.Message, System.Diagnostics.EventLogEntryType.Warning);
                }
            }

            //promote the result to the provided destination property
            if (PromoteProperty)
            {
                pInMsg.Context.Promote(new ContextProperty(DestinationPath), result);
            }else
            {
                pInMsg.Context.Write(new ContextProperty(DestinationPath), result);
            }

            return pInMsg;
        }
    }
}
