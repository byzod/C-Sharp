using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Machine
{
	class GCJ_2013_R1B_A : IProblemSolver
	{

		#region Properties

		/// <summary>
		/// Should write file in binary mode
		/// </summary>
		public string ID
		{
			get
			{
				return "GCJ.2013.R1B.A";
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
			List<Task> tasks =this.GetTask(option.Input.Content[0]);

			// Result
			List<int> results = new List<int>(tasks.Count);

			// Solver
			foreach (var task in tasks)
			{
				int addOp = 0;
				int minOp = 0;
				int size = task.StartSize;

				task.Motes.Sort();

				int maxSize = task.Motes.Last();

				minOp = task.Motes.Count;

				if (task.StartSize > 1)
				{
					for (int i = 0; i < task.Motes.Count; i++)
					{
						if (size <= task.Motes[i])
						{
							while (size <= task.Motes[i])
							{
								size += size - 1;
								addOp++;
							}
						}

						size += task.Motes[i];

						minOp = Math.Min(minOp, (task.Motes.Count - i - 1) + addOp);
					}
				}

				results.Add(minOp);
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
				string[] nums = data[2 * i + 1].Split(' ');

				Task task = new Task(int.Parse(nums[0]), int.Parse(nums[1]));

				string[] motes = data[2 * i + 2].Split(' ');

				for (int j = 0; j < motes.Length; j++)
				{
					task.Motes.Add(int.Parse(motes[j]));
				}

				tasks.Add(task);
			}

			return tasks;
		}

		class Task
		{
			public int StartSize { get; set; }
			public List<int> Motes { get; set; }

			public Task(int startSize, int motesCount)
			{
				this.StartSize = startSize;
				this.Motes = new List<int>(motesCount);
			}
		}
	}
}
