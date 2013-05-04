/* Copyright (C) 2012  Byzod
 * Contact me : byzzod@gmail.com
 * 
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/. */

#define PROHL

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml.Linq;
using ByzodToolkit;

namespace ImpellerVibrationChecker
{
	/// <summary>
	/// MainWindow.xaml 的交互逻辑
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
			InitializeAppArgs();
		}

		private void InitializeAppArgs()
		{
			IVCMain.Title =
				App.ResourceAssembly.GetName().Name + " "
				+ App.ResourceAssembly.GetName().Version.ToString()
#if BETA
				+ " Beta"
#endif
			;
			sectionsPanel.Width = new GridLength(0);
			configPanel.Width = new GridLength(1, GridUnitType.Star);
			resultPanel.Width = new GridLength(1, GridUnitType.Star);
			this.Impeller = new ImpellerData();
			this.Checker = new FrequencyChecker();
			this.State = new ReadyState();
			this.Config = new XmlConfig();
			Impeller.SavePath = null;
			UpdateArgsBindings();
			methodChoice.SelectedIndex = 0;
			supportTypeFree.IsChecked = true;
			Checker.ProhlOmega_Changed += ShowProgressBar;
			progressBar.Visibility = System.Windows.Visibility.Hidden;
			methodChoice.Items.Add(CalculateMethod.Rayleigh);
			methodChoice.Items.Add(CalculateMethod.Iteration);
			methodChoice.Items.Add(CalculateMethod.Prohl);
			ShowStatusMsg("初始化完毕");

#if DEBUG
			Impeller.InnerImpeller.Height = 0.3;
			Impeller.InnerImpeller.Density = 5e3;
			Impeller.InnerImpeller.E = 2e10;
			Impeller.State.ProhlStep = 50 * 2 * Math.PI;
			Impeller.State.CheckToOmega = 2000 * 2 * Math.PI;

			var testSections = new List<FullSection>(10);
			for (int i = 0; i < 10; i++)
			{
				testSections.Add(new FullSection((i + 1) * 0.3 / 10, 1e-8, 5e-4, 0.075));
			}
			foreach (var section in testSections)
			{
				Impeller.Sections.Add(section);
			}
#endif
		}

		#region Properties
		public ImpellerData Impeller { get; set; }
		public FrequencyChecker Checker { get; set; }
		public ReadyState State { get; set; }
		public XmlConfig Config { get; set; }
		public CalculateMethod ResultMethod { get; set; }
		#endregion Properties


		#region UI Handler
		private void UpdateUI()
		{
			impellerHeight.Text = Impeller.InnerImpeller.Height.ToString();
			impellerDensity.Text = Impeller.InnerImpeller.Density.ToString();
			impellerE.Text = Impeller.InnerImpeller.E.ToString();
			methodFromFreq.Text = (Impeller.State.CheckFromOmega / (2 * Math.PI)).ToString();
			methodToFreq.Text = (Impeller.State.CheckToOmega / (2 * Math.PI)).ToString();
			switch ((CalculateMethod)methodChoice.SelectedItem)
			{
				case CalculateMethod.Rayleigh:
					break;
				case CalculateMethod.Iteration:
					methodMinTor.Text = Impeller.InnerImpeller.MinTolerance.ToString();
					break;
				case CalculateMethod.Prohl:
					methodMinTor.Text = Impeller.InnerImpeller.MinStepDiff.ToString();
					break;
				default:
					break;
			}
			methodStep.Text = (Impeller.State.ProhlStep / (2 * Math.PI)).ToString();
			if (Impeller.State.EndStateVector[0, 0] == 1)
			{
				supportTypeFree.IsChecked = true;
			}
			else
			{
				supportTypePinned.IsChecked = true;
			}
			sectionsData.Items.Refresh();
			ShowStatusMsg("");
		}

		private void sectionsBtn_Click(object sender, RoutedEventArgs e)
		{
			sectionsPanel.Width = new GridLength(1, GridUnitType.Star);
			resultCanvas.Visibility = System.Windows.Visibility.Hidden;
			configPanel.Width = new GridLength(0);
			resultPanel.Width = new GridLength(0);

		}
		private void updateSectionsDataBtn_Click(object sender, RoutedEventArgs e)
		{
			sectionsPanel.Width = new GridLength(0);
			resultCanvas.Visibility = System.Windows.Visibility.Visible;
			configPanel.Width = new GridLength(1, GridUnitType.Star);
			resultPanel.Width = new GridLength(1, GridUnitType.Star);
		}
		private void resizeBtn_Click(object sender, RoutedEventArgs e)
		{
			var senderBtn = (Button)sender;
			if (senderBtn.Tag.ToString() == "small")
			{
				senderBtn.Content = "还原窗口";
				senderBtn.Tag = "big";
				configPanel.Width = new GridLength(0);
			}
			else
			{
				senderBtn.Content = "扩大窗口";
				senderBtn.Tag = "small";
				configPanel.Width = new GridLength(1, GridUnitType.Star);
			}
		}
		private void textBoxSelectionGlow(object sender, RoutedEventArgs e)
		{
			if (e.Source.GetType() == typeof(TextBox))
			{
				if (e.RoutedEvent.Name == "GotFocus")
				{
					var glowEffect = new System.Windows.Media.Effects.DropShadowEffect();
					Color color = (Color)ColorConverter.ConvertFromString("#4989FF");
					glowEffect.BlurRadius = 15;
					glowEffect.Color = color;
					glowEffect.Opacity = 0.3;
					glowEffect.ShadowDepth = 0;
					((TextBox)e.Source).Effect = glowEffect;
				}
				if (e.RoutedEvent.Name == "LostFocus")
				{
					((TextBox)e.Source).Effect = null;
				}
			}
		}

		private void addSectionCountBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			var ctBox = (TextBox)sender;
			int count;
			try
			{
				count = Convert.ToInt32(ctBox.Text);
				ctBox.BorderThickness = new Thickness(1);
				ctBox.BorderBrush = (Brush)FindResource("DefaultTextBoxBorderBrush");
				ctBox.ToolTip = null;
				if (count < 0)
				{
					ctBox.BorderThickness = new Thickness(1.5);
					ctBox.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#CCD80000"));
					ctBox.ToolTip = "个数不能为负";
					count = 0;
				}
			}
			catch
			{
				ctBox.BorderThickness = new Thickness(1.5);
				ctBox.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#CCD80000"));
				ctBox.ToolTip = "请输入正确的数量";
				count = 0;
			}
			ctBox.Tag = count;
		}
		private void methodChoice_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			switch ((CalculateMethod)methodChoice.SelectedItem)
			{
				case CalculateMethod.Rayleigh:
					methodStep.IsEnabled = false;
					supportTypePinned.IsEnabled = false;
					supportTypeFree.IsEnabled = false;
					methodFromFreq.IsEnabled = false;
					methodToFreq.IsEnabled = false;
					methodMinTor.IsEnabled = false;
					impellerHeight.IsEnabled = false;
					impellerDensity.IsEnabled = false;
					break;
				case CalculateMethod.Iteration:
					methodStep.IsEnabled = false;
					supportTypePinned.IsEnabled = false;
					supportTypeFree.IsEnabled = false;
					methodFromFreq.IsEnabled = false;
					methodToFreq.IsEnabled = false;
					methodMinTor.IsEnabled = true;
					impellerHeight.IsEnabled = true;
					impellerDensity.IsEnabled = true;
					break;
				case CalculateMethod.Prohl:
					methodStep.IsEnabled = true;
					supportTypePinned.IsEnabled = true;
					supportTypeFree.IsEnabled = true;
					methodFromFreq.IsEnabled = true;
					methodToFreq.IsEnabled = true;
					methodMinTor.IsEnabled = true;
					impellerHeight.IsEnabled = false;
					impellerDensity.IsEnabled = false;
					break;
				default:
					break;
			}
			UpdateUI();
		}
		private void supportType_Checked(object sender, RoutedEventArgs e)
		{
			if (supportTypeFree.IsChecked == true)
			{
				Impeller.State.EndStateVector = new MathNet.Numerics.LinearAlgebra.Matrix(new double[][]{
					new double[] { 1 },
					new double[] { 1 },
					new double[] { 0 },
					new double[] { 0 }
				});
			}
			if (supportTypePinned.IsChecked == true)
			{
				Impeller.State.EndStateVector = new MathNet.Numerics.LinearAlgebra.Matrix(new double[][]{
					new double[] { 0 },
					new double[] { 1 },
					new double[] { 0 },
					new double[] { 1 }
				});
			}
		}

		private string ShowResult(Impeller impeller, CalculateMethod method)
		{
			string resultString = "";
			StringBuilder yString = new StringBuilder();

			switch (method)
			{
				case CalculateMethod.Rayleigh:
					for (int i = 0; i < Impeller.InnerImpeller.Y.Count; i++)
					{
						yString
							.Append(Impeller.InnerImpeller.MSections[i].Position.ToString())
							.Append(" : ")
							.Append(Impeller.InnerImpeller.Y[i].ToString())
							.Append("\n");
					}
					resultString = "计算方法：Rayleigh法"
						+ "\n一阶固有频率：" + Impeller.InnerImpeller.LegacyVibrationFrequency.ToString() + " Hz"
						+ "\n\n振型（位置，单位m : 振幅，单位m）\n"
						+ yString.ToString();
					break;
				case CalculateMethod.Iteration:
					for (int i = 0; i < Impeller.InnerImpeller.Y.Count; i++)
					{
						yString
							.Append(Impeller.InnerImpeller.PSections[i].Position.ToString())
							.Append(" : ")
							.Append(Impeller.InnerImpeller.Y[i].ToString())
							.Append("\n");
					}
					resultString = "计算方法：振型迭代法"
						+ "\n迭代次数：" + Impeller.InnerImpeller.IterationCount
						+ "\n一阶固有频率: " + Impeller.InnerImpeller.LegacyVibrationFrequency.ToString() + " Hz"
						+ "\n\n振型（位置，单位m : 振幅，相对值）\n"
						+ yString.ToString();
					break;
				case CalculateMethod.Prohl:
					int power = 1;
					resultString = "计算方法：Prohl传递矩阵法"
						+ "\n检测次数：" + (Impeller.InnerImpeller.IterationCount + 1).ToString();
					foreach (var freq in Impeller.InnerImpeller.VibrationFrequency)
					{
						resultString += "\n" + (power++) + "阶固有频率: " + freq + " Hz";
					}
					break;
				default:
					break;
			}

			return resultString;
		}
		private void CalcAbortedMessageShower()
		{
			MessageBox.Show(
				"存在以下错误，请检查后再运行"
				+ (State.IsArgsOK ? "" : "\n*叶片参数或计算参数有误")
				+ (State.IsSectionEnough ? "" : "\n*截面数量过少")
				+ (State.IsSectionsDataOK ? "" : "\n*截面参数有误")
				, "计算中止"
			);
		}
		private void ResultCanvasPainter(CalculateMethod method)
		{
			resultCanvas.Children.Clear();
			Brush fillBrush = (Brush)FindResource("ResultLineGradientBlue");
			double height = resultCanvas.ActualHeight;
			double width = resultCanvas.ActualWidth;
			double dotDiameter = width * 0.05;
			if (dotDiameter > 20) dotDiameter = 20;
			double axisGap = 18;
			int power = 0;

			Polyline yLine = new Polyline();
			yLine.Stroke = fillBrush;
			List<Ellipse> dots = new List<Ellipse>();
			List<AxisItem> xAxisItems = new List<AxisItem>();
			List<AxisItem> yAxisItems = new List<AxisItem>();

			Polyline xAxis = new Polyline();
			Polyline yAxis = new Polyline();
			xAxis.Points.Add(new Point(0, height - axisGap));
			xAxis.Points.Add(new Point(width, height - axisGap));
			xAxis.Stroke = new SolidColorBrush(Colors.Black);
			yAxis.Points.Add(new Point(axisGap, 0));
			yAxis.Points.Add(new Point(axisGap, height));
			yAxis.Stroke = new SolidColorBrush(Colors.Black);

			TextBlock yHeader = new TextBlock();
			TextBlock xAxisHeader = new TextBlock();
			xAxisHeader.FontSize = 12;
			TextBlock yAxisHeader = new TextBlock();
			yAxisHeader.FontSize = 12;
			
			Canvas.SetLeft(yHeader, width * 0.382);
			Canvas.SetLeft(yLine, axisGap);
			Canvas.SetBottom(xAxisHeader, axisGap + 3);
			Canvas.SetRight(xAxisHeader, 0);
			Canvas.SetTop(yAxisHeader, -axisGap);
			switch (method)
			{
				case CalculateMethod.Rayleigh:
					if (Impeller.InnerImpeller.Y.Count <= 0) return;
					yLine.Points = GetPointsFromY(Impeller.InnerImpeller, height - axisGap, width - axisGap);
					xAxisItems = GetAxisItemsFromDoubleList(Impeller.Sections.Select(sec => sec.Position).ToList(), AxisType.X, new Point(axisGap, height - axisGap), width - axisGap, showMin:false);
					yAxisItems = GetAxisItemsFromDoubleList(Impeller.InnerImpeller.Y, AxisType.Y, new Point(axisGap, height - axisGap), height - axisGap, showMax:false);
					yHeader.Text = "振型曲线";
					xAxisHeader.Text = "Position(m)";
					yAxisHeader.Text = "Y(m)";
					break;
				case CalculateMethod.Iteration:
					if (Impeller.InnerImpeller.Y.Count <= 0) return;
					yLine.Points = GetPointsFromY(Impeller.InnerImpeller, height - axisGap, width - axisGap);
					xAxisItems = GetAxisItemsFromDoubleList(Impeller.Sections.Select(sec => sec.Position).ToList(), AxisType.X, new Point(axisGap, height - axisGap), width - axisGap, showMin: false);
					yAxisItems = GetAxisItemsFromDoubleList(Impeller.InnerImpeller.Y, AxisType.Y, new Point(axisGap, height - axisGap), height - axisGap);
					yHeader.Text = "振型曲线";
					xAxisHeader.Text = "Position(m)";
					yAxisHeader.Text = "Y(1)";
					break;
				case CalculateMethod.Prohl:
					if (Impeller.InnerImpeller.VibrationFrequency.Count <= 0) return;
					double heightScale = (height - axisGap - dotDiameter) / (Impeller.State.CheckToOmega / (2 * Math.PI));
					List<double> freqRange = new List<double>(100);
					for (int i = 0; i <= 100; i++)
					{
						freqRange.Add(Impeller.State.CheckFromOmega / (2 * Math.PI) 
							+ i * (Impeller.State.CheckToOmega - Impeller.State.CheckFromOmega) / (2 * Math.PI * 100));
					}
					yAxisItems = GetAxisItemsFromDoubleList(freqRange, AxisType.Y, new Point(axisGap, height - axisGap), height - axisGap);
					yHeader.Text = "各阶固有频率分布";
					xAxisHeader.Text = "阶数";
					yAxisHeader.Text = "频率(Hz)";

					foreach (var freq in Impeller.InnerImpeller.VibrationFrequency)
					{
						var freqDot = new Ellipse();
						var axisItem = new AxisItem();
						double xPosition = 0;
						freqDot.Height = dotDiameter;
						freqDot.Width = dotDiameter;
						freqDot.Fill = fillBrush;
						freqDot.ToolTip = "第" + (++power) + "阶\n" + freq + " Hz";
						freqDot.MouseEnter += new MouseEventHandler(freqDot_MouseEnter);
						freqDot.MouseLeave += new MouseEventHandler(freqDot_MouseLeave);

						xPosition = axisGap + (width - axisGap - dotDiameter) * power / (Impeller.InnerImpeller.VibrationFrequency.Count + 1);
						Canvas.SetLeft(freqDot, xPosition);
						Canvas.SetBottom(freqDot, freq * heightScale + axisGap);
						xAxisItems.Add(GetAxisItemFromPoint(AxisType.X, new Point(xPosition + dotDiameter / 2, height - axisGap), power.ToString()));
						dots.Add(freqDot);
					}
					break;
				default:
					break;
			}
			resultCanvas.Children.Add(xAxis);
			resultCanvas.Children.Add(yAxis);
			resultCanvas.Children.Add(xAxisHeader);
			resultCanvas.Children.Add(yAxisHeader);
			resultCanvas.Children.Add(yHeader);
			resultCanvas.Children.Add(yLine);
			foreach (var item in xAxisItems)
			{
				resultCanvas.Children.Add(item.Label);
				resultCanvas.Children.Add(item.Mark);
			}
			foreach (var item in yAxisItems)
			{
				resultCanvas.Children.Add(item.Label);
				resultCanvas.Children.Add(item.Mark);
			}
			foreach (var dot in dots)
			{
				resultCanvas.Children.Add(dot);
			}
		}

		void freqDot_MouseEnter(object sender, MouseEventArgs e)
		{
			var glowEffect = new System.Windows.Media.Effects.DropShadowEffect();
			Color color = (Color)ColorConverter.ConvertFromString("#00AAFF");
			glowEffect.BlurRadius = ((Ellipse)e.Source).Width;
			glowEffect.Color = color;
			glowEffect.Opacity = 0.5;
			glowEffect.ShadowDepth = 0;
			((Ellipse)e.Source).Effect = glowEffect;
		}
		void freqDot_MouseLeave(object sender, MouseEventArgs e)
		{
			((Ellipse)e.Source).Effect = null;
		}
		private void resultCanvas_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			if (resultCanvas.Children != null && resultCanvas.Children.Count > 0)
			{
				ResultCanvasPainter(ResultMethod);
			}
		}
		private void ShowStatusMsg(string msg, int interval = 1500)
		{
			statusMessage.Text = msg;
			var timer = new System.Timers.Timer();
			timer.Interval = interval;
			timer.Start();
			timer.Elapsed += new System.Timers.ElapsedEventHandler((senderTimer, eTimer) =>
			{
				timer.Stop();
				this.Dispatcher.Invoke(new Action(() =>
				{
					statusMessage.Text = "";
				}));
			});
		}
		private void HideProgressBarAtDelay(int delay = 500)
		{
			var timer = new System.Timers.Timer();
			timer.Interval = delay;
			timer.Start();
			timer.Elapsed += new System.Timers.ElapsedEventHandler((senderTimer, eTimer) =>
			{
				timer.Stop();
				this.Dispatcher.Invoke(new Action(() =>
				{
					progressBar.Visibility = System.Windows.Visibility.Hidden;
				}));
			});
		}
		private void ShowProgressBar(Impeller impeller, ProhlState state)
		{
			progressBar.Dispatcher.Invoke(new Action(() =>
			{
				progressBar.Value = state.Omega;
			}), System.Windows.Threading.DispatcherPriority.ContextIdle);
		}

		private void IVCMain_DragEnter(object sender, DragEventArgs e)
		{
			if (e.Data.GetDataPresent(DataFormats.Text) || e.Data.GetDataPresent(DataFormats.FileDrop))
			{
				e.Effects = DragDropEffects.Link;
			}
			else
			{
				e.Effects = DragDropEffects.None;
			}
		}
		private void IVCMain_Drop(object sender, DragEventArgs e)
		{
			string filePath = "";
			if (e.Data.GetDataPresent(DataFormats.Text))
			{
				PasteDataStringHandler(e.Data.GetData(DataFormats.Text).ToString());
			}
			else if (e.Data.GetDataPresent(DataFormats.FileDrop))
			{
				filePath = ((string[])e.Data.GetData(DataFormats.FileDrop))[0].ToString();
				FileOpResult fileOpResult = ImportImpeller(filePath, "full");
				switch (fileOpResult)
				{
					case FileOpResult.Succeed:
						Impeller.SavePath = openFileDlg.FileName;
						resultCanvas.Children.Clear();
						UpdateArgsBindings();
						UpdateUI();
						ShowStatusMsg("文件读取成功");
						if (Impeller.InnerImpeller.Comment != null) MessageBox.Show(Impeller.InnerImpeller.Comment);
						break;
					case FileOpResult.Fail:
						ShowStatusMsg("文件读取失败");
						break;
					case FileOpResult.UnknownFormat:
						ShowStatusMsg("不是有效的叶片文件格式");
						break;
					default:
						ShowStatusMsg("异常：未知的操作状态");
						break;
				}
			}
			else
			{
				return;
			}
		}
		#endregion UI Handler
		

		#region Data Handler
		private void UpdateArgsBindings()
		{
			impellerDensity.DataContext = Impeller.InnerImpeller;
			impellerE.DataContext = Impeller.InnerImpeller;
			impellerHeight.DataContext = Impeller.InnerImpeller;

			methodFromFreq.DataContext = Impeller.State;
			methodToFreq.DataContext = Impeller.State;
			methodStep.DataContext = Impeller.State;
			methodMinTor.DataContext = Impeller.InnerImpeller;

			sectionsData.ItemsSource = Impeller.Sections;
		}
		private void setToDefaultBtn_Click(object sender, RoutedEventArgs e)
		{
			Impeller.InnerImpeller = new Impeller();
			Impeller.Sections = new List<FullSection>();
			Impeller.State = new ProhlState();
			sectionsData.ItemsSource = Impeller.Sections;
			resultBox.Text = "";
			resultCanvas.Children.Clear();
			UpdateUI();
		}
		private void addSectionBtn_Click(object sender, RoutedEventArgs e)
		{
			int count = Convert.ToInt32(addSectionCountBox.Tag);
			for (int i = 0; i < count; i++)
			{
				Impeller.Sections.Add(new FullSection(0, 1, 1, 1));				
			}
			sectionsData.Items.Refresh();
		}
		private void deleteSelectedSectionBtn_Click(object sender, RoutedEventArgs e)
		{
			var selects = sectionsData.SelectedItems;
			if (selects != null)
			{
				foreach (var section in selects)
				{
					var sectionToDelete = section as FullSection;
					Impeller.Sections.Remove(sectionToDelete);
				}
			}
			try
			{
				sectionsData.Items.Refresh();
			}
			catch{}
		}

		private Impeller FullSectionsDepartor(List<FullSection> fullSections, Impeller target)
		{
			fullSections = fullSections.OrderBy(sec => sec.Position).ToList();

			target.MSections = new List<MechanicalSection>(fullSections.Count);
			target.PSections = new List<PhysicalSection>(fullSections.Count);
			foreach (var section in fullSections)
			{
				target.MSections.Add(new MechanicalSection(section.Position, section.InertiaMoment, section.Mass));
				target.PSections.Add(new PhysicalSection(section.Position, section.InertiaMoment, section.Area));
			}
			return target;
		}
		private PointCollection GetPointsFromY(Impeller impeller, double canvasHeight, double canvasWidth)
		{
			var points = new List<Point>(impeller.Y.Count);
			double heightScale = canvasHeight / impeller.Y.Max();
			double widthScale = canvasWidth / impeller.MSections.Max(sec => sec.Position);

			points.Add(new Point(0, canvasHeight));
			for (int i = 0; i < impeller.Y.Count; i++)
			{
				points.Add(new Point(impeller.MSections[i].Position * widthScale, canvasHeight - impeller.Y[i] * heightScale));
			}
			return new PointCollection(points);
		}
		private List<AxisItem> GetAxisItemsFromDoubleList(List<double> list, AxisType axisType, Point origin, double length, bool showMin = true, bool showMax = true)
		{
			if (list == null || list.Count <= 0)
			{
				return null;
			}
			List<AxisItem> items = new List<AxisItem>(list.Count);
			double markSize = 3;
			double max = list.Max();
			double min = list.Min();
			double unit = Math.Pow(10, Math.Floor(Math.Log10(max)) - 1);

			int maxMark = (int)Math.Floor(max / unit);
			int minMark = (int)Math.Floor(min / unit);
			int diff = maxMark - minMark;

			List<double> marks = new List<double>();
			if (showMin) marks.Add(min);
			if (diff <= 10)
			{
				for (int i = minMark; i <= maxMark; i++)
				{
					if (i % 2 == 0) marks.Add(i * unit);
				}
			}
			else if (diff <= 20)
			{
				for (int i = minMark; i <= maxMark; i++)
				{
					if (i % 5 == 0) marks.Add(i * unit);
				}
			}
			else if (diff <= 50)
			{
				for (int i = minMark; i <= maxMark; i++)
				{
					if (i % 10 == 0) marks.Add(i * unit);
				}
			}
			else if (diff <= 100)
			{
				for (int i = minMark; i <= maxMark; i++)
				{
					if (i % 20 == 0) marks.Add(i * unit);
				}
			}
			if(showMax) marks.Add(max);

			foreach (var mark in marks)
			{
				TextBlock tb = new TextBlock();
				Line ln = new Line();

				if (Math.Abs(mark - 0) < 1e-50 )
				{
					tb.Text = null;
				}
				else
				{
					tb.Text = mark.ToString();
				}

				tb.TextAlignment = TextAlignment.Right;
				ln.Stroke = new SolidColorBrush(Colors.Black);
				switch (axisType)
				{
					case AxisType.X:
						Canvas.SetBottom(tb, 0);
						Canvas.SetRight(tb, length - mark * length / max);

						ln.Y1 = origin.Y;
						ln.Y2 = origin.Y - markSize;
						ln.X1 = mark * length / max + origin.X;
						ln.X2 = ln.X1;
						break;
					case AxisType.Y:
						Canvas.SetTop(tb, length - mark * length / max);
						Canvas.SetLeft(tb, 0);

						ln.Y1 = origin.Y - mark * length / max;
						ln.Y2 = ln.Y1;
						ln.X1 = origin.X; 
						ln.X2 = origin.X + markSize;
						break;
					default:
						break;
				}

				items.Add(new AxisItem
					{
						Label = tb,
						Mark = ln
					}
				);
			}
			return items;
		}
		private AxisItem GetAxisItemFromPoint(AxisType axisType, Point point, string label)
		{
			if (point == null)
			{
				return null;
			}
			AxisItem item = new AxisItem();
			double markSize = 3;

			TextBlock tb = new TextBlock();
			Line ln = new Line();

			tb.Text = label;
			tb.TextAlignment = TextAlignment.Right;
			ln.Stroke = new SolidColorBrush(Colors.Black);
			switch (axisType)
			{
				case AxisType.X:
					Canvas.SetBottom(tb, 0);
					Canvas.SetLeft(tb, point.X);

					ln.Y1 = point.Y;
					ln.Y2 = point.Y - markSize;
					ln.X1 = point.X;
					ln.X2 = ln.X1;
					break;
				case AxisType.Y:
					throw new NotImplementedException();
				default:
					break;
			}

			item = new AxisItem
			{
				Label = tb,
				Mark = ln
			};

			return item;
		}

		private string ExportCanvasToImageOrBase64String(Canvas surface, string path = null, Size? canvasSize = null)
		{
			string base64Image = null;
			if (surface.Children.Count > 0)
			{
				// Save current canvas transform
				Transform transform = surface.LayoutTransform;
				// reset current transform (in case it is scaled or rotated)
				surface.LayoutTransform = null;

				// Get the size of canvas
				Size size;
				if (canvasSize.HasValue)
				{
					size = canvasSize.Value;
				}
				else
				{
					size = new Size(surface.ActualWidth, surface.ActualHeight);
				}

				// Create a render bitmap and push the surface to it
				RenderTargetBitmap renderBitmap =
					new RenderTargetBitmap(
					(int)size.Width,
					(int)size.Height,
					96d,
					96d,
					PixelFormats.Pbgra32
				);
				renderBitmap.Render(surface);
				try
				{
					// Create a file stream for saving image
					if (path != null)
					{
						using (FileStream pngStream = new FileStream(path, FileMode.Create))
						{
							// Use png encoder for our data
							PngBitmapEncoder encoder = new PngBitmapEncoder();
							// push the rendered bitmap to it
							encoder.Frames.Add(BitmapFrame.Create(renderBitmap));
							// save the data to the stream
							encoder.Save(pngStream);
						}
					}
					// Save image to string
					else
					{
						using (MemoryStream pngStream = new MemoryStream())
						{
							// Use png encoder for our data
							PngBitmapEncoder encoder = new PngBitmapEncoder();
							// push the rendered bitmap to it
							encoder.Frames.Add(BitmapFrame.Create(renderBitmap));
							// save the data to the stream
							encoder.Save(pngStream);
							byte[] imageBytes = pngStream.GetBuffer();
							if (imageBytes != null)
							{
								base64Image = Convert.ToBase64String(imageBytes);
							}
						}
					}
				}
				catch { }
				finally
				{
					// Restore previously saved layout
					surface.LayoutTransform = transform;
				}
			}
			return base64Image;
		}

		private ImpellerData ImpellerParser(string impellerDataStringOrXmlPath, ImpellerDataType dataType)
		{
			ImpellerData impeller = new ImpellerData();
			switch (dataType)
			{
				case ImpellerDataType.String:
					if (impellerDataStringOrXmlPath == null
						|| impellerDataStringOrXmlPath.Length <= 0)
					{
						return null;
					}
					string impellerArgsString = Regex.Match(impellerDataStringOrXmlPath, @"(?<=\[Impeller\]\r\n)(?>[^\[\]]+)").Value;
					string sectionsString = Regex.Match(impellerDataStringOrXmlPath, @"(?<=\[Sections\]\r\n)(?>[^\[\]]+)").Value;
					string resultString = Regex.Match(impellerDataStringOrXmlPath, @"(?<=\[Result\]\r\n)(?>[^\[\]]+)").Value;
					string temp = "";

					if (impellerArgsString.Length <= 0
						&& sectionsString.Length <= 0
						&& resultString.Length <= 0)
					{
						return null;
					}

					if (impellerArgsString.Length > 0)
					{
						try
						{
							temp = Regex.Match(impellerArgsString, @"(?<=Height=)[-\w\.]+").Value.Trim();
							if (temp.Length > 0)
							{
								impeller.InnerImpeller.Height =
									Convert.ToDouble(temp);
							}
						}
						catch { impeller.ErrorArgs.Add("Height"); }
						try
						{
							temp = Regex.Match(impellerArgsString, @"(?<=Density=)[-\w\.]+").Value.Trim();
							if (temp.Length > 0)
							{
								impeller.InnerImpeller.Density =
									Convert.ToDouble(temp);
							}
						}
						catch { impeller.ErrorArgs.Add("Density"); }
						try
						{
							temp = Regex.Match(impellerArgsString, @"(?<=E=)[-\w\.]+").Value.Trim();
							if (temp.Length > 0)
							{
								impeller.InnerImpeller.E =
									Convert.ToDouble(temp);
							}
						}
						catch { impeller.ErrorArgs.Add("E"); }
						try
						{
							temp = Regex.Match(impellerArgsString, @"(?<=From Freq=)[-\w\.]+").Value.Trim();
							if (temp.Length > 0)
							{
								impeller.State.CheckFromOmega =
									Convert.ToDouble(temp) * 2 * Math.PI;
							}
						}
						catch { impeller.ErrorArgs.Add("From Freq"); }
						try
						{
							temp = Regex.Match(impellerArgsString, @"(?<=To Freq=)[-\w\.]+").Value.Trim();
							if (temp.Length > 0)
							{
								impeller.State.CheckToOmega =
									Convert.ToDouble(temp) * 2 * Math.PI;
							}
						}
						catch { impeller.ErrorArgs.Add("To Freq"); }
						try
						{
							temp = Regex.Match(impellerArgsString, @"(?<=Step=)[-\w\.]+").Value.Trim();
							if (temp.Length > 0)
							{
								impeller.State.ProhlStep =
									Convert.ToDouble(temp) * 2 * Math.PI;
							}
						}
						catch { impeller.ErrorArgs.Add("Step"); }
						try
						{
							temp = Regex.Match(impellerArgsString, @"(?<=Min Tolerance=)[-\w\.]+").Value.Trim();
							if (temp.Length > 0)
							{
								impeller.InnerImpeller.MinTolerance =
									Convert.ToDouble(temp);
							}
						}
						catch { impeller.ErrorArgs.Add("Min Tolerance"); }
						try
						{
							temp = Regex.Match(impellerArgsString, @"(?<=Min Step=)[-\w\.]+").Value.Trim();
							if (temp.Length > 0)
							{
								impeller.InnerImpeller.MinStepDiff =
									Convert.ToDouble(temp);
							}
						}
						catch { impeller.ErrorArgs.Add("Min Step"); }
					}

					if (sectionsString.Length > 0)
					{
						int index = 0;
						double position = 0.0;
						double inertiaMoment = 1.0;
						double area = 1.0;
						double mass = 1.0;
						MatchCollection sections = Regex.Matches(sectionsString, @"(?<Index>\d+):\s+(?<Position>[\w\.-]+),(?<InertiaMoment>[\w\.-]+),(?<Area>[\w\.-]+),(?<Mass>[\w\.-]+)");

						foreach (Match sec in sections)
						{
							try
							{
								index = Convert.ToInt32(sec.Groups["Index"].Value);
								position = Convert.ToDouble(sec.Groups["Position"].Value);
								inertiaMoment = Convert.ToDouble(sec.Groups["InertiaMoment"].Value);
								area = Convert.ToDouble(sec.Groups["Area"].Value);
								mass = Convert.ToDouble(sec.Groups["Mass"].Value);

								impeller.Sections.Add(new FullSection(position, inertiaMoment, area, mass));
							}
							catch
							{
								impeller.ErrorArgs.Add(index.ToString());
							}
						}
					}

					if (resultString.Length > 0)
					{
						impeller.InnerImpeller.Comment = resultString;
					}
					break;
				case ImpellerDataType.Xml:
					var loadResult = Config.LoadFromFile(impellerDataStringOrXmlPath);
					switch (loadResult)
					{
						case XmlConfig.OperateConfigFileResult.FileNotExist:
						case XmlConfig.OperateConfigFileResult.IOError:
						case XmlConfig.OperateConfigFileResult.InvalidPath:
							impeller = null;
							break;
						case XmlConfig.OperateConfigFileResult.Succeed:
							if (Config.ConfigXML.Element("Impeller") == null
								&& Config.ConfigXML.Element("Sections") == null
								&& Config.ConfigXML.Element("Result") == null)
							{
								return null;
							}

							if (Config.ConfigXML.Element("Impeller") != null)
							{
								try
								{
									impeller.InnerImpeller.Height
										= Convert.ToDouble(Config.GetVal("Impeller", "Height"));
								}
								catch { impeller.ErrorArgs.Add("Height"); }
								try
								{
									impeller.InnerImpeller.Density
										= Convert.ToDouble(Config.GetVal("Impeller", "Density"));
								}
								catch { impeller.ErrorArgs.Add("Density"); }
								try
								{
									impeller.InnerImpeller.E
										= Convert.ToDouble(Config.GetVal("Impeller", "E"));
								}
								catch { impeller.ErrorArgs.Add("E"); }
								try
								{
									impeller.State.CheckFromOmega
										= Convert.ToDouble(Config.GetVal("Impeller", "FromFreq")) * 2 * Math.PI;
								}
								catch { impeller.ErrorArgs.Add("FromFreq"); }
								try
								{
									impeller.State.CheckToOmega
										= Convert.ToDouble(Config.GetVal("Impeller", "ToFreq")) * 2 * Math.PI;
								}
								catch { impeller.ErrorArgs.Add("ToFreq"); }
								try
								{
									impeller.State.ProhlStep
										= Convert.ToDouble(Config.GetVal("Impeller", "Step")) * 2 * Math.PI;
								}
								catch { impeller.ErrorArgs.Add("Step"); }
								try
								{
									impeller.InnerImpeller.MinTolerance
										= Convert.ToDouble(Config.GetVal("Impeller", "MinTolerance"));
								}
								catch { impeller.ErrorArgs.Add("MinTolerance"); }
								try
								{
									impeller.InnerImpeller.MinStepDiff
										= Convert.ToDouble(Config.GetVal("Impeller", "MinStep"));
								}
								catch { impeller.ErrorArgs.Add("MinStep"); }
							}

							if (Config.ConfigXML.Element("Sections") != null)
							{
								int index = 0;
								Regex indexRegex = new Regex(@"(?<=Section)\d+", RegexOptions.Compiled);
								double position = 0.0;
								double inertiaMoment = 1.0;
								double area = 1.0;
								double mass = 1.0;

								var sections = Config.ConfigXML.Element("Sections").Elements();

								foreach (var sec in sections)
								{
									try
									{
										index = Convert.ToInt32(indexRegex.Match(sec.Name.ToString()).Value);
										position = Convert.ToDouble(sec.Attribute("Position").Value);
										inertiaMoment = Convert.ToDouble(sec.Attribute("InertiaMoment").Value);
										area = Convert.ToDouble(sec.Attribute("Area").Value);
										mass = Convert.ToDouble(sec.Attribute("Mass").Value);

										impeller.Sections.Add(new FullSection(position, inertiaMoment, area, mass));
									}
									catch
									{
										impeller.ErrorArgs.Add(index.ToString());
									}
								}
							}

							if (Config.ConfigXML.Element("Result") != null)
							{
								try
								{
									impeller.InnerImpeller.Comment
										= Config.ConfigXML.Element("Result").Value;
								}
								catch
								{
									impeller.ErrorArgs.Add("Result");
								}
							}
							break;
						default:
							return null;
					}
					break;
				default:
					return null;			
			}
			return impeller;
		}
		private string ParseImpellerArgsToString()
		{
			string impellerArgsString = "[Impeller]";
			impellerArgsString += "\r\nHeight=" + impellerHeight.Text;
			impellerArgsString += "\r\nDensity=" + impellerDensity.Text;
			impellerArgsString += "\r\nE=" + impellerE.Text;
			impellerArgsString += "\r\nFrom Freq=" + methodFromFreq.Text;
			impellerArgsString += "\r\nTo Freq=" + methodToFreq.Text;
			impellerArgsString += "\r\nStep=" + methodStep.Text;
			impellerArgsString += "\r\nMin Tolerance=" + Impeller.InnerImpeller.MinTolerance.ToString();
			impellerArgsString += "\r\nMin Step=" + Impeller.InnerImpeller.MinStepDiff.ToString();
			return impellerArgsString;
		}
		private string ParseSectionsToString()
		{
			StringBuilder sectionsString = new StringBuilder("[Sections]\r\n(Position,Inertia Moment,Area,Mass)\r\n");
			int count = 0;
			foreach (var sec in Impeller.Sections)
			{
				sectionsString
					.Append(count++).Append(":\t")
					.Append(sec.Position.ToString()).Append(",")
					.Append(sec.InertiaMoment.ToString()).Append(",")
					.Append(sec.Area.ToString()).Append(",")
					.Append(sec.Mass.ToString()).Append("\r\n");
			}
			return sectionsString.ToString();
		}
		private string ParseResultToString()
		{
			string resultString = "[Result]\r\n";
			resultString += resultBox.Text.Replace("\n", "\r\n");
			return resultString;
		}
		private XElement ParseImpellerArgsToElement()
		{
			XElement impellerArgsElement = new XElement("Impeller");
			impellerArgsElement.Add(
				new XElement("Height",impellerHeight.Text),
				new XElement("Density",impellerDensity.Text),
				new XElement("E",impellerE.Text),
				new XElement("FromFreq",methodFromFreq.Text),
				new XElement("ToFreq",methodToFreq.Text),
				new XElement("Step",methodStep.Text),
				new XElement("MinTolerance",Impeller.InnerImpeller.MinTolerance.ToString()),
				new XElement("MinStep",Impeller.InnerImpeller.MinStepDiff.ToString())
			);
			return impellerArgsElement;
		}
		private XElement ParseSectionsToElement()
		{
			XElement sectionsElement = new XElement("Sections");
			int count = 0;
			sectionsElement.Add(new XAttribute("Count", Impeller.Sections.Count));
			foreach (var sec in Impeller.Sections)
			{
				sectionsElement.Add(
					new XElement("Section"+(count++.ToString()),
						new XAttribute("Position", sec.Position),
						new XAttribute("InertiaMoment", sec.InertiaMoment),
						new XAttribute("Area", sec.Area),
						new XAttribute("Mass", sec.Mass)
					)
				);
			}
			return sectionsElement;
		}
		private XElement ParseResultToElement()
		{
			XElement resultElement = new XElement("Result");
			resultElement.Add(
				resultBox.Text.Replace("\n", "\r\n")
			);
			return resultElement;
		}
		
		private StringBuilder ParseImpellerToHTMLString(string ImpellerName, ResultReportFileMode mode)
		{
			//懒得用DOM方式所以写得太丑啦不要看呀>~<
			StringBuilder HTMLString = new StringBuilder();
			try
			{
				if (mode == ResultReportFileMode.Create)
				{
					HTMLString.Append(@"<!DOCTYPE HTML>
<html lang=""zh-CN"" reporttype=""CalculationReport"" reportver=""")
							.Append(App.ResourceAssembly.GetName().Version.ToString())
							.Append(@""">
<head>
	<meta charset=""UTF-8"">
	<title>Impeller Vibration Checker Report</title>
	<style type=""text/css"">
		body{
			background:#111111;
		}
		.Impeller{
			height:20em;
			width:90%;
			left:3%;
			margin:2em 0 2em 0;
			background:#4496E7;
			border: solid 0.7em #4496E7;
			border-radius: 0.7em;
			box-shadow:0 0 1em 0.3em #fff;
			position:relative;
		}
		.ContentBlock{
			top:0%;
			width:49.5%;
			background:#ccd;
			box-shadow:0 0 1em 0.05em #fff inset;
			position:absolute;
			overflow:auto;
		}
		.ResultBlock{
			top:20%;
			height:80%;
			left:0;
		}
		.ImpellerArgsBlock{
			top:10%;
			height:90%;
			right:0;
		}
		.ResultHeader{
			top:10%;
			height:7%;
			position:absolute;
		}
		.ImpellerArgsHeader{
			top:0%;
			height:7%;
			left:50.5%;
			position:absolute;
		}
		.ImpellerNameContainer{
			top:0%;
			left:0%;
			height:1.4em;
			width:49.5%;
			overflow:hidden;
			position:absolute;
		}
		.ImpellerName{
			height:100%;
			width:100%;
			min-width:1em;
			display:inline;
			margin-left:1em;
			position:absolute;
			background:#E8EEFF;
			cursor:default;
		}
		.Content{
			margin:1em;
		}
		h{
			text-shadow: 0 0 6px #FFFFFF, 0 0 6px #FFFFFF;
			cursor:default;
		}
		.CloseButton{
			right:0.5em;
			height:1.5em;
			width:1.5em;
			border-radius:0.75em;
			background:#E8EEFF;
			position:absolute;
		}
		.MinButton{
			right:3em;
			top:0.45em;
			height:0.6em;
			width:1.5em;
			border-radius:0.1em;
			background:#E8EEFF;
			position:absolute;
		}
		.btn:hover{
			background:#D3D9EA;
		}
		.CloseButton:hover{
			background:#FF7C7C;
		}
		.btn:active{
			background:#D3D9EA;
			box-shadow:0 1px 2px rgba(0, 0, 0, 0.3) inset;
		}
		.CloseButton:active{
			background:#DD5D5D;
		}
		#Header{
			font-family:georgia;
			font-weight:bold;
			font-size: 2.5em;
			text-shadow: 0px 0px 10px #E8EEFF, 1px 1px 5px #00F;
			color: #EEE ; 
			text-align: center;
			cursor:default;
		}
		.toolbtn{
			height:2em;
			width:12%;
			top:1em;
			font-family:georgia;
			font-weight:bold;
			font-size: 1em;
			color: #EEE ; 
			background:#4D90FE;
			border: 1px solid #3079ED;
			position:absolute;
		}
		#ShrikAllBtn{
			left:23%;
		}
		#ExpandAllBtn{
			left:43%;
		}
		#ToggleAllBtn{
			left:63%;
		}
		.toolbtn:hover{
			background:#357AE8;
			border:1px solid #2F5BB7;
		}
		.toolbtn:focus{
			background:#4D90FE;
			border:1px solid #4D90FE;
			box-shadow:0 0 0 1px rgba(255, 255, 255, 0.5) inset;
		}
		.toolbtn:active{
			background:#357AE8;
			border:1px solid #2F5BB7;
			box-shadow:0 1px 2px rgba(0, 0, 0, 0.3) inset;
		}
	</style>
	<script type=""text/javascript"">
		function toggleSize(impeller){
			var size = impeller.getAttribute(""size"");
			if(size === ""max""){
				impeller.getElementsByClassName(""ResultHeader"")[0].style.display = ""none"";
				impeller.getElementsByClassName(""ImpellerArgsHeader"")[0].style.display = ""none"";
				impeller.getElementsByClassName(""ContentBlock"")[0].style.display = ""none"";
				impeller.getElementsByClassName(""ContentBlock"")[1].style.display = ""none"";
				impeller.getElementsByClassName(""ImpellerName"")[0].title = ""Click to Expand"";
				impeller.getElementsByClassName(""ImpellerNameContainer"")[0].style.width = ""80%"";
				impeller.getElementsByClassName(""MinButton"")[0].title = ""Expand"";
				impeller.getElementsByClassName(""MinButton"")[0].style.height = ""1.2em"";
				impeller.getElementsByClassName(""MinButton"")[0].style.top = ""0.15em"";
				impeller.style.height = ""1.5em"";
				impeller.setAttribute(""size"", ""min"");
			}
			else if(size === ""min""){
				impeller.getElementsByClassName(""ResultHeader"")[0].style.display = ""block"";
				impeller.getElementsByClassName(""ImpellerArgsHeader"")[0].style.display = ""block"";
				impeller.getElementsByClassName(""ContentBlock"")[0].style.display = ""block"";
				impeller.getElementsByClassName(""ContentBlock"")[1].style.display = ""block"";
				impeller.getElementsByClassName(""ImpellerName"")[0].title = ""Click to Shrink"";
				impeller.getElementsByClassName(""ImpellerNameContainer"")[0].style.width = ""49.5%"";
				impeller.getElementsByClassName(""MinButton"")[0].title = ""Shrink"";
				impeller.getElementsByClassName(""MinButton"")[0].style.height = ""0.6em"";
				impeller.getElementsByClassName(""MinButton"")[0].style.top = ""0.45em"";
				impeller.style.height = ""20em"";
				impeller.setAttribute(""size"", ""max"");
			}else{
				return;
			}
		}
		function batchToggleSize(mode){
			var impellers = document.getElementsByClassName(""Impeller"");
			switch(mode)
			{
				case ""shrink"":
					for(var index = 0; index < impellers.length; index++){
						impellers[index].setAttribute(""size"", ""max"");
						toggleSize(impellers[index]);
					}
					break;
				case ""expand"":
					for(var index = 0; index < impellers.length; index++){
						impellers[index].setAttribute(""size"", ""min"");
						toggleSize(impellers[index]);
					}
					break;
				case ""toggle"":
					for(var index = 0; index < impellers.length; index++){
						toggleSize(impellers[index]);
					}
					break;
				default:
					break;
			}
		}
	</script>
</head>
<body>
	<div id=""Header"">
		Impeller Vibration Checker Report
	</div>
	<div id=""Toolbar"" style=""position:relative;height:3em;"">
		<button id=""ShrikAllBtn"" class=""toolbtn"" onclick=""batchToggleSize('shrink')"">Shrink All</button>
		<button id=""ExpandAllBtn"" class=""toolbtn"" onclick=""batchToggleSize('expand')"">Expand All</button>
		<button id=""ToggleAllBtn"" class=""toolbtn"" onclick=""batchToggleSize('toggle')"">Toggle All</button>
	</div>"
					);
				}
				//叶片部分
				HTMLString.Append(@"
	<div class=""Impeller"" size=""max"">
		<div class=""ImpellerNameContainer"">
			<h>Impeller Name :</h>
			<div class=""ImpellerName btn"" title=""Click to shrink"" onclick=""toggleSize(this.parentElement.parentElement)"">
				");
				//加入叶片名
				HTMLString.Append(ImpellerName + "\r\n");
				HTMLString.Append(@"			</div>
		</div>
		<div class=""ResultHeader"">
			<h>Calculation Result :</h>
		</div>
		<div class=""ResultBlock ContentBlock"">
			<div class=""Result Content"">
				<img src=""data:image/png;base64,");
				//加入base64结果图
				HTMLString.Append(
					ExportCanvasToImageOrBase64String(
						resultCanvas,
						canvasSize: new Size(resultCanvas.ActualWidth * 1.1, resultCanvas.ActualHeight * 1.2)
					)
				);
				HTMLString.Append(@"""></img><br/>
				");
				//加入结果内容
				HTMLString.Append(
					EncodeHTMLComponent(ParseResultToString()).Replace(@"<br/>", "\r\n\t\t\t\t<br/>")
				);
				HTMLString.Append(@"
			</div>
		</div>
		<div class=""ImpellerArgsHeader"">
			<h>Impeller Arguments :</h>
		</div>
		<div class=""ImpellerArgsBlock ContentBlock"">
			<div class=""ImpellerArgs Content"">
				");
				//加入叶片参数
				HTMLString.Append(
					EncodeHTMLComponent(ParseImpellerArgsToString()).Replace(@"<br/>", "\r\n\t\t\t\t<br/>")
				);
				HTMLString.Append("\r\n\t\t\t\t<br/>\r\n\t\t\t\t<br/>");
				HTMLString.Append(
					EncodeHTMLComponent(ParseSectionsToString()).Replace(@"<br/>", "\r\n\t\t\t\t<br/>")
				);
				HTMLString.Append(@"
			</div>
		</div>
		<div class=""CloseButton btn"" title=""Hide this impeller temporarily"" onclick=""this.parentElement.style.display = 'none'""></div>
		<div class=""MinButton btn"" title=""Shrink"" onclick=""toggleSize(this.parentElement)""></div>
	</div >
"
				);
				if(mode == ResultReportFileMode.Create){
					HTMLString.Append(
@"</body>
</html>"			);
				}
			}
			catch
			{
				return null;
			}
			return HTMLString;
		}
		private string EncodeHTMLComponent(string str)
		{
			str = str.Replace("&", @"&amp;")
				.Replace("&",@"&amp;")
				.Replace("<",@"&lt;")
				.Replace(">",@"&gt;")
				.Replace("\'",@"&apos;")
				.Replace("\"",@"&quot;")
				.Replace("\r\n",@"<br/>")
				.Replace(" ",@"&nbsp;")
				.Replace("\t",@"&nbsp;&nbsp;&nbsp;&nbsp;");
			return str;
		}
		#endregion Data Handler


		#region IO Handler
		private FileOpResult ImportImpeller(string path, string mode)
		{
			string ext = "";
			var impellerData = new ImpellerData();
			try
			{
				ext = System.IO.Path.GetExtension(path).ToLower();
			}
			catch
			{
				return FileOpResult.Fail;
			}

			switch (ext)
			{
				case ".xml":
					impellerData = ImpellerParser(path, ImpellerDataType.Xml);
					break;
				case ".txt":
					string impellerString = "";
					try
					{
						impellerString = File.ReadAllText(path);
					}
					catch
					{
						impellerData = null;
					}					
					impellerData = ImpellerParser(impellerString, ImpellerDataType.String);
					break;
				default:
					return FileOpResult.UnknownFormat;
			}

			if (impellerData != null)
			{
				string errorArgsMsg = "";
				switch (mode)
				{
					case "full":
						Impeller.InnerImpeller = impellerData.InnerImpeller;
						Impeller.State = impellerData.State;
						Impeller.Sections = impellerData.Sections;
						resultBox.Text = impellerData.InnerImpeller.Comment;
						errorArgsMsg = "读取文件成功，但是以下参数/截面有误，未能导入：";
						break;
					case "sec":
						Impeller.Sections = impellerData.Sections;
						errorArgsMsg = "读取文件成功，但是以下截面有误，未能导入：";
						break;
					default:
						return FileOpResult.Fail;
				}
				Impeller.InnerImpeller.Comment = null;

				if (impellerData.ErrorArgs.Count > 0)
				{
					int errorSectionsCount = 0;
					int errorArgsCount = 0;
					foreach (var error in impellerData.ErrorArgs)
					{
						if (Regex.IsMatch(error, @"\d+"))
						{
							errorSectionsCount++;
							errorArgsMsg += "\r\n第" + error + "个截面";
						}
						else
						{
							errorArgsCount++;
							if (mode == "full") errorArgsMsg += "\r\n" + error;
						}
					}
					Impeller.InnerImpeller.Comment = errorArgsMsg;
				}
			}
			else
			{
				return FileOpResult.Fail;
			}

			return FileOpResult.Succeed;
		}
		private FileOpResult ExportImpeller(string path, string mode)
		{
			string ext = "";
			StringBuilder impellerDataString = new StringBuilder("["
				+ App.ResourceAssembly.GetName().Name + " "
				+ App.ResourceAssembly.GetName().Version.ToString() + "]\r\n");
			XElement impellerDataElement = new XElement("ImpellerVibrationChecker");
			impellerDataElement.Add(new XAttribute("Ver", App.ResourceAssembly.GetName().Version.ToString()));
			try
			{
				ext = System.IO.Path.GetExtension(path).ToLower();
			}
			catch
			{
				return FileOpResult.Fail;
			}

			switch (ext)
			{
				case ".xml":
					Config.Path = path;
					try
					{
						switch (mode)
						{
							case "full":
								impellerDataElement.Add(ParseImpellerArgsToElement());
								impellerDataElement.Add(ParseSectionsToElement());
								impellerDataElement.Add(ParseResultToElement());
								break;
							case "sec":
								impellerDataElement.Add(ParseSectionsToElement());
								break;
							default:
								return FileOpResult.Fail;
						}
					}
					catch
					{
						return FileOpResult.UnknownFormat;
					}
					Config.ConfigXML = impellerDataElement;
					var saveResult = Config.SaveToFile();
					switch (saveResult)
					{
						case XmlConfig.OperateConfigFileResult.FileNotExist:
						case XmlConfig.OperateConfigFileResult.IOError:
						case XmlConfig.OperateConfigFileResult.InvalidPath:
							return FileOpResult.Fail;
						case XmlConfig.OperateConfigFileResult.Succeed:
						default:
							break;
					}
					break;
				case ".txt":
					try
					{
						switch (mode)
						{
							case "full":
								impellerDataString
									.Append(ParseImpellerArgsToString()).Append("\r\n\r\n")
									.Append(ParseSectionsToString()).Append("\r\n")
									.Append(ParseResultToString());
								break;
							case "sec":
								impellerDataString
									.Append(ParseSectionsToString());
								break;
							default:
								return FileOpResult.Fail;
						}
					}
					catch
					{
						return FileOpResult.UnknownFormat;
					}
					try
					{
						File.WriteAllText(path, impellerDataString.ToString());
					}
					catch
					{
						return FileOpResult.Fail;
					}
					break;
				default:
					return FileOpResult.UnknownFormat;
			}
			return FileOpResult.Succeed;
		}
		private FileOpResult ResultReportGenerator(string path, ResultReportType type, ResultReportFileMode mode)
		{
			StringBuilder HTMLString = new StringBuilder();
			if (path == null) return FileOpResult.Fail;
			switch (type)
			{
				case ResultReportType.HTML:
					switch (mode)
					{
						case ResultReportFileMode.Create:
							HTMLString = ParseImpellerToHTMLString("Impeller 0", mode);
							try
							{
								File.WriteAllText(path, HTMLString.ToString());
							}
							catch
							{
								return FileOpResult.Fail;
							}
							break;
						case ResultReportFileMode.Append:
							try
							{
								HTMLString = new StringBuilder(File.ReadAllText(path));
							}
							catch
							{
								return FileOpResult.Fail;
							}
							if (Regex.IsMatch(HTMLString.ToString(), @"reporttype=""CalculationReport"""))
							{
								int impellerCount = Regex.Matches(HTMLString.ToString(), @"<div class=""Impeller""").Count;
								HTMLString.Replace(
									"</body>",
									ParseImpellerToHTMLString("Impeller " + impellerCount, mode)
										.Append("\r\n</body>")
										.ToString()
								);
								try
								{
									File.WriteAllText(path, HTMLString.ToString());
								}
								catch
								{
									return FileOpResult.Fail;
								}
							}
							else
							{
								return FileOpResult.UnknownFormat;
							}
							break;
						default:
							return FileOpResult.Fail;
					}
					break;
				default:
					return FileOpResult.Fail;
			}
			return FileOpResult.Succeed;
		}
		#endregion IO Handler


		#region Calculation Handler
		private void runBtn_Click(object sender, RoutedEventArgs e)
		{
			if (sectionsData.Items.Count <= 1)
			{
				State.IsSectionEnough = false;
			}
			else { State.IsSectionEnough = true; }
			UpdateArgsState();
			UpdateSectionsDataState();

			if (State.CanRun)
			{
				//run
				progressBar.Minimum = Impeller.State.CheckFromOmega;
				progressBar.Value = progressBar.Minimum;
				progressBar.Maximum = Impeller.State.CheckToOmega;
				progressBar.Visibility = System.Windows.Visibility.Visible;
				this.Cursor = Cursors.Wait;
				runBtn.IsEnabled = false;
				setToDefaultBtn.IsEnabled = false;

				resultBox.Text = "";

				FullSectionsDepartor(Impeller.Sections, Impeller.InnerImpeller);

				try
				{
					switch ((CalculateMethod)methodChoice.SelectedItem)
					{
						case CalculateMethod.Rayleigh:
							ResultMethod = CalculateMethod.Rayleigh;
							Impeller.InnerImpeller = Checker.Rayleigh(Impeller.InnerImpeller, 9.793);
							break;
						case CalculateMethod.Iteration:
							ResultMethod = CalculateMethod.Iteration;
							Impeller.InnerImpeller = Checker.Iteration(Impeller.InnerImpeller);
							break;
						case CalculateMethod.Prohl:
							ResultMethod = CalculateMethod.Prohl;
							Impeller.InnerImpeller = Checker.Prohl(Impeller.InnerImpeller, Impeller.State);
							break;
						default:
							break;
					}
					UpdateArgsBindings();
					Impeller.InnerImpeller.Comment = ShowResult(Impeller.InnerImpeller, (CalculateMethod)methodChoice.SelectedItem);
					ResultCanvasPainter((CalculateMethod)methodChoice.SelectedItem);
					ShowStatusMsg("计算已完成");
				}
				catch(Exception ex)
				{
					Impeller.InnerImpeller.Comment = "计算失败\n请检测参数和截面是否有错\n可能的原因：\n*叶片参数有误；\n*计算参数有误；\n*截面参数有误；\n*存在相同位置的截面。";
					ShowStatusMsg("异常:" + ex.Message);
				}
				resultBox.Text = Impeller.InnerImpeller.Comment;

				this.Cursor = Cursors.Arrow;
				runBtn.IsEnabled = true;
				setToDefaultBtn.IsEnabled = true;
				progressBar.Value = progressBar.Maximum;

				HideProgressBarAtDelay();
			}
			else
			{
				CalcAbortedMessageShower();
				ShowStatusMsg("计算已中止");
			}
		}
		private void UpdateArgsState()
		{
			try
			{
				State.IsArgsOK = (
					(impellerHeight.Tag == null ? true : Convert.ToBoolean(impellerHeight.Tag))
					&& (impellerDensity.Tag == null ? true : Convert.ToBoolean(impellerDensity.Tag))
					&& (impellerE.Tag == null ? true : Convert.ToBoolean(impellerE.Tag))
					&& (methodFromFreq.Tag == null ? true : Convert.ToBoolean(methodFromFreq.Tag))
					&& (methodToFreq.Tag == null ? true : Convert.ToBoolean(methodToFreq.Tag))
					&& (methodStep.Tag == null ? true : Convert.ToBoolean(methodStep.Tag))
					&& (methodMinTor.Tag == null ? true : Convert.ToBoolean(methodMinTor.Tag))
				);
			}
			catch
			{
				State.IsArgsOK = false;
			}
		}
		private void UpdateSectionsDataState()
		{
			for (int i = 0; i < sectionsData.Items.Count; i++)
			{
				DependencyObject obj = sectionsData.ItemContainerGenerator.ContainerFromIndex(i);
				if (obj!= null && Validation.GetHasError(obj))
				{
					State.IsSectionsDataOK = false;
					return;
				}
			}
			State.IsSectionsDataOK = true;
		}
		#endregion Calculation Handler


		#region Command Handler
		ImpellerGenerator impellerGen;
		private void impellerGeneratorBtn_Click(object sender, RoutedEventArgs e)
		{
			if (impellerGen == null || impellerGen.IsLoaded == false)
			{
				impellerGen = new ImpellerGenerator();
				impellerGen.ResultApply += GeneratedImpellerDataPasteHandler;
				impellerGen.Owner = this;
				impellerGen.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;
				impellerGen.Show();
			}
			else
			{
				impellerGen.Activate();
			}
		}
		YComparer yComp;
		private void yComparerBtn_Click(object sender, RoutedEventArgs e)
		{
			if (yComp == null || yComp.IsLoaded == false)
			{
				yComp = new YComparer();
				yComp.ExportImage += ExportCanvasToImageOrBase64String;
				yComp.GetAxisItems += GetAxisItemsFromDoubleList;
				yComp.ComparerCalculate += ComparerCalculateHandler;
				yComp.Owner = this;
				yComp.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;
				yComp.Show();
			}
			else
			{
				yComp.Activate();
			}

		}
		About abt;
		private void aboutBtn_Click(object sender, RoutedEventArgs e)
		{
			if (abt == null || abt.IsLoaded == false)
			{
				abt = new About();
				abt.Owner = this;
				abt.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;
				abt.ShowDialog();
			}
			else
			{
				abt.Activate();
			}

		}
		private void helpBtn_Click(object sender, RoutedEventArgs e)
		{
			if (File.Exists("IVCHelpFile.html"))
			{
				try
				{
					System.Diagnostics.Process.Start("IVCHelpFile.html");
				}
				catch
				{
					ShowStatusMsg("打开帮助文件失败");
				}
			}
			else
			{
				ShowStatusMsg("没有找到帮助文件");
			}
		}

		System.Windows.Forms.OpenFileDialog openFileDlg = new System.Windows.Forms.OpenFileDialog();
		System.Windows.Forms.SaveFileDialog saveFileDlg = new System.Windows.Forms.SaveFileDialog();
		private void New_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			setToDefaultBtn_Click(null, null);
			Impeller.SavePath = null;
		}
		private void Open_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			openFileDlg.Filter = "叶片文件(*.xml;*.txt)|*.xml;*.txt|所有文件(*.*)|*.*";
			var openResult = openFileDlg.ShowDialog();
			if (openResult == System.Windows.Forms.DialogResult.OK)
			{
				FileOpResult fileOpResult = FileOpResult.Fail;
				if (e != null)
				{
					fileOpResult = ImportImpeller(openFileDlg.FileName, e.Parameter == null ? "full" : e.Parameter.ToString());
				}
				switch (fileOpResult)
				{
					case FileOpResult.Succeed:
						Impeller.SavePath = openFileDlg.FileName;
						resultCanvas.Children.Clear();
						UpdateArgsBindings();
						UpdateUI();
						ShowStatusMsg("文件读取成功");
						if(Impeller.InnerImpeller.Comment != null)MessageBox.Show(Impeller.InnerImpeller.Comment);
						break;
					case FileOpResult.Fail:
						ShowStatusMsg("文件读取失败");
						break;
					case FileOpResult.UnknownFormat:
						ShowStatusMsg("不是有效的叶片文件格式");
						break;
					default:
						ShowStatusMsg("异常：未知的操作状态");
						break;
				}
			}
		}
		private void Save_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			FileOpResult fileOpResult = FileOpResult.Fail;
			if (Impeller.IsSavePathExist)
			{
				fileOpResult = ExportImpeller(Impeller.SavePath, "full");
			}
			else
			{
				SaveAs_Executed(null, null);
				return;
			}
			switch (fileOpResult)
			{
				case FileOpResult.Succeed:
					ShowStatusMsg("保存成功");
					break;
				case FileOpResult.Fail:
					ShowStatusMsg("保存失败");
					break;
				case FileOpResult.UnknownFormat:
					ShowStatusMsg("不是有效的叶片文件格式");
					break;
				default:
					ShowStatusMsg("异常：未知的操作状态");
					break;
			}
		}
		private void SaveAs_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			saveFileDlg.Filter = "叶片文件(*.xml)|*.xml|叶片文件(*.txt)|*.txt|所有文件(*.*)|*.*";
			saveFileDlg.OverwritePrompt = true;
			saveFileDlg.FileName = "ImpellerData";
			var saveResult = saveFileDlg.ShowDialog();
			if (saveResult == System.Windows.Forms.DialogResult.OK)
			{
				FileOpResult fileOpResult = FileOpResult.Fail;
				if (e != null)
				{
					fileOpResult = ExportImpeller(saveFileDlg.FileName, e.Parameter == null ? "full" : e.Parameter.ToString());
				}
				else
				{
					fileOpResult = ExportImpeller(saveFileDlg.FileName, "full");
				}
				switch (fileOpResult)
				{
					case FileOpResult.Succeed:
						Impeller.SavePath = saveFileDlg.FileName;
						ShowStatusMsg("保存成功");
						break;
					case FileOpResult.Fail:
						ShowStatusMsg("保存失败");
						break;
					case FileOpResult.UnknownFormat:
						ShowStatusMsg("不是有效的叶片文件格式");
						break;
					default:
						ShowStatusMsg("异常：未知的操作状态");
						break;
				}
			}
		}
		private void Close_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			this.Close();
		}

		private void Copy_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			var full = new StringBuilder();
			full.Append(ParseImpellerArgsToString()).Append("\r\n\r\n")
				.Append(ParseSectionsToString()).Append("\r\n")
				.Append(ParseResultToString());
			try
			{
				Clipboard.SetText(full.ToString());
			}
			catch
			{
				ShowStatusMsg("异常:复制到剪切板失败");
				return;
			}
			ShowStatusMsg("复制成功！");
		}

		private void Paste_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			PasteDataStringHandler();
		}
		private void PasteDataStringHandler(string impellerString = null)
		{
			var impellerData = new ImpellerData();
			if (impellerString == null)
			{
				try
				{
					impellerString = Clipboard.GetText();
				}
				catch
				{
					ShowStatusMsg("异常:获取剪切板失败");
					return;
				}
			}

			if (impellerString != null && impellerString.Length > 0)
			{
				impellerData = ImpellerParser(impellerString, ImpellerDataType.String);
				if (impellerData != null)
				{
					Impeller.InnerImpeller = impellerData.InnerImpeller;
					Impeller.State = impellerData.State;
					Impeller.Sections = impellerData.Sections;
					resultBox.Text = impellerData.InnerImpeller.Comment;
					resultCanvas.Children.Clear();
					UpdateArgsBindings();
					UpdateUI();
					ShowStatusMsg("粘贴成功！");
					if (impellerData.ErrorArgs.Count > 0)
					{
						int errorSectionsCount = 0;
						int errorArgsCount = 0;
						string errorArgsMsg = "粘贴成功，但是以下参数/截面有误，未能导入：";
						foreach (var error in impellerData.ErrorArgs)
						{
							if (Regex.IsMatch(error, @"\d+"))
							{
								errorSectionsCount++;
								errorArgsMsg += "\r\n第" + error + "个截面";
							}
							else
							{
								errorArgsCount++;
								errorArgsMsg += "\r\n" + error;
							}
						}
						ShowStatusMsg(" 已忽略的异常参数： " + errorArgsCount + "个, 异常截面： "
							+ errorSectionsCount + "个");
						MessageBox.Show(errorArgsMsg);
					}
				}
				else
				{
					ShowStatusMsg("无法识别的数据格式");
					return;
				}
			}
		}
		
		private void copyImpellerArgsBtn_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				Clipboard.SetText(ParseImpellerArgsToString());
			}
			catch
			{
				ShowStatusMsg("异常:复制到剪切板失败");
				return;
			}
			ShowStatusMsg("复制成功！");
		}
		private void pasteImpellerArgsBtn_Click(object sender, RoutedEventArgs e)
		{
			string impellerArgsString = "";
			var impellerData = new ImpellerData();
			try
			{
				impellerArgsString = Clipboard.GetText();
			}
			catch
			{
				ShowStatusMsg("异常:获取剪切板失败");
				return;
			}
			if (impellerArgsString != null && impellerArgsString.Length > 0)
			{
				impellerData = ImpellerParser(impellerArgsString, ImpellerDataType.String);
				if (impellerData != null)
				{
					Impeller.InnerImpeller = impellerData.InnerImpeller;
					Impeller.State = impellerData.State;
					resultCanvas.Children.Clear();
					resultBox.Text = "";
					UpdateArgsBindings();
					UpdateUI();
					ShowStatusMsg("粘贴成功！");
					if (impellerData.ErrorArgs.Count > 0)
					{
						string errorArgsMsg = "粘贴成功，但是以下参数有误，未能导入：";
						foreach (var error in impellerData.ErrorArgs)
						{
							errorArgsMsg += "\r\n" + error;
						}
						ShowStatusMsg(" 已忽略的异常参数： " + impellerData.ErrorArgs.Count + "个");
						MessageBox.Show(errorArgsMsg);
					}
				}
				else
				{
					ShowStatusMsg("无法识别的数据格式");
					return;
				}
			}
		}
		
		private void copySectionsDataBtn_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				Clipboard.SetText(ParseSectionsToString());
			}
			catch
			{
				ShowStatusMsg("异常:复制到剪切板失败");
				return;
			}
			ShowStatusMsg("复制成功！");
		}
		private void pasteSectionsDataBtn_Click(object sender, RoutedEventArgs e)
		{
			string sectionsString = "";
			var impellerData = new ImpellerData();
			try
			{
				sectionsString = Clipboard.GetText();
			}
			catch
			{
				ShowStatusMsg("异常:获取剪切板失败");
				return;
			}
			if (sectionsString != null && sectionsString.Length > 0)
			{
				impellerData = ImpellerParser(sectionsString, ImpellerDataType.String);
				if (impellerData != null)
				{
					Impeller.Sections = impellerData.Sections;
					resultCanvas.Children.Clear();
					resultBox.Text = "";
					UpdateArgsBindings();
					UpdateUI();
					ShowStatusMsg("粘贴成功！");
					if (impellerData.ErrorArgs.Count > 0)
					{
						string errorArgsMsg = "粘贴成功，但是以下截面的参数有误，未能导入：";
						foreach (var error in impellerData.ErrorArgs)
						{
							errorArgsMsg += "\r\n第" + error + "个截面";
						}
						ShowStatusMsg(" 已忽略的异常截面： " + impellerData.ErrorArgs.Count + "个");
						MessageBox.Show(errorArgsMsg);
					}
				}
				else
				{
					ShowStatusMsg("无法识别的数据格式");
					return;
				}
			}
		}

		private void copyResultBtn_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				Clipboard.SetText(ParseResultToString());
			}
			catch
			{
				ShowStatusMsg("异常:复制到剪切板失败");
				return;
			}
			ShowStatusMsg("复制成功！");
		}
		private void pasteResultBtn_Click(object sender, RoutedEventArgs e)
		{
			string resultString = "";
			var impellerData = new ImpellerData();
			try
			{
				resultString = Clipboard.GetText();
			}
			catch
			{
				ShowStatusMsg("异常:获取剪切板失败");
				return;
			}
			if (resultString != null && resultString.Length > 0)
			{
				impellerData = ImpellerParser(resultString, ImpellerDataType.String);
				if (impellerData != null)
				{
					resultCanvas.Children.Clear();
					resultBox.Text = impellerData.InnerImpeller.Comment;
					ShowStatusMsg("粘贴成功！");
				}
				else
				{
					ShowStatusMsg("无法识别的数据格式");
					return;
				}
			}
		}
		private void clearResultBtn_Click(object sender, RoutedEventArgs e)
		{
			resultCanvas.Children.Clear();
			resultBox.Text = "";
		}

		private void exportResultBtn_Click(object sender, RoutedEventArgs e)
		{
			saveFileDlg.Filter = "计算结果报告(*.html)|*.html|所有文件(*.*)|*.*";
			saveFileDlg.FileName = "CalculationReport";
			saveFileDlg.OverwritePrompt = false;
			if (resultCanvas.Children.Count <= 0 && resultBox.Text.Length <= 0)
			{
				ShowStatusMsg("没有结果可供导出",3000);
			}
			else
			{
				var saveResult = saveFileDlg.ShowDialog();
				if (saveResult == System.Windows.Forms.DialogResult.OK)
				{
					bool reportExist = false;
					FileOpResult fileOpResult = FileOpResult.Fail;
					if (File.Exists(saveFileDlg.FileName))
					{
						reportExist = true;
						//追加内容
						fileOpResult = ResultReportGenerator(
							saveFileDlg.FileName,
							ResultReportType.HTML,
							ResultReportFileMode.Append
						);
					}
					else
					{
						reportExist = false;
						//新建报告
						fileOpResult = ResultReportGenerator(
							saveFileDlg.FileName,
							ResultReportType.HTML,
							ResultReportFileMode.Create
						);
					}
					switch (fileOpResult)
					{
						case FileOpResult.Succeed:
							Impeller.SavePath = saveFileDlg.FileName;
							if (reportExist)
							{
								ShowStatusMsg("报告追加成功");
							}
							else
							{
								ShowStatusMsg("报告导出成功");
							}
							break;
						case FileOpResult.Fail:
							if (reportExist)
							{
								ShowStatusMsg("报告追加失败");
							}
							else
							{
								ShowStatusMsg("报告导出失败");
							}
							break;
						case FileOpResult.UnknownFormat:
							ShowStatusMsg("不是有效的结果报告格式，追加失败");
							break;
						default:
							ShowStatusMsg("异常：未知的操作状态");
							break;
					}
				}
			}			
		}

		private void GeneratedImpellerDataPasteHandler(string impellerString)
		{
			PasteDataStringHandler(impellerString);
		}
		private List<List<double>> ComparerCalculateHandler()
		{
			if (sectionsData.Items.Count <= 1)
			{
				State.IsSectionEnough = false;
			}
			else { State.IsSectionEnough = true; }
			UpdateArgsState();
			UpdateSectionsDataState();

			List<double> positions = new List<double>();
			List<double> rayleighYList = new List<double>();
			List<double> iterationYList = new List<double>();
			List<double> rayleighFreq = new List<double>(1);
			List<double> iterationFreq = new List<double>(1);

			if (State.CanRun)
			{
				//run
				FullSectionsDepartor(Impeller.Sections, Impeller.InnerImpeller);
				positions = Impeller.Sections.Select(sec => sec.Position).ToList();

				try
				{
					ResultMethod = CalculateMethod.Rayleigh;
					Impeller.InnerImpeller = Checker.Rayleigh(Impeller.InnerImpeller, 9.793);
					rayleighYList = Impeller.InnerImpeller.Y;
					rayleighFreq.Add(Impeller.InnerImpeller.LegacyVibrationFrequency);
					var maxRayleighY = rayleighYList.Max();
					for (int i = 0; i < rayleighYList.Count; i++)
					{
						rayleighYList[i] /= maxRayleighY;
					}

					ResultMethod = CalculateMethod.Iteration;
					Impeller.InnerImpeller = Checker.Iteration(Impeller.InnerImpeller);
					iterationYList = Impeller.InnerImpeller.Y;
					iterationFreq.Add(Impeller.InnerImpeller.LegacyVibrationFrequency);

					UpdateArgsBindings();
				}
				catch (Exception ex)
				{
					Impeller.InnerImpeller.Comment = "计算失败\n请检测参数和截面是否有错\n可能的原因：\n*叶片参数有误；\n*计算参数有误；\n*截面参数有误；\n*存在相同位置的截面。";
					ShowStatusMsg("异常:" + ex.Message);
				}
			}
			else
			{
				CalcAbortedMessageShower();
				ShowStatusMsg("计算已中止");
			}

			List<List<double>> param = new List<List<double>>(5);
			param.Add(positions);
			param.Add(rayleighYList);
			param.Add(iterationYList);
			param.Add(rayleighFreq);
			param.Add(iterationFreq);
			return param;
		}
		#endregion Command Handler
	}

	#region Aid classes
	#region Data Aid
	//Converters
	public class DoubleToStringConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			if (targetType != typeof(string)) { return null; }
			return ((double)value).ToString();
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			if (targetType != typeof(double)) { return null; }
			double pos;
			try
			{
				pos = System.Convert.ToDouble(value);
			}
			catch
			{
				return null;
			}
			return pos;
		}
	}
	public class OmegaToFreqStringConverter : IValueConverter
	{

		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			if (targetType != typeof(string)) { return null; }
			return ((double)value/(2 * Math.PI)).ToString();
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			if (targetType != typeof(double)) { return null; }
			double omega;
			try
			{
				omega = System.Convert.ToDouble(value) * 2 * Math.PI;
			}
			catch
			{
				return null;
			}
			return omega;
		}
	}
	public class AxisItem
	{
		public TextBlock Label { get; set; }
		public Line Mark { get; set; }
	}
	#endregion Data Aid

	#region UI Aid
	//Validation
	public class ImpellerArgsRule : ValidationRule
	{
		private bool isCanBeZero = true;
		public bool IsCanBeZero
		{
			get { return isCanBeZero; }
			set { isCanBeZero = value; }
		}


		public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
		{
			double val;
			if (value.ToString().Length == 0)
			{
				return new ValidationResult(false, "参数不能为空");
			}
			try
			{
				val = System.Convert.ToDouble(value);
			}
			catch
			{
				return new ValidationResult(false, "参数必须为数字");
			}

			if (val > 0 || (val == 0 && IsCanBeZero))
			{
				return ValidationResult.ValidResult;
			}
			else
			{
				if (IsCanBeZero)
				{
					return new ValidationResult(false, "参数不能为负");
				}
				else
				{
					return new ValidationResult(false, "参数必须为正");
				}
			}
		}
	}
	#endregion 

	#region Data Aid
	//Data source
	public class ImpellerData
	{
		private Impeller innerImpeller = new Impeller();
		public Impeller InnerImpeller
		{
			get { return innerImpeller; }
			set { innerImpeller = value; }
		}
		private List<FullSection> sections = new List<FullSection>(200);
		public List<FullSection> Sections
		{
			get { return sections; }
			set { sections = value; }
		}
		private ProhlState state = new ProhlState();
		public ProhlState State
		{
			get { return state; }
			set { state = value; }
		}
		private List<string> errorArgs = new List<string>(200);
		public List<string> ErrorArgs
		{
			get { return errorArgs; }
			set { errorArgs = value; }
		}
		public string SavePath { get; set; }
		public bool IsSavePathExist 
		{
			get
			{
				return this.SavePath != null;
			}
		}
	}
	#endregion Data Aid

	#region Calc Aid
	//Calc source
	public class ReadyState
	{
		private bool canRun;
		public bool CanRun
		{
			get 
			{
				return this.IsArgsOK && this.IsSectionsDataOK && this.IsSectionEnough;
			}
			set { canRun = value; }
		}

		public bool IsSectionsDataOK { get; set; }
		public bool IsSectionEnough { get; set; }
		public bool IsArgsOK { get; set; }
	}
	#endregion Calc Aid
	#endregion Aid classes

	#region Aid enums
	public enum CalculateMethod
	{
		Rayleigh = 0,
		Iteration = 1,
		Prohl = 2
	}
	public enum FileOpResult
	{
		Succeed = 0,
		Fail = 1,
		UnknownFormat = 2
	}
	public enum ImpellerDataType
	{
		String = 0,
		Xml = 1
	}
	public enum ResultReportType
	{
		HTML = 0
	}
	public enum ResultReportFileMode
	{
		Create = 0,
		Append = 1
	}
	public enum AxisType
	{
		X = 0,
		Y = 1
	}
	#endregion Aid enums
}
