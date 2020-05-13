using BizTalkComponents.Utils;
using Microsoft.BizTalk.Message.Interop;
using BizTalkComponents.PipelineComponents.SharepointLookup;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Winterdom.BizTalk.PipelineTesting;

namespace TestPipelineComponent
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            string Key = txtKey.Text;
            string PropertyPath = txtPropertyPath.Text;
            string DestinationPath = txtDestinationPath.Text;
            string ListName = txtListName.Text;

            IBaseMessage message = MessageHelper.CreateFromString("<testfile/>");
            message.Context.Promote(new ContextProperty(PropertyPath), Key);

            var component = new SharepointLookup()
            {
                Disabled = false,
                PropertyPath = PropertyPath,
                DestinationPath = DestinationPath,
                ListName = ListName,
                ThrowException = true,
                PromoteProperty = true
            };

            SendPipelineWrapper sendPipeline = PipelineFactory.CreateEmptySendPipeline();
            sendPipeline.AddComponent(component, PipelineStage.PreAssemble);

            IBaseMessage results = sendPipeline.Execute(message);

            string result = (string)results.Context.Read(new ContextProperty(DestinationPath));
            txtResult.Text = result;
        }
    }
}
