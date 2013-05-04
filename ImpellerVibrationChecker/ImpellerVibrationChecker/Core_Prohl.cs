/* Copyright (C) 2012  Byzod
 * Contact me : byzzod@gmail.com
 * 
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/. */

using System;
using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra;

#if TRACER
using System.Diagnostics;
#endif

namespace ImpellerVibrationChecker
{
	public partial class FrequencyChecker
	{
		public delegate void ProhlOmegaChangeHandler(Impeller impeller, ProhlState state);
		/// <summary>
		/// Fire after one calculation, when the checking omega is changed
		/// </summary>
		public event ProhlOmegaChangeHandler ProhlOmega_Changed;
		/// <summary>
		/// Prohl matrix methord host
		/// </summary>
		/// <param name="impellerInput">Input impeller</param>
		/// <param name="state">Some settings and states</param>
		/// <returns>Impeller with calculated data</returns>
		public Impeller Prohl(Impeller impellerInput, ProhlState state)
		{	
			var impeller = new Impeller(impellerInput);
			var freqPoints = new Dictionary<double,int>();
			int mainLoopCount = 0;
			state.Omega = state.CheckFromOmega;
			state.residualMoment = 0;

			if (state.Omega > state.CheckToOmega)
			{
				ProhlCalculator(impeller, state);
			}
			else
			{
				int anchorSign; //store the residualMoment sign
				double anchor; //store the freq
				double determinantAnchor; //store the residualMoment
				double freqPoint;
				while (state.Omega <= state.CheckToOmega && mainLoopCount++ < impeller.MaxProhlSearchCount)
				{
					//search the frequency point
					anchorSign = Math.Sign(state.residualMoment);
					ProhlCalculator(impeller, state);

					if (Math.Sign(state.residualMoment) * anchorSign < 0)
					{
						anchor = state.Omega;
						determinantAnchor = state.residualMoment;
						
						//find freq point
						freqPoint = ProhlFound(impeller, state);

						if (impeller.VibrationFrequency.IndexOf(freqPoint) < 0)
						{
							impeller.VibrationFrequency.Add(freqPoint);
							freqPoints.Add(freqPoint, impeller.IterationCount);
						}	
						state.Omega = anchor + state.ProhlStep;
						state.residualMoment = determinantAnchor;
						if (ProhlOmega_Changed != null)
						{
							ProhlOmega_Changed(impeller, state);
							//System.Windows.MessageBox.Show("");
						}
					}
					else
					{
						state.Omega += state.ProhlStep;
						if (ProhlOmega_Changed != null)
						{
							ProhlOmega_Changed(impeller, state);
							//System.Windows.MessageBox.Show("");
						}
					}
				}
			}
			impeller.VibrationFrequency = new List<double>();
			foreach( var freqPoint in freqPoints){
				impeller.VibrationFrequency.Add(freqPoint.Key / (2 * Math.PI));
			}
			impeller.IterationCount = mainLoopCount;

			return impeller;
		}

		/// <summary>
		/// Iteration frequency founder
		/// </summary>
		/// <param name="impeller">Input impeller</param>
		/// <param name="state">State object</param>
		private double ProhlFound(Impeller impeller, ProhlState state)
		{
			int searchCount = 0;
			double step = state.ProhlStep;
			int jumpSign = Math.Sign(state.residualMoment);
			int landSign = jumpSign;
			int direction = -1; // -1 = backwards , 1 = forwards

			while (step > impeller.MinStepDiff && searchCount < impeller.MaxProhlOmegaSearchCount)
			{
				step /= 2;
				state.Omega += direction * step;

				//caculate transfer matrix and residual moment 
				ProhlCalculator(impeller, state);

				landSign = Math.Sign(state.residualMoment);

				if (jumpSign != landSign)
				{
					direction *= -1;
					jumpSign = landSign;
				}
				searchCount++;
			}
			impeller.IterationCount = searchCount;
			state.TempOmegaPoint = state.Omega;
			return state.Omega;
		}
#if TRACER
		Stopwatch watch = new Stopwatch();
#endif
		/// <summary>
		/// Impeller vibration frequency calculater with Prohl method
		/// </summary>
		/// <param name="impeller">Input impeller</param>
		/// <param name="state">State object</param>
		private void ProhlCalculator(Impeller impeller, ProhlState state)
		{
#if TRACER
			watch.Start();
#endif
			// the non-zero numbers of root state vector
			const int ROOT_STATE_VECTOR_SIZE = 2;
			var transferMatrix = new Matrix(4, 4);
			var residualMatrix = new Matrix(2, 2);
			//vibration frequency order
			var rootStateVector = new Matrix[2]{				
				new Matrix(
					new double[][]{
						new double[] {0},
						new double[] {0},
						new double[] {1},
						new double[] {0}
					}
				),
				new Matrix(
					new double[][]{
						new double[] {0},
						new double[] {0},
						new double[] {0},
						new double[] {1}
					}
				)
			};
			//section transfer matrix and hole transfer matrix(from root to end)
			var sectionTransferMatrix = new Matrix(
				new double[][]{
					new double[] {1,1,1,1},
					new double[] {0,1,1,1},
					new double[] {0,0,1,1},
					new double[] {1,1,1,1}
				}
			);

#if TRACER
			long step1 = watch.ElapsedTicks;
#endif
			double freq2 = state.Omega * state.Omega;
			for (var j = 0; j < ROOT_STATE_VECTOR_SIZE; j++)
			{
				for (var i = 0; i < impeller.MSections.Count; i++)
				{
					double L = (i == 0 ? impeller.MSections[0].Position
						: impeller.MSections[i].Position - impeller.MSections[i - 1].Position);
					//L_EJ = L / EJ
					double L_EJ = L / (impeller.E * impeller.MSections[i].InertiaMoment);
					double m = impeller.MSections[i].Mass;
					//freq2xm = freq2
					double freq2xm = freq2 * m;

					sectionTransferMatrix[3, 0] = freq2xm;

					sectionTransferMatrix[0, 1] = L;
					sectionTransferMatrix[3, 1] = freq2xm * L;

					sectionTransferMatrix[0, 2] = L * L_EJ / 2;
					sectionTransferMatrix[1, 2] = L_EJ;
					sectionTransferMatrix[3, 2] = freq2xm * L * L_EJ / 2;

					sectionTransferMatrix[0, 3] = L * L * L_EJ / 6;
					sectionTransferMatrix[1, 3] = L * L_EJ / 2;
					sectionTransferMatrix[2, 3] = L;
					sectionTransferMatrix[3, 3] = 1 + freq2xm * L * L * L_EJ / 6;

					transferMatrix = (i == 0 ? sectionTransferMatrix.Clone()
						: sectionTransferMatrix.Multiply(transferMatrix));
				}

				state.TransferMatrix = transferMatrix;
				state.StateVector = transferMatrix.Multiply(rootStateVector[j]);

				for (var i = 0; i < state.EndStateVector.RowCount; i++)
				{
					if (state.EndStateVector[i, 0] == 0 )
					{
						if (residualMatrix[0, j] == 0)
						{
							residualMatrix[0, j] = state.StateVector[i, 0];
							continue;
						}
						residualMatrix[1, j] = state.StateVector[i, 0];
						break;
					}
				}
			}
#if TRACER
			long step2 = watch.ElapsedTicks;
			watch.Reset();
			System.Windows.MessageBox.Show(
				"Run time" +
				"\nstep1: " + step1 +
				"\nstep2: " + (step2 - step1)
			);
#endif
			state.residualMoment = residualMatrix.Determinant();
		}
	}
}