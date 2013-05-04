using System;
using System.Collections.Generic;
using System.Linq;

namespace Machine
{
	class GCJ_2012_R1B_A : IProblemSolver
	{

		#region Properties

		/// <summary>
		/// Should write file in binary mode
		/// </summary>
		public string ID
		{
			get
			{
				return "gcj.2012.r1b.a";
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
		/// Problem solve method
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
			List<List<int>> tasks = this.GetShows(option.Input.Content[0]);

			// Result
			List<List<double>> results = new List<List<double>>(tasks.Count);

			// Solver
			foreach (var task in tasks)
			{
				double scoreSum = task.Sum();
				double safeScore = 0;
				List<double> minPercentage = new List<double>(task.Count);

				safeScore = scoreSum * 2.0 / (double)task.Count;

				int safeCount = 0;
				double overSafeSum = 0;
				for (int i = 0; i < task.Count; i++)
				{
					if (task[i] > safeScore)
					{
						safeCount++;
						overSafeSum += task[i] - safeScore;
					}
				}

				safeScore -= overSafeSum / (task.Count - safeCount);

				for (int i = 0; i < task.Count; i++)
				{
					minPercentage.Add(
						task[i] > safeScore ?
						0
						: ((safeScore - task[i]) / scoreSum) * 100
					);
				}

				results.Add(minPercentage);
			}

			// Output
			option.Output.Content.Add(new List<string>(results.Count));
			for (int i = 0; i < results.Count; i++)
			{
				option.Output.Content[0].Add(
					"Case #" + (i + 1) + ": "
					+ string.Join(
						" ",
						(from min in results[i]
						 select min.ToString())
						 .ToArray()
					)
				);
			}
		}

		public List<List<int>> GetShows(List<string> data)
		{
			int count = int.Parse(data[0]);
			List<List<int>> shows = new List<List<int>>(count);

			for (int i = 0; i < count; i++)
			{
				string[] scores = data[i + 1].Split(' ');
				int contestantsCount = int.Parse(scores[0]);
				List<int> contestants = new List<int>(contestantsCount);
				for (int j = 0; j < contestantsCount; j++)
				{
					contestants.Add(int.Parse(scores[j + 1]));
				}
				shows.Add(contestants);
			}

			return shows;
		}
	}
}
