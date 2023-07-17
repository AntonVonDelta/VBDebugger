﻿using NetModels;
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

            [Browsable(false)]
            public List<VariableT> Locals { get; set; }
        }
        class LocalModel
        {
            public string Name { get; set; }
            public string Value { get; set; }
        }

        private readonly DataGridView _stackFramesView;
        private readonly DataGridView _localsView;
        private readonly TextBox _currentInstructionView;
        private TextBox _stackMessagesView;
        private readonly BindingList<FrameModel> _stackFramesViewSource = new BindingList<FrameModel>();
        private readonly BindingList<LocalModel> _localsViewSource = new BindingList<LocalModel>();

        public StackView(DataGridView stackFramesView, DataGridView localsView, TextBox currentInstructionView, TextBox stackMessagesView)
        {
            _stackFramesView = stackFramesView;
            _localsView = localsView;
            _currentInstructionView = currentInstructionView;
            _stackMessagesView = stackMessagesView;

            _stackFramesView.DataSource = _stackFramesViewSource;
            _localsView.DataSource = _localsViewSource;

            _stackFramesView.SelectionChanged += stackFramesView_SelectionChanged;
        }


        public void Clear()
        {
            _currentInstructionView.Text = "";
            _stackMessagesView.Text = "";
            _stackFramesViewSource.Clear();
            _localsViewSource.Clear();
        }

        public void LoadStackFrames(StackDumpT stackDump)
        {
            var selectedFrames = _stackFramesView.SelectedRows;

            _currentInstructionView.Text = ToString(stackDump.CurentInstruction);
            _stackMessagesView.Text = string.Join("\r\n", stackDump.Messages);

            _stackFramesViewSource.Clear();
            foreach (var frame in stackDump.Frames)
            {
                var frameName = $"{frame.Reference.Filename} - {frame.Reference.ScopeName}";
                var newFrame = new FrameModel()
                {
                    Frame = frameName,
                    Locals = frame.Locals
                };

                _stackFramesViewSource.Add(newFrame);
            }

            // Select most recent stack
            if (_stackFramesView.Rows.Count != 0)
            {
                _stackFramesView.ClearSelection();
                _stackFramesView.Rows[_stackFramesView.Rows.Count - 1].Selected = true;
            }
        }


        private string ToString(SourceCodeReferenceT reference)
        {
            return $"{reference.Filename} - {reference.ScopeName} on line {reference.LineNumber}";
        }


        private void stackFramesView_SelectionChanged(object sender, EventArgs e)
        {
            var selectedRows = _stackFramesView.SelectedRows;
            FrameModel selectedFrame;

            _localsViewSource.Clear();

            if (selectedRows.Count == 0) return;
            selectedFrame = (FrameModel)selectedRows[0].DataBoundItem;

            foreach (var local in selectedFrame.Locals)
            {
                _localsViewSource.Add(new LocalModel()
                {
                    Name = local.Name,
                    Value = local.Value
                });
            }
        }
    }
}
