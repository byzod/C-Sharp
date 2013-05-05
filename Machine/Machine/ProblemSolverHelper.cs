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
	}
}
