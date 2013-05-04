using System;
using System.Windows.Forms;

namespace YeaBase64
{
	public partial class CodePageHelp : Form
	{
		public delegate void openHelpFormHandler(object sender, EventArgs e);
		public event openHelpFormHandler OpenHelpFormHandler;
		public CodePageHelp()
		{
			InitializeComponent();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			this.linkLabel1.LinkVisited = true;
			System.Diagnostics.Process.Start("http://msdn.microsoft.com/library/system.text.encoding.aspx");
		}

		private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{	
			this.linkLabel1.LinkVisited = true;
			OpenHelpFormHandler(sender, e);
			this.Close();
		}
	}
}
