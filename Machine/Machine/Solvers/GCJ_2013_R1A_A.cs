using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Machine
{
	class GCJ_2013_R1A_A : IProblemSolver
	{

		#region Properties

		/// <summary>
		/// Should write file in binary mode
		/// </summary>
		public string ID
		{
			get
			{
				return "gcj.2013.r1a.a";
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
			List<Task> tasks = this.GetTask(option.Input.Content[0]);

			// Result
			List<Int64> results = new List<Int64>(tasks.Count);

			// Solver
			int count = 0;
			foreach (var task in tasks)
			{
				count++;

				Int64 low = 1;
				Int64 high = (Int64)Math.Sqrt(task.Ink / 2); // 2*n^2 + (2r-1)n > 2*n^2

				while (low < high)
				{
					Int64 mid = (low + high) / 2;
					Int64 inkNeeded = (2 * mid * mid + (2 * task.Radius - 1) * mid);

					if (inkNeeded > task.Ink || inkNeeded < 0)
					{
						high = mid - 1;
					}
					else
					{
						low = mid + 1;
					}
				}

				results.Add(
					((2 * low * low + (2 * task.Radius - 1) * low) > task.Ink)
					? low - 1
					: low
				);
			}

			// Output
			option.Output.Content.Add(new List<string>(results.Count));
			for (int i = 0; i < results.Count; i++)
			{
				option.Output.Content[0].Add(
					"Case #" + (i + 1) + ": " + results[i]
				);
			}
		}

		List<Task> GetTask(List<string> data)
		{
			int count = int.Parse(data[0]);
			List<Task> tasks = new List<Task>(count);

			for (int i = 0; i < count; i++)
			{
				string[] nums = data[i + 1].Split(' ');

				Task task = new Task();
				task.Radius = Int64.Parse(nums[0]);
				task.Ink = Int64.Parse(nums[1]);

				tasks.Add(task);
			}

			return tasks;
		}

		struct Task
		{
			public Int64 Radius;
			public Int64 Ink;
		}
	}
}
