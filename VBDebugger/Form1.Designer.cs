
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
            this.tabViewBottom = new System.Windows.Forms.TabControl();
            this.tabOutput = new System.Windows.Forms.TabPage();
            this.tabLocals = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtRemote = new System.Windows.Forms.TextBox();
            this.btnAttachDebugger = new System.Windows.Forms.Button();
            this.rtbSourceCode = new System.Windows.Forms.RichTextBox();
            this.txtOuput = new System.Windows.Forms.TextBox();
            this.btnBreakContinue = new System.Windows.Forms.Button();
            this.btnStepOver = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.mainSplitContainer)).BeginInit();
            this.mainSplitContainer.Panel1.SuspendLayout();
            this.mainSplitContainer.Panel2.SuspendLayout();
            this.mainSplitContainer.SuspendLayout();
            this.tabViewBottom.SuspendLayout();
            this.tabOutput.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
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
            // tabViewBottom
            // 
            this.tabViewBottom.Controls.Add(this.tabOutput);
            this.tabViewBottom.Controls.Add(this.tabLocals);
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
            // tabLocals
            // 
            this.tabLocals.Location = new System.Drawing.Point(4, 22);
            this.tabLocals.Name = "tabLocals";
            this.tabLocals.Padding = new System.Windows.Forms.Padding(3);
            this.tabLocals.Size = new System.Drawing.Size(1095, 334);
            this.tabLocals.TabIndex = 1;
            this.tabLocals.Text = "Locals";
            this.tabLocals.UseVisualStyleBackColor = true;
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
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1103, 398);
            this.tableLayoutPanel1.TabIndex = 3;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnStepOver);
            this.panel1.Controls.Add(this.btnBreakContinue);
            this.panel1.Controls.Add(this.txtRemote);
            this.panel1.Controls.Add(this.btnAttachDebugger);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1097, 34);
            this.panel1.TabIndex = 0;
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
            this.rtbSourceCode.Location = new System.Drawing.Point(3, 43);
            this.rtbSourceCode.Name = "rtbSourceCode";
            this.rtbSourceCode.Size = new System.Drawing.Size(1097, 352);
            this.rtbSourceCode.TabIndex = 1;
            this.rtbSourceCode.Text = "";
            // 
            // txtOuput
            // 
            this.txtOuput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtOuput.Location = new System.Drawing.Point(3, 3);
            this.txtOuput.Multiline = true;
            this.txtOuput.Name = "txtOuput";
            this.txtOuput.Size = new System.Drawing.Size(1089, 294);
            this.txtOuput.TabIndex = 0;
            // 
            // btnBreakContinue
            // 
            this.btnBreakContinue.Location = new System.Drawing.Point(475, 8);
            this.btnBreakContinue.Name = "btnBreakContinue";
            this.btnBreakContinue.Size = new System.Drawing.Size(117, 20);
            this.btnBreakContinue.TabIndex = 5;
            this.btnBreakContinue.Text = "Break/Continue";
            this.btnBreakContinue.UseVisualStyleBackColor = true;
            this.btnBreakContinue.Click += new System.EventHandler(this.btnBreakContinue_Click);
            // 
            // btnStepOver
            // 
            this.btnStepOver.Location = new System.Drawing.Point(598, 8);
            this.btnStepOver.Name = "btnStepOver";
            this.btnStepOver.Size = new System.Drawing.Size(104, 20);
            this.btnStepOver.TabIndex = 6;
            this.btnStepOver.Text = "Step Over";
            this.btnStepOver.UseVisualStyleBackColor = true;
            this.btnStepOver.Click += new System.EventHandler(this.btnStepOver_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1103, 728);
            this.Controls.Add(this.mainSplitContainer);
            this.Name = "Form1";
            this.Text = "Form1";
            this.mainSplitContainer.Panel1.ResumeLayout(false);
            this.mainSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.mainSplitContainer)).EndInit();
            this.mainSplitContainer.ResumeLayout(false);
            this.tabViewBottom.ResumeLayout(false);
            this.tabOutput.ResumeLayout(false);
            this.tabOutput.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
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
        private System.Windows.Forms.Button btnBreakContinue;
    }
}

