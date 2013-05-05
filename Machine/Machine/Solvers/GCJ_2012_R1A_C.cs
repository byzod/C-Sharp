using System;
using System.Collections.Generic;
using System.Linq;

namespace Machine
{
	class GCJ_2012_R1A_C : IProblemSolver
	{
		#region Properties

		/// <summary>
		/// Should write file in binary mode
		/// </summary>
		public string ID
		{
			get
			{
				return "gcj.2012.r1a.c";
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
		/// Line Switch Solver
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
			List<List<CarState>> ccTasks = this.GetRoadStates(option.Input.Content[0]);

			// Result
			List<double> maxSafeSeconds = new List<double>(ccTasks.Count);

			// Solve
			foreach (var task in ccTasks)
			{
				RoadEnding ending = this.GetEndingOfRoad(task, new CollideTimeState(0,-1,-1));

				maxSafeSeconds.Add(
					ending.CanSafeDriveForever ?
					-1 : ending.TimeBeforeCollide
				);
			}

			// Output
			option.Output.Content.Add(new List<string>(maxSafeSeconds.Count));
			for (int i = 0; i < maxSafeSeconds.Count; i++)
			{
				option.Output.Content[0].Add(
					"Case #" + (i + 1)
					+ ": "
					+ (maxSafeSeconds[i] < 0 ?
						"Possible" :
						maxSafeSeconds[i].ToString())
				);
			}
		}

		/// <summary>
		/// Get formatted input
		/// </summary>
		/// <param name="data">String list</param>
		/// <returns>Road state list</returns>
		List<List<CarState>> GetRoadStates(List<string> data)
		{
			List<List<List<string>>> roadsStringList = ProblemSolverHelper.ConvertToCodeJamStandardStringLists(data);
			List<List<CarState>> roadStates = new List<List<CarState>>(Convert.ToInt32(data[0]));

			foreach (var roadCars in roadsStringList)
			{
				List<CarState> roadState = new List<CarState>(roadCars.Count);

				for (int i = 0; i < roadCars.Count; i++)
				{
					roadState.Add(
						new CarState(
							i,
							roadCars[i][0] == "L" ? CarState.LaneSide.Left : CarState.LaneSide.Right,
							Convert.ToInt32(roadCars[i][1]),
							Convert.ToInt32(roadCars[i][2])
						)
					);
				}

				roadStates.Add(roadState);
			}

			return roadStates;
		}

		/// <summary>
		/// Get the ending of road state
		/// </summary>
		/// <param name="roadState">Road state</param>
		/// <param name="timeState">Time state</param>
		/// <returns>Ending of road</returns>
		RoadEnding GetEndingOfRoad(List<CarState> roadState, CollideTimeState timeState)
		{
			RoadEnding ending = new RoadEnding();

			CollideTimeState aState = this.GetCollideTimeState(roadState, timeState.Time, timeState.CarAIndex);
			CollideTimeState bState = this.GetCollideTimeState(roadState, timeState.Time, timeState.CarBIndex);

			CollideTimeState maxState = null;
			bool isAStateBetter = true;

			if (this.CompareNegativeAsInfinity(aState.Time - timeState.Time, bState.Time - timeState.Time) > 0) // aState is better
			{
				maxState = aState;
				isAStateBetter = true;
			}
			else
			{
				maxState = bState;
				isAStateBetter = false;
			}

			// t > 0, should recursive call to solve
			// t = 0, it's going to crash
			// t < 0, cheers!
			if (maxState.Time - timeState.Time > 0)
			{
				if (isAStateBetter && timeState.CarAIndex >= 0)
				{
					roadState[timeState.CarAIndex].ChangeLane();
				}
				else if(timeState.CarBIndex >= 0)
				{
					roadState[timeState.CarBIndex].ChangeLane();
				}

				// Recursive call
				ending = this.GetEndingOfRoad(
					roadState,
					maxState
				);
			}
			else if (maxState.Time - timeState.Time == 0)
			{
				// BAD END
				ending.CanSafeDriveForever = false;
				ending.TimeBeforeCollide = timeState.Time;
			}
			else
			{
				// GOOD END
				ending.CanSafeDriveForever = true;
				ending.TimeBeforeCollide = -1;
			}

			return ending;
		}

		/// <summary>
		/// Get the max time before collide
		/// </summary>
		/// <param name="roadState">Road state</param>
		/// <param name="time">Time passed</param>
		/// <param name="carIndexChangingLane">Index of car that changing lane</param>
		/// <returns>Time state</returns>
		CollideTimeState GetCollideTimeState(List<CarState> roadState, double time, Int32 carIndexChangingLane)
		{
			CollideTimeState maxTimeState = new CollideTimeState(time, -1, -1);
			double?[] minTimeBeforeCollide = new double?[2];
			Int32[][] carIndex = new Int32[2][];
			carIndex[0] = new Int32[2];
			carIndex[1] = new Int32[2]; 

			// If index not in road, treat as bad end
			// But if index < 0, that means do-not-change-lane-of-any-car
			// Currently this only happend at the begainning of process
			if (carIndexChangingLane < roadState.Count)
			{
				// Try change lane
				if (carIndexChangingLane >= 0)
				{
					roadState[carIndexChangingLane].ChangeLane();
				}

				List<List<CarState>> carsOnLanes = new List<List<CarState>>(2)
				{
					(from car in roadState
					 where car.Lane == CarState.LaneSide.Left
					 orderby car.PositionAt(time)
					 select car)
					.ToList(),

					(from car in roadState
					 where car.Lane == CarState.LaneSide.Right
					 orderby car.PositionAt(time)
					 select car)
					.ToList()
				};

				for (int i = 0; i < carsOnLanes.Count; i++)
				{
					for (int j = 0; j < carsOnLanes[i].Count; j++)
					{
						if (j == carsOnLanes[i].Count - 1) // The last car
						{
							minTimeBeforeCollide[i] = (minTimeBeforeCollide[i] < 0 || !minTimeBeforeCollide[i].HasValue) ?
								-1 :
								minTimeBeforeCollide[i];
						}
						else
						{
							if (carsOnLanes[i][j].Speed > carsOnLanes[i][j + 1].Speed)
							{
								double distance = carsOnLanes[i][j + 1].PositionAt(time) > carsOnLanes[i][j].PositionAt(time) + carsOnLanes[i][j].Length ?
													carsOnLanes[i][j + 1].PositionAt(time) - (carsOnLanes[i][j].PositionAt(time) + carsOnLanes[i][j].Length)
													: 0;
								double timeBeforeCollide = distance / (carsOnLanes[i][j].Speed - carsOnLanes[i][j + 1].Speed);

								if (minTimeBeforeCollide[i].HasValue)
								{
									// Only store the minimum collide time
									if (this.CompareNegativeAsInfinity(timeBeforeCollide, minTimeBeforeCollide[i].Value) < 0)
									{
										minTimeBeforeCollide[i] = timeBeforeCollide;
										carIndex[i][0] = carsOnLanes[i][j].ID;
										carIndex[i][1] = carsOnLanes[i][j + 1].ID;
									}
								}
								else
								{
									minTimeBeforeCollide[i] = timeBeforeCollide;
									carIndex[i][0] = carsOnLanes[i][j].ID;
									carIndex[i][1] = carsOnLanes[i][j + 1].ID;
								}
							}
							else
							{
								double distance = carsOnLanes[i][j + 1].PositionAt(time) - (carsOnLanes[i][j].PositionAt(time) + carsOnLanes[i][j].Length);

								if (distance >= 0)
								{
									minTimeBeforeCollide[i] = (minTimeBeforeCollide[i] < 0 || !minTimeBeforeCollide[i].HasValue) ?
										-1 
										: minTimeBeforeCollide[i];
								}
								else
								{
									minTimeBeforeCollide[i] = 0;
								}
							}
						}
					}
				}

				// Change back
				if (carIndexChangingLane >= 0)
				{
					roadState[carIndexChangingLane].ChangeLane();
				}
			}

			bool leftLaneIsMoreDangerous = false;
			bool atLeastOneLaneIsDangerous = false;

			if (minTimeBeforeCollide[0].HasValue 
				&& minTimeBeforeCollide[1].HasValue)
			{
				atLeastOneLaneIsDangerous = true;

				if (this.CompareNegativeAsInfinity(minTimeBeforeCollide[0].Value, minTimeBeforeCollide[1].Value) < 0)
				{
					leftLaneIsMoreDangerous = true;
				}
				else
				{
					leftLaneIsMoreDangerous = false;
				}
			}
			else if(minTimeBeforeCollide[0].HasValue || minTimeBeforeCollide[1].HasValue)
			{
				atLeastOneLaneIsDangerous = true;

				if (minTimeBeforeCollide[0].HasValue)
				{
					leftLaneIsMoreDangerous = true;
				}
				else
				{
					leftLaneIsMoreDangerous = false;
				}
			}

			if (atLeastOneLaneIsDangerous)
			{
				double minTime = 0;

				if (leftLaneIsMoreDangerous)
				{
					minTime = minTimeBeforeCollide[0].Value;
					maxTimeState.CarAIndex = carIndex[0][0];
					maxTimeState.CarBIndex = carIndex[0][1];
				}
				else
				{
					minTime = minTimeBeforeCollide[1].Value;
					maxTimeState.CarAIndex = carIndex[1][0];
					maxTimeState.CarBIndex = carIndex[1][1];
				}

				if (minTime < 0)
				{
					maxTimeState.Time += -1;
				}
				else
				{
					maxTimeState.Time += minTime;
				}
			}

			return maxTimeState;
		}

		/// <summary>
		/// Compare value with rule that negative value represents infinity. Return 0 if val A == val B, 1 if val A > val B, else -1
		/// </summary>
		/// <param name="valA">Value A</param>
		/// <param name="valB">Value B</param>
		/// <returns>Compare result</returns>
		Int32 CompareNegativeAsInfinity(double valA, double valB)
		{
			Int32 aCompareToB = 0;

			if (valA < 0 && valB >= 0)
			{
				aCompareToB = 1;
			}
			else if (valB < 0 && valA >=0)
			{
				aCompareToB = -1;
			}
			else
			{
				if (valA > valB)
				{
					aCompareToB = 1;
				}
				else if(valA < valB)
				{
					aCompareToB = -1;
				}
				else
				{
					aCompareToB = 0;
				}
			}

			return aCompareToB;
		}

		class CarState
		{
			public enum LaneSide
			{
				Left = 0,
				Right = 1
			}

			public int ID { get; set; }
			public int Speed { get; set; }
			public double Position { get; set; }
			public LaneSide Lane { get; set; }
			public double Length { get { return 5; } }

			public CarState(Int32 id, LaneSide lane, Int32 speed, Int32 position)
			{
				this.ID = id;
				this.Lane = lane;
				this.Speed = speed;
				this.Position = position;
			}

			public double PositionAt(double time)
			{
				return this.Position + time * Speed;
			}

			public void ChangeLane()
			{
				if (this.Lane == LaneSide.Left)
				{
					this.Lane = LaneSide.Right;
				}
				else
				{
					this.Lane = LaneSide.Left;
				}
			}
		}

		class RoadEnding
		{
			public bool CanSafeDriveForever { get; set; }
			public double TimeBeforeCollide { get; set; }

			public RoadEnding()
			{
				this.CanSafeDriveForever = false;
				this.TimeBeforeCollide = 0;
			}
		}

		class CollideTimeState
		{
			public double Time { get; set; }
			public Int32 CarAIndex { get; set; }
			public Int32 CarBIndex { get; set; }

			public CollideTimeState(double time, Int32 carA, Int32 carB)
			{
				this.Time = time;
				this.CarAIndex = carA;
				this.CarBIndex = carB;
			}
		}
	}
}
