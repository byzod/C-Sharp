namespace YeaBase64
{
	partial class MainForm
	{
		/// <summary>
		/// 必需的设计器变量。
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// 清理所有正在使用的资源。
		/// </summary>
		/// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows 窗体设计器生成的代码

		/// <summary>
		/// 设计器支持所需的方法 - 不要
		/// 使用代码编辑器修改此方法的内容。
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.label1 = new System.Windows.Forms.Label();
			this.labelBase64Copied = new System.Windows.Forms.Label();
			this.base64DropPanel = new System.Windows.Forms.Panel();
			this.label6 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.base64FilePathBox = new System.Windows.Forms.TextBox();
			this.base64FileModeCheckBox = new System.Windows.Forms.CheckBox();
			this.openBase64FileButton = new System.Windows.Forms.Button();
			this.base64Box = new System.Windows.Forms.RichTextBox();
			this.textFilePathBox = new System.Windows.Forms.TextBox();
			this.encodingchoiceBox1 = new System.Windows.Forms.ComboBox();
			this.labelTextCopied = new System.Windows.Forms.Label();
			this.textDropPanel = new System.Windows.Forms.Panel();
			this.label4 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.codePageTextBox = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.base642textbutton = new System.Windows.Forms.Button();
			this.openTextFileButton = new System.Windows.Forms.Button();
			this.text2base64button = new System.Windows.Forms.Button();
			this.textBox = new System.Windows.Forms.RichTextBox();
			this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
			this.MainmenuStrip = new System.Windows.Forms.MenuStrip();
			this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.helpHelpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.changelogToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.helpMessageToolTip = new System.Windows.Forms.ToolTip(this.components);
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.base64DropPanel.SuspendLayout();
			this.textDropPanel.SuspendLayout();
			this.MainmenuStrip.SuspendLayout();
			this.SuspendLayout();
			// 
			// splitContainer1
			// 
			resources.ApplyResources(this.splitContainer1, "splitContainer1");
			this.splitContainer1.Name = "splitContainer1";
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.label1);
			this.splitContainer1.Panel1.Controls.Add(this.labelBase64Copied);
			this.splitContainer1.Panel1.Controls.Add(this.base64DropPanel);
			this.splitContainer1.Panel1.Controls.Add(this.base64FilePathBox);
			this.splitContainer1.Panel1.Controls.Add(this.base64FileModeCheckBox);
			this.splitContainer1.Panel1.Controls.Add(this.openBase64FileButton);
			this.splitContainer1.Panel1.Controls.Add(this.base64Box);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.textFilePathBox);
			this.splitContainer1.Panel2.Controls.Add(this.encodingchoiceBox1);
			this.splitContainer1.Panel2.Controls.Add(this.labelTextCopied);
			this.splitContainer1.Panel2.Controls.Add(this.textDropPanel);
			this.splitContainer1.Panel2.Controls.Add(this.codePageTextBox);
			this.splitContainer1.Panel2.Controls.Add(this.label2);
			this.splitContainer1.Panel2.Controls.Add(this.base642textbutton);
			this.splitContainer1.Panel2.Controls.Add(this.openTextFileButton);
			this.splitContainer1.Panel2.Controls.Add(this.text2base64button);
			this.splitContainer1.Panel2.Controls.Add(this.textBox);
			// 
			// label1
			// 
			resources.ApplyResources(this.label1, "label1");
			this.label1.Name = "label1";
			// 
			// labelBase64Copied
			// 
			resources.ApplyResources(this.labelBase64Copied, "labelBase64Copied");
			this.labelBase64Copied.ForeColor = System.Drawing.SystemColors.GrayText;
			this.labelBase64Copied.Name = "labelBase64Copied";
			// 
			// base64DropPanel
			// 
			this.base64DropPanel.AllowDrop = true;
			resources.ApplyResources(this.base64DropPanel, "base64DropPanel");
			this.base64DropPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.base64DropPanel.Controls.Add(this.label6);
			this.base64DropPanel.Controls.Add(this.label7);
			this.base64DropPanel.Name = "base64DropPanel";
			this.base64DropPanel.DragDrop += new System.Windows.Forms.DragEventHandler(this.droppanel2_DragDrop);
			this.base64DropPanel.DragEnter += new System.Windows.Forms.DragEventHandler(this.droppanel_DragEnter);
			// 
			// label6
			// 
			resources.ApplyResources(this.label6, "label6");
			this.label6.ForeColor = System.Drawing.SystemColors.GrayText;
			this.label6.Name = "label6";
			// 
			// label7
			// 
			resources.ApplyResources(this.label7, "label7");
			this.label7.ForeColor = System.Drawing.SystemColors.GrayText;
			this.label7.Name = "label7";
			// 
			// base64FilePathBox
			// 
			this.base64FilePathBox.AllowDrop = true;
			resources.ApplyResources(this.base64FilePathBox, "base64FilePathBox");
			this.base64FilePathBox.ForeColor = System.Drawing.SystemColors.WindowText;
			this.base64FilePathBox.Name = "base64FilePathBox";
			this.base64FilePathBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox_SelectAll);
			// 
			// base64FileModeCheckBox
			// 
			resources.ApplyResources(this.base64FileModeCheckBox, "base64FileModeCheckBox");
			this.base64FileModeCheckBox.Name = "base64FileModeCheckBox";
			this.base64FileModeCheckBox.UseVisualStyleBackColor = true;
			this.base64FileModeCheckBox.CheckedChanged += new System.EventHandler(this.fileModeCheckBox1_CheckedChanged);
			this.base64FileModeCheckBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.otherHotkeyHandler);
			// 
			// openBase64FileButton
			// 
			resources.ApplyResources(this.openBase64FileButton, "openBase64FileButton");
			this.openBase64FileButton.Name = "openBase64FileButton";
			this.openBase64FileButton.UseVisualStyleBackColor = true;
			this.openBase64FileButton.Click += new System.EventHandler(this.openFileButton2_Click);
			this.openBase64FileButton.KeyDown += new System.Windows.Forms.KeyEventHandler(this.otherHotkeyHandler);
			// 
			// base64Box
			// 
			this.base64Box.AllowDrop = true;
			resources.ApplyResources(this.base64Box, "base64Box");
			this.base64Box.DetectUrls = false;
			this.base64Box.Name = "base64Box";
			this.base64Box.DragDrop += new System.Windows.Forms.DragEventHandler(this.Box_DragDrop);
			this.base64Box.DragEnter += new System.Windows.Forms.DragEventHandler(this.Box_DragEnter);
			this.base64Box.TextChanged += new System.EventHandler(this.base64Box_TextChanged);
			this.base64Box.KeyDown += new System.Windows.Forms.KeyEventHandler(this.otherHotkeyHandler);
			// 
			// textFilePathBox
			// 
			this.textFilePathBox.AllowDrop = true;
			resources.ApplyResources(this.textFilePathBox, "textFilePathBox");
			this.textFilePathBox.ForeColor = System.Drawing.SystemColors.WindowText;
			this.textFilePathBox.Name = "textFilePathBox";
			this.textFilePathBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox_SelectAll);
			// 
			// encodingchoiceBox1
			// 
			resources.ApplyResources(this.encodingchoiceBox1, "encodingchoiceBox1");
			this.encodingchoiceBox1.CausesValidation = false;
			this.encodingchoiceBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.encodingchoiceBox1.FormattingEnabled = true;
			this.encodingchoiceBox1.Items.AddRange(new object[] {
            resources.GetString("encodingchoiceBox1.Items"),
            resources.GetString("encodingchoiceBox1.Items1"),
            resources.GetString("encodingchoiceBox1.Items2"),
            resources.GetString("encodingchoiceBox1.Items3"),
            resources.GetString("encodingchoiceBox1.Items4"),
            resources.GetString("encodingchoiceBox1.Items5"),
            resources.GetString("encodingchoiceBox1.Items6")});
			this.encodingchoiceBox1.Name = "encodingchoiceBox1";
			this.encodingchoiceBox1.SelectedValueChanged += new System.EventHandler(this.encodingchoiceBox1_SelectedValueChanged);
			this.encodingchoiceBox1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.otherHotkeyHandler);
			// 
			// labelTextCopied
			// 
			resources.ApplyResources(this.labelTextCopied, "labelTextCopied");
			this.labelTextCopied.ForeColor = System.Drawing.SystemColors.GrayText;
			this.labelTextCopied.Name = "labelTextCopied";
			// 
			// textDropPanel
			// 
			this.textDropPanel.AllowDrop = true;
			resources.ApplyResources(this.textDropPanel, "textDropPanel");
			this.textDropPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.textDropPanel.Controls.Add(this.label4);
			this.textDropPanel.Controls.Add(this.label3);
			this.textDropPanel.Name = "textDropPanel";
			this.textDropPanel.DragDrop += new System.Windows.Forms.DragEventHandler(this.droppanel1_DragDrop);
			this.textDropPanel.DragEnter += new System.Windows.Forms.DragEventHandler(this.droppanel_DragEnter);
			// 
			// label4
			// 
			resources.ApplyResources(this.label4, "label4");
			this.label4.ForeColor = System.Drawing.SystemColors.GrayText;
			this.label4.Name = "label4";
			// 
			// label3
			// 
			resources.ApplyResources(this.label3, "label3");
			this.label3.ForeColor = System.Drawing.SystemColors.GrayText;
			this.label3.Name = "label3";
			// 
			// codePageTextBox
			// 
			resources.ApplyResources(this.codePageTextBox, "codePageTextBox");
			this.codePageTextBox.Name = "codePageTextBox";
			this.codePageTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox_SelectAll);
			// 
			// label2
			// 
			resources.ApplyResources(this.label2, "label2");
			this.label2.Name = "label2";
			// 
			// base642textbutton
			// 
			resources.ApplyResources(this.base642textbutton, "base642textbutton");
			this.base642textbutton.Name = "base642textbutton";
			this.base642textbutton.UseVisualStyleBackColor = true;
			this.base642textbutton.Click += new System.EventHandler(this.base642textbutton_Click);
			this.base642textbutton.KeyDown += new System.Windows.Forms.KeyEventHandler(this.otherHotkeyHandler);
			// 
			// openTextFileButton
			// 
			resources.ApplyResources(this.openTextFileButton, "openTextFileButton");
			this.openTextFileButton.Name = "openTextFileButton";
			this.openTextFileButton.UseVisualStyleBackColor = true;
			this.openTextFileButton.Click += new System.EventHandler(this.openFileButton1_Click);
			this.openTextFileButton.KeyDown += new System.Windows.Forms.KeyEventHandler(this.otherHotkeyHandler);
			// 
			// text2base64button
			// 
			resources.ApplyResources(this.text2base64button, "text2base64button");
			this.text2base64button.Name = "text2base64button";
			this.text2base64button.UseVisualStyleBackColor = true;
			this.text2base64button.Click += new System.EventHandler(this.text2base64button_Click);
			this.text2base64button.KeyDown += new System.Windows.Forms.KeyEventHandler(this.otherHotkeyHandler);
			// 
			// textBox
			// 
			this.textBox.AcceptsTab = true;
			this.textBox.AllowDrop = true;
			resources.ApplyResources(this.textBox, "textBox");
			this.textBox.DetectUrls = false;
			this.textBox.Name = "textBox";
			this.textBox.DragDrop += new System.Windows.Forms.DragEventHandler(this.Box_DragDrop);
			this.textBox.DragEnter += new System.Windows.Forms.DragEventHandler(this.Box_DragEnter);
			this.textBox.TextChanged += new System.EventHandler(this.textBox_TextChanged);
			this.textBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.otherHotkeyHandler);
			// 
			// saveFileDialog1
			// 
			resources.ApplyResources(this.saveFileDialog1, "saveFileDialog1");
			this.saveFileDialog1.OverwritePrompt = false;
			// 
			// MainmenuStrip
			// 
			this.MainmenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.helpToolStripMenuItem,
            this.optionsToolStripMenuItem});
			resources.ApplyResources(this.MainmenuStrip, "MainmenuStrip");
			this.MainmenuStrip.Name = "MainmenuStrip";
			// 
			// helpToolStripMenuItem
			// 
			this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.helpHelpToolStripMenuItem,
            this.changelogToolStripMenuItem,
            this.aboutToolStripMenuItem});
			this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
			resources.ApplyResources(this.helpToolStripMenuItem, "helpToolStripMenuItem");
			// 
			// helpHelpToolStripMenuItem
			// 
			this.helpHelpToolStripMenuItem.Name = "helpHelpToolStripMenuItem";
			resources.ApplyResources(this.helpHelpToolStripMenuItem, "helpHelpToolStripMenuItem");
			this.helpHelpToolStripMenuItem.Click += new System.EventHandler(this.helpHelpToolStripMenuItem_Click);
			// 
			// changelogToolStripMenuItem
			// 
			this.changelogToolStripMenuItem.Name = "changelogToolStripMenuItem";
			resources.ApplyResources(this.changelogToolStripMenuItem, "changelogToolStripMenuItem");
			this.changelogToolStripMenuItem.Click += new System.EventHandler(this.changelogToolStripMenuItem_Click);
			// 
			// aboutToolStripMenuItem
			// 
			this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
			resources.ApplyResources(this.aboutToolStripMenuItem, "aboutToolStripMenuItem");
			this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
			// 
			// optionsToolStripMenuItem
			// 
			this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
			resources.ApplyResources(this.optionsToolStripMenuItem, "optionsToolStripMenuItem");
			this.optionsToolStripMenuItem.Click += new System.EventHandler(this.optionsToolStripMenuItem_Click);
			// 
			// MainForm
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.MainmenuStrip);
			this.Controls.Add(this.splitContainer1);
			this.Name = "MainForm";
			this.Load += new System.EventHandler(this.MainForm_Load);
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.otherHotkeyHandler);
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel1.PerformLayout();
			this.splitContainer1.Panel2.ResumeLayout(false);
			this.splitContainer1.Panel2.PerformLayout();
			this.splitContainer1.ResumeLayout(false);
			this.base64DropPanel.ResumeLayout(false);
			this.base64DropPanel.PerformLayout();
			this.textDropPanel.ResumeLayout(false);
			this.textDropPanel.PerformLayout();
			this.MainmenuStrip.ResumeLayout(false);
			this.MainmenuStrip.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox textFilePathBox;
		private System.Windows.Forms.Button openTextFileButton;
		private System.Windows.Forms.ComboBox encodingchoiceBox1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button text2base64button;
		private System.Windows.Forms.Button base642textbutton;
		private System.Windows.Forms.Panel textDropPanel;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.SaveFileDialog saveFileDialog1;
		private System.Windows.Forms.CheckBox base64FileModeCheckBox;
		private System.Windows.Forms.TextBox base64FilePathBox;
		private System.Windows.Forms.Button openBase64FileButton;
		private System.Windows.Forms.Panel base64DropPanel;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.TextBox codePageTextBox;
		private System.Windows.Forms.MenuStrip MainmenuStrip;
		private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem helpHelpToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
		private System.Windows.Forms.ToolTip helpMessageToolTip;
		private System.Windows.Forms.RichTextBox base64Box;
		private System.Windows.Forms.RichTextBox textBox;
		private System.Windows.Forms.ToolStripMenuItem changelogToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
		private System.Windows.Forms.Label labelBase64Copied;
		private System.Windows.Forms.Label labelTextCopied;
		private System.Windows.Forms.SplitContainer splitContainer1;
	}
}

