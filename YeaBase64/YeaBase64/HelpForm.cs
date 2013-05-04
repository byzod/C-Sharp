using System;
using System.Windows.Forms;

namespace YeaBase64
{
	public partial class HelpForm : Form
	{
		public HelpForm()
		{
			InitializeComponent();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			this.linkLabel2.LinkVisited = true;
			System.Diagnostics.Process.Start("http://msdn.microsoft.com/library/system.text.encoding.aspx");
		}

		private void HelpForm_Load(object sender, EventArgs e)
		{
			try
			{
				this.Location = new System.Drawing.Point(this.Owner.Location.X - 50, this.Owner.Location.Y - 50);
			}
			catch { }
		}
	}
}
