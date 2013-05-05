using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Machine
{
	class GCJ_2013_R1B_B : IProblemSolver
	{

		#region Properties

		/// <summary>
		/// Should write file in binary mode
		/// </summary>
		public string ID
		{
			get
			{
				return "GCJ.2013.R1B.B";
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
			List<List<List<double>>> tasks = this.GetTask(ProblemSolverHelper.ConvertToDataLists<double>(option.Input.Content[0]));

			// Result
			List<double> results = new List<double>(tasks.Count);

			// Solver
			foreach (var task in tasks)
			{
				throw (new NotImplementedException());
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


		public List<List<List<double>>> GetTask(List<List<double>> doubleLists)
		{
			List<List<List<double>>> taskLists;

			if (doubleLists.Count <= 0 || (doubleLists.Count > 0 && doubleLists[0].Count <= 0))
			{
				taskLists = new List<List<List<double>>>(0);
			}
			else
			{
				// Line 0 is tasks count
				taskLists = new List<List<List<double>>>((Int32)doubleLists[0][0]);
			}

			// Line 0 is tasks count, tasks start from line 1
			for (int i = 1; i < doubleLists.Count; i++)
			{
				List<List<double>> dataLists;
				if (doubleLists[i].Count <= 0)
				{
					dataLists = new List<List<double>>(0);
				}
				else
				{
					dataLists = new List<List<double>>((Int32)doubleLists[i][0]);
				}

				// Each task chunk consist of a line of task count N
				// And fallowing N lines of task data
				for (int j = 0; j < dataLists.Capacity; j++)
				{
					dataLists.Add(doubleLists[i + j + 1]);
				}

				taskLists.Add(dataLists);

				// Update the position
				i += dataLists.Capacity;
			}

			return taskLists;
		}
	}
}
