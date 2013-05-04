namespace YeaBase64
{
	partial class Option
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Option));
			this.cancelButton = new System.Windows.Forms.Button();
			this.saveButton = new System.Windows.Forms.Button();
			this.optionTabs = new System.Windows.Forms.TabControl();
			this.generalPage = new System.Windows.Forms.TabPage();
			this.imageStyleGroupBox = new System.Windows.Forms.GroupBox();
			this.imageAutoDetectCheckBox = new System.Windows.Forms.CheckBox();
			this.radioButtonImageBase64StyleNone = new System.Windows.Forms.RadioButton();
			this.textBoxStyleAfter = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.extMonitorTextBox = new System.Windows.Forms.TextBox();
			this.textBoxStyleBefore = new System.Windows.Forms.TextBox();
			this.labelImageBase64Style = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.radioButtonImageBase64StyleHeader = new System.Windows.Forms.RadioButton();
			this.radioButtonImageBase64StyleStyle = new System.Windows.Forms.RadioButton();
			this.radioButtonImageBase64StyleDIY = new System.Windows.Forms.RadioButton();
			this.checkBoxRealtimeConvert = new System.Windows.Forms.CheckBox();
			this.checkBoxAutoCopy = new System.Windows.Forms.CheckBox();
			this.textBoxAutoConvertThreshold = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.comboBoxLanguageChoice = new System.Windows.Forms.ComboBox();
			this.textBoxTextBoxFont = new YeaBase64.FontTextBox();
			this.textBoxBase64BoxFont = new YeaBase64.FontTextBox();
			this.labelTextBoxFont = new System.Windows.Forms.Label();
			this.labelBase64BoxFont = new System.Windows.Forms.Label();
			this.advancedPage = new System.Windows.Forms.TabPage();
			this.labelEasterEgg = new System.Windows.Forms.Label();
			this.fontDialog1 = new System.Windows.Forms.FontDialog();
			this.resetAllButton = new System.Windows.Forms.Button();
			this.applyButton = new System.Windows.Forms.Button();
			this.optionTabs.SuspendLayout();
			this.generalPage.SuspendLayout();
			this.imageStyleGroupBox.SuspendLayout();
			this.advancedPage.SuspendLayout();
			this.SuspendLayout();
			// 
			// cancelButton
			// 
			resources.ApplyResources(this.cancelButton, "cancelButton");
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.UseVisualStyleBackColor = true;
			this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
			// 
			// saveButton
			// 
			resources.ApplyResources(this.saveButton, "saveButton");
			this.saveButton.Name = "saveButton";
			this.saveButton.UseVisualStyleBackColor = true;
			this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
			// 
			// optionTabs
			// 
			resources.ApplyResources(this.optionTabs, "optionTabs");
			this.optionTabs.Controls.Add(this.generalPage);
			this.optionTabs.Controls.Add(this.advancedPage);
			this.optionTabs.Name = "optionTabs";
			this.optionTabs.SelectedIndex = 0;
			// 
			// generalPage
			// 
			this.generalPage.BackColor = System.Drawing.SystemColors.Control;
			this.generalPage.Controls.Add(this.imageStyleGroupBox);
			this.generalPage.Controls.Add(this.checkBoxRealtimeConvert);
			this.generalPage.Controls.Add(this.checkBoxAutoCopy);
			this.generalPage.Controls.Add(this.textBoxAutoConvertThreshold);
			this.generalPage.Controls.Add(this.label1);
			this.generalPage.Controls.Add(this.comboBoxLanguageChoice);
			this.generalPage.Controls.Add(this.textBoxTextBoxFont);
			this.generalPage.Controls.Add(this.textBoxBase64BoxFont);
			this.generalPage.Controls.Add(this.labelTextBoxFont);
			this.generalPage.Controls.Add(this.labelBase64BoxFont);
			resources.ApplyResources(this.generalPage, "generalPage");
			this.generalPage.Name = "generalPage";
			// 
			// imageStyleGroupBox
			// 
			this.imageStyleGroupBox.Controls.Add(this.imageAutoDetectCheckBox);
			this.imageStyleGroupBox.Controls.Add(this.radioButtonImageBase64StyleNone);
			this.imageStyleGroupBox.Controls.Add(this.textBoxStyleAfter);
			this.imageStyleGroupBox.Controls.Add(this.label2);
			this.imageStyleGroupBox.Controls.Add(this.extMonitorTextBox);
			this.imageStyleGroupBox.Controls.Add(this.textBoxStyleBefore);
			this.imageStyleGroupBox.Controls.Add(this.labelImageBase64Style);
			this.imageStyleGroupBox.Controls.Add(this.label3);
			this.imageStyleGroupBox.Controls.Add(this.radioButtonImageBase64StyleHeader);
			this.imageStyleGroupBox.Controls.Add(this.radioButtonImageBase64StyleStyle);
			this.imageStyleGroupBox.Controls.Add(this.radioButtonImageBase64StyleDIY);
			resources.ApplyResources(this.imageStyleGroupBox, "imageStyleGroupBox");
			this.imageStyleGroupBox.Name = "imageStyleGroupBox";
			this.imageStyleGroupBox.TabStop = false;
			// 
			// imageAutoDetectCheckBox
			// 
			resources.ApplyResources(this.imageAutoDetectCheckBox, "imageAutoDetectCheckBox");
			this.imageAutoDetectCheckBox.Name = "imageAutoDetectCheckBox";
			this.imageAutoDetectCheckBox.UseVisualStyleBackColor = true;
			this.imageAutoDetectCheckBox.CheckedChanged += new System.EventHandler(this.Option_Changed);
			// 
			// radioButtonImageBase64StyleNone
			// 
			resources.ApplyResources(this.radioButtonImageBase64StyleNone, "radioButtonImageBase64StyleNone");
			this.radioButtonImageBase64StyleNone.Checked = true;
			this.radioButtonImageBase64StyleNone.Name = "radioButtonImageBase64StyleNone";
			this.radioButtonImageBase64StyleNone.TabStop = true;
			this.radioButtonImageBase64StyleNone.UseVisualStyleBackColor = true;
			this.radioButtonImageBase64StyleNone.CheckedChanged += new System.EventHandler(this.radioButtonsImageStyle_CheckedChanged);
			// 
			// textBoxStyleAfter
			// 
			resources.ApplyResources(this.textBoxStyleAfter, "textBoxStyleAfter");
			this.textBoxStyleAfter.Name = "textBoxStyleAfter";
			this.textBoxStyleAfter.ReadOnly = true;
			this.textBoxStyleAfter.TextChanged += new System.EventHandler(this.Option_Changed);
			// 
			// label2
			// 
			resources.ApplyResources(this.label2, "label2");
			this.label2.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
			this.label2.Name = "label2";
			// 
			// extMonitorTextBox
			// 
			resources.ApplyResources(this.extMonitorTextBox, "extMonitorTextBox");
			this.extMonitorTextBox.Name = "extMonitorTextBox";
			this.extMonitorTextBox.TextChanged += new System.EventHandler(this.Option_Changed);
			// 
			// textBoxStyleBefore
			// 
			resources.ApplyResources(this.textBoxStyleBefore, "textBoxStyleBefore");
			this.textBoxStyleBefore.Name = "textBoxStyleBefore";
			this.textBoxStyleBefore.ReadOnly = true;
			this.textBoxStyleBefore.TextChanged += new System.EventHandler(this.Option_Changed);
			// 
			// labelImageBase64Style
			// 
			resources.ApplyResources(this.labelImageBase64Style, "labelImageBase64Style");
			this.labelImageBase64Style.Name = "labelImageBase64Style";
			// 
			// label3
			// 
			resources.ApplyResources(this.label3, "label3");
			this.label3.Name = "label3";
			// 
			// radioButtonImageBase64StyleHeader
			// 
			resources.ApplyResources(this.radioButtonImageBase64StyleHeader, "radioButtonImageBase64StyleHeader");
			this.radioButtonImageBase64StyleHeader.Name = "radioButtonImageBase64StyleHeader";
			this.radioButtonImageBase64StyleHeader.UseVisualStyleBackColor = true;
			this.radioButtonImageBase64StyleHeader.CheckedChanged += new System.EventHandler(this.radioButtonsImageStyle_CheckedChanged);
			// 
			// radioButtonImageBase64StyleStyle
			// 
			resources.ApplyResources(this.radioButtonImageBase64StyleStyle, "radioButtonImageBase64StyleStyle");
			this.radioButtonImageBase64StyleStyle.Name = "radioButtonImageBase64StyleStyle";
			this.radioButtonImageBase64StyleStyle.UseVisualStyleBackColor = true;
			this.radioButtonImageBase64StyleStyle.CheckedChanged += new System.EventHandler(this.radioButtonsImageStyle_CheckedChanged);
			// 
			// radioButtonImageBase64StyleDIY
			// 
			resources.ApplyResources(this.radioButtonImageBase64StyleDIY, "radioButtonImageBase64StyleDIY");
			this.radioButtonImageBase64StyleDIY.Name = "radioButtonImageBase64StyleDIY";
			this.radioButtonImageBase64StyleDIY.UseVisualStyleBackColor = true;
			this.radioButtonImageBase64StyleDIY.CheckedChanged += new System.EventHandler(this.radioButtonsImageStyle_CheckedChanged);
			// 
			// checkBoxRealtimeConvert
			// 
			resources.ApplyResources(this.checkBoxRealtimeConvert, "checkBoxRealtimeConvert");
			this.checkBoxRealtimeConvert.Checked = true;
			this.checkBoxRealtimeConvert.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkBoxRealtimeConvert.Name = "checkBoxRealtimeConvert";
			this.checkBoxRealtimeConvert.UseVisualStyleBackColor = true;
			this.checkBoxRealtimeConvert.CheckedChanged += new System.EventHandler(this.Option_Changed);
			// 
			// checkBoxAutoCopy
			// 
			resources.ApplyResources(this.checkBoxAutoCopy, "checkBoxAutoCopy");
			this.checkBoxAutoCopy.Checked = true;
			this.checkBoxAutoCopy.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkBoxAutoCopy.Name = "checkBoxAutoCopy";
			this.checkBoxAutoCopy.UseVisualStyleBackColor = true;
			this.checkBoxAutoCopy.CheckedChanged += new System.EventHandler(this.Option_Changed);
			// 
			// textBoxAutoConvertThreshold
			// 
			resources.ApplyResources(this.textBoxAutoConvertThreshold, "textBoxAutoConvertThreshold");
			this.textBoxAutoConvertThreshold.Name = "textBoxAutoConvertThreshold";
			this.textBoxAutoConvertThreshold.TextChanged += new System.EventHandler(this.textBoxAutoConvertThreshold_TextChanged);
			// 
			// label1
			// 
			resources.ApplyResources(this.label1, "label1");
			this.label1.Name = "label1";
			// 
			// comboBoxLanguageChoice
			// 
			this.comboBoxLanguageChoice.FormattingEnabled = true;
			this.comboBoxLanguageChoice.Items.AddRange(new object[] {
            resources.GetString("comboBoxLanguageChoice.Items"),
            resources.GetString("comboBoxLanguageChoice.Items1")});
			resources.ApplyResources(this.comboBoxLanguageChoice, "comboBoxLanguageChoice");
			this.comboBoxLanguageChoice.MaximumSize = new System.Drawing.Size(75, 0);
			this.comboBoxLanguageChoice.Name = "comboBoxLanguageChoice";
			this.comboBoxLanguageChoice.TextChanged += new System.EventHandler(this.Option_Changed);
			// 
			// textBoxTextBoxFont
			// 
			resources.ApplyResources(this.textBoxTextBoxFont, "textBoxTextBoxFont");
			this.textBoxTextBoxFont.MaximumSize = new System.Drawing.Size(169, 21);
			this.textBoxTextBoxFont.Name = "textBoxTextBoxFont";
			this.textBoxTextBoxFont.Tag = new System.Drawing.Font("Georgia", 9F);
			this.textBoxTextBoxFont.Click += new System.EventHandler(this.fontChoiceBox_Click);
			this.textBoxTextBoxFont.TextChanged += new System.EventHandler(this.Option_Changed);
			this.textBoxTextBoxFont.KeyDown += new System.Windows.Forms.KeyEventHandler(this.fontChoiceBox_KeyDown);
			// 
			// textBoxBase64BoxFont
			// 
			resources.ApplyResources(this.textBoxBase64BoxFont, "textBoxBase64BoxFont");
			this.textBoxBase64BoxFont.MaximumSize = new System.Drawing.Size(169, 21);
			this.textBoxBase64BoxFont.Name = "textBoxBase64BoxFont";
			this.textBoxBase64BoxFont.Tag = new System.Drawing.Font("Georgia", 9F);
			this.textBoxBase64BoxFont.Click += new System.EventHandler(this.fontChoiceBox_Click);
			this.textBoxBase64BoxFont.TextChanged += new System.EventHandler(this.Option_Changed);
			this.textBoxBase64BoxFont.KeyDown += new System.Windows.Forms.KeyEventHandler(this.fontChoiceBox_KeyDown);
			// 
			// labelTextBoxFont
			// 
			resources.ApplyResources(this.labelTextBoxFont, "labelTextBoxFont");
			this.labelTextBoxFont.Name = "labelTextBoxFont";
			// 
			// labelBase64BoxFont
			// 
			resources.ApplyResources(this.labelBase64BoxFont, "labelBase64BoxFont");
			this.labelBase64BoxFont.Name = "labelBase64BoxFont";
			// 
			// advancedPage
			// 
			this.advancedPage.BackColor = System.Drawing.SystemColors.Control;
			this.advancedPage.Controls.Add(this.labelEasterEgg);
			resources.ApplyResources(this.advancedPage, "advancedPage");
			this.advancedPage.Name = "advancedPage";
			// 
			// labelEasterEgg
			// 
			resources.ApplyResources(this.labelEasterEgg, "labelEasterEgg");
			this.labelEasterEgg.Name = "labelEasterEgg";
			// 
			// fontDialog1
			// 
			this.fontDialog1.AllowVerticalFonts = false;
			this.fontDialog1.ShowEffects = false;
			// 
			// resetAllButton
			// 
			resources.ApplyResources(this.resetAllButton, "resetAllButton");
			this.resetAllButton.Name = "resetAllButton";
			this.resetAllButton.UseVisualStyleBackColor = true;
			this.resetAllButton.Click += new System.EventHandler(this.resetAllButton_Click);
			// 
			// applyButton
			// 
			resources.ApplyResources(this.applyButton, "applyButton");
			this.applyButton.Name = "applyButton";
			this.applyButton.UseVisualStyleBackColor = true;
			this.applyButton.Click += new System.EventHandler(this.applyButton_Click);
			// 
			// Option
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.applyButton);
			this.Controls.Add(this.optionTabs);
			this.Controls.Add(this.resetAllButton);
			this.Controls.Add(this.saveButton);
			this.Controls.Add(this.cancelButton);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "Option";
			this.ShowInTaskbar = false;
			this.Load += new System.EventHandler(this.Option_Load);
			this.optionTabs.ResumeLayout(false);
			this.generalPage.ResumeLayout(false);
			this.generalPage.PerformLayout();
			this.imageStyleGroupBox.ResumeLayout(false);
			this.imageStyleGroupBox.PerformLayout();
			this.advancedPage.ResumeLayout(false);
			this.advancedPage.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button cancelButton;
		private System.Windows.Forms.Button saveButton;
		private System.Windows.Forms.TabControl optionTabs;
		private System.Windows.Forms.TabPage generalPage;
		private System.Windows.Forms.TabPage advancedPage;
		private System.Windows.Forms.FontDialog fontDialog1;
		private YeaBase64.FontTextBox textBoxTextBoxFont;
		private YeaBase64.FontTextBox textBoxBase64BoxFont;
		private System.Windows.Forms.Label labelTextBoxFont;
		private System.Windows.Forms.Label labelBase64BoxFont;
		private System.Windows.Forms.Label labelEasterEgg;
		private System.Windows.Forms.Button resetAllButton;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox comboBoxLanguageChoice;
		private System.Windows.Forms.RadioButton radioButtonImageBase64StyleStyle;
		private System.Windows.Forms.RadioButton radioButtonImageBase64StyleHeader;
		private System.Windows.Forms.RadioButton radioButtonImageBase64StyleNone;
		private System.Windows.Forms.Label labelImageBase64Style;
		private System.Windows.Forms.CheckBox checkBoxAutoCopy;
		private System.Windows.Forms.TextBox textBoxAutoConvertThreshold;
		private System.Windows.Forms.CheckBox checkBoxRealtimeConvert;
		private System.Windows.Forms.RadioButton radioButtonImageBase64StyleDIY;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox textBoxStyleAfter;
		private System.Windows.Forms.TextBox textBoxStyleBefore;
		private System.Windows.Forms.Button applyButton;
		private System.Windows.Forms.GroupBox imageStyleGroupBox;
		private System.Windows.Forms.TextBox extMonitorTextBox;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.CheckBox imageAutoDetectCheckBox;
	}
}