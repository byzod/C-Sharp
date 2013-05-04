using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Machine
{
	class GCJ_2013_QR_D : IProblemSolver
	{
		#region Properties

		/// <summary>
		/// Should write file in binary mode
		/// </summary>
		public string ID
		{
			get
			{
				return "gcj.2013.qr.d";
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
			List<Task> tasks = this.GetTasks(option.Input.Content[0]);

			// Result
			List<List<int>> results = new List<List<int>>(tasks.Count);

			// Solver
			foreach (var task in tasks)
			{
				List<int> trace = new List<int>(task.Chests.Count);
				int[] chestCount = new int[201];
				//int[][] chestMatrix = new int[201][];
				//for (int i = 0; i < chestMatrix.Length; i++)
				//{
				//	chestMatrix[i] = new int[201];
				//}

				// Check possibility first
				bool impossibleToLootAll = false;
				int[] allKeys = new int[201];
				foreach (var chest in task.Chests)
				{
					chestCount[chest.LockType] += 1; // Initialize chest count BTW
					this.LootAllKeys(allKeys, chest.Keys);
				}

				foreach (var chest in task.Chests)
				{
					// A chest that no key or not enough keys to open
					if (this.TryOpen(chest, allKeys) == false)
					{
						impossibleToLootAll = true;
					};
				}

				// solve only if there's a chance
				if (impossibleToLootAll == false)
				{
					int[] keysNeeded = new int[201];
					for (int i = 0; i < task.Keys.Length; i++)
					{
						keysNeeded[i] =
							chestCount[i] > task.Keys[i] ?
							chestCount[i] - task.Keys[i]
							: 0;
					}



				}

				results.Add(trace);
			}

			// Output
			option.Output.Content.Add(new List<string>(results.Count));
			for (int i = 0; i < results.Count; i++)
			{
				string resultString = "";

				if (results[i].Count > 0)
				{

					resultString = string.Join(
						" ", 
						(from str in results[i]
						select str.ToString())
						.ToArray()						
					);
				}
				else
				{
					resultString = "IMPOSSIBLE";
				}

				option.Output.Content[0].Add(
					"Case #" + (i + 1) + ": " + resultString
				);
			}
		}

		/// <summary>
		/// Get task
		/// </summary>
		/// <param name="dataRaw"></param>
		/// <returns></returns>
		List<Task> GetTasks(List<string> dataRaw)
		{
			var data = ProblemSolverHelper.ConvertToDataLists<string>(dataRaw);

			int count = int.Parse(data[0][0]);
			List<Task> tasks = new List<Task>(count);

			for (int i = 1; i < data.Count; i++)
			{
				int startKeysCount = int.Parse(data[i][0]);
				int chestCount = int.Parse(data[i][1]);

				Task task = new Task(chestCount);

				// get keys
				for (int j = 0; j < startKeysCount; j++)
				{
					int key = int.Parse(data[i + 1][j]);
					task.Keys[key] += 1;
				}

				// get chests
				for (int j = 0; j < chestCount; j++)
				{
					int lockType = int.Parse(data[i + j + 2][0]);
					int size = int.Parse(data[i + j + 2][1]);

					Chest chest = new Chest(lockType, size);

					for (int k = 0; k < size; k++)
					{
						int key = int.Parse(data[i + j + 2][k + 2]);
						chest.Keys.Add(key);
					}

					task.Chests.Add(chest);
				}

				i += 1 + chestCount;

				tasks.Add(task);
			}

			return tasks;
		}

		/// <summary>
		/// Try open chest, return if success
		/// </summary>
		/// <param name="chest"></param>
		/// <returns></returns>
		bool TryOpen(Chest chest, Int32[] ownKeys)
		{
			bool succeed = false;

			if ( ownKeys[chest.LockType] > 0)
			{
				succeed = true;
				ownKeys[chest.LockType]--;
				this.LootAllKeys(ownKeys, chest.Keys);
			}

			return succeed;
		}

		/// <summary>
		/// Loot keys
		/// </summary>
		/// <param name="ownKeys"></param>
		/// <param name="lootKeys"></param>
		void LootAllKeys(Int32[] ownKeys, List<int> lootKeys)
		{
			for (int i = 0; i < lootKeys.Count; i++)
			{
				ownKeys[lootKeys[i]] += 1;
			}
		}

		class Task
		{
			// key is key type, value is count
			public Int32[] Keys { get; set; }
			public List<Chest> Chests { get; set; }

			public Task(int chestCount)
			{
				this.Keys = new Int32[201];
				this.Chests = new List<Chest>(chestCount);
			}
		}

		class Chest
		{
			public int LockType { get; set; }
			public List<int> Keys { get; set; }
			public bool Looted { get; set; }

			public Chest(int lockType, int size)
			{
				this.Looted = false;
				this.LockType = lockType;
				this.Keys = new List<int>(size);
			}
		}
	}
}
