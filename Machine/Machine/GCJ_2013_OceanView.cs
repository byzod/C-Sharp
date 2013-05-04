using System;
using System.Collections.Generic;
using System.Linq;

namespace Machine
{
	/// <summary>
	/// Google code jam 2013 Ocean View
	/// </summary>
	class GCJ_2013_OceanView : IProblemSolver
	{
		#region Properties

		/// <summary>
		/// Should write file in binary mode
		/// </summary>
		public string ID
		{
			get
			{
				return "gcj.2013.oceanview";
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
		/// Ocean view Solver
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
				option.Warnings.Add( this.ID + " can only process " + this.MaxFiles + " files a time, the files after the first one will be ignored.");
			}

			// Data parse
			List<List<Int32>> inputData =
				ProblemSolverHelper.ConvertToCodeJamInputInOneLineFormat(ProblemSolverHelper.ConvertToDataLists<Int32>(option.Input.Content[0]));

			// Solve the problem
			List<Int32> houseNeedToDestroy = this.SolveCore(inputData);

			// Parse result to string lists
			option.Output.Content.Add(new List<string>(houseNeedToDestroy.Count));
			for (int i = 0; i < houseNeedToDestroy.Count; i++)
			{
				option.Output.Content[0].Add(
					"Case #" + (i+1) + ": " + houseNeedToDestroy[i]
					//new List<string>(1) { "Case #" + (i+1) + ": " + houseNeedToDestroy[i] + "/" + inputData[i].Count } //debug
				);
			}
		}
		
		/// <summary>
		/// Core method of this solver
		/// </summary>
		/// <param name="dataList">Formatted data list</param>
		/// <returns>Result list</returns>
		private List<Int32> SolveCore(List<List<Int32>> dataList)
		{
			List<Int32> houseNeedToDestroy = new List<int>(dataList.Count);

			foreach (List<Int32> data in dataList)
			{
				houseNeedToDestroy
					.Add(data.Count - this.GetMaxSubSeriesLength(data));
			}

			return houseNeedToDestroy;
		}

		/// <summary>
		/// Get max sub series length of given Int32 list
		/// </summary>
		/// <param name="data">Int32 list</param>
		/// <returns>Max sub series length</returns>
		private Int32 GetMaxSubSeriesLength(List<Int32> data)
		{
			// Largest common series length array. LCSL[p] store the max LCS length with the end element data[p]
			List<Int32> LCSL = new List<int>(data.Count);
			for (int i = 0; i < data.Count; i++)
			{
				if (i == 0)
				{
					// LCSL[1] = 1, the only element's LCS is itself
					LCSL.Add(1);
				}
				else
				{
					// LCSL[i] is 1 if data[i] is smaller than any data[k]
					// LCSL[i] is LCSL[p] + 1, where LCSL[p] is the max element that data[p] < data[i]
					// That is to say:
					// LCSL[i] = Max{  data[i] > data[k] ? LCSL_k : 1 } + 1
					// Where k = 0,1...i-1
					Int32 maxLCSL = 0;
					for (int j = 0; j < i; j++)
					{
						if (data[i] > data[j] && LCSL[j] > maxLCSL)
						{
							maxLCSL = LCSL[j];
						}
					}
					LCSL.Add(maxLCSL + 1);
				}
			}

			// The maximum LCS length is what we want
			return LCSL.Max();
		}
	}
}
