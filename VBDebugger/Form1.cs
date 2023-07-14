using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VBDebugger.Debugger;
using System.Net;
using System.Text.RegularExpressions;

namespace VBDebugger
{
    public partial class Form1 : Form
    {
        DebuggerClient debugger;

        public Form1()
        {
            InitializeComponent();
        }

        private void AddLog(string message)
        {
            txtOuput.Text = $"{message}\n{txtOuput.Text}";
        }



        private async void btnAttachDebugger_Click(object sender, EventArgs e)
        {
            if (debugger != null && debugger.Attached) debugger.Dispose();

            btnAttachDebugger.Enabled = false;

            try
            {
                var completeAddress = txtRemote.Text;
                var addressParts = completeAddress.Split(':');
                var endpoint = new IPEndPoint(IPAddress.Parse(addressParts[0]), int.Parse(addressParts[1]));

                debugger = new DebuggerClient(endpoint, (string message) => AddLog(message));

                await debugger.Attach();

                AddLog($"Attached to {endpoint}");
            }
            catch (Exception ex)
            {
                AddLog(ex.Message);
            }
            finally
            {
                btnAttachDebugger.Enabled = true;
            }
        }

        private void btnBreakContinue_Click(object sender, EventArgs e)
        {
            if (debugger == null || debugger.Attached)
            {
                AddLog("No debugger attached");
                return;
            }

            //debugger
        }

        private void btnStepOver_Click(object sender, EventArgs e)
        {
            if (debugger == null || debugger.Attached)
            {
                AddLog("No debugger attached");
                return;
            }
        }
    }
}
