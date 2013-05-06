using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Machine
{
	/// <summary>
	/// Dummy solver, simply returns the input.
	/// </summary>
	class Test : IProblemSolver
	{
		#region Properties

		/// <summary>
		/// Should write file in binary mode
		/// </summary>
		public string ID
		{
			get
			{
				return "test";
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
		/// Dummy Solver, simply returns the input.
		/// </summary>
		/// <param name="option">Options</param>
		public void Solve(ref Options option)
		{
			option.IsBinaryLoad = false;
			option.IsBinaryWrite = false;

            option.LoadContent();
            option.Output.Content = option.Input.Content;
		}
	}



	#region Empty Solver Template
	
	class EmptySolverTemplate : IProblemSolver
	{

		#region Properties

		/// <summary>
		/// Should write file in binary mode
		/// </summary>
		public string ID
		{
			get
			{
				return "gcj.2013.xxxx.a";
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
			List<int> results = new List<int>(tasks.Count);
			//List<double> results = new List<double>(tasks.Count);

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

		/// <summary>
		/// Get task array
		/// </summary>
		/// <param name="data">Raw input string</param>
		/// <returns>List of task array</returns>
		List<Task> GetTask(List<string> data)
		{
			int count = int.Parse(data[0]);
			List<Task> tasks = new List<Task>(count);

			for (int i = 0; i < count; i++)
			{
				Task task = new Task();

				tasks.Add(task);
			}

			return tasks;
		}

		/// <summary>
		/// Task object
		/// </summary>
		class Task
		{
			public int IntProp { get; set; }

			public Task(int integer)
			{
				this.IntProp = integer;
			}

			public Task(){	}
		}
	}

	#endregion
}
