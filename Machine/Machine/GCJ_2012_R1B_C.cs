using System;
using System.Collections.Generic;
using System.Linq;

namespace Machine
{
	class GCJ_2012_R1B_C : IProblemSolver
	{

		#region Properties

		/// <summary>
		/// Should write file in binary mode
		/// </summary>
		public string ID
		{
			get
			{
				return "gcj.2012.r1b.c";
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
			List<List<int>> tasks = this.GetSets(option.Input.Content[0]);

			// Result
			List<List<int>> results = new List<List<int>>(tasks.Count);

			// Solver
			foreach (var task in tasks)
			{
				List<Limit> sumLimits = new List<Limit>(task.Count);
				for (int i = 1; i < task.Count; i++)
				{
					int max = 0;
					int min = 0;
					for (int j = 0; j < i; j++)
					{
						min += task[j];
						max += task[task.Count - j - 1];
					}
					sumLimits.Add(new Limit(min, max));
				}

				for (int i = 0; i < task.Count / 2; i++)
				{
					// find a sum of array
					int sum = 0;
					for (int j = 0; j < task.Count; j++)
					{
						if (sum > sumLimits[j].Min && sum < sumLimits[j].Max)
						{
							// check all array with length j+1
						}
					}
				}
			}

			// Output
			option.Output.Content.Add(new List<string>(results.Count));
			for (int i = 0; i < results.Count; i++)
			{
				string result = "Case #" + (i + 1) + ": ";

				if (results[i].Count > 0)
				{
					result += "\n";
					result +=
						string.Join(
							" ",
							(from num in results[i]
							 select num.ToString())
							.ToArray()
						);
				}

				if (results[i + 1].Count > 0)
				{
					result += "\n";
					result +=
						string.Join(
							" ",
							(from num in results[i + 1]
							 select num.ToString())
							.ToArray()
						);
				}

				option.Output.Content[0].Add(result);
			}
		}

		public List<List<int>> GetSets(List<string> data)
		{
			int count = int.Parse(data[0]);
			List<List<int>> sets = new List<List<int>>(count);

			for (int i = 0; i < count; i++)
			{
				string[] nums = data[i + 1].Split(' ');
				List<int> set = new List<int>(int.Parse(nums[0]));
				for (int j = 0; j < set.Capacity; j++)
				{
					set.Add(int.Parse(nums[j + 1]));
				}
				sets.Add(set);
			}
			return sets;
		}
		
		class Limit
		{
			public int Max { get; set; }
			public int Min { get; set; }
			public Limit(int min, int max)
			{
				this.Min = min;
				this.Max = max;
			}
		}
	}

}
