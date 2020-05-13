using Microsoft.BizTalk.Message.Interop;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Configuration;
using System.IO;
using Winterdom.BizTalk.PipelineTesting;

namespace BizTalkComponents.PipelineComponents.SharepointLookup.Tests.UnitTests
{
    [TestClass]
    public class SharepointLookupTests
    {
        [TestMethod]
        public void Test()
        {
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            if (config.AppSettings.Settings["SharePointSite"] == null)
            {
                config.AppSettings.Settings.Add("SharePointSite", "https://TestUrl.org");
            }
            else
            {
                config.AppSettings.Settings["SharePointSite"].Value = "https://TestUrl.org";
            }
            
            string Key = "274";
            string DestinationPath = "https://Test.Schemas.PropertySchema#Destination";
            
            IBaseMessage message = MessageHelper.CreateFromString("<Test/>");
            message.Context.Write("Key", "https://Test.Schemas.PropertySchema", Key);
            message.Context.Write("DestinationPath", "https://Test.Schemas.PropertySchema", DestinationPath);

            var component = new SharepointLookup()
            {
                Disabled = false,
                PropertyPath = "https://Test.Schemas.PropertySchema#Key",
                DestinationPath = "https://Test.Schemas.PropertySchema#DestinationPath",
                ListName = "TestList",
                ThrowException = true,
                PromoteProperty = true
            };

            SendPipelineWrapper sendPipeline = PipelineFactory.CreateEmptySendPipeline();
            sendPipeline.AddComponent(component, PipelineStage.PreAssemble);

            IBaseMessage results = sendPipeline.Execute(message);

            Assert.AreEqual(Key, results.Context.Read("StoreId", "https://Test.Schemas.PropertySchema"));

            Assert.IsTrue(results.Context.IsPromoted("DestinationPath", "https://Test.Schemas.PropertySchema"));

        }

    }
}
