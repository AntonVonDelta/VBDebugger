
namespace VBDebugger
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.mainSplitContainer = new System.Windows.Forms.SplitContainer();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.chkBreakOnException = new System.Windows.Forms.CheckBox();
            this.btnContinue = new System.Windows.Forms.Button();
            this.btnStepOver = new System.Windows.Forms.Button();
            this.btnBreak = new System.Windows.Forms.Button();
            this.txtRemote = new System.Windows.Forms.TextBox();
            this.btnAttachDebugger = new System.Windows.Forms.Button();
            this.rtbSourceCode = new System.Windows.Forms.RichTextBox();
            this.tabViewBottom = new System.Windows.Forms.TabControl();
            this.tabOutput = new System.Windows.Forms.TabPage();
            this.txtOuput = new System.Windows.Forms.TextBox();
            this.tabLocals = new System.Windows.Forms.TabPage();
            this.tabStackFrames = new System.Windows.Forms.TabPage();
            this.tblLayoutStackFrames = new System.Windows.Forms.TableLayoutPanel();
            this.txtCurrentInstruction = new System.Windows.Forms.TextBox();
            this.dgvStackFrames = new System.Windows.Forms.DataGridView();
            this.txtStackMessages = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.mainSplitContainer)).BeginInit();
            this.mainSplitContainer.Panel1.SuspendLayout();
            this.mainSplitContainer.Panel2.SuspendLayout();
            this.mainSplitContainer.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tabViewBottom.SuspendLayout();
            this.tabOutput.SuspendLayout();
            this.tabStackFrames.SuspendLayout();
            this.tblLayoutStackFrames.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvStackFrames)).BeginInit();
            this.SuspendLayout();
            // 
            // mainSplitContainer
            // 
            this.mainSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainSplitContainer.Location = new System.Drawing.Point(0, 0);
            this.mainSplitContainer.Name = "mainSplitContainer";
            this.mainSplitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // mainSplitContainer.Panel1
            // 
            this.mainSplitContainer.Panel1.Controls.Add(this.tableLayoutPanel1);
            // 
            // mainSplitContainer.Panel2
            // 
            this.mainSplitContainer.Panel2.Controls.Add(this.tabViewBottom);
            this.mainSplitContainer.Size = new System.Drawing.Size(1103, 728);
            this.mainSplitContainer.SplitterDistance = 398;
            this.mainSplitContainer.TabIndex = 1;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.rtbSourceCode, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1103, 398);
            this.tableLayoutPanel1.TabIndex = 3;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.chkBreakOnException);
            this.panel1.Controls.Add(this.btnContinue);
            this.panel1.Controls.Add(this.btnStepOver);
            this.panel1.Controls.Add(this.btnBreak);
            this.panel1.Controls.Add(this.txtRemote);
            this.panel1.Controls.Add(this.btnAttachDebugger);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1097, 64);
            this.panel1.TabIndex = 0;
            // 
            // chkBreakOnException
            // 
            this.chkBreakOnException.AutoSize = true;
            this.chkBreakOnException.Location = new System.Drawing.Point(543, 37);
            this.chkBreakOnException.Name = "chkBreakOnException";
            this.chkBreakOnException.Size = new System.Drawing.Size(118, 17);
            this.chkBreakOnException.TabIndex = 8;
            this.chkBreakOnException.Text = "Break on exception";
            this.chkBreakOnException.UseVisualStyleBackColor = true;
            // 
            // btnContinue
            // 
            this.btnContinue.Location = new System.Drawing.Point(464, 34);
            this.btnContinue.Name = "btnContinue";
            this.btnContinue.Size = new System.Drawing.Size(71, 20);
            this.btnContinue.TabIndex = 7;
            this.btnContinue.Text = "Continue";
            this.btnContinue.UseVisualStyleBackColor = true;
            this.btnContinue.Click += new System.EventHandler(this.btnContinue_Click);
            // 
            // btnStepOver
            // 
            this.btnStepOver.Location = new System.Drawing.Point(543, 8);
            this.btnStepOver.Name = "btnStepOver";
            this.btnStepOver.Size = new System.Drawing.Size(104, 20);
            this.btnStepOver.TabIndex = 6;
            this.btnStepOver.Text = "Step Over";
            this.btnStepOver.UseVisualStyleBackColor = true;
            this.btnStepOver.Click += new System.EventHandler(this.btnStepOver_Click);
            // 
            // btnBreak
            // 
            this.btnBreak.Location = new System.Drawing.Point(464, 8);
            this.btnBreak.Name = "btnBreak";
            this.btnBreak.Size = new System.Drawing.Size(73, 20);
            this.btnBreak.TabIndex = 5;
            this.btnBreak.Text = "Break";
            this.btnBreak.UseVisualStyleBackColor = true;
            this.btnBreak.Click += new System.EventHandler(this.btnBreak_Click);
            // 
            // txtRemote
            // 
            this.txtRemote.Location = new System.Drawing.Point(9, 9);
            this.txtRemote.Name = "txtRemote";
            this.txtRemote.Size = new System.Drawing.Size(278, 20);
            this.txtRemote.TabIndex = 4;
            // 
            // btnAttachDebugger
            // 
            this.btnAttachDebugger.Location = new System.Drawing.Point(293, 8);
            this.btnAttachDebugger.Name = "btnAttachDebugger";
            this.btnAttachDebugger.Size = new System.Drawing.Size(130, 21);
            this.btnAttachDebugger.TabIndex = 3;
            this.btnAttachDebugger.Text = "Attach to process";
            this.btnAttachDebugger.UseVisualStyleBackColor = true;
            this.btnAttachDebugger.Click += new System.EventHandler(this.btnAttachDebugger_Click);
            // 
            // rtbSourceCode
            // 
            this.rtbSourceCode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbSourceCode.Location = new System.Drawing.Point(3, 73);
            this.rtbSourceCode.Name = "rtbSourceCode";
            this.rtbSourceCode.Size = new System.Drawing.Size(1097, 352);
            this.rtbSourceCode.TabIndex = 1;
            this.rtbSourceCode.Text = "";
            // 
            // tabViewBottom
            // 
            this.tabViewBottom.Controls.Add(this.tabOutput);
            this.tabViewBottom.Controls.Add(this.tabLocals);
            this.tabViewBottom.Controls.Add(this.tabStackFrames);
            this.tabViewBottom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabViewBottom.Location = new System.Drawing.Point(0, 0);
            this.tabViewBottom.Name = "tabViewBottom";
            this.tabViewBottom.SelectedIndex = 0;
            this.tabViewBottom.Size = new System.Drawing.Size(1103, 326);
            this.tabViewBottom.TabIndex = 0;
            // 
            // tabOutput
            // 
            this.tabOutput.Controls.Add(this.txtOuput);
            this.tabOutput.Location = new System.Drawing.Point(4, 22);
            this.tabOutput.Name = "tabOutput";
            this.tabOutput.Padding = new System.Windows.Forms.Padding(3);
            this.tabOutput.Size = new System.Drawing.Size(1095, 300);
            this.tabOutput.TabIndex = 0;
            this.tabOutput.Text = "Output";
            this.tabOutput.UseVisualStyleBackColor = true;
            // 
            // txtOuput
            // 
            this.txtOuput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtOuput.Location = new System.Drawing.Point(3, 3);
            this.txtOuput.Multiline = true;
            this.txtOuput.Name = "txtOuput";
            this.txtOuput.ReadOnly = true;
            this.txtOuput.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtOuput.Size = new System.Drawing.Size(1089, 294);
            this.txtOuput.TabIndex = 0;
            // 
            // tabLocals
            // 
            this.tabLocals.Location = new System.Drawing.Point(4, 22);
            this.tabLocals.Name = "tabLocals";
            this.tabLocals.Padding = new System.Windows.Forms.Padding(3);
            this.tabLocals.Size = new System.Drawing.Size(1095, 300);
            this.tabLocals.TabIndex = 1;
            this.tabLocals.Text = "Locals";
            this.tabLocals.UseVisualStyleBackColor = true;
            // 
            // tabStackFrames
            // 
            this.tabStackFrames.Controls.Add(this.tblLayoutStackFrames);
            this.tabStackFrames.Location = new System.Drawing.Point(4, 22);
            this.tabStackFrames.Name = "tabStackFrames";
            this.tabStackFrames.Size = new System.Drawing.Size(1095, 300);
            this.tabStackFrames.TabIndex = 2;
            this.tabStackFrames.Text = "Stack Frames";
            this.tabStackFrames.UseVisualStyleBackColor = true;
            // 
            // tblLayoutStackFrames
            // 
            this.tblLayoutStackFrames.ColumnCount = 2;
            this.tblLayoutStackFrames.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblLayoutStackFrames.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 350F));
            this.tblLayoutStackFrames.Controls.Add(this.txtCurrentInstruction, 0, 0);
            this.tblLayoutStackFrames.Controls.Add(this.dgvStackFrames, 0, 1);
            this.tblLayoutStackFrames.Controls.Add(this.txtStackMessages, 1, 0);
            this.tblLayoutStackFrames.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblLayoutStackFrames.Location = new System.Drawing.Point(0, 0);
            this.tblLayoutStackFrames.Name = "tblLayoutStackFrames";
            this.tblLayoutStackFrames.RowCount = 2;
            this.tblLayoutStackFrames.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tblLayoutStackFrames.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tblLayoutStackFrames.Size = new System.Drawing.Size(1095, 300);
            this.tblLayoutStackFrames.TabIndex = 0;
            // 
            // txtCurrentInstruction
            // 
            this.txtCurrentInstruction.Location = new System.Drawing.Point(3, 3);
            this.txtCurrentInstruction.Name = "txtCurrentInstruction";
            this.txtCurrentInstruction.ReadOnly = true;
            this.txtCurrentInstruction.Size = new System.Drawing.Size(485, 20);
            this.txtCurrentInstruction.TabIndex = 0;
            // 
            // dgvStackFrames
            // 
            this.dgvStackFrames.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvStackFrames.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvStackFrames.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvStackFrames.Location = new System.Drawing.Point(3, 43);
            this.dgvStackFrames.Name = "dgvStackFrames";
            this.dgvStackFrames.RowHeadersVisible = false;
            this.dgvStackFrames.Size = new System.Drawing.Size(739, 254);
            this.dgvStackFrames.TabIndex = 1;
            // 
            // txtStackMessages
            // 
            this.txtStackMessages.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtStackMessages.Location = new System.Drawing.Point(748, 3);
            this.txtStackMessages.Multiline = true;
            this.txtStackMessages.Name = "txtStackMessages";
            this.txtStackMessages.ReadOnly = true;
            this.tblLayoutStackFrames.SetRowSpan(this.txtStackMessages, 2);
            this.txtStackMessages.Size = new System.Drawing.Size(344, 294);
            this.txtStackMessages.TabIndex = 2;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1103, 728);
            this.Controls.Add(this.mainSplitContainer);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.mainSplitContainer.Panel1.ResumeLayout(false);
            this.mainSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.mainSplitContainer)).EndInit();
            this.mainSplitContainer.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tabViewBottom.ResumeLayout(false);
            this.tabOutput.ResumeLayout(false);
            this.tabOutput.PerformLayout();
            this.tabStackFrames.ResumeLayout(false);
            this.tblLayoutStackFrames.ResumeLayout(false);
            this.tblLayoutStackFrames.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvStackFrames)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer mainSplitContainer;
        private System.Windows.Forms.TabControl tabViewBottom;
        private System.Windows.Forms.TabPage tabOutput;
        private System.Windows.Forms.TabPage tabLocals;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox txtRemote;
        private System.Windows.Forms.Button btnAttachDebugger;
        private System.Windows.Forms.RichTextBox rtbSourceCode;
        private System.Windows.Forms.TextBox txtOuput;
        private System.Windows.Forms.Button btnStepOver;
        private System.Windows.Forms.Button btnBreak;
        private System.Windows.Forms.Button btnContinue;
        private System.Windows.Forms.TabPage tabStackFrames;
        private System.Windows.Forms.CheckBox chkBreakOnException;
        private System.Windows.Forms.TableLayoutPanel tblLayoutStackFrames;
        private System.Windows.Forms.TextBox txtCurrentInstruction;
        private System.Windows.Forms.DataGridView dgvStackFrames;
        private System.Windows.Forms.TextBox txtStackMessages;
    }
}

