using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Machine
{
	class GCJ_2013_R1A_C : IProblemSolver
	{

		#region Properties

		/// <summary>
		/// Should write file in binary mode
		/// </summary>
		public string ID
		{
			get
			{
				return "GCJ.2013.R1A.C";
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
				int[] factors = new int[] { 2, 3, 5, 7 };

				for (int i = 0; i < task.R; i++)
				{
					List<int> productNums = task.Products[i];
					List<Product> products = new List<Product>(productNums.Count);

					for (int j = 0; j < productNums.Count; j++)
					{
						products.Add(
							new Product(this.GetFactors(productNums[j], factors))
						);
					}
				}
			}

			// Output
			option.Output.Content.Add(new List<string>(results.Count));
			option.Output.Content[0].Add("Case #1:");

			for (int i = 0; i < results.Count; i++)
			{
				string guess = "";
				for (int j = 0; j < results[i].Count; j++)
				{
					guess += results[i][j];
				}
				option.Output.Content[0].Add(guess);
			}
		}

		List<int> GetFactors(int num, int[] factorList)
		{
			int temp = num;
			int factorIndex = 0;
			List<int> factors = new List<int>();

			while (temp != 1 && factorIndex < factorList.Length)
			{
				if (temp % factorList[factorIndex] == 0)
				{
					factors.Add(factorList[factorIndex]);
					temp /= factorList[factorIndex];
				}
				else
				{
					factorIndex++;
				}
			}

			return factors;
		}

		List<Task> GetTasks(List<string> data)
		{
			int count = int.Parse(data[0]);
			List<Task> tasks = new List<Task>(count);

			string[] rnmk = data[1].Split(' ');

			Task task = new Task(
				int.Parse(rnmk[0]),
				int.Parse(rnmk[1]),
				int.Parse(rnmk[2]),
				int.Parse(rnmk[3])
			);

			task.Products = new List<List<int>>(task.R);

			for (int i = 0; i < task.R; i++)
			{
				string[] products = data[i + 2].Split(' ');
				task.Products.Add(
					(from num in products
					 select int.Parse(num))
					 .ToList()
				);
			}

			tasks.Add(task);

			return tasks;
		}

		class Task
		{
			public int R { get; set; }
			public int N { get; set; }
			public int M { get; set; }
			public int K { get; set; }
			public List<List<int>> Products { get; set; }

			public Task(int r, int n, int m, int k)
			{
				this.R = r;
				this.N = n;
				this.M = m;
				this.K = k;
			}
		}

		class Product
		{
			private Dictionary<int, int> factorsCount;
			public List<int> Factors { get; set; }

			public Product(List<int> factors)
			{
				this.Factors = factors;
				this.factorsCount = new Dictionary<int, int>(4);

				for (int i = 0; i < factors.Count; i++)
				{
					int count = 0;
					if (this.factorsCount.TryGetValue(factors[i], out count))
					{
						this.factorsCount[factors[i]]++;
					}
					else
					{
						this.factorsCount.Add(factors[i], 1);
					}
				}
			}

			public int FactorCount(int factor)
			{
				int count = 0;

				if (!this.factorsCount.TryGetValue(factor, out count))
				{
					count = 0;
				}

				return count;
			}

			public int FactorCount()
			{
				return this.Factors.Count;
			}
		}
	}
}
