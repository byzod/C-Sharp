using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Machine
{
	class GCJ_2013_QR_A : IProblemSolver
	{
		#region Properties

		/// <summary>
		/// Should write file in binary mode
		/// </summary>
		public string ID
		{
			get
			{
				return "gcj.2013.qr.a";
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
			List<int[][]> tasks = this.GetBoard(option.Input.Content[0]);

			// Result
			List<double> results = new List<double>(tasks.Count);

			// Solver
			foreach (var task in tasks)
			{
				int winState = 0;

				// diag
				winState =  this.GetLineState(task[0][0], task[1][1], task[2][2], task[3][3]);
				if (winState != 0)
				{
					results.Add(winState);
					continue;
				}

				// diag
				winState = this.GetLineState(task[0][3], task[1][2], task[2][1], task[3][0]);
				if (winState != 0)
				{
					results.Add(winState);
					continue;
				}

				//v
				for (int i = 0; i < 4; i++)
				{
					winState = this.GetLineState(task[0][i], task[1][i], task[2][i], task[3][i]);
					if (winState != 0)
					{
						results.Add(winState);
						break;
					}
				}

				if (winState != 0) continue;

				//h
				for (int i = 0; i < 4; i++)
				{
					winState = this.GetLineState(task[i][0], task[i][1], task[i][2], task[i][3]);
					if (winState != 0)
					{
						results.Add(winState);
						break;
					}
				}

				if (winState != 0) continue;

				if (winState == 0)
				{
					for (int i = 0; i < 4; i++)
					{
						if (task[i].Contains(0))
						{
							winState = 0; // incomplete
							break;
						}
						else
						{
							winState = 3; // draw
						}
					}
					results.Add(winState);
				}
			}

			// Output
			option.Output.Content.Add(new List<string>(results.Count));
			for (int i = 0; i < results.Count; i++)
			{
				string resultStr = "";

				if (results[i] == 1)
				{
					resultStr = "X won";
				}
				else if (results[i] == -1)
				{
					resultStr = "O won";
				}
				else if (results[i] == 3)
				{
					resultStr = "Draw";
				}
				else
				{
					resultStr = "Game has not completed";
				}

				option.Output.Content[0].Add(
					"Case #" + (i + 1) + ": " + resultStr
				);
			}
		}

		// 0 = no one win, 1 = X win, -1 = O win
		public int GetLineState(int a, int b, int c, int d)
		{
			int[] line = new int[4] { a, b, c, d };
			int state = 0;
			int tempState = 3;

			for (int i = 0; i < 4; i++)
			{
				if (tempState == line[i] || tempState == 2 || line[i] == 2|| tempState == 3)
				{
					tempState = line[i] == 2 ? 
						tempState
						: line[i];
				}
				else
				{
					tempState = 0;
				}
			}

			switch (tempState)
			{
				case 1:
					state = 1;
					break;
				case -1:
					state = -1;
					break;
				default:
					state = 0;
					break;
			}

			return state;
		}

		public List<int[][]> GetBoard(List<string> data)
		{
			int count = int.Parse(data[0]);
			var boards = new List<int[][]>(count);

			for (int i = 0; i < count; i++)
			{
				var board = new int[4][];
				for (int j = 0; j < 4; j++)
				{
					board[j] = new int[4];
				}

				for (int j = 0; j < 4; j++)
				{
					Int32 c = 0;
					foreach (char grid in data[i*5+j+1])
					{
						switch (grid)
						{
							case '.':
								board[j][c] = 0;
								break;
							case 'X':
								board[j][c] = 1;
								break;
							case 'O':
								board[j][c] = -1;
								break;
							case 'T':
								board[j][c] = 2;
								break;
							default:
								break;
						}
						c++;
					} 
				}

				boards.Add(board);
			}

			return boards;
		}
	}
}
