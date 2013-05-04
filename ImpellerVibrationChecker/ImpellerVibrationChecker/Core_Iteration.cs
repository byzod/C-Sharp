/* Copyright (C) 2012  Byzod
 * Contact me : byzzod@gmail.com
 * 
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/. */

using System;
using System.Collections.Generic;
using System.Linq;

namespace ImpellerVibrationChecker
{
	public partial class FrequencyChecker
	{
		public delegate List<double> AssumeDeflectionFunction(List<PhysicalSection> sections, double height);
		/// <summary>
		/// Iteration methord host
		/// </summary>
		/// <param name="impellerInput">Input impeller</param>
		/// <returns>Impeller with calculated data</returns>
		public Impeller Iteration(Impeller impellerInput)
		{
			var impeller = new Impeller(impellerInput);
			var impellerNew = new Impeller();

			//initial the Y(x) array
			//get Y(x) array
			impeller.Y = new List<double>(impeller.PSections.Count);
			for (int i = 0; i < impeller.PSections.Count; i++)
			{
				//Type 1 : Y(x) = (x / height)^2
				impeller.Y.Add( Math.Pow(impeller.PSections[i].Position / impeller.Height, 2));
			}

			int iterationCount = 0;
			while (true)
			{
				iterationCount++;
				//get new impeller Y
				impellerNew = IterationCalculater(impeller);
				impellerNew.IterationCount = iterationCount;
				//if impeller Y is precise enough or if cycled too many times, quit iteration
				if (ImpellerDifference(impeller, impellerNew) < impeller.MinTolerance
					|| iterationCount > impeller.MaxIterationCount)
				{
					impeller = impellerNew;
					break;
				}
				impeller = impellerNew;
			}

			return impeller;
		}
		/// <summary>
		/// Iteration methord host
		/// </summary>
		/// <param name="impellerInput">Input impeller</param>
		/// <returns>Impeller with calculated data</returns>
		public Impeller Iteration(Impeller impellerInput, AssumeDeflectionFunction deflectionFunc)
		{
			var impeller = new Impeller(impellerInput);
			var impellerNew = new Impeller();

			//initial the Y(x) array
			//get Y(x) array
			try
			{				
				impeller.Y = deflectionFunc(impeller.PSections, impeller.Height);
			}
			catch
			{
				throw;
			}

			int iterationCount = 0;
			while (true)
			{
				iterationCount++;
				//get new impeller Y
				impellerNew = IterationCalculater(impeller);
				impellerNew.IterationCount = iterationCount;
				//if impeller Y is precise enough or if cycled too many times, quit iteration
				if (ImpellerDifference(impeller, impellerNew) < impeller.MinTolerance
					|| iterationCount > impeller.MaxIterationCount)
				{
					impeller = impellerNew;
					break;
				}
				impeller = impellerNew;
			}

			return impeller;
		}
		
		/// <summary>
		/// Impeller vibration frequency calculater with iteration method
		/// </summary>
		/// <param name="impeller">Input impeller</param>
		/// <returns>Calculated impeller</returns>
		private Impeller IterationCalculater(Impeller impeller)
		{
			//cache result array
			var resultCache = new List<double>(new double[impeller.PSections.Count]);
			var resultCache2 = new List<double>(new double[impeller.PSections.Count]);
			var resultImpeller = new Impeller(impeller);

			// (A*Y_ij)_k or q_k/pw^2
			for (int i = 0; i < impeller.PSections.Count; i++)
			{
				resultCache[i] = impeller.Y[i] * impeller.PSections[i].Area;
			}

			//first and second integral
			for (int integralCount = 2; integralCount > 0; integralCount--)
			{
				for (int i = 0; i < impeller.PSections.Count; i++)
				{
					if (i == 0)
					{
						resultCache2[i] = 0;
					}
					else
					{
						// (2/pw^2)*q_mk  then  4Q_mk/(pw^2*Δx)
						resultCache2[i] = resultCache[i] + resultCache[i - 1];
					}
				}
				for (int i = impeller.PSections.Count - 1; i >= 0; i--)
				{
					if (i == impeller.PSections.Count - 1)
					{
						resultCache[i] = 0;
					}
					else
					{
						// 2Q_k/(pw^2*Δx) then  (4/(pw^2*Δx^2))*M_k
						resultCache[i] = resultCache[i + 1] + resultCache2[i + 1];
					}
				}
			}


			// (4E/(pw^2*Δx^2))*(ΔY')/Δx
			for (int i = 0; i < impeller.PSections.Count; i++)
			{
				resultCache[i] = resultCache[i] / impeller.PSections[i].InertiaMoment;
			}
			//third and fourth integral
			for (int integralCount = 2; integralCount > 0; integralCount--)
			{
				for (int i = 0; i < impeller.PSections.Count; i++)
				{
					if (i == 0)
					{
						resultCache2[i] = 0;
					}
					else
					{
						// (8E/(pw^2*Δx^2))*(ΔY')_m/Δx^2  then  (16E/(pw^2*Δx^3))*(ΔY')_m/Δx
						resultCache2[i] = resultCache[i] + resultCache[i - 1];
					}
				}
				for (int i = 0; i < impeller.PSections.Count; i++)
				{
					if (i == 0)
					{
						resultCache[i] = 0;
					}
					else
					{
						// (8E/(pw^2*Δx^3))*(ΔY')/Δx^2  then  (16E/(pw^2*Δx^4))*ΔY'
						resultCache[i] = resultCache[i - 1] + resultCache2[i];
					}
				}
			}

			// ΔY', i.e. impeller Y New = (cache1[i] / max(cache1) )
			double maxResult = resultCache.Max();
			for (int i = 0; i < resultCache.Count; i++)
			{
				resultCache[i] = resultCache[i] / maxResult;
			}
			resultImpeller.Y = resultCache;

			//ω = (4 / Δx^2) * (sqrt(E/(ρZ)))
			resultImpeller.LegacyVibrationFrequency = 4 / Math.Pow(impeller.PAvePieceLength, 2)
				 * Math.Sqrt(impeller.E / (impeller.Density * maxResult));
			//f = ω / 2π
			resultImpeller.LegacyVibrationFrequency /= (2 * Math.PI);

			return resultImpeller;
		}

		
		/// <summary>
		/// Check the difference between two impeller Y
		/// </summary>
		/// <param name="impellerYA">The first impeller Y array</param>
		/// <param name="impellerYB">The second impeller Y array</param>
		/// <returns>Total difference</returns>
		private double ImpellerDifference(Impeller impellerA, Impeller impellerB)
		{
			double totalMess = 0;
			if (impellerA.Y.Count != impellerB.Y.Count)
			{
				return -1;
			}
			for (int i = 0; i < impellerA.Y.Count; i++)
			{
				totalMess += Math.Abs(impellerA.Y[i] - impellerB.Y[i]);
			}
			return totalMess;
		}

	}
}
