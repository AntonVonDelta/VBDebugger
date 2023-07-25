using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using VBDebugger.Debugger;

namespace VBDebugger {
    public partial class Form1 : Form {
        private void UpdateState(State newState) {
            switch (_state) {
                case State.None:
                    // Initial view setup
                    btnBreak.Enabled = false;
                    btnStepOver.Enabled = false;
                    btnContinue.Enabled = false;
                    chkBreakOnException.Enabled = false;
                    break;

                case State.RunningWithCondition:
                    btnBreak.Enabled = false;
                    break;
            }

            _state = newState;

            switch (_state) {
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


                case State.PausingExecution:
                    btnBreak.Enabled = false;
                    break;
                case State.RedirectPausedExecution:
                    break;
                case State.RedirectPauseExecutionFailed:
                    break;

                case State.PausedExecution:
                    btnStepOver.Enabled = true;
                    btnContinue.Enabled = true;
                    chkBreakOnException.Enabled = true;
                    break;

                case State.ResumingExecution:
                    btnBreak.Enabled = false;
                    btnStepOver.Enabled = false;
                    btnContinue.Enabled = false;
                    chkBreakOnException.Enabled = false;
                    break;
                case State.RedirectResumedExecution:
                    btnBreak.Enabled = true;
                    break;
                case State.RedirectRunningWithCondition:
                    break;


                case State.RunningWithCondition:
                    btnBreak.Enabled = true;
                    break;
                case State.RedirectRunningWithConditionTriggered:
                    break;


                case State.SteppingOver:
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
                case State.RedirectStepOverFailed:
                    break;

                case State.DebuggingFailed:
                    btnBreak.Enabled = false;
                    btnStepOver.Enabled = false;
                    btnContinue.Enabled = false;
                    chkBreakOnException.Enabled = false;
                    break;
            }
        }

        private async Task NextFlow(CancellationToken token = default) {
            switch (_state) {
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

                case State.PausingExecution:
                    break;
                case State.RedirectPausedExecution:
                    await FlowPausedExecution();
                    break;
                case State.RedirectPauseExecutionFailed:
                    await FlowDebuggingFailed();
                    break;

                case State.PausedExecution:
                    break;

                case State.ResumingExecution:
                    break;
                case State.RedirectResumedExecution:
                    break;
                case State.RedirectRunningWithCondition:
                    await FlowRunningWithCondition(token);
                    break;
                case State.RedirectResumingExecutionFailed:
                    await FlowDebuggingFailed();
                    break;

                case State.RunningWithCondition:
                    break;
                case State.RedirectRunningWithConditionTriggered:
                    await FlowPausedExecution();
                    break;
                case State.RedirectRunningWithConditionFailed:
                    await FlowDebuggingFailed();
                    break;

                case State.RedirectStepOverFailed:
                    await FlowDebuggingFailed();
                    break;

                case State.DebuggingFailed:
                    await FlowChangingSettings();
                    break;
            }
        }

        private async Task FlowChangingSettings() {
            UpdateState(State.ChangingSettings);

            txtRemote.Text = Properties.Settings.Default.RemoteAddress;

            return;
        }

        private async Task FlowSavingSettings() {
            UpdateState(State.SavingSettings);

            Properties.Settings.Default.RemoteAddress = txtRemote.Text;

            await NextFlow();
        }

        private async Task FlowAttachingDebugger() {
            UpdateState(State.AttachingDebugger);

            try {
                var completeAddress = txtRemote.Text;
                var addressParts = completeAddress.Split(':');
                var endpoint = new IPEndPoint(IPAddress.Parse(addressParts[0]), int.Parse(addressParts[1]));

                _debugger = new DebuggerClient(endpoint, (string message) => AddLog(message));

                if (await _debugger.Attach()) {
                    AddLog($"Attached to {endpoint}");
                    UpdateState(State.RedirectAttachSuccess);
                } else {
                    AddLog($"Failed to attach {endpoint}");
                    UpdateState(State.RedirectAttachFailed);
                }
            } catch (Exception ex) {
                AddLog(ex.Message);
                UpdateState(State.RedirectAttachFailed);
            }

            await NextFlow();
        }

        private async Task FlowPausedExecution() {
            UpdateState(State.PausedExecution);
            await NextFlow();
        }

        private async Task FlowRunningWithCondition(CancellationToken token) {
            UpdateState(State.RunningWithCondition);

            try {
                var initialException = _debugger.CurrentException;

                while (!token.IsCancellationRequested) {
                    if (!await _debugger.StepOver()) {
                        UpdateState(State.RedirectRunningWithConditionFailed);
                        await NextFlow();
                        return;
                    }

                    LoadCurrentStackDump();


                    // Blindspot here: if a new exception is raised with same number and description as last one
                    // (and without a reset in between)
                    // then the exception will not be seen as a being a new one but ignored
                    if (_debugger.CurrentException == null ||
                        _debugger.CurrentException.Number == 0 ||
                        InstructionException.Equals(initialException, _debugger.CurrentException)) {

                        // No new exception found

                        initialException = _debugger.CurrentException;
                        continue;
                    }

                    // Process new exception
                    break;
                }

                UpdateState(State.RedirectRunningWithConditionTriggered);
            } catch (Exception ex) {
                AddLog(ex.Message);
                UpdateState(State.RedirectRunningWithConditionFailed);
            }

            await NextFlow();
        }



        private async Task FlowDebuggingFailed() {
            UpdateState(State.DebuggingFailed);
            await NextFlow();
        }
    }
}
