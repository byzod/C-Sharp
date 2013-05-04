using System;
using System.Collections.Generic;
using System.Linq;

namespace Machine
{
	/// <summary>
	/// Provides instance of problem solver helper which can call solvers, parse input list, etc.
	/// </summary>
	class ProblemSolverHelper
	{
		/// <summary>
		/// State of problem solving
		/// </summary>
		public enum SolveState
		{
			/// <summary>
			/// Problem solved...hopefully
			/// </summary>
			Done = 0,
			/// <summary>
			/// Error occurs when try to solving problem
			/// </summary>
			Error = 1,
			/// <summary>
			/// No such solver
			/// </summary>
			SolverNotExist = 2,
		}

		//public delegate List<List<Int32>> InputStringFormatter()
		// TODO: formmater

		/// <summary>
		/// Solvers collection
		/// </summary>
		public Dictionary<string, IProblemSolver> ProblemSolvers { get; set; }

		/// <summary>
		/// Initialize new instance of proble solver helper that with available solvers list
		/// </summary>
		public ProblemSolverHelper()
		{
			// All solvers list
			List<IProblemSolver> solvers = new List<IProblemSolver>()
			{
				// Add new solvers here
				new Test(),
				new Test_SortComp(),
				new GCJ_2013_OceanView(),
				new GCJ_2013_BabyHeight(),
				new GCJ_2013_Hedgemony(),
				new GCJ_2012_WF_A(),
				new GCJ_2012_WF_E(),
				new GCJ_2012_R1A_A(),
				new GCJ_2012_R1A_B(),
				new GCJ_2012_R1A_C(),
				new GCJ_2012_R1B_A(),
				new GCJ_2012_R1B_B(),
				new GCJ_2012_R1B_C(),
				new GCJ_2013_QR_A(),
				new GCJ_2013_QR_B(),
#if debug
				new GCJ_2013_QR_C(),
#endif
				new GCJ_2013_QR_D(),
				new GCJ_2013_R1A_A(),
				new GCJ_2013_R1A_B(),
				new GCJ_2013_R1A_C(),
				new GCJ_2013_R1B_A(),
				new GCJ_2013_R1B_B(),
				new GCJ_2013_R1B_C(),
			};
			
			this.ProblemSolvers = new Dictionary<string, IProblemSolver>(solvers.Count);

			foreach (var solver in solvers)
			{
				this.ProblemSolvers.Add(solver.ID.ToLower(), solver);
			}
		}

		/// <summary>
		/// Call proper solver to solve the problem
		/// </summary>
		/// <param name="solverID">ID of the solver to call</param>
		/// <param name="options">Options</param>
		/// <returns>State of solving</returns>
		public SolveState CallSolver(string solverID, ref Options options)
		{
			SolveState state;
			IProblemSolver solver;

			this.ProblemSolvers.TryGetValue(solverID.ToLower(), out solver);

			if (solver != null)
			{
				try
				{
					solver.Solve(ref options);
					state = SolveState.Done;
				}
				catch (Exception ex)
				{
					state = SolveState.Error;
					options.Errors.Add(ex.Message);
				}

			}
			else
			{
				state = SolveState.SolverNotExist;
			}

			return state;
		}

		/// <summary>
		/// Convert string list to data lists list (Numbers or strings)
		/// </summary>
		/// <param name="stringLists">String list</param>
		/// <returns>Data lists list</returns>
		public static List<List<T>> ConvertToDataLists<T>(List<string> stringList)
			where T : IConvertible
		{
			List<List<T>> dataLists = new List<List<T>>(stringList.Count);
			char[] splitCharacters = new char[1] { ' ' };

			foreach (string str in stringList)
			{
				string[] strFragment = str.Split(splitCharacters, StringSplitOptions.RemoveEmptyEntries);
				List<T> dataList = new List<T>(strFragment.Length);

				foreach (string fragment in strFragment)
				{
					if (fragment.Length > 0)
					{
						// Convert string fragment to given type T, assume T is Int32, double or string
						dataList.Add((T)Convert.ChangeType(fragment, typeof(T)));
					}
				}
				dataLists.Add(dataList);
			}
			return dataLists;
		}

		/// <summary>
		/// Convert integer lists list to google code jam input-in-one-line format
		/// </summary>
		/// <param name="int32Lists">Int32 lists list</param>
		/// <returns>Formatted Int32 lists list</returns>
		public static List<List<Int32>> ConvertToCodeJamInputInOneLineFormat(List<List<Int32>> int32Lists)
		{
			List<List<Int32>> dataLists;

			if (int32Lists.Count <= 0 || (int32Lists.Count > 0 && int32Lists[0].Count <= 0))
			{
				dataLists = new List<List<Int32>>(0);
			}
			else
			{
				dataLists = new List<List<Int32>>(int32Lists[0][0]);
			}

			// Line 0 is lines count, data start from line 1
			for (int i = 1; i < int32Lists.Count; i++)
			{
				// Each data chunk consist of two lines, the first one is data count
				i++; 
				// The second line is data itself
				dataLists.Add(int32Lists[i]);
			}

			return dataLists;
		}

		/// <summary>
		/// Convert double lists list to google code jam input-in-one-line format
		/// </summary>
		/// <param name="doubleLists">Double lists list</param>
		/// <returns>Formatted double lists list</returns>
		public static List<List<double>> ConvertToCodeJamInputInOneLineFormat(List<List<double>> doubleLists)
		{
			List<List<double>> dataLists;

			if (doubleLists.Count <= 0 || (doubleLists.Count > 0 && doubleLists[0].Count <= 0))
			{
				dataLists = new List<List<double>>(0);
			}
			else
			{
				dataLists = new List<List<double>>((Int32)doubleLists[0][0]);
			}

			// Line 0 is lines count, data start from line 1
			for (int i = 1; i < doubleLists.Count; i++)
			{
				// Each data chunk consist of two lines, the first one is data count
				i++;
				// The second line is data itself
				dataLists.Add(doubleLists[i]);
			}

			return dataLists;
		}

		/// <summary>
		/// Convert integer lists list to google code jam multi-line per task input format
		/// </summary>
		/// <param name="int32Lists">Int32 lists list</param>
		/// <returns>Formatted Int32 task list</returns>
		public static List<List<List<Int32>>> ConvertToCodeJamStandardFormat(List<List<Int32>> int32Lists)
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

		/// <summary>
		/// Convert double lists list to google code jam multi-line per task input format
		/// </summary>
		/// <param name="int32Lists">Double lists list</param>
		/// <returns>Formatted double task list</returns>
		public static List<List<List<double>>> ConvertToCodeJamStandardFormat(List<List<double>> doubleLists)
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

		/// <summary>
		/// Convert string list to google code jam multi-line per task format string list
		/// </summary>
		/// <param name="stringLists">String list</param>
		/// <returns>Formatted string task list</returns>
		public static List<List<List<string>>> ConvertToCodeJamStandardStringLists(List<string> stringList)
		{
			List<List<List<string>>> taskLists = new List<List<List<string>>>(stringList.Count);
			char[] splitCharacters = new char[1] { ' ' };

			if (stringList.Count <= 0 || (stringList.Count > 0 && stringList[0].Length <= 0))
			{
				taskLists = new List<List<List<string>>>(0);
			}
			else
			{
				// Line 0 is tasks count
				taskLists = new List<List<List<string>>>(Convert.ToInt32(stringList[0]));
			}

			// Line 0 is tasks count, tasks start from line 1
			for (int i = 1; i < stringList.Count; i++)
			{
				List<List<string>> dataList;
				if (stringList[i].Length <= 0)
				{
					dataList = new List<List<string>>(0);
				}
				else
				{
					dataList = new List<List<string>>(Convert.ToInt32(stringList[i]));
				}

				// Each task chunk consist of a line of task count N
				// And fallowing N lines of task data
				for (int j = 0; j < dataList.Capacity; j++)
				{
					List<string> fragments =
						stringList[i + j + 1]
						.Split(splitCharacters, StringSplitOptions.RemoveEmptyEntries)
						.ToList();
					dataList.Add(fragments);
				}

				taskLists.Add(dataList);

				// Update the position
				i += dataList.Capacity;
			}

			return taskLists;
		}

	}
}
