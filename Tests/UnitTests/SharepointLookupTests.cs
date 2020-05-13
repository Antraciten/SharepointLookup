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
            config.AppSettings.Settings.Add("SharePointSite", "https://ibizcloud.sharepoint.com/sites/Lvenskiold2");
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");

            string file = "../../../TestData/ShipmentBooking_v1.0.xml";
            
            string StoreId = "274";
            string DestinationPath = "https://Test.Schemas.PropertySchema#Destination";
            
            IBaseMessage message = MessageHelper.CreateFromStream(File.OpenRead(file));
            message.Context.Write("StoreId", "https://Test.Schemas.PropertySchema", StoreId);
            message.Context.Write("DestinationPath", "https://Test.Schemas.PropertySchema", DestinationPath);

            var component = new SharepointLookup()
            {
                Disable = false,
                PropertyPath = "https://Test.Schemas.PropertySchema#StoreId",
                DestinationPath = "https://Test.Schemas.PropertySchema#DestinationPath",
                ListName = "BASELINE_ORDERNOTIFICATION_TO_ENDPOINT",
                ThrowException = true,
                PromoteProperty = true
            };

            SendPipelineWrapper sendPipeline = PipelineFactory.CreateEmptySendPipeline();
            sendPipeline.AddComponent(component, PipelineStage.PreAssemble);

            IBaseMessage results = sendPipeline.Execute(message);

            Assert.AreEqual(StoreId, results.Context.Read("StoreId", "https://Test.Schemas.PropertySchema"));

            Assert.IsTrue(results.Context.IsPromoted("DestinationPath", "https://Test.Schemas.PropertySchema"));

        }

    }
}
