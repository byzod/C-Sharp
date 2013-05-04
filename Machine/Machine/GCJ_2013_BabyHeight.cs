using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Machine
{
	class GCJ_2013_BabyHeight : IProblemSolver
	{
		#region Properties

		/// <summary>
		/// Should write file in binary mode
		/// </summary>
		public string ID
		{
			get
			{
				return "gcj.2013.babyheight";
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
		/// Baby Height solver
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

			// Data parse && solve
			List<double> babyHeightInInches =
				this.GetBabyHeightInInches(option.Input.Content[0]);

			// Baby height parse
			option.Output.Content.Add(new List<string>(babyHeightInInches.Count));
			for (int i = 0; i < babyHeightInInches.Count; i++)
			{
				option.Output.Content[0].Add(
					"Case #" + (i + 1) + ": " + this.ParseBabyHeight(babyHeightInInches[i])
				);
			}
		}

		/// <summary>
		/// Get the baby height in inches
		/// </summary>
		/// <param name="data">Input data</param>
		/// <returns>Baby height in inches (rounded)</returns>
		private List<double> GetBabyHeightInInches(List<string> data)
		{
			List<double> heightList = new List<double>(data.Count - 1);

			string heightPattern = "([BG]) (\\d)\'(\\d\\d?)\" (\\d)\'(\\d\\d?)\"";
			for (int i = 1; i < data.Count; i++)
			{
				Match match = Regex.Match(data[i], heightPattern);
				if (match.Success && match.Groups.Count == 6)
				{
					heightList
						.Add(
								(
									(match.Groups[1].Value == "B" ? 5 : -5)
									+ Convert.ToDouble(match.Groups[2].Value) * 12
									+ Convert.ToDouble(match.Groups[3].Value)
									+ Convert.ToDouble(match.Groups[4].Value) * 12
									+ Convert.ToDouble(match.Groups[5].Value)
								)
								/ 2
							);
				}
				else
				{
					throw new ArgumentException("Invalid height string: " + data[i]);
				}
			}

			return heightList;
		}

		/// <summary>
		/// Parse baby height to code jam format
		/// </summary>
		/// <param name="height">Baby's height</param>
		/// <returns>Output string</returns>
		private string ParseBabyHeight(double height)
		{
			string heightStr = "";
			Int32 maxHeight = (Int32)Math.Floor(height + 4);
			Int32 minHeight = (Int32)Math.Ceiling(height - 4);
			heightStr =
				(minHeight / 12) + "\'"
				+ (minHeight % 12) + "\" to "
				+ (maxHeight / 12) + "\'"
				+ (maxHeight % 12) + "\"";

			return heightStr;
		}
	}
}
