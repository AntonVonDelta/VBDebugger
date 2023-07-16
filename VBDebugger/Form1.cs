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
using VBDebugger.Subviews;

namespace VBDebugger
{
    public partial class Form1 : Form
    {
        private DebuggerClient _debugger;
        private readonly StackView _stackView;

        public Form1()
        {
            InitializeComponent();

            _stackView = new StackView(dgvStackFrames, txtCurrentInstruction, txtStackMessages);
        }

        private void AddLog(string message)
        {
            txtOuput.Text = $"{txtOuput.Text}{message}\r\n";
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            txtRemote.Text = Properties.Settings.Default.RemoteAddress;
        }

        private void LoadCurrentStackDump()
        {
            var stackDump = _debugger.CurrentStackDump;

            _stackView.LoadStackFrames(stackDump);
        }
        private void UnloadCurrentStackDump()
        {
            _stackView.Clear();
        }


        private async void btnAttachDebugger_Click(object sender, EventArgs e)
        {
            if (_debugger != null && _debugger.Attached) _debugger.Dispose();

            btnAttachDebugger.Enabled = false;
            Properties.Settings.Default.RemoteAddress = txtRemote.Text;

            try
            {
                var completeAddress = txtRemote.Text;
                var addressParts = completeAddress.Split(':');
                var endpoint = new IPEndPoint(IPAddress.Parse(addressParts[0]), int.Parse(addressParts[1]));

                _debugger = new DebuggerClient(endpoint, (string message) => AddLog(message));

                await _debugger.Attach();

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

        private async void btnBreak_Click(object sender, EventArgs e)
        {
            btnBreak.Enabled = false;

            try
            {
                if (_debugger == null || !_debugger.Attached)
                {
                    AddLog("No debugger attached");
                    return;
                }

                if (await _debugger.Pause())
                    LoadCurrentStackDump();
            }
            finally
            {
                btnBreak.Enabled = true;
            }
        }

        private async void btnContinue_Click(object sender, EventArgs e)
        {
            btnContinue.Enabled = false;

            UnloadCurrentStackDump();

            try
            {
                if (_debugger == null || !_debugger.Attached)
                {
                    AddLog("No debugger attached");
                    return;
                }

                await _debugger.Resume();
            }
            finally
            {
                btnContinue.Enabled = true;
            }
        }

        private async void btnStepOver_Click(object sender, EventArgs e)
        {
            btnStepOver.Enabled = false;

            try
            {
                if (_debugger == null || !_debugger.Attached)
                {
                    AddLog("No debugger attached");
                    return;
                }

                if (await _debugger.StepOver())
                    LoadCurrentStackDump();
            }
            finally
            {
                btnStepOver.Enabled = true;
            }
        }
    }
}
