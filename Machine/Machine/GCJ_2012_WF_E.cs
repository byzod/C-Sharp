using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Machine
{
	class GCJ_2012_WF_E : IProblemSolver
	{
		#region Properties

		/// <summary>
		/// Should write file in binary mode
		/// </summary>
		public string ID
		{
			get
			{
				return "gcj.2012.wf.e";
			}
		}

		/// <summary>
		/// Maximum files that this solver can handle
		/// </summary>
		public Int32 MaxFiles
		{
			get
			{
				return 1;
			}
		}

		#endregion
		
		/// <summary>
		/// Paths smasher
		/// </summary>
		/// <param name="option">Options</param>
		public void Solve(ref Options option)
		{
			// Pre-process
			option.IsBinaryLoad = false;
			option.IsBinaryWrite = false;
			option.LoadContent();

			// Can We handle that?
			if (option.Input.Content.Count > this.MaxFiles)
			{
				option.Warnings.Add(this.ID + " can only process " + this.MaxFiles + " files a time, the files after the first one will be ignored.");
			}

			// Data parse
			List<List<List<Int32>>> pathList =
				this.GetPathList(option.Input.Content[0]);

			// Solve
			List<Int32> pathsCount = new List<int>(pathList.Count);
			foreach (var paths in pathList)
			{
				bool infinity = false;
				Int32 position = 1;
				string pathFollowed = "";

				while (!infinity)
				{
					if (position > paths.Count) // We're out
					{
						pathsCount.Add(pathFollowed.Length);
						break;
					}
					else
					{
						pathFollowed += position;
						paths[position-1][2] = (paths[position-1][2] + 1) % 2;

						if (Regex.IsMatch(pathFollowed, "((\\d)\\d+)\\1\\2"))
						{
							infinity = true;
							pathsCount.Add(0);
						}
						else
						{
							position =
								paths[position-1][2] == 1
								?
								paths[position-1][0]
								:
								paths[position-1][1];
						}
					}
				}
			}

			// Parse result to string lists
			option.Output.Content.Add(new List<string>(pathsCount.Count));
			for (int i = 0; i < pathsCount.Count; i++)
			{
				option.Output.Content[0].Add(
					"Case #" + (i + 1) + ": " + (pathsCount[i] == 0 ? "Infinity" : pathsCount[i].ToString())
				);
			}
		}

		private List<List<List<Int32>>> GetPathList(List<string> data)
		{
			List<List<List<Int32>>> pathList = new List<List<List<Int32>>>(Convert.ToInt32(data[0]));

			string pathPattern = "(\\d+) (\\d+)";
			for (int i = 1; i < data.Count; i++)
			{
				Int32 pathCount = Convert.ToInt32(data[i]);

				List<List<Int32>> paths = new List<List<Int32>>(pathCount);

				for (int j = 1; j < pathCount; j++)
				{
					Match match = Regex.Match(data[i + j], pathPattern);
					if (match.Success)
					{
						paths.Add(
									new List<Int32>()
									{
										Convert.ToInt32(match.Groups[1].Value),
										Convert.ToInt32(match.Groups[2].Value),
										0 // Status. 0 = none/even visited, 1 = odd visited
									}
								);
					}
					else
					{
						throw new ArgumentException("Invalid string: " + data[i]);
					}
				}

				pathList.Add(paths);

				i += pathCount - 1;
			}

			return pathList;
		}
	}
}
