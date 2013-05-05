using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Machine
{
	class GCJ_2013_R1A_B : IProblemSolver
	{

		#region Properties

		/// <summary>
		/// Should write file in binary mode
		/// </summary>
		public string ID
		{
			get
			{
				return "GCJ.2013.R1A.B";
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
			List<Task> tasks = this.GetTasks(option.Input.Content[0]);

			// Result
			List<Int64> results = new List<Int64>(tasks.Count);

			// Solver
			foreach (var task in tasks)
			{
				List<int> spanArray = this.GetSpanArray(task.Vs);
				int recoverSteps = task.E % task.R == 0 
					? task.E / task.R 
					: (task.E / task.R) + 1;
				Int64 maxJoy = 0;
				Int64 energy = task.E;

				for (int i = 0; i < task.Vs.Count; i++)
				{
					if (spanArray[i] >= recoverSteps || spanArray[i] == 0)
					{
						// use all
						maxJoy += energy * task.Vs[i];
						energy = 0;
					}
					else
					{
						// use as much
						maxJoy += (energy + spanArray[i] * task.R - task.E) * task.Vs[i];
						energy = task.E - spanArray[i] * task.R;
					}

					// Recover
					energy = energy + task.R > task.E
						? task.E
						: energy + task.R;
				}

				results.Add(maxJoy);
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

		List<int> GetSpanArray(List<int> arr)
		{
			int[] spanArr = new int[arr.Count];
			Stack<int> stack = new Stack<int>();

			for (int i = arr.Count - 1; i >= 0; i--)
			{
				while (stack.Count > 0 )
				{
					if (arr[i] >= arr[stack.Peek()])
					{
						stack.Pop();
					}
					else
					{
						break;
					}
				}

				int h = 0;
				if (stack.Count == 0)
				{
					h = i;
				}
				else
				{
					h = stack.Peek();
				}

				spanArr[i] = h - i;
				stack.Push(i);
			}

			return spanArr.ToList() ;
		}

		List<Task> GetTasks(List<string> data)
		{
			int count = int.Parse(data[0]);
			List<Task> tasks = new List<Task>(count);

			for (int i = 0; i < count; i++)
			{
				string[] ern = data[2 * i + 1].Split(' ');
				string[] vs  = data[2 * i + 2].Split(' ');

				Task task = new Task(
					int.Parse(ern[0]),
					int.Parse(ern[1]),
					int.Parse(ern[2])
				);

				task.Vs = (from num in vs
						   select int.Parse(num))
						  .ToList();

				tasks.Add(task);
			}

			return tasks;

		}

		class Task
		{
			public int E { get; set; }
			public int R { get; set; }
			public int N { get; set; }
			public List<int> Vs { get; set; }

			public Task(int e, int r, int n)
			{
				this.E = e;
				this.R = r;
				this.N = n;
			}
		}
	}
}
