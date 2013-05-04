using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Machine
{
	class GCJ_2013_QR_B : IProblemSolver
	{
		#region Properties

		/// <summary>
		/// Should write file in binary mode
		/// </summary>
		public string ID
		{
			get
			{
				return "gcj.2013.qr.b";
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
			List<Lawn> tasks = this.GetLawns(option.Input.Content[0]);

			// Result
			List<bool> results = new List<bool>(tasks.Count);

			// Solver
			foreach (var task in tasks)
			{
				MaxHeights maxHs = this.GetMaxHeightPerRow(task);

				bool canAchieve = true;

				for (int i = 0; i < task.Height; i++)
				{
					for (int j = 0; j < task.Width; j++)
					{
						if (task.Layout[i][j] < maxHs.Row[i] && task.Layout[i][j] < maxHs.Column[j])
						{
							canAchieve = false;
							break;
						}
					}
				}

				results.Add(canAchieve);
			}

			// Output
			option.Output.Content.Add(new List<string>(results.Count));
			for (int i = 0; i < results.Count; i++)
			{
				option.Output.Content[0].Add(
					"Case #" + (i + 1) + ": "
					+ (results[i] ? "YES" : "NO")
				);
			}
		}

		List<Lawn> GetLawns(List<string> data)
		{

			int count = int.Parse(data[0]);
			var lawns = new List<Lawn>(count);
			
			for (int i = 1; i < data.Count; i++)
			{
				if (count-- <= 0)
				{
					break;
				}
				var dim = data[i].Split(' ');
				int height = Int32.Parse(dim[0]);
				int width = Int32.Parse(dim[1]);

				var lawn = new Lawn(width, height);

				for (int j = 0; j < height; j++)
				{
					var line = data[i + j + 1].Split(' ');
					for (int k = 0; k < line.Length; k++)
					{
						lawn.Layout[j][k] = Int32.Parse(line[k]);
					}
				}

				lawns.Add(lawn);
				i += lawn.Height;
			}

			return lawns;
		}

		MaxHeights GetMaxHeightPerRow(Lawn lawn)
		{
			MaxHeights maxHeights = new MaxHeights(lawn.Width, lawn.Height);

			//rows
			for (int i = 0; i < lawn.Height; i++)
			{
				int maxH = 0;
				for (int j = 0; j < lawn.Width; j++)
				{
					if (lawn.Layout[i][j] > maxH)
					{
						maxH = lawn.Layout[i][j];
					}
				}

				maxHeights.Row[i] = maxH;
			}

			// col
			for (int i = 0; i < lawn.Width; i++)
			{
				int maxH = 0;

				for (int j = 0; j < lawn.Height; j++)
				{
					if (lawn.Layout[j][i] > maxH)
					{
						maxH = lawn.Layout[j][i];
					}
				}

				maxHeights.Column[i] = maxH;
			}

			return maxHeights;
		}
		
		class Lawn
		{
			public int Width { get; set; }
			public int Height { get; set; }
			public int[][] Layout { get; set; }

			public Lawn(int width, int height)
			{
				this.Width = width;
				this.Height = height;
				this.Layout = new int[height][];
				for (int i = 0; i < height; i++)
				{
					this.Layout[i] = new int[width];
				}
			}
		}

		class MaxHeights
		{
			public int[] Row { get; set; }
			public int[] Column { get; set; }

			public MaxHeights(int width, int height)
			{
				this.Column = new int[width];
				this.Row = new int[height];
			}
		}
	}

}
