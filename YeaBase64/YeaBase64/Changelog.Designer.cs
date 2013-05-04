namespace YeaBase64
{
	partial class Changelog
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Changelog));
			this.changelogRichTextBox = new System.Windows.Forms.RichTextBox();
			this.button1 = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// changelogRichTextBox
			// 
			resources.ApplyResources(this.changelogRichTextBox, "changelogRichTextBox");
			this.changelogRichTextBox.Name = "changelogRichTextBox";
			this.changelogRichTextBox.ReadOnly = true;
			this.changelogRichTextBox.TextChanged += new System.EventHandler(this.changelogRichTextBox_TextChanged);
			// 
			// button1
			// 
			resources.ApplyResources(this.button1, "button1");
			this.button1.Name = "button1";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// Changelog
			// 
			this.AcceptButton = this.button1;
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.button1);
			this.Controls.Add(this.changelogRichTextBox);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "Changelog";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.RichTextBox changelogRichTextBox;
		private System.Windows.Forms.Button button1;
	}
}