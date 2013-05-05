using System;
using System.Collections.Generic;

namespace Machine
{
	class GCJ_2013_Hedgemony :IProblemSolver
	{
		#region Properties

		/// <summary>
		/// Should write file in binary mode
		/// </summary>
		public string ID
		{
			get
			{
				return "gcj.2013.hedgemony";
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
		/// Hedgemony solver
		/// </summary>
		/// <param name="option">Options</param>
		/// <returns>Output list</returns>
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

			// Data parse
			List<List<Int32>> inputData = this.GetTask(ProblemSolverHelper.ConvertToDataLists<Int32>(option.Input.Content[0]));

			List<double> bushesHeights = new List<double>(inputData.Count);

			// Solve the problem
			foreach (List<Int32> heights in inputData)
			{
				double newBushHeight = 0.0;
				for (int i = 0; i < heights.Count - 1; i++)
				{
					if (i == 0)
					{
						newBushHeight = heights[0];
					}
					else
					{
						newBushHeight = (newBushHeight + heights[i + 1]) / 2.0;
						if (newBushHeight > heights[i])
						{
							newBushHeight = heights[i];
						}
					}
				}
				bushesHeights.Add(newBushHeight);
			}
			
			// Parse result to string lists
			option.Output.Content.Add(new List<string>(bushesHeights.Count));
			for (int i = 0; i < bushesHeights.Count; i++)
			{
				option.Output.Content[0].Add(
					"Case #" + (i + 1) + ": " + bushesHeights[i]
				);
			}
		}


		public List<List<Int32>> GetTask(List<List<Int32>> int32Lists)
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
	}
}
