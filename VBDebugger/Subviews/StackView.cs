using NetModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VBDebugger.Subviews
{
    public class StackView
    {
        class FrameModel
        {
            public string Frame { get; set; }
        }


        private readonly DataGridView _localsView;
        private readonly TextBox _currentInstructionView;
        private TextBox _stackMessagesView;
        private readonly BindingList<FrameModel> _localViewSource = new BindingList<FrameModel>();

        public StackView(DataGridView localsView, TextBox currentInstructionView, TextBox stackMessagesView)
        {
            _localsView = localsView;
            _currentInstructionView = currentInstructionView;
            _stackMessagesView = stackMessagesView;

            _localsView.DataSource = _localViewSource;
        }

        public void Clear()
        {
            _currentInstructionView.Text = "";
            _stackMessagesView.Text = "";
            _localViewSource.Clear();
        }

        public void LoadStackFrames(StackDumpT stackDump)
        {
            _currentInstructionView.Text = ToString(stackDump.CurentInstruction);
            _stackMessagesView.Text = string.Join("\r\n", stackDump.Messages);

            _localViewSource.Clear();
            foreach (var frame in stackDump.Frames)
            {
                _localViewSource.Add(new FrameModel()
                {
                    Frame = ToString(frame.Reference)
                });
            }
        }


        private string ToString(SourceCodeReferenceT reference)
        {
            return $"{reference.Filename} - {reference.ScopeName} on line {reference.LineNumber}";
        }
    }
}
