namespace Machine_GUI
{
	partial class MainWindow
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
			this.problemLabel = new System.Windows.Forms.Label();
			this.problemSelector = new System.Windows.Forms.ComboBox();
			this.inputPathBox = new System.Windows.Forms.TextBox();
			this.inputLabel = new System.Windows.Forms.Label();
			this.outputLabel = new System.Windows.Forms.Label();
			this.outputPathBox = new System.Windows.Forms.TextBox();
			this.runBtn = new System.Windows.Forms.Button();
			this.inputBox = new System.Windows.Forms.RichTextBox();
			this.outputBox = new System.Windows.Forms.RichTextBox();
			this.isInputFilePath = new System.Windows.Forms.CheckBox();
			this.isOutputFilePath = new System.Windows.Forms.CheckBox();
			this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
			this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
			this.clearPathBtn = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// problemLabel
			// 
			this.problemLabel.AutoSize = true;
			this.problemLabel.Location = new System.Drawing.Point(14, 10);
			this.problemLabel.Name = "problemLabel";
			this.problemLabel.Size = new System.Drawing.Size(105, 14);
			this.problemLabel.TabIndex = 0;
			this.problemLabel.Text = "Problem Solver";
			// 
			// problemSelector
			// 
			this.problemSelector.FormattingEnabled = true;
			this.problemSelector.Location = new System.Drawing.Point(125, 7);
			this.problemSelector.Name = "problemSelector";
			this.problemSelector.Size = new System.Drawing.Size(548, 22);
			this.problemSelector.TabIndex = 1;
			// 
			// inputPathBox
			// 
			this.inputPathBox.Location = new System.Drawing.Point(125, 37);
			this.inputPathBox.Name = "inputPathBox";
			this.inputPathBox.Size = new System.Drawing.Size(488, 22);
			this.inputPathBox.TabIndex = 2;
			this.inputPathBox.Click += new System.EventHandler(this.inputPathBox_Click);
			// 
			// inputLabel
			// 
			this.inputLabel.AutoSize = true;
			this.inputLabel.Location = new System.Drawing.Point(14, 41);
			this.inputLabel.Name = "inputLabel";
			this.inputLabel.Size = new System.Drawing.Size(77, 14);
			this.inputLabel.TabIndex = 3;
			this.inputLabel.Text = "Input Path";
			// 
			// outputLabel
			// 
			this.outputLabel.AutoSize = true;
			this.outputLabel.Location = new System.Drawing.Point(14, 72);
			this.outputLabel.Name = "outputLabel";
			this.outputLabel.Size = new System.Drawing.Size(84, 14);
			this.outputLabel.TabIndex = 4;
			this.outputLabel.Text = "Output Path";
			// 
			// outputPathBox
			// 
			this.outputPathBox.Location = new System.Drawing.Point(125, 69);
			this.outputPathBox.Name = "outputPathBox";
			this.outputPathBox.Size = new System.Drawing.Size(488, 22);
			this.outputPathBox.TabIndex = 5;
			this.outputPathBox.Click += new System.EventHandler(this.outputPathBox_Click);
			// 
			// runBtn
			// 
			this.runBtn.Location = new System.Drawing.Point(16, 100);
			this.runBtn.Name = "runBtn";
			this.runBtn.Size = new System.Drawing.Size(543, 44);
			this.runBtn.TabIndex = 8;
			this.runBtn.Text = "RUN";
			this.runBtn.UseVisualStyleBackColor = true;
			this.runBtn.Click += new System.EventHandler(this.runBtn_Click);
			// 
			// inputBox
			// 
			this.inputBox.BackColor = System.Drawing.Color.Black;
			this.inputBox.DetectUrls = false;
			this.inputBox.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.inputBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(233)))), ((int)(((byte)(0)))));
			this.inputBox.Location = new System.Drawing.Point(16, 152);
			this.inputBox.Name = "inputBox";
			this.inputBox.Size = new System.Drawing.Size(314, 285);
			this.inputBox.TabIndex = 9;
			this.inputBox.Text = "Input lines go here.";
			// 
			// outputBox
			// 
			this.outputBox.BackColor = System.Drawing.Color.Black;
			this.outputBox.DetectUrls = false;
			this.outputBox.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.outputBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(233)))), ((int)(((byte)(0)))));
			this.outputBox.Location = new System.Drawing.Point(359, 152);
			this.outputBox.Name = "outputBox";
			this.outputBox.Size = new System.Drawing.Size(314, 285);
			this.outputBox.TabIndex = 10;
			this.outputBox.Text = "Output lines go here.";
			// 
			// isInputFilePath
			// 
			this.isInputFilePath.AutoSize = true;
			this.isInputFilePath.Checked = true;
			this.isInputFilePath.CheckState = System.Windows.Forms.CheckState.Checked;
			this.isInputFilePath.Location = new System.Drawing.Point(619, 40);
			this.isInputFilePath.Name = "isInputFilePath";
			this.isInputFilePath.Size = new System.Drawing.Size(54, 18);
			this.isInputFilePath.TabIndex = 11;
			this.isInputFilePath.Text = "File";
			this.isInputFilePath.UseVisualStyleBackColor = true;
			this.isInputFilePath.CheckedChanged += new System.EventHandler(this.isInputFilePath_CheckedChanged);
			// 
			// isOutputFilePath
			// 
			this.isOutputFilePath.AutoSize = true;
			this.isOutputFilePath.Checked = true;
			this.isOutputFilePath.CheckState = System.Windows.Forms.CheckState.Checked;
			this.isOutputFilePath.Location = new System.Drawing.Point(619, 71);
			this.isOutputFilePath.Name = "isOutputFilePath";
			this.isOutputFilePath.Size = new System.Drawing.Size(54, 18);
			this.isOutputFilePath.TabIndex = 12;
			this.isOutputFilePath.Text = "File";
			this.isOutputFilePath.UseVisualStyleBackColor = true;
			this.isOutputFilePath.CheckedChanged += new System.EventHandler(this.isOutputFilePath_CheckedChanged);
			// 
			// openFileDialog1
			// 
			this.openFileDialog1.DefaultExt = "in";
			this.openFileDialog1.FileName = "openFileDialog1";
			this.openFileDialog1.Title = "Choose the input file";
			// 
			// saveFileDialog1
			// 
			this.saveFileDialog1.DefaultExt = "out";
			this.saveFileDialog1.Title = "Choose where the output should be";
			// 
			// clearPathBtn
			// 
			this.clearPathBtn.Location = new System.Drawing.Point(565, 100);
			this.clearPathBtn.Name = "clearPathBtn";
			this.clearPathBtn.Size = new System.Drawing.Size(108, 43);
			this.clearPathBtn.TabIndex = 13;
			this.clearPathBtn.Text = "Clear Path";
			this.clearPathBtn.UseVisualStyleBackColor = true;
			this.clearPathBtn.Click += new System.EventHandler(this.clearPathBtn_Click);
			// 
			// MainWindow
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.Control;
			this.ClientSize = new System.Drawing.Size(687, 451);
			this.Controls.Add(this.clearPathBtn);
			this.Controls.Add(this.isOutputFilePath);
			this.Controls.Add(this.isInputFilePath);
			this.Controls.Add(this.outputBox);
			this.Controls.Add(this.inputBox);
			this.Controls.Add(this.runBtn);
			this.Controls.Add(this.outputPathBox);
			this.Controls.Add(this.outputLabel);
			this.Controls.Add(this.inputLabel);
			this.Controls.Add(this.inputPathBox);
			this.Controls.Add(this.problemSelector);
			this.Controls.Add(this.problemLabel);
			this.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.MaximumSize = new System.Drawing.Size(695, 485);
			this.MinimumSize = new System.Drawing.Size(695, 485);
			this.Name = "MainWindow";
			this.Text = "Machine GUI";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label problemLabel;
		private System.Windows.Forms.ComboBox problemSelector;
		private System.Windows.Forms.TextBox inputPathBox;
		private System.Windows.Forms.Label inputLabel;
		private System.Windows.Forms.Label outputLabel;
		private System.Windows.Forms.TextBox outputPathBox;
		private System.Windows.Forms.Button runBtn;
		private System.Windows.Forms.RichTextBox inputBox;
		private System.Windows.Forms.RichTextBox outputBox;
		private System.Windows.Forms.CheckBox isInputFilePath;
		private System.Windows.Forms.CheckBox isOutputFilePath;
		private System.Windows.Forms.OpenFileDialog openFileDialog1;
		private System.Windows.Forms.SaveFileDialog saveFileDialog1;
		private System.Windows.Forms.Button clearPathBtn;
	}
}