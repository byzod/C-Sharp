using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Machine
{
	class Test_SortComp : IProblemSolver
	{
		#region Properties

		/// <summary>
		/// Should write file in binary mode
		/// </summary>
		public string ID
		{
			get
			{
				return "test.sortcomp";
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
			int arrSize = int.Parse(option.Input.Content[0][0]);
			int cycle = 100000;
			int sizeForQuick = arrSize * arrSize;
			int sizeForBubble = arrSize / (int)Math.Log(arrSize, 2) * 30;

			// Result
			List<string> results = new List<string>(3);

			// Compare
			int[] arr0 = GetRandomArr(sizeForBubble);
			int[] arr1 = GetRandomArr(sizeForQuick);
			//int[] arr0 = GetSequenceArr(sizeForBubble, -1);
			//int[] arr1 = GetSequenceArr(sizeForQuick, -1);

			//ByzodToolkit.CodeTimer.Initialize();

			//ByzodToolkit.CodeTimer.Time("Empty loop: (" + cycle + " cycles)", cycle, () => { int[] arrResult = arr0; });

			//ByzodToolkit.CodeTimer.Time("Bubble sort(" + sizeForBubble + " x " + cycle + " cycles): ", cycle, () => { BubbleSort(arr0); });

			//ByzodToolkit.CodeTimer.Time("Quick sort(" + sizeForQuick + " x " + cycle + " cycles): ", cycle, () => { QuickSort(arr1, 0, arrSize - 1); });
			
		}

		/// <summary>
		/// Implementation of quick sort
		/// </summary>
		/// <param name="arr">Array to sort</param>
		/// <returns></returns>
		public int[] QuickSort(int[] arr, int from, int to)
		{
			if (from < to)
			{
				int mid = arr[(from + to) / 2];
				int left = from - 1;
				int right = to + 1;

				while (true)
				{
					while (arr[++left] < mid && left <= to) ;

					while (arr[--right] > mid && right >= from) ;

					if (left < right)
					{
						int temp = arr[left];
						arr[left] = arr[right];
						arr[right] = temp;
					}
					else
					{
						break;
					}
				}

				QuickSort(arr, from, left - 1);
				QuickSort(arr, right + 1, to);
			}

			return arr;
		}

		/// <summary>
		/// Implementation of bubble sort
		/// </summary>
		/// <param name="arr">Array to sort</param>
		/// <returns></returns>
		public int[] BubbleSort(int[] arr)
		{
			for (int i = 1; i < arr.Length; i++)
			{
				for (int j = i - 1; j >= 0; j--)
				{
					if (arr[j] > arr[j + 1])
					{
						// Swap
						int temp = arr[j];
						arr[j] = arr[j + 1];
						arr[j + 1] = temp;
					}
					else
					{
						break;
					}
				}
			}

			return arr;
		}

		/// <summary>
		/// Get a array of given size filled with random numbers
		/// </summary>
		/// <param name="arrSize">Size of the array</param>
		/// <returns></returns>
		public int[] GetRandomArr(int arrSize)
		{
			int[] arr = new int[arrSize];
			Random rand = new Random();
			
			for (int i = 0; i < arrSize; i++)
			{
				arr[i] = rand.Next();
			}

			return arr;
		}

		/// <summary>
		/// Get a sorted array of given direction
		/// </summary>
		/// <param name="arrSize">Size of the array</param>
		/// <param name="direction">1 for increasing, -1 for decreasing, 0 for array of same value</param>
		/// <returns></returns>
		public int[] GetSequenceArr(int arrSize, int direction)
		{
			int[] arr = new int[arrSize];

			for (int i = 0; i < arrSize; i++)
			{
				arr[i] = arrSize + direction;
			}

			return arr;
		}
	}
}
