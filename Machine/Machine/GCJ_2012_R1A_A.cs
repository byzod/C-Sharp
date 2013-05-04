using System;
using System.Collections.Generic;
using System.Linq;

namespace Machine
{
	class GCJ_2012_R1A_A : IProblemSolver
	{
		#region Properties

		/// <summary>
		/// Should write file in binary mode
		/// </summary>
		public string ID
		{
			get
			{
				return "gcj.2012.r1a.a";
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
		/// Password Solver
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
			List<PasswordTask> pwTasks = this.GetProbList(option.Input.Content[0]);

			// Result
			List<double> expectTries = new List<double>(pwTasks.Count);

			// Solver
			foreach (var task in pwTasks)
			{
				// Prob partial multiple cache, probMul[k] means at least k character is right
				List<double> probMul = new List<double>(task.A + 1);
				// E cache, A times hit backspace, 1 time for enter directly
				List<double> E = new List<double>(task.A + 2);

				// probMul[0] = 1
				probMul.Add(1.0);

				// probMul[k] = p[0]*...p[k-1]
				for (int i = 1; i < task.A + 1; i++)
				{
					if (i == 1)
					{
						// probMul[0] = P[0]
						probMul.Add(task.ProbabilitiesList[0]);
					}
					else 
					{
						// probMul[i] = P[i-1]*...*P[0]
						probMul.Add(task.ProbabilitiesList[i - 1] * probMul[i - 1]);
					}
				}

				// A's backspace
				for (int i = 0; i < task.A + 1; i++)
				{
					E.Add(
						((task.B - task.A + 1 + 2 * i) * probMul[task.A - i])
						+ (2 * task.B - task.A + 2 + 2 * i) * (1 - probMul[task.A - i])
					);
				}

				// 1 enter case
				E.Add(task.B + 2);

				expectTries.Add(E.Min());
			}

			// Output
			option.Output.Content.Add(new List<string>(expectTries.Count));
			for (int i = 0; i < expectTries.Count; i++)
			{
				option.Output.Content[0].Add(
					"Case #" + (i + 1) + ": " + expectTries[i]
				);
			}
		}

		/// <summary>
		/// Get task list
		/// </summary>
		/// <param name="data">String list</param>
		/// <returns>Task list</returns>
		List<PasswordTask> GetProbList(List<string> data)
		{
			List<List<string>> stringList = ProblemSolverHelper.ConvertToDataLists<string>(data);
			List<PasswordTask> passwordTaskList = new List<PasswordTask>(Convert.ToInt32(data[0]));

			for (int i = 1; i < data.Count; i++)
			{
				PasswordTask pwTask = new PasswordTask();
				// A and B
				pwTask.A = Convert.ToInt32(stringList[i][0]);
				pwTask.B = Convert.ToInt32(stringList[i][1]);

				// Probabilities
				pwTask.ProbabilitiesList = new List<Double>(stringList[i + 1].Count);
				for (int j = 0; j < pwTask.ProbabilitiesList.Capacity; j++)
				{
					pwTask.ProbabilitiesList.Add(Convert.ToDouble(stringList[i + 1][j]));
				}

				passwordTaskList.Add(pwTask);
				i++;
			}

			return passwordTaskList;
		}

		class PasswordTask
		{
			public int A { get; set; }
			public int B { get; set; }
			public List<Double> ProbabilitiesList { get; set; }
		}
	}
}
