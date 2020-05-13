using System;
using System.Collections;
using System.Linq;
using BizTalkComponents.Utils;
using Microsoft.BizTalk.Component.Interop;

namespace BizTalkComponents.PipelineComponents.SharepointLookup
{
    public partial class SharepointLookup
    {
        public string Name { get { return "SharepointLookup"; } }
        public string Version { get { return "1.0"; } }
        public string Description { get { return "Retrives a value from Sharepoint list using a key from promoted property"; } }

        public void GetClassID(out Guid classID)
        {
            classID = new Guid("F0D70BD2-9449-11EA-9A14-51F167B95080");
        }

        public void InitNew()
        {

        }

        public IEnumerator Validate(object projectSystem)
        {
            return ValidationHelper.Validate(this, false).ToArray().GetEnumerator();
        }

        public bool Validate(out string errorMessage)
        {
            var errors = ValidationHelper.Validate(this, true).ToArray();

            if (errors.Any())
            {
                errorMessage = string.Join(",", errors);

                return false;
            }

            errorMessage = string.Empty;

            return true;
        }

        public IntPtr Icon { get { return IntPtr.Zero; } }

        public void Load(IPropertyBag propertyBag, int errorLog)
        {
             var props = this.GetType().GetProperties(System.Reflection.BindingFlags.Public 
             | System.Reflection.BindingFlags.Instance);

             foreach (var prop in props)
             {
                 if (prop.CanRead & prop.CanWrite)
                 {
                     prop.SetValue(this, PropertyBagHelper.ReadPropertyBag(propertyBag, prop.Name, 
                        prop.GetValue(this)));
                 }
             }
        }

        public void Save(IPropertyBag propertyBag, bool clearDirty, bool saveAllProperties)
        {
            var props = this.GetType().GetProperties(System.Reflection.BindingFlags.Public 
                | System.Reflection.BindingFlags.Instance);

             foreach (var prop in props)
             {
                 if (prop.CanRead & prop.CanWrite)
                 {
                     PropertyBagHelper.WritePropertyBag(propertyBag, prop.Name, prop.GetValue(this));
                 }
             }
        }
    }
}
