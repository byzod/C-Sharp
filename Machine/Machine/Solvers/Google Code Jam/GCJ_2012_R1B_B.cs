using System;
using System.Collections.Generic;
using System.Linq;

namespace Machine
{
	class GCJ_2012_R1B_B : IProblemSolver
	{

		#region Properties

		/// <summary>
		/// Should write file in binary mode
		/// </summary>
		public string ID
		{
			get
			{
				return "gcj.2012.r1b.b";
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

		enum MoveState
		{
			KayakWithouWait = 0,
			DragWithouWait = 1,
			KayakWithWait = 2,
			DragWithWait = 3,
			CannotMove = 4
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
			List<Task> tasks = this.GetTasks(option.Input.Content[0]);

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

		/// <summary>
		/// Get tasks
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		List<Task> GetTasks(List<string> data)
		{
			int count = int.Parse(data[0]);
			List<Task> tasks = new List<Task>(count);

			for (int i = 1; i < data.Count; i++)
			{
				string[] taskStr = data[i].Split(' ');
				Task task = new Task(
						int.Parse(taskStr[0]),
						new Cell[
							int.Parse(taskStr[1]),
							int.Parse(taskStr[2])
						]
				);

				int rowCount = task.Cells.GetUpperBound(0) + 1;
				int colCount = task.Cells.GetUpperBound(1) + 1;

				for (int j = 0; j < rowCount; j++)
				{
					string[] ceilings = data[i + j + 1].Split(' ');
					for (int k = 0; k < colCount; k++)
					{
						task.Cells[j, k] = new Cell(0, 0);
						task.Cells[j, k].Ceiling = int.Parse(ceilings[k]);
					}
				}

				for (int j = 0; j < rowCount; j++)
				{
					string[] floors = data[i + j + rowCount + 1].Split(' ');
					for (int k = 0; k < colCount; k++)
					{
						task.Cells[j, k].Floor = int.Parse(floors[k]);
					}
				}

				tasks.Add(task);

				i += 2 * rowCount;
			}

			return tasks;
		}

		/// <summary>
		/// Move from cell a to cell b, return time and state
		/// </summary>
		/// <param name="cellFrom"></param>
		/// <param name="cellTo"></param>
		/// <returns></returns>
		MoveResult TryMove(Cell from, Cell to, int waterLevel)
		{
			MoveResult result = new MoveResult(0, MoveState.CannotMove);

			if (from.Ceiling - Math.Max(to.Floor, waterLevel) >= 50
				|| to.Ceiling - Math.Max(from.Floor, waterLevel) >= 50)
			{
				if (waterLevel >= from.Floor + 20)
				{
					result.Time = 1;
					result.State = MoveState.KayakWithouWait;
				}
				else
				{
					result.Time = 10;
					result.State = MoveState.DragWithouWait;
				}
			}
			else
			{
				if (waterLevel >= from.Floor && to.Ceiling - from.Floor >= 50)
				{
					result.Time= ((waterLevel - (to.Ceiling - 50)) / 10)
						+ ((to.Ceiling - from.Floor >= 70) ? 1 : 10);

					result.State = (to.Ceiling - from.Floor >= 70) ?
						MoveState.KayakWithWait
						: MoveState.DragWithWait;
				}
				else
				{
					result.Time = 0;
					result.State = MoveState.CannotMove;
				}
			}

			return result;
		}

		class Cell
		{
			public int Floor { get; set; }
			public int Ceiling { get; set; }
			public double MinReachTime { get; set; }
			public bool Visited { get; set; }

			public Cell(int floor, int ceiling)
			{
				this.Floor = floor;
				this.Ceiling = ceiling;
				this.MinReachTime = -1;
				this.Visited = false;
			}
		}

		class Task
		{
			public int WaterLevel { get; set; }
			public Cell[,] Cells { get; set; }

			public Task(int waterLevel, Cell[,] cells)
			{
				this.WaterLevel = waterLevel;
				this.Cells = cells;
			}
		}

		class MoveResult
		{
			public int Time { get; set; }
			public MoveState State { get; set; }

			public MoveResult(int time, MoveState state)
			{
				this.Time = time;
				this.State = state;
			}
		}
	}
}
