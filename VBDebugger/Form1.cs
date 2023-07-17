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
using System.Threading;

namespace VBDebugger
{
    public partial class Form1 : Form
    {
        enum State
        {
            None,
            ChangingSettings,
            IntroAttachToRemote,
            SavingSettings,

            AttachingDebugger,
            RedirectAttachSuccess,  // Non-flow states (used only for bridging over the next flow) 
            RedirectAttachFailed,   // Non-flow states (used only for bridging over the next flow)

            PausingExecution,
            RedirectPausedExecution,

            ResumingExecution,
            RedirectResumedExecution,
            RedirectRunningWithCondition,

            SteppingOver,
            RedirectSteppedOver,

            RunningWithCondition,

            RedirectDebuggingFailed,
        }

        private State _state = State.None;
        private DebuggerClient _debugger;
        private readonly StackView _stackView;
        private CancellationTokenSource _runningCts;
        private Task _runningWithCondition;

        public Form1()
        {
            InitializeComponent();

            _stackView = new StackView(dgvStackFrames, dgvLocals, txtCurrentInstruction, txtStackMessages);
        }


        private void AddLog(string message)
        {
            txtOuput.Text = $"{txtOuput.Text}{message}\r\n";
        }


        private async void Form1_Load(object sender, EventArgs e)
        {
            await NextFlow();
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
            UpdateState(State.IntroAttachToRemote);
            await NextFlow();
        }

        private async void btnBreak_Click(object sender, EventArgs e)
        {
            UpdateState(State.PausingExecution);

            try
            {
                if (_state == State.RunningWithCondition)
                {
                    // This is actually a simulated "running" state
                    _runningCts.Cancel();

                    // The running flow will be signaled to close
                    // and will complete the transition to whichever state it wants
                    return;
                }
                else
                {
                    if (await _debugger.Pause())
                    {
                        LoadCurrentStackDump();
                        UpdateState(State.RedirectPausedExecution);
                    }
                    else
                    {
                        UpdateState(State.RedirectDebuggingFailed);
                    }
                }
            }
            catch (Exception ex)
            {
                AddLog(ex.Message);

                UpdateState(State.RedirectDebuggingFailed);
            }

            await NextFlow();
        }

        private async void btnContinue_Click(object sender, EventArgs e)
        {
            UpdateState(State.ResumingExecution);

            UnloadCurrentStackDump();

            try
            {
                if (chkBreakOnException.Checked)
                {
                    UpdateState(State.RedirectRunningWithCondition);

                    _runningCts = new CancellationTokenSource();

                    _runningWithCondition = NextFlow(_runningCts.Token);
                    await _runningWithCondition;
                    return;
                }
                else
                {
                    if (await _debugger.Resume())
                        UpdateState(State.RedirectResumedExecution);
                    else
                        UpdateState(State.RedirectDebuggingFailed);
                }
            }
            catch (Exception ex)
            {
                AddLog(ex.Message);

                UpdateState(State.RedirectDebuggingFailed);
            }

            await NextFlow();
        }

        private async void btnStepOver_Click(object sender, EventArgs e)
        {
            UpdateState(State.SteppingOver);

            try
            {
                if (await _debugger.StepOver())
                {
                    LoadCurrentStackDump();
                    UpdateState(State.RedirectSteppedOver);
                }
                else
                {
                    UpdateState(State.RedirectDebuggingFailed);
                }
            }
            catch (Exception ex)
            {
                AddLog(ex.Message);

                UpdateState(State.RedirectDebuggingFailed);
            }

            await NextFlow();
        }
    }
}
