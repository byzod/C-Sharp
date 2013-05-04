using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Machine
{
	class GCJ_2012_WF_A : IProblemSolver
	{
		#region Properties

		/// <summary>
		/// Should write file in binary mode
		/// </summary>
		public string ID
		{
			get
			{
				return "gcj.2012.wf.a";
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
		/// Zombie smasher
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
			List<List<List<Int32>>> zombieList =
				this.GetZombieList(option.Input.Content[0]);

			// Sort by zombie appear time
			foreach (var zombies in zombieList)
			{
				zombies.Sort((listX, listY) => {
					return (listX[2] == listY[2])
							?
							(Math.Abs(listX[0]) + Math.Abs(listX[1]) - Math.Abs(listY[0]) - Math.Abs(listY[1]))
							:
							listX[2] - listY[2];
				});
			}

			// Solve
			List<Int32> smashCount = new List<int>(zombieList.Count);
			for (int i = 0; i < zombieList.Count; i++)
			{
				Int32 t = 0;
				Int32 x = 0;
				Int32 y = 0;
				Int32 count = 0;

				for (int j = 0; j < zombieList[i].Count; j++)
				{
					Int32 td = GetMoveTime(x, y, zombieList[i][j][0], zombieList[i][j][1]);
					td =
						(td > 750) || (j == 0)
						?
						td
						:
						750;

					if (t + td < zombieList[i][j][2] + 1000)
					{
						t = t + td;
						x = zombieList[i][j][0];
						y = zombieList[i][j][1];
						count += 1;
					}
				}

				smashCount.Add(count);
			}

			// Parse result to string lists
			option.Output.Content.Add(new List<string>(smashCount.Count));
			for (int i = 0; i < smashCount.Count; i++)
			{
				option.Output.Content[0].Add(
					"Case #" + (i + 1) + ": " + smashCount[i]
				);
			}
		}

		private Int32 GetMoveTime(Int32 x1, Int32 y1, Int32 x2, Int32 y2)
		{
			Int32 distance = Math.Max(Math.Abs(y2 - y1), Math.Abs(x2 - x1));
			return distance * 100;
		}

		private List<List<List<Int32>>> GetZombieList(List<string> data)
		{
			List<List<List<Int32>>> zombieList = new List<List<List<Int32>>>(Convert.ToInt32(data[0]));

			string zombiePattern = "([\\d\\-]+) ([\\d\\-]+) (\\d+)";
			for (int i = 1; i < data.Count; i++)
			{
				Int32 zombieCount = Convert.ToInt32(data[i]);
				i += 1;

				List<List<Int32>> zombies = new List<List<Int32>>(zombieCount);

				for (int j = 0; j < zombieCount; j++)
				{
					Match match = Regex.Match(data[i+j], zombiePattern);
					if (match.Success)
					{
						zombies.Add(
										new List<Int32>()
										{
											Convert.ToInt32(match.Groups[1].Value),
											Convert.ToInt32(match.Groups[2].Value),
											Convert.ToInt32(match.Groups[3].Value)
										}
									);
					}
					else
					{
						throw new ArgumentException("Invalid string: " + data[i]);
					}
				}

				zombieList.Add(zombies);

				i += zombieCount - 1;
			}

			return zombieList;
		}
	}
}
