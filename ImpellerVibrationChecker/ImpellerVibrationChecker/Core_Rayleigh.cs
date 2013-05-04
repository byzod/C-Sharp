/* Copyright (C) 2012  Byzod
 * Contact me : byzzod@gmail.com
 * 
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/. */

using System;
using System.Collections.Generic;

namespace ImpellerVibrationChecker
{
	public partial class FrequencyChecker
	{
		/// <summary>
		/// Rayleigh methord host
		/// </summary>
		/// <param name="impellerInput">Input impeller</param>
		/// <param name="g">Gravitational acceleration, m/s^2</param>
		/// <returns>Impeller with calculated data</returns>
		public Impeller Rayleigh(Impeller impellerInput, double g)
		{
			var impeller = RayleighCalculater(impellerInput, g);

			double uTemp = 0;
			double dTemp = 0;
			for (var i = 0; i < impeller.MSections.Count; i++)
			{
				uTemp += impeller.MSections[i].Mass * impeller.Y[i];
				dTemp += impeller.MSections[i].Mass * Math.Pow(impeller.Y[i], 2);
			}
			//ω = sqrt( g * ∑(mi*Yi) / ∑(mi*Yi^2) )
			impeller.LegacyVibrationFrequency = Math.Sqrt(g * uTemp / dTemp) / (2 * Math.PI);

			return impeller;
		}

		/// <summary>
		/// Impeller vibration frequency calculater with Rayleigh method
		/// </summary>
		/// <param name="impeller">Input impeller</param>
		/// <returns>Calculated impeller</returns>
		private Impeller RayleighCalculater(Impeller impeller, double g)
		{
			//cache result array
			var resultCache = new List<double>(new double[impeller.MSections.Count]);
			var resultCache2 = new List<double>(new double[impeller.MSections.Count]);

			for (int i = 0; i < impeller.MSections.Count; i++)
			{
				if (i == 0)
				{
					resultCache[i] = impeller.MSections[i+1].Mass / impeller.MSections[i+1].Position;
				}
				else
				{
					resultCache[i] = impeller.MSections[i].Mass
						/ (impeller.MSections[i].Position - impeller.MSections[i-1].Position);
				}
			}

			//first and second integral
			for (int integralCount = 2; integralCount > 0; integralCount--)
			{
				for (int i = 0; i < impeller.MSections.Count; i++)
				{
					if (i == 0)
					{
						resultCache2[i] = 0;
					}
					else
					{
						resultCache2[i] = (resultCache[i] + resultCache[i - 1]) / 2;
					}
				}
				for (int i = impeller.MSections.Count - 1; i >= 0; i--)
				{
					if (i == impeller.MSections.Count - 1)
					{
						resultCache[i] = 0;
					}
					else
					{
						resultCache[i] = resultCache[i + 1] 
							+ resultCache2[i + 1] * (impeller.MSections[i + 1].Position - impeller.MSections[i].Position);
					}
				}
			}


			for (int i = 0; i < impeller.MSections.Count; i++)
			{
				resultCache[i] = resultCache[i] / impeller.MSections[i].InertiaMoment;
			}
			//third and fourth integral
			for (int integralCount = 2; integralCount > 0; integralCount--)
			{
				for (int i = 0; i < impeller.MSections.Count; i++)
				{
					if (i == 0)
					{
						resultCache2[i] = 0;
					}
					else
					{
						resultCache2[i] = (resultCache[i] + resultCache[i - 1]) / 2;
					}
				}
				for (int i = 0; i < impeller.MSections.Count; i++)
				{
					if (i == 0)
					{
						resultCache[i] = 0;
					}
					else
					{
						resultCache[i] = resultCache[i - 1]
							+ resultCache2[i] * (impeller.MSections[i].Position - impeller.MSections[i-1].Position);
					}
				}
			}


			for (var i = 0; i < resultCache.Count; i++)
			{
				resultCache[i] = resultCache[i] * g / impeller.E;
			}

			impeller.Y = resultCache;

			return impeller;
		}
	}
}
