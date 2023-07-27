using NetModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Linq;

namespace VBDebugger.Subviews {
    public class StackView {
        class FrameModel {
            public string Frame { get; set; }

            [Browsable(false)]
            public StackFrameT StackFrame { get; set; }
        }

        class LocalModel {
            public string Name { get; set; }
            public string Value { get; set; }
        }

        private readonly DataGridView _stackFramesView;
        private readonly DataGridView _localsView;
        private readonly TextBox _currentInstructionView;
        private TextBox _stackMessagesView;
        private readonly BindingList<FrameModel> _stackFramesViewSource = new BindingList<FrameModel>();
        private readonly BindingList<LocalModel> _localsViewSource = new BindingList<LocalModel>();

        public event Action<StackFrameT> StackFrameSelected;

        public StackView(DataGridView stackFramesView, DataGridView localsView, TextBox currentInstructionView, TextBox stackMessagesView) {
            _stackFramesView = stackFramesView;
            _localsView = localsView;
            _currentInstructionView = currentInstructionView;
            _stackMessagesView = stackMessagesView;

            _stackFramesView.DataSource = _stackFramesViewSource;
            _localsView.DataSource = _localsViewSource;

            _stackFramesView.SelectionChanged += stackFramesView_SelectionChanged;
        }


        public void Clear() {
            _currentInstructionView.Text = "";
            _stackMessagesView.Text = "";
            _stackFramesViewSource.Clear();
            _localsViewSource.Clear();
        }

        public void LoadStackFrames(StackDumpT stackDump) {
            var selectedFrames = _stackFramesView.SelectedRows;
            IEnumerable<FrameModel> stackFramesModels;

            stackFramesModels = stackDump.Frames.Select(el => {
                var frameName = $"{el.Reference.Filename} - {el.Reference.ScopeName}";

                return new FrameModel() {
                    Frame = frameName,
                    StackFrame = el
                };
            });

            _stackMessagesView.Text = string.Join("\r\n", stackDump.Messages);

            _stackFramesViewSource.Clear();
            AtomicUpdateStackFramesView(stackFramesModels.ToList());

            // Select most recent stack
            if (_stackFramesView.Rows.Count != 0) {
                _stackFramesView.ClearSelection();
                _stackFramesView.Rows[_stackFramesView.Rows.Count - 1].Selected = true;
            }
        }

        private void AtomicUpdateStackFramesView(List<FrameModel> data) {
            _stackFramesView.SelectionChanged -= stackFramesView_SelectionChanged;
            foreach (var item in data)
                _stackFramesViewSource.Add(item);
            _stackFramesView.SelectionChanged += stackFramesView_SelectionChanged;
        }

        private string ToString(SourceCodeReferenceT reference) {
            return $"{reference.Filename} - {reference.ScopeName} on line {reference.LineNumber}";
        }


        private void OnStackFrameSelected(StackFrameT stackFrame) {
            if (StackFrameSelected != null) StackFrameSelected(stackFrame);
        }

        private void stackFramesView_SelectionChanged(object sender, EventArgs e) {
            var selectedRows = _stackFramesView.SelectedRows;
            FrameModel selectedFrame;

            _localsViewSource.Clear();

            if (selectedRows.Count == 0) return;
            selectedFrame = (FrameModel)selectedRows[0].DataBoundItem;

            // Raise event
            OnStackFrameSelected(selectedFrame.StackFrame);

            // Show current instruction in this stack frame
            _currentInstructionView.Text = ToString(selectedFrame.StackFrame.CurrentInstruction);

            foreach (var local in selectedFrame.StackFrame.Locals) {
                _localsViewSource.Add(new LocalModel() {
                    Name = local.Name,
                    Value = local.Value
                });
            }
        }
    }
}
