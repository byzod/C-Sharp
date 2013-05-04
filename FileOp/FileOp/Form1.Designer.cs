namespace FileOp
{
	partial class MoveMethodChose
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MoveMethodChose));
			this.Check_All = new System.Windows.Forms.CheckBox();
			this.btn_OverR = new System.Windows.Forms.Button();
			this.btn_Cancel = new System.Windows.Forms.Button();
			this.btn_Ren = new System.Windows.Forms.Button();
			this.label_disp = new System.Windows.Forms.Label();
			this.panel1 = new System.Windows.Forms.Panel();
			this.panel2 = new System.Windows.Forms.Panel();
			this.panel1.SuspendLayout();
			this.panel2.SuspendLayout();
			this.SuspendLayout();
			// 
			// Check_All
			// 
			resources.ApplyResources(this.Check_All, "Check_All");
			this.Check_All.Name = "Check_All";
			this.Check_All.UseVisualStyleBackColor = true;
			// 
			// btn_OverR
			// 
			resources.ApplyResources(this.btn_OverR, "btn_OverR");
			this.btn_OverR.Name = "btn_OverR";
			this.btn_OverR.UseVisualStyleBackColor = true;
			this.btn_OverR.Click += new System.EventHandler(this.btn_OverR_Click);
			// 
			// btn_Cancel
			// 
			resources.ApplyResources(this.btn_Cancel, "btn_Cancel");
			this.btn_Cancel.Name = "btn_Cancel";
			this.btn_Cancel.UseVisualStyleBackColor = true;
			this.btn_Cancel.Click += new System.EventHandler(this.btn_Cancel_Click);
			// 
			// btn_Ren
			// 
			resources.ApplyResources(this.btn_Ren, "btn_Ren");
			this.btn_Ren.Name = "btn_Ren";
			this.btn_Ren.UseVisualStyleBackColor = true;
			this.btn_Ren.Click += new System.EventHandler(this.btn_Ren_Click);
			// 
			// label_disp
			// 
			resources.ApplyResources(this.label_disp, "label_disp");
			this.label_disp.MaximumSize = new System.Drawing.Size(235, 0);
			this.label_disp.Name = "label_disp";
			// 
			// panel1
			// 
			resources.ApplyResources(this.panel1, "panel1");
			this.panel1.Controls.Add(this.label_disp);
			this.panel1.Name = "panel1";
			// 
			// panel2
			// 
			this.panel2.Controls.Add(this.btn_Cancel);
			this.panel2.Controls.Add(this.Check_All);
			this.panel2.Controls.Add(this.btn_OverR);
			this.panel2.Controls.Add(this.btn_Ren);
			resources.ApplyResources(this.panel2, "panel2");
			this.panel2.Name = "panel2";
			// 
			// MoveMethodChose
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ControlBox = false;
			this.Controls.Add(this.panel2);
			this.Controls.Add(this.panel1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Name = "MoveMethodChose";
			this.TopMost = true;
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.panel2.ResumeLayout(false);
			this.panel2.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.CheckBox Check_All;
		private System.Windows.Forms.Button btn_OverR;
		private System.Windows.Forms.Button btn_Cancel;
		private System.Windows.Forms.Button btn_Ren;
		private System.Windows.Forms.Label label_disp;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Panel panel2;

	}
}