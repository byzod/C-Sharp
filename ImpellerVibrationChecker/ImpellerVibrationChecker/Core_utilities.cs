/* Copyright (C) 2012  Byzod
 * Contact me : byzzod@gmail.com
 * 
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/. */

using System;
using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra;

namespace ImpellerVibrationChecker
{
	/// <summary>
	/// Section base 
	/// </summary>
	public class Section
	{
		/// <summary>
		/// Initialize
		/// </summary>
		/// <param name="positon">Length to root, m</param>
		/// <param name="inertiaMoment">Moment of inerita in this section, m^4</param>
		public Section(double position, double inertiaMoment)
		{
			this.Position = position;
			this.InertiaMoment = inertiaMoment;
		}

		private double position = 0.0;
		private double inertiaMoment = 1.0;

		/// <summary>
		/// L, Length to root, m
		/// </summary>
		public double Position
		{
			get { return position; }
			set
			{
				if (value >= 0)
				{
					position = value;
				}
				else
				{
					throw new ArgumentOutOfRangeException("Position", "Attempt to set negative value");
				}
			}

		}
		/// <summary>
		/// J, Moment of inerita in this section, m^4
		/// </summary>
		public double InertiaMoment
		{
			get { return inertiaMoment; }
			set
			{
				if (value > 0)
				{
					inertiaMoment = value;
				}
				else
				{
					throw new ArgumentOutOfRangeException("InertiaMoment", "Attempt to set negative value or zero");
				}
			}
		}
	}
	/// <summary>
	/// Physical model section
	/// </summary>
	public class PhysicalSection
		: Section
	{
		/// <summary>
		/// Initialize
		/// </summary>
		/// <param name="positon">Length to root, m</param>
		/// <param name="inertiaMoment">Moment of inerita in this section, m^4</param>
		/// <param name="area">Section area, m^2</param>
		public PhysicalSection(double position, double inertiaMoment, double area)
			: base(position, inertiaMoment)
		{
			base.Position = position;
			base.InertiaMoment = inertiaMoment;
			this.Area = area;
		}
		private double area = 1.0;
		/// <summary>
		/// A, Section area, m^2
		/// </summary>
		public double Area
		{
			get { return area; }
			set
			{
				if (value > 0)
				{
					area = value;
				}
				else
				{
					throw new ArgumentOutOfRangeException("Area", "Attempt to set negative value or zero");
				}
			}
		}
	}
	/// <summary>
	/// Mechanical model section
	/// </summary>
	public class MechanicalSection
		: Section
	{
		/// <summary>
		/// Initialize
		/// </summary>
		/// <param name="position">Length to root, m</param>
		/// <param name="inertiaMoment">Moment of inerita in this section, m^4</param>
		/// <param name="mass">Section mass, kg</param>
		public MechanicalSection(double position, double inertiaMoment, double mass)
			: base(position, inertiaMoment)
		{
			base.Position = position;
			base.InertiaMoment = inertiaMoment;
			this.Mass = mass;
		}
		private double mass = 1.0;
		/// <summary>
		/// M, mass of the piece between section(n) and section(n-1), kg
		/// </summary>
		public double Mass
		{
			get { return mass; }
			set
			{
				if (value > 0)
				{
					mass = value;
				}
				else
				{
					throw new ArgumentOutOfRangeException("Mass", "Attempt to set negative value or zero");
				}
			}
		}

	}
		
	/// <summary>
	/// Full model section
	/// </summary>
	public class FullSection
		: PhysicalSection
	{
		public FullSection(double position, double inertiaMoment, double area, double mass)
			: base(position, inertiaMoment, area)
		{
			base.Position = position;
			base.InertiaMoment = inertiaMoment;
			this.Area = area;
			this.Mass = mass;
		}
			
		private double mass = 1.0;
		/// <summary>
		/// M, mass of the piece between section(n) and section(n-1), kg
		/// </summary>
		public double Mass
		{
			get { return mass; }
			set
			{
				if (value > 0)
				{
					mass = value;
				}
				else
				{
					throw new ArgumentOutOfRangeException("Mass", "Attempt to set negative value or zero");
				}
			}
		}
	}
	






	/// <summary>
	/// Impeller Object
	/// </summary>
	public class Impeller
	{
		#region Initialize
		public Impeller() { }

		public Impeller(double height, double E, double density)
		{
			this.Height = height;
			this.E = E;
			this.Density = density;
		}

		public Impeller(double height, double E, double density, List<PhysicalSection> pSections)
		{
			this.Height = height;
			this.E = E;
			this.Density = density;
			this.PSections = pSections;
		}

		public Impeller(double height, double E, double density, List<MechanicalSection> pSections)
		{
			this.Height = height;
			this.E = E;
			this.Density = density;
			this.MSections = mSections;
		}
		/// <summary>
		/// Make a copy from existed impeller
		/// </summary>
		/// <param name="impeller">Impeller copy</param>
		public Impeller(Impeller impeller)
		{
			this.Comment = impeller.Comment;
			if (impeller.Density >= 0) this.Density = impeller.Density;
			if (impeller.E >= 0) this.E = impeller.E;
			if (impeller.Height >= 0) this.Height = impeller.Height;
			if (impeller.IterationCount >= 0) this.IterationCount = impeller.IterationCount;
			if (impeller.LegacyVibrationFrequency >= 0) this.LegacyVibrationFrequency = impeller.LegacyVibrationFrequency;
			if (impeller.MaxIterationCount >= 0) this.MaxIterationCount = impeller.MaxIterationCount;
			if (impeller.MaxProhlOmegaSearchCount >= 0) this.MaxProhlOmegaSearchCount = impeller.MaxProhlOmegaSearchCount;
			if (impeller.MaxProhlSearchCount >= 0) this.MaxProhlSearchCount = impeller.MaxProhlSearchCount;
			if (impeller.MinStepDiff > 0) this.MinStepDiff = impeller.MinStepDiff;
			if (impeller.MinTolerance > 0) this.MinTolerance = impeller.MinTolerance;
			if (impeller.MSections != null) this.MSections = impeller.MSections;
			if (impeller.PSections != null) this.PSections = impeller.PSections;
			if (impeller.VibrationFrequency != null) this.VibrationFrequency = impeller.VibrationFrequency;
			if (impeller.Y != null) this.Y = impeller.Y;
		}
		#endregion Initialize


		#region Properties
		private const double DEFAULT_MIN_TOLERANCE = 1e-20;
		private const double DEFAULT_MIN_STEP_DIFF = 1e-10;
		private const int DEFAULT_MAX_ITERATION_COUNT = 10000;
		private const int DEFAULT_MAX_PROHL_SEARCH_COUNT = 2000;
		private const int DEFAULT_MAX_PROHL_OMEGA_SEARCH_COUNT = 500;



		private double height = 1.0;
		private double e = 1.0;
		private double density = 1.0;
		private int iterationCount = 0;
		private List<double> vibrationFrequency = new List<double>();
		private double legacyVibrationFrequency = 0.0;
		private List<double> y = new List<double>();
		private List<MechanicalSection> mSections = new List<MechanicalSection>();
		private List<PhysicalSection> pSections = new List<PhysicalSection>();
		private double minTolerance = DEFAULT_MIN_TOLERANCE;
		private double minStepDiff = DEFAULT_MIN_STEP_DIFF;
		private int maxIterationCount = DEFAULT_MAX_ITERATION_COUNT;
		private int maxProhlSearchCount = DEFAULT_MAX_PROHL_SEARCH_COUNT;
		private int maxProhlOmegaSearchCount = DEFAULT_MAX_PROHL_OMEGA_SEARCH_COUNT;


		/// <summary>
		/// Height of impeller, meter
		/// </summary>
		public double Height
		{
			get { return height; }
			set
			{
				if (value > 0)
				{
					height = value;
				}
				else
				{
					throw new ArgumentOutOfRangeException("Height", "Attempt to set negative value or zero");
				}
			}
		}
		/// <summary>
		/// Modulus of elasticity, kg/m^2
		/// </summary>
		public double E
		{
			get { return e; }
			set
			{
				if (value > 0)
				{
					e = value;
				}
				else
				{
					throw new ArgumentOutOfRangeException("E", "Attempt to set negative value or zero");
				}
			}
		}
		/// <summary>
		/// Density, kg/m^3
		/// </summary>
		public double Density
		{
			get { return density; }
			set
			{
				if (value > 0)
				{
					density = value;
				}
				else
				{
					throw new ArgumentOutOfRangeException("Density", "Attempt to set negative value or zero");
				}
			}
		}
		/// <summary>
		/// How many times the caculation method runs
		/// </summary>
		public int IterationCount
		{
			get { return iterationCount; }
			set
			{
				if (value >= 0)
				{
					iterationCount = value;
				}
				else
				{
					throw new ArgumentOutOfRangeException("IterationCount", "Attempt to set negative value");
				}
			}
		}
		/// <summary>
		/// Natural vibration frequency, Hz
		/// </summary>
		public List<double> VibrationFrequency
		{
			get { return vibrationFrequency; }
			set
			{
				if (value != null)
				{
					vibrationFrequency = value;
				}
				else
				{
					throw new ArgumentNullException("VibrationFrequency");
				}
			}
		}
		/// <summary>
		/// Natural vibration frequency calculated by legacy method, Hz
		/// </summary>
		public double LegacyVibrationFrequency
		{
			get { return legacyVibrationFrequency; }
			set
			{
				if (value >= 0)
				{
					legacyVibrationFrequency = value;
				}
				else
				{
					throw new ArgumentOutOfRangeException("LegacyVibrationFrequency", "Attempt to set negative value");
				}
			}
		}
		/// <summary>
		/// Deflection list of impeller
		/// </summary>
		public List<double> Y
		{
			get { return y; }
			set
			{
				if (value != null)
				{
					y = value;
				}
				else
				{
					throw new ArgumentNullException("Y");
				}
			}
		}
		/// <summary>
		/// Ordered mechanical sections data
		/// </summary>
		public List<MechanicalSection> MSections
		{
			get { return mSections; }
			set
			{
				if (value != null)
				{
					mSections = value;
				}
				else
				{
					throw new ArgumentNullException("MSections");
				}
			}
		}
		/// <summary>
		/// Ordered physical sections data
		/// </summary>
		public List<PhysicalSection> PSections
		{
			get { return pSections; }
			set
			{
				if (value != null)
				{
					pSections = value;
				}
				else
				{
					throw new ArgumentNullException("PSections");
				}
			}
		}
		/// <summary>
		/// Average length of mechanical section piece, readonly, meter
		/// </summary>
		public double MAvePieceLength
		{
			get
			{
				if (MSections.Count > 0)
				{
					return Height / MSections.Count;
				}
				else
				{
					throw new DivideByZeroException();
				}
			}
		}
		/// <summary>
		/// Average length of Phycial section piece, readonly, meter
		/// </summary>
		public double PAvePieceLength
		{
			get
			{
				if (PSections.Count - 1 > 0)
				{
					return Height / (PSections.Count - 1);
				}
				else
				{
					throw new DivideByZeroException();
				}
			}
		}
		/// <summary>
		/// The minimum difference count required for iteration method. Must be positive.
		/// <para>When the total difference count is lesser than this, consider it's the proper answer.</para>
		/// </summary>
		public double MinTolerance
		{
			get { return minTolerance; }
			set
			{
				if (value > 0)
				{
					minTolerance = value;
				}
				else
				{
					throw new ArgumentOutOfRangeException("MinTolerance", "Attempt to set negative value or zero");
				}
			}
		}
		/// <summary>
		/// The minimum step difference required for Prohl method. Must be positive.
		/// <para>When the step is lesser than this, consider it's the proper answer.</para>
		/// </summary>
		public double MinStepDiff
		{
			get { return minStepDiff; }
			set
			{
				if (value > 0)
				{
					minStepDiff = value;
				}
				else
				{
					throw new ArgumentOutOfRangeException("MinStepDiff", "Attempt to set negative value or zero");
				}
			}
		}
		/// <summary>
		/// The maximum cycle count for iteration method.
		/// <para>When cycled times is more than this, quit the iteration simulation.</para>
		/// </summary>
		public int MaxIterationCount
		{
			get { return maxIterationCount; }
			set
			{
				if (value >= 0)
				{
					maxIterationCount = value;
				}
				else
				{
					throw new ArgumentOutOfRangeException("MaxIterationCount", "Attempt to set negative value");
				}
			}
		}
		/// <summary>
		/// The maximum search count for Prohl method main seach.
		/// <para>When search times is more than this, quit the step search.</para>
		/// </summary>
		public int MaxProhlSearchCount
		{
			get { return maxProhlSearchCount; }
			set
			{
				if (value >= 0)
				{
					maxProhlSearchCount = value;
				}
				else
				{
					throw new ArgumentOutOfRangeException("MaxProhlSearchCount", "Attempt to set negative value");
				}
			}
		}
		/// <summary>
		/// The maximum search count for Prohl method omega search.
		/// <para>When search times is more than this, quit the omega search.</para>
		/// </summary>
		public int MaxProhlOmegaSearchCount
		{
			get { return maxProhlOmegaSearchCount; }
			set
			{
				if (value >= 0)
				{
					maxProhlOmegaSearchCount = value;
				}
				else
				{
					throw new ArgumentOutOfRangeException("MaxProhlOmegaSearchCount", "Attempt to set negative value");
				}
			}
		}
		/// <summary>
		/// Comment string
		/// </summary>
		public string Comment { get; set; }



		#endregion Properties

		#region Methods

		#endregion Methods
	}



	/// <summary>
	/// State object for Prohl method
	/// </summary>
	public class ProhlState
	{
		private double omega = 0.0;
		private double prohlStep = 40.0 * Math.PI;
		private double checkFromOmega = 0.0;
		private double checkToOmega = 0.0;
		private Matrix endStateVector = new Matrix(new double[][]{
				new double[] { 1 },
				new double[] { 1 },
				new double[] { 0 },
				new double[] { 0 },
			}); //free impeller

		/// <summary>
		/// ω, the angular velocity. rad/s
		/// </summary>
		public double Omega
		{
			get { return omega; }
			set
			{
				if (value >= 0)
				{
					omega = value;
				}
				else
				{
					throw new ArgumentOutOfRangeException("Omega", "Attempt to set negative value");
				}
			}
		}
		/// <summary>
		/// Frequency search step in Prohl method, must be positive. rad/s
		/// </summary>
		public double ProhlStep
		{
			get { return prohlStep; }
			set
			{
				if (value > 0)
				{
					prohlStep = value;
				}
				else
				{
					throw new ArgumentOutOfRangeException("ProhlStep", "Attempt to set negative value or zero");
				}
			}
		}
		/// <summary>
		/// The determinant of the partial judge matrix, 1
		/// </summary>
		public double residualMoment { get; set; }
		/// <summary>
		/// Search start form this angular velocity. rad/s
		/// </summary>
		public double CheckFromOmega
		{
			get { return checkFromOmega; }
			set
			{
				if (value >= 0)
				{
					checkFromOmega = value;
				}
				else
				{
					throw new ArgumentOutOfRangeException("CheckFromOmega", "Attempt to set negative value");
				}
			}
		}
		/// <summary>
		/// Search below this angular velocity. rad/s
		/// </summary>
		public double CheckToOmega
		{
			get { return checkToOmega; }
			set
			{
				if (value >= 0)
				{
					checkToOmega = value;
				}
				else
				{
					throw new ArgumentOutOfRangeException("CheckToOmega", "Attempt to set negative value");
				}
			}
		}
		/// <summary>
		/// The process state vector, 4x1 matrix
		/// </summary> 
		public Matrix StateVector { get; set; }
		/// <summary>
		/// The impeller end state vector, 4x1 matrix, only two number can be 1
		/// </summary>
		public Matrix EndStateVector
		{
			get { return endStateVector; }
			set
			{
				if (value.ColumnCount == 1 && value.RowCount == 4
					&& value[0, 0] + value[1, 0] + value[2, 0] + value[3, 0] == 2)
				{
					endStateVector = value;
				}
				else
				{
					throw new ArgumentOutOfRangeException("EndStateVector", "Invalid vector");
				}
			}
		}
		/// <summary>
		/// The transfer matrix, 4x4
		/// </summary>
		public Matrix TransferMatrix { get; set; }
		/// <summary>
		/// Hold the temporary angular velocity point
		/// </summary>
		public double TempOmegaPoint { get; set; }

	}
}
