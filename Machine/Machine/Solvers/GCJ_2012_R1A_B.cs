using System;
using System.Collections.Generic;
using System.Linq;

namespace Machine
{
	class GCJ_2012_R1A_B : IProblemSolver
	{
		#region Properties

		/// <summary>
		/// Should write file in binary mode
		/// </summary>
		public string ID
		{
			get
			{
				return "gcj.2012.r1a.b";
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
		/// Kindom Rush Solver
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

			// Get input
			List<List<List<Int32>>> krTasks = this.GetTask(ProblemSolverHelper.ConvertToDataLists<Int32>(option.Input.Content[0]));

			// Result
			List<Int32> playLevelsCount = new List<int>(krTasks.Count);

			// Store levels and sort them by level 2 hardness
			foreach (var task in krTasks)
			{
				List<List<KRLevel>> SortedLevels = new List<List<KRLevel>>(2002); // Limit
				Int32 hardest = 0;

				// Initialize sorted levels
				for (int i = 0; i < SortedLevels.Capacity; i++)
				{
					// Assume the size of levels of same hard star requirement
					SortedLevels.Add(new List<KRLevel>(200));
				}
				for (int i = 0; i < task.Count; i++)
				{
					// SortedLevels[k] are all levels with hard=k
					SortedLevels[task[i][1]]
						.Add(new KRLevel(task[i][0], task[i][1]));
					hardest = task[i][1] > hardest ? task[i][1] : hardest;
				}
				foreach (var levelWithSameHard in SortedLevels)
				{
					// Decrease sort by level.Easy
					levelWithSameHard.Sort((x, y) => { return y.Easy - x.Easy; });
				}

				// Simulate
				// Stars got
				Int32 stars = 0;
				// Levels beaten
				Int32 levelBeaten = 0;
				// Levels played
				Int32 levelPlayed = 0;
				// All levels beaten
				bool AllStars = false;

				while (true)
				{
					// 0 = none can beat, 1 = beat a easy, 2 = beat a hard
					Int32 beatState = 0;

					// We (can) beat all levels
					if (stars >= hardest || levelBeaten >= task.Count)
					{
						// All left levels can be beaten in one run
						levelPlayed += task.Count - levelBeaten;
						AllStars = true;
						break;
					}

					// Try beat hard
					for (int i = stars; i >= 0; i--)
					{
						if (SortedLevels[i].Count > 0) 
						{
							KRLevel hardestLevel = SortedLevels[i].First();
							stars += 2 - hardestLevel.Progress; // 2 or 1 start got
							levelBeaten++;
							levelPlayed++;
							beatState = 2;
							SortedLevels[i].Remove(hardestLevel);
							break;
						}
					}

					if (beatState < 2)
					{
						// Try beat easy
						for (int i = hardest; i >= 0; i--)
						{
							if (SortedLevels[i].Count > 0)
							{
								for (int j = 0; j < SortedLevels[i].Count; j++)
								{
									KRLevel hardestLevel = SortedLevels[i][j];
									if (stars >= hardestLevel.Easy) // 1 start got
									{
										stars++;
										//levelBeaten++;  No this line cuz' it's not beaten
										levelPlayed++;
										beatState = 1;
										hardestLevel.Progress = 1;
										break;
									}
									else
									{
										continue;
									}
								}
							}

							if (beatState > 0)
							{
								break;
							}
						}
					}

					// You Can (Not) Advance
					if (beatState == 0)
					{
						AllStars = false;
						break;
					}
				}

				if (AllStars)
				{
					playLevelsCount.Add(levelPlayed);
				}
				else
				{
					playLevelsCount.Add(0);
				}
			}

			// Output
			option.Output.Content.Add(new List<string>(playLevelsCount.Count));
			for (int i = 0; i < playLevelsCount.Count; i++)
			{
				option.Output.Content[0].Add(
					"Case #" + (i + 1) 
					+ ": " 
					+ (playLevelsCount[i] == 0 ?
						"Too Bad" :
						playLevelsCount[i].ToString())
				);
			}
		}


		public List<List<List<Int32>>> GetTask(List<List<Int32>> int32Lists)
		{
			List<List<List<Int32>>> taskLists;

			if (int32Lists.Count <= 0 || (int32Lists.Count > 0 && int32Lists[0].Count <= 0))
			{
				taskLists = new List<List<List<Int32>>>(0);
			}
			else
			{
				// Line 0 is tasks count
				taskLists = new List<List<List<Int32>>>(int32Lists[0][0]);
			}

			// Line 0 is tasks count, tasks start from line 1
			for (int i = 1; i < int32Lists.Count; i++)
			{
				List<List<Int32>> dataLists;
				if (int32Lists[i].Count <= 0)
				{
					dataLists = new List<List<int>>(0);
				}
				else
				{
					dataLists = new List<List<int>>(int32Lists[i][0]);
				}

				// Each task chunk consist of a line of task count N
				// And fallowing N lines of task data
				for (int j = 0; j < dataLists.Capacity; j++)
				{
					dataLists.Add(int32Lists[i + j + 1]);
				}

				taskLists.Add(dataLists);

				// Update the position
				i += dataLists.Capacity;
			}

			return taskLists;
		}

		class KRLevel
		{
			public int Easy { get; set; }
			public int Hard { get; set; }
			// 0 = never played, 1=easy passed, 2=beaten
			public int Progress { get; set; }

			public KRLevel(Int32 easy, Int32 hard)
			{
				this.Easy = easy;
				this.Hard = hard;
			}
		}
	}
}
