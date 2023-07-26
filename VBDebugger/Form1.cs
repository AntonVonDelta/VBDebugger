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
using System.IO;
using NetModels;

namespace VBDebugger {
    public partial class Form1 : Form {
        enum State {
            None,

            ChangingSettings,
            IntroAttachToRemote,
            SavingSettings,

            AttachingDebugger,
            RedirectAttachSuccess,  // Non-flow states (used only for bridging over the next flow) 
            RedirectAttachFailed,   // Non-flow states (used only for bridging over the next flow)

            PausingExecution,
            RedirectPausedExecution,
            RedirectPauseExecutionFailed,

            PausedExecution,

            ResumingExecution,
            RedirectResumedExecution,
            RedirectRunningWithCondition,
            RedirectResumingExecutionFailed,

            RunningWithCondition,
            RedirectRunningWithConditionTriggered,
            RedirectRunningWithConditionFailed,

            SteppingOver,
            RedirectSteppedOver,
            RedirectStepOverFailed,

            DebuggingFailed,
        }

        private State _state = State.None;
        private DebuggerClient _debugger;
        private readonly StackView _stackView;
        private CancellationTokenSource _runningCts;
        private Task _runningWithCondition;
        private string _solutionFolderPath;
        private List<string> _solutionFilesFilePaths;
        private string _loadedSourceCodeFilepath;

        public Form1() {
            InitializeComponent();

            _stackView = new StackView(dgvStackFrames, dgvLocals, txtCurrentInstruction, txtStackMessages);
            _stackView.StackFrameSelected += stackView_StackFrameSelected;
        }

        private async void Form1_Load(object sender, EventArgs e) {
            await NextFlow();
        }


        private void AddLog(string message) {
            txtOuput.Text = $"{txtOuput.Text}{message}\r\n";
        }

        private void LoadCurrentStackDump() {
            var stackDump = _debugger.CurrentStackDump;

            _stackView.LoadStackFrames(stackDump);
        }
        private void UnloadCurrentStackDump() {
            _stackView.Clear();
        }
        private void LoadSourceCodeFromReference(SourceCodeReferenceT reference) {
            foreach (TreeNode node in treeViewFiles.Nodes) {
                var sourceFilepath = (string)node.Tag;
                var filename = Path.GetFileName(sourceFilepath);

                if (filename == reference.Filename) {
                    treeViewFiles.SelectedNode = node;

                    LoadSourceCodeFromNode(node);
                    SelectLineInCodeFromReference(reference);

                    break;
                }
            }
        }

        private void LoadSourceCodeFromNode(TreeNode node) {
            var nodeFilepath = (string)node.Tag;

            if (_loadedSourceCodeFilepath == nodeFilepath) return;

            _loadedSourceCodeFilepath = nodeFilepath;

            using (var stream = new StreamReader(nodeFilepath)) {
                rtbSourceCode.Text = stream.ReadToEnd();
            }
        }

        private void SelectLineInCodeFromReference(SourceCodeReferenceT reference) {
            for (int i = 0; i < rtbSourceCode.Lines.Length; i++) {
                var line = rtbSourceCode.Lines[i];

                if (Regex.IsMatch(line, $@"^\s*(?:DebugEnterProcedure|DebugLog|DebugLeaveProcedure).+{reference.LineNumber}")) {
                    rtbSourceCode.HighlightLine(i, Color.LightBlue);
                    break;
                }
            }
        }

        private void stackView_StackFrameSelected(NetModels.StackFrameT stackFrame) {
            LoadSourceCodeFromReference(stackFrame.CurrentInstruction);
        }

        private async void btnAttachDebugger_Click(object sender, EventArgs e) {
            if (string.IsNullOrEmpty(_solutionFolderPath)) {
                MessageBox.Show("You must select the solution path where source code resides");
                return;
            }

            UpdateState(State.IntroAttachToRemote);
            await NextFlow();
        }

        private async void btnBreak_Click(object sender, EventArgs e) {
            var previousState = _state;

            UpdateState(State.PausingExecution);

            try {
                if (previousState == State.RunningWithCondition) {
                    // This is actually a simulated "running" state
                    _runningCts.Cancel();

                    // The running flow will be signaled to close
                    // and will complete the transition to whichever state it wants
                    return;
                } else {
                    if (await _debugger.Pause()) {
                        LoadCurrentStackDump();
                        UpdateState(State.RedirectPausedExecution);
                    } else {
                        UpdateState(State.RedirectPauseExecutionFailed);
                    }
                }
            } catch (Exception ex) {
                AddLog(ex.Message);
                UpdateState(State.RedirectPauseExecutionFailed);
            }

            await NextFlow();
        }

        private async void btnContinue_Click(object sender, EventArgs e) {
            UpdateState(State.ResumingExecution);

            UnloadCurrentStackDump();

            try {
                if (chkBreakOnException.Checked) {
                    UpdateState(State.RedirectRunningWithCondition);

                    _runningCts = new CancellationTokenSource();

                    _runningWithCondition = NextFlow(_runningCts.Token);
                    await _runningWithCondition;
                    return;
                } else {
                    if (await _debugger.Resume())
                        UpdateState(State.RedirectResumedExecution);
                    else
                        UpdateState(State.RedirectResumingExecutionFailed);
                }
            } catch (Exception ex) {
                AddLog(ex.Message);

                UpdateState(State.RedirectResumingExecutionFailed);
            }

            await NextFlow();
        }

        private async void btnStepOver_Click(object sender, EventArgs e) {
            UpdateState(State.SteppingOver);

            try {
                if (await _debugger.StepOver()) {
                    LoadCurrentStackDump();
                    UpdateState(State.RedirectSteppedOver);
                } else {
                    UpdateState(State.RedirectStepOverFailed);
                }
            } catch (Exception ex) {
                AddLog(ex.Message);

                UpdateState(State.RedirectStepOverFailed);
            }

            await NextFlow();
        }

        private void btnSolutionPath_Click(object sender, EventArgs e) {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK) {
                _solutionFolderPath = folderBrowserDialog1.SelectedPath;
            }
        }

        private void treeViewFiles_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e) {
            LoadSourceCodeFromNode(e.Node);
        }
    }
}
