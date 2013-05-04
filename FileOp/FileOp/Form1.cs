using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FileOp
{
	public partial class MoveMethodChose : Form
	{
		public static bool IsFile;
		public MoveMethodChose(string e,bool isfile)
		{
			InitializeComponent();
			IsFile = isfile;
			if (IsFile == true)
			{
				label_disp.Text = "文件 " + e + " 已存在，是否要覆盖？";
			}
			else
			{
				label_disp.Text = "文件夹 " + e + " 已存在，是否要合并？";
				btn_OverR.Text = "合并";
			}
		}
		public ObjMove.MoveMethod movemethod{get;set;}


		private void btn_OverR_Click(object sender, EventArgs e)
		{
			if (Check_All.Checked)
			{
				if (IsFile) ObjMove.AllOverR = true;
				else ObjMove.AllOverR_D = true;
			}
			movemethod = ObjMove.MoveMethod.OverR;
			this.Close();
		}

		private void btn_Cancel_Click(object sender, EventArgs e)
		{
			if (Check_All.Checked)
			{
				if (IsFile) ObjMove.AllSkip = true;
				else ObjMove.AllSkip_D = true;
			}
			movemethod = ObjMove.MoveMethod.None;
			this.Close();
		}

		private void btn_Ren_Click(object sender, EventArgs e)
		{
			if (Check_All.Checked)
			{
				if (IsFile) ObjMove.AllRename = true;
				else ObjMove.AllRename_D = true;
			}
			movemethod = ObjMove.MoveMethod.Ren;
			this.Close();
		}
	}
}
