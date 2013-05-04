using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FileOp
{
	class Class1
	{
		/*移动文件和列表，并处理重名，权限不够等问题
			foreach (FileInfo srcFile in srcFiles)
			{
				if (File.Exists(desPathstr + "\\" + srcFile.Name))
				{
					int IndexNum = 0;
					//如果有重名，则加上_n后缀。n从1开始直到不重名为止。n不大于1000，避免死循环
					for (int i = 0; i < 1000; i++)
					{
						if (!File.Exists(desPathstr + "\\" + Path.GetFileNameWithoutExtension(srcFile.FullName) + @"_" + i.ToString() + srcFile.Extension))
						{
							IndexNum = i;
							break;
						}
					}
					srcFile.MoveTo(desPathstr + "\\" + Path.GetFileNameWithoutExtension(srcFile.FullName) + @"_" + IndexNum.ToString() + srcFile.Extension); //重命名
				}
				else srcFile.MoveTo(desPathstr + "\\" + srcFile.Name); //不覆盖
			}


			foreach (DirectoryInfo srcDirc in srcDircs)
			{
				if (Directory.Exists(desPathstr + "\\" + srcDirc.Name))
				{
					int IndexNum = 0;
					//如果有重名，则加上_n后缀。n从1开始直到不重名为止。n不大于1000，避免死循环
					for (int i = 0; i < 1000; i++)
					{
						if (!Directory.Exists(desPathstr + "\\" + srcDirc.Name + @"_" + i.ToString()))
						{
							IndexNum = i; 
							break;
						}
					}
					srcDirc.MoveTo(desPathstr + "\\" + srcDirc.Name + @"_" + IndexNum.ToString()); //重命名
				}else srcDirc.MoveTo(desPathstr + "\\" + srcDirc.Name); //不覆盖
			}*/
	}
}
