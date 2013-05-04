#if debug
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Numerics;

namespace Machine
{
	class GCJ_2013_QR_C : IProblemSolver
	{
		#region Properties

		/// <summary>
		/// Should write file in binary mode
		/// </summary>
		public string ID
		{
			get
			{
				return "gcj.2013.qr.c";
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
			switch (option.Problem.Args[0])
			{
				case "S":
					// Result
					List<Int32> results = new List<Int32>(200);

					List<Endpoint> tasks = new List<Endpoint>(100);
					tasks = this.GetEndpoints(option.Input.Content[0]);

					// Solver
					foreach (var task in tasks)
					{
						Int32 count = 0;

						// Search directly
						Int64 root = (int)Math.Sqrt(task.A);

						// validate
						while (root * root < task.A)
						{
							root++;
						}

						while (root * root <= task.B)
						{
							if (this.IsPalindrome(root.ToString())
								&& this.IsPalindrome((root * root).ToString()))
							{
								count++;
							}
							root++;
						}
						results.Add(count);
					}
			
					// Output
					option.Output.Content.Add(new List<string>(results.Count));
					for (int i = 0; i < results.Count; i++)
					{
						option.Output.Content[0].Add(
							"Case #" + (i + 1) + ": " + results[i]
						);
					}
					break;
				case "L1":
					//dead
					break;
				case "L2":
					// Result
					List<BigInteger> bigResults = new List<BigInteger>(1000);
					List<BigEndpoint> bigTasks = this.GetBigEndpoints(option.Input.Content[0]);

					foreach (var task in bigTasks)
					{
						BigInteger count = 0;
						BigInteger rootA = BigIntegerSqrt(task.A);
						BigInteger rootB = BigIntegerSqrt(task.B);

						// crop
						if (BigInteger.Multiply(rootA, rootA) < task.A)
						{
							rootA += 1;
						}
						if (BigInteger.Multiply(rootB, rootB) > task.B)
						{
							rootB -= 1;
						}

						if (rootA <= rootB)
						{
							string rootAStr = rootA.ToString();
							string rootBStr = rootB.ToString();

							int firstNumOfA = (rootAStr[0] - '0');
							int lastNumOfA = (rootAStr[rootAStr.Length - 1] - '0');

							int firstNumOfB = (rootBStr[0] - '0');
							int lastNumOfB = (rootBStr[rootBStr.Length - 1] - '0');

							if (rootAStr.Length == rootBStr.Length)
							{
								// Add length same as A, A to B
								count += GetPalindromeCount(
									rootAStr.Length,
									firstNumOfA,
									firstNumOfB,
									lastNumOfA,
									lastNumOfB
								);
							}
							else // root a < root b
							{
								for (int numLength = rootAStr.Length + 1; numLength < rootBStr.Length; numLength++)
								{
									// Add length same as A, A to B
									count += GetPalindromeCount(
										numLength,
										1, 9,
										0, 9
									);
								}

								// Add length with A
								count += GetPalindromeCount(
									rootAStr.Length,
									firstNumOfA,
									9,
									lastNumOfA,
									9
								);

								// Add length with B
								count += GetPalindromeCount(
									rootBStr.Length,
									1,
									firstNumOfB,
									0,
									lastNumOfB
								);
							}
						}

						bigResults.Add(count);
					}
			
					// Output
					option.Output.Content.Add(new List<string>(bigResults.Count));
					for (int i = 0; i < bigResults.Count; i++)
					{
						option.Output.Content[0].Add(
							"Case #" + (i + 1) + ": " + bigResults[i]
						);
					}
					break;
				default:
					break;
			}
		}

		/// <summary>
		/// Get end points
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		public List<Endpoint> GetEndpoints(List<string> data)
		{
			int count = int.Parse(data[0]);
			var endpoints = new List<Endpoint>(count);

			for (int i = 0; i < count; i++)
			{
				var points = data[i+1].Split(' ');

				endpoints.Add(
					new Endpoint(
						Int64.Parse(points[0]),
						Int64.Parse(points[1])
					)
				);
			}

			return endpoints;
		}

		/// <summary>
		/// Really big
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		public List<BigEndpoint> GetBigEndpoints(List<string> data)
		{
			int count = int.Parse(data[0]);
			var endpoints = new List<BigEndpoint>(count);

			for (int i = 0; i < count; i++)
			{
				string[] points = data[i + 1].Split(' ');
				BigInteger  a,
							b;
				BigInteger.TryParse(points[0], out a);
				BigInteger.TryParse(points[1], out b);

				endpoints.Add(
					new BigEndpoint(a, b)
				);
			}

			return endpoints;
		}

		/// <summary>
		/// Check if it's palindrome
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		public bool IsPalindrome(string str)
		{
			var isPalindrome = true;

			// length 1 is always a palindrome
			if (str.Length > 1)
			{
				for (int i = 0; i < str.Length/2; i++)
				{
					if (str[i] != str[str.Length - i - 1])
					{
						isPalindrome = false;
						break;
					}
				}
			}

			return isPalindrome;
		}

		/// <summary>
		/// Get palindrome count of given length of num, start number within range
		/// </summary>
		public BigInteger GetPalindromeCount(int numLength, int firstNumFrom, int firstNumTo, int lastNumFrom, int lastNumTo)
		{
			BigInteger count = 0;
			int from = firstNumFrom * 10 + lastNumFrom;
			int to = firstNumTo * 10 + lastNumTo;

			if (numLength > 0
				&& firstNumTo >= firstNumFrom)
			{
				if (numLength == 1)
				{
					count = (firstNumTo - firstNumFrom) + 1;
				}
				else
				{
					int subCount = 0;
					for (int i = from; i <= to; i++)
					{
						if (i == 0
							|| i == 11
							|| i == 22
							|| i == 33
							|| i == 44
							|| i == 55
							|| i == 66
							|| i == 77
							|| i == 88
							|| i == 99
							)
						{
							subCount++;
						}
					}

					int k = (numLength - 1) / 2;
					count = subCount * BigInteger.Pow(10, k);
				}
			}
			else
			{
				throw (new ArgumentException("Invalid range"));
			}

			return count;
		}

		/// <summary>
		/// Get Sqrt
		/// </summary>
		/// <param name="x"></param>
		/// <returns></returns>
		public static BigInteger BigIntegerSqrt(BigInteger x)
		{
			if (x.Sign < 0) throw new ArgumentOutOfRangeException("x", "must greater than 0");
			BigInteger low, high;
			GetLowAndHigh(x, out low, out high);
			var mid = low;
			var cmp = 0;
			while (low.CompareTo(high) <= 0)
			{
				mid = (low + high) / 2;
				cmp = (mid * mid).CompareTo(x);
				if (cmp < 0) low = mid + 1;
				else if (cmp > 0) high = mid + (-1);
				else return mid;
			}
			if (cmp > 0) mid--;
			return mid;
		}

		static void GetLowAndHigh(BigInteger x, out BigInteger low, out BigInteger high)
		{
			var n = x.ToByteArray().Length;
			if (n < 2)
			{
				low = 0;
				high = x;
				return;
			}
			var bs = new byte[n / 2 + 1];
			var k = bs.Length - 2;
			if (n % 2 == 0)
			{
				bs[k] = 0x0B;
				low = new BigInteger(bs);
				bs[k] = 0xB6;
				high = new BigInteger(bs);
			}
			else
			{
				bs[k] = 0xB5;
				low = new BigInteger(bs);
				bs[k] = 0x51;
				bs[k + 1] = 0x0B;
				high = new BigInteger(bs);
			}
		}
	}

	class Endpoint
	{
		public Int64 A { get; set; }
		public Int64 B { get; set; }

		public Endpoint(Int64 a, Int64 b)
		{
			this.A = a;
			this.B = b;
		}
	}

	class BigEndpoint
	{
		public BigInteger A { get; set; }
		public BigInteger B { get; set; }

		public BigEndpoint(BigInteger a, BigInteger b)
		{
			this.A = a;
			this.B = b;
		}
	}
}
#endif