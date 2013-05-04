using System;
//using System.Collections.Generic;
//using System.Linq;
using System.Windows.Forms;
using System.IO;
using Microsoft.Win32;

namespace FileOp
{
	/* 包含“移动所有文件移动到父目录”功能所需的变量，方法
	 */

	 public class ObjMove
	{
		//移动参数
		public static bool AllOverR = false, AllRename = false, AllSkip = false,
			 AllOverR_D = false, AllRename_D = false, AllSkip_D = false;
		public enum MoveMethod {OverR,Ren,Dirc,None};

		//静态方法。移动文件微操作。
		public static void Mov(FileInfo srcPath, string desPath, MoveMethod movemethod)
		{
			int IndexNum = 0;
			switch(movemethod)
			{
				case MoveMethod.Dirc: //直接移动。不做任何异常处理
					srcPath.MoveTo(desPath + "\\" + Path.GetFileNameWithoutExtension(srcPath.FullName) + srcPath.Extension);
					break;
				case MoveMethod.OverR://覆盖（先删除后移动），如果无权删除，则重命名
					try
					{
						File.Delete(desPath + "\\" + Path.GetFileNameWithoutExtension(srcPath.FullName) + srcPath.Extension);
						srcPath.MoveTo(desPath + "\\" + Path.GetFileNameWithoutExtension(srcPath.FullName) + srcPath.Extension);
					}
					catch(UnauthorizedAccessException)
					{
						//如果有重名，则加上_n后缀。n从1开始直到不重名为止。n不大于100000，避免死循环
						for (int i = 0; i < 100000; i++)
						{
							if (!File.Exists(desPath + "\\" + Path.GetFileNameWithoutExtension(srcPath.FullName) + @"_" + i.ToString() + srcPath.Extension))
							{
								IndexNum = i;
								break;
							}
						}
						srcPath.MoveTo(desPath + "\\" + Path.GetFileNameWithoutExtension(srcPath.FullName) + @"_" + IndexNum.ToString() + srcPath.Extension); 
					} 
					break;
				case MoveMethod.Ren://重命名后移动
					//如果有重名，则加上_n后缀。n从1开始直到不重名为止。n不大于100000，避免死循环
					for (int i = 0; i < 100000; i++)
					{
						if (!File.Exists(desPath + "\\" + Path.GetFileNameWithoutExtension(srcPath.FullName) + @"_" + i.ToString() + srcPath.Extension))
						{
							IndexNum = i;
							break;
						}
					}
					srcPath.MoveTo(desPath + "\\" + Path.GetFileNameWithoutExtension(srcPath.FullName) + @"_" + IndexNum.ToString() + srcPath.Extension);
					break;
				case MoveMethod.None://跳过
					break;
			}			
		}		
		//静态方法。移动目录微操作。重载
		public static void Mov(DirectoryInfo srcPath, string desPath, MoveMethod movemethod)
		{
			int IndexNum = 0;
			switch (movemethod)
			{
				case MoveMethod.Dirc: //直接移动。不做任何异常处理
					srcPath.MoveTo(desPath + "\\" + srcPath.Name);
					break;
				case MoveMethod.OverR://覆盖/合并（递归处理）当目标目录为空或者到没有重名时停止					
					MoveFile(srcPath, desPath + "\\" + srcPath.Name);
					MoveDirc(srcPath, desPath + "\\" + srcPath.Name);
					try
					{
						srcPath.Delete();
					}
					catch
					{
						//MessageBox.Show("无法删除"+srcPath.ToString()+"\n目录不是空的");
					}
					break;
				case MoveMethod.Ren://重命名后移动
					//如果有重名，则加上_n后缀。n从1开始直到不重名为止。n不大于100000，避免死循环
					for (int i = 0; i < 100000; i++)
					{
						if (!Directory.Exists(desPath + "\\" + srcPath.Name + @"_" + i.ToString()))
						{
							IndexNum = i;
							break;
						}
					}
					srcPath.MoveTo(desPath + "\\" + srcPath.Name + @"_" + IndexNum.ToString());
					break;
				case MoveMethod.None://跳过
					break;
			}
		}
		 		
		//静态方法。文件移动操作
		public static void MoveFile(DirectoryInfo srcPath, string desPath)
		{
			//获取文件列表
			FileInfo[] srcFiles = srcPath.GetFiles();

			foreach (FileInfo srcFile in srcFiles)
			{
				if (!File.Exists(desPath + "\\" + Path.GetFileNameWithoutExtension(srcFile.FullName) + srcFile.Extension))
				{
					//无冲突则直接移动
					Mov(srcFile, desPath, MoveMethod.Dirc);
				}
				else
				{
					if (AllRename == true) Mov(srcFile, desPath, MoveMethod.Ren);
					else if (AllOverR == true) Mov(srcFile, desPath, MoveMethod.OverR);
					else if (AllSkip == true) ;
					//弹出询问对话框
					else
					{
						MoveMethodChose alertbox = new MoveMethodChose(srcFile.Name, true);
						alertbox.ShowDialog();
						switch (alertbox.movemethod)
						{
							case MoveMethod.OverR:
								Mov(srcFile, desPath, MoveMethod.OverR);
								break;
							case MoveMethod.Ren:
								Mov(srcFile, desPath, MoveMethod.Ren);
								break;
							case MoveMethod.None:
								break;
						}
					}
				}
			}
		}
		//静态方法。目录移动操作
		public static void MoveDirc(DirectoryInfo srcPath, string desPath)
		{			
			//获取目录列表
			DirectoryInfo[] srcDircs = srcPath.GetDirectories();

			foreach (DirectoryInfo srcDirc in srcDircs)
			{
				string debug = desPath + "\\" + srcDirc.Name;
				if (!Directory.Exists(desPath + "\\" + srcDirc.Name))
				{
					//无冲突则直接移动
					Mov(srcDirc, desPath, MoveMethod.Dirc);
				}
				else
				{
					if (AllRename_D == true) Mov(srcDirc, desPath, MoveMethod.Ren);
					//合并
					else if (AllOverR_D == true) Mov(srcDirc, desPath, MoveMethod.OverR);
					else if (AllSkip_D == true) ;
					//弹出询问对话框
					else
					{
						MoveMethodChose alertbox = new MoveMethodChose(srcDirc.Name, false);
						alertbox.ShowDialog();
						switch (alertbox.movemethod)
						{
							case MoveMethod.OverR:
								Mov(srcDirc, desPath, MoveMethod.OverR);
								break;
							case MoveMethod.Ren:
								Mov(srcDirc, desPath, MoveMethod.Ren);
								break;
							case MoveMethod.None:
								break;
						}
					}
				}
			}
		}

		//静态方法。主操作。将srcFile目录下的所有文件和文件夹移动到父目录
		public static void MoveUp(string Pathstr)
		{

			if ( String.Equals(Pathstr,"Byzod",StringComparison.OrdinalIgnoreCase) )
			{
				MessageBox.Show("Byzod is a good guy!");
				return;
			}
			else if (Pathstr == @"/?")
			{
				MessageBox.Show(Register.HELP_EN + "\n\n" + Register.HELP_CN);
				return;
			}

			//获得上级目录，输入的路径不能以\结束
			DirectoryInfo srcPath = new DirectoryInfo(Pathstr);
			if (srcPath.Exists)
			{
				string desPath = Pathstr.Substring(0, Pathstr.LastIndexOf("\\"));
				//移动文件
				MoveFile(srcPath, desPath);
				//移动目录
				MoveDirc(srcPath, desPath);
			}
		}
	}

	class Register//用于注册所包含的功能
	{
		//帮助文档
		public static string HELP_EN = "This application is used to move all the files and directories of the target directory to the current folder.\nA register information should shown after run, a right-click menu 'Extract!' would be added if register succeed.\nTo uninstall, simply run the app again.";
		public static string HELP_CN = "本程序作用为将目标目录中的所有文件移动到当前目录，若有重名则自动命名。\n当运行程序后会提示注册，注册成功后在目录上右键会多出一个右键菜单'Extract!'，单击即可。\n如果想卸载再运行本程序一次即可。";
		//静态方法。检测注册表，如果已经添加右键菜单，不做任何操作 否则就询问是否注册
		public static void Regist(string Version)
		{
			//注册到注册表，添加一个右键菜单

			if (Registry.GetValue(@"HKEY_CLASSES_ROOT\Directory\shell\Extract", "installed", null) == null)
			{
				if (MessageBox.Show(Register.HELP_EN + "\n\n" + Register.HELP_CN + "\n\n" + "Register to the right menu of files and directories?\n注册到文件/目录右键菜单？\n                                                         copyright 2011 by Byzod", "注册 - Extractor " + Version, MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk)
					== DialogResult.No) return;
			}
			else
			{
				DialogResult result = MessageBox.Show("已注册，是否卸载？\n按“取消”更新注册表\n\ncopyright 2011 by Byzod", @"卸载/更新 - Extractor " + Version, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information, MessageBoxDefaultButton.Button3);
				if (result == DialogResult.Yes)
				{
					//从注册表删除
					RegistryKey hkcr = Registry.ClassesRoot;
					hkcr.DeleteSubKeyTree(@"Directory\shell\Extract", false);
					return;
				}
				else if (result == DialogResult.No) return;
			}
			Registry.SetValue(@"HKEY_CLASSES_ROOT\Directory\shell\Extract","",@"Extract!(&X)");
			Registry.SetValue(@"HKEY_CLASSES_ROOT\Directory\shell\Extract", "installed", "1");
			Registry.SetValue(@"HKEY_CLASSES_ROOT\Directory\shell\Extract\command", "", @"""" + Application.ExecutablePath + @""" ""%1""");
			return;
		}
	}

	



	static class Program
	{
		/// <summary>
		/// 应用程序的主入口点。
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(true);
			const string APPVERSION = "1.1.2.20120506";
			
			if (args.Length == 0)
			{	
				//如果双击就注册/更新/卸载
				Register.Regist(APPVERSION);
				//ObjMove.MoveUp(@"Q:\Down\test\1"); //debug
			}
			else
			{
				//若有参数，从右键菜单读入的目录参数，执行移动操作
				ObjMove.MoveUp(args[0]);				
			}
			return;
		}
	}
}
