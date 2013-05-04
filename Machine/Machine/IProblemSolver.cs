using System;

namespace Machine
{
	/// <summary>
	/// Defines general problem solver
	/// </summary>
	interface IProblemSolver
	{
		/// <summary>
		/// ID of the problem solver
		/// </summary>
		string ID { get; }

		/// <summary>
		/// Maximum files that this solver can handle
		/// </summary>
		Int32 MaxFiles { get; }

		/// <summary>
		/// Solve problem with information and data from option object and store answer into the same option object
		/// </summary>
		/// <param name="option">Options</param>
		/// <returns>Output list</returns>
		void Solve(ref Options option);
	}
}
