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
        enum State
        {
            None,
            ChangingSettings,
            IntroAttachToRemote,
            SavingSettings,

            AttachingDebugger,
            RedirectAttachSuccess,  // Non-flow states (used only for bridging over the next flow) 
            RedirectAttachFailed,   // Non-flow states (used only for bridging over the next flow)

            IntroPausingExecution,
            RedirectPausedExecution,

            IntroResumingExecution,
            RedirectResumedExecution,

            IntroSteppingOver,
            RedirectSteppedOver,

            RedirectDebuggingFailed,
        }

        private State _state = State.None;
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
            UpdateState(State.IntroPausingExecution);

            try
            {
                if (_debugger == null || !_debugger.Attached)
                {
                    AddLog("No debugger attached");
                    return;
                }

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
            catch (Exception ex)
            {
                AddLog(ex.Message);

                UpdateState(State.RedirectDebuggingFailed);
            }

            await NextFlow();
        }

        private async void btnContinue_Click(object sender, EventArgs e)
        {
            UpdateState(State.IntroResumingExecution);

            UnloadCurrentStackDump();

            try
            {
                if (_debugger == null || !_debugger.Attached)
                {
                    AddLog("No debugger attached");
                    return;
                }

                if (await _debugger.Resume())
                    UpdateState(State.RedirectResumedExecution);
                else UpdateState(State.RedirectDebuggingFailed);
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
            UpdateState(State.IntroSteppingOver);

            try
            {
                if (_debugger == null || !_debugger.Attached)
                {
                    AddLog("No debugger attached");
                    return;
                }

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
