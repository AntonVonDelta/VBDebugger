﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VBDebugger.Debugger;

namespace VBDebugger
{
    public partial class Form1 : Form
    {
        private void UpdateState(State newState)
        {
            switch (_state)
            {
                case State.None:
                    // Initial view setup
                    btnBreak.Enabled = false;
                    btnStepOver.Enabled = false;
                    btnContinue.Enabled = false;
                    chkBreakOnException.Enabled = false;
                    break;
            }

            _state = newState;

            switch (_state)
            {
                case State.ChangingSettings:
                    txtRemote.Enabled = true;
                    btnAttachDebugger.Enabled = true;
                    break;

                case State.SavingSettings:
                    txtRemote.Enabled = false;
                    btnAttachDebugger.Enabled = false;
                    break;

                case State.RedirectAttachSuccess:
                    btnBreak.Enabled = true;
                    break;

                case State.RedirectAttachFailed:
                    break;



                case State.IntroPausingExecution:
                    btnBreak.Enabled = false;
                    break;
                case State.RedirectPausedExecution:
                    btnStepOver.Enabled = true;
                    btnContinue.Enabled = true;
                    chkBreakOnException.Enabled = true;
                    break;


                case State.IntroResumingExecution:
                    btnBreak.Enabled = false;
                    btnStepOver.Enabled = false;
                    btnContinue.Enabled = false;
                    chkBreakOnException.Enabled = false;
                    break;
                case State.RedirectResumedExecution:
                    btnBreak.Enabled = true;
                    break;


                case State.IntroSteppingOver:
                    btnBreak.Enabled = false;
                    btnStepOver.Enabled = false;
                    btnContinue.Enabled = false;
                    chkBreakOnException.Enabled = false;
                    break;

                case State.RedirectSteppedOver:
                    btnStepOver.Enabled = true;
                    btnContinue.Enabled = true;
                    chkBreakOnException.Enabled = true;
                    break;


                case State.RedirectDebuggingFailed:
                    btnBreak.Enabled = false;
                    btnStepOver.Enabled = false;
                    btnContinue.Enabled = false;
                    chkBreakOnException.Enabled = false;
                    break;
            }
        }

        private async Task NextFlow()
        {
            switch (_state)
            {
                case State.None:
                    await FlowChangingSettings();
                    break;

                case State.ChangingSettings:
                    break;
                case State.IntroAttachToRemote:
                    await FlowSavingSettings();
                    await FlowAttachingDebugger();
                    break;
                case State.SavingSettings:
                    break;

                case State.RedirectAttachSuccess:
                    break;
                case State.RedirectAttachFailed:
                    await FlowChangingSettings();
                    break;

                case State.IntroPausingExecution:
                    break;
                case State.RedirectPausedExecution:
                    break;

                case State.IntroResumingExecution:
                    break;
                case State.RedirectResumedExecution:
                    break;


                case State.RedirectDebuggingFailed:
                    await FlowChangingSettings();
                    break;
            }
        }

        private async Task FlowChangingSettings()
        {
            UpdateState(State.ChangingSettings);

            txtRemote.Text = Properties.Settings.Default.RemoteAddress;

            return;
        }

        private async Task FlowSavingSettings()
        {
            UpdateState(State.SavingSettings);

            Properties.Settings.Default.RemoteAddress = txtRemote.Text;

            await NextFlow();
        }

        private async Task FlowAttachingDebugger()
        {
            UpdateState(State.AttachingDebugger);

            try
            {
                var completeAddress = txtRemote.Text;
                var addressParts = completeAddress.Split(':');
                var endpoint = new IPEndPoint(IPAddress.Parse(addressParts[0]), int.Parse(addressParts[1]));

                _debugger = new DebuggerClient(endpoint, (string message) => AddLog(message));

                if(await _debugger.Attach())
                {
                    AddLog($"Attached to {endpoint}");
                    UpdateState(State.RedirectAttachSuccess);
                }
                else
                {
                    AddLog($"Failed to attach {endpoint}");
                    UpdateState(State.RedirectAttachFailed);
                }
            }
            catch (Exception ex)
            {
                AddLog(ex.Message);
                UpdateState(State.RedirectAttachFailed);
            }

            await NextFlow();
        }
    }
}