/* Copyright (C) 2012  Byzod
 * Contact me : byzzod@gmail.com
 * 
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/. */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ImpellerVibrationChecker
{
	/// <summary>
	/// YComparer.xaml 的交互逻辑
	/// </summary>
	public partial class YComparer : Window
	{
		public delegate string ExportImageDelegate(Canvas surface, string path = null, Size? canvasSize = null);
		public delegate List<List<double>> ComparerCalculator();
		public delegate List<AxisItem> GetAxisItemsFromDoubleListDelegate(List<double> list, AxisType axisType, Point origin, double length, bool showMin = true, bool showMax = true);
		public event ExportImageDelegate ExportImage;
		public event ComparerCalculator ComparerCalculate;
		public event GetAxisItemsFromDoubleListDelegate GetAxisItems;

		public YComparer()
		{
			InitializeComponent();
		}

		//prop
		public int CopyFailRetryCount { get; set; }
		public List<List<double>> Params { get; set; }
		public List<List<double>> DiffList { get; set; }
		//prop end

		//UI
		private void closetBtn_Click(object sender, RoutedEventArgs e)
		{
			this.Close();
		}
		private void runBtn_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				if (ComparerCalculate != null)
				{
					Params = ComparerCalculate();
					DiffList = ParamsAnaly(Params);
				}
			}
			catch
			{
				MessageBox.Show("计算失败，请检查叶片、计算和截面参数");
				return;
			}
			if (Params.Count > 0
				&& DiffList.Count > 0
				&& Params[0].Count > 0
				&& Params[1].Count > 0
				&& Params[2].Count > 0
				&& Params[3].Count > 0
				&& Params[4].Count > 0
				&& DiffList[0].Count > 0
				&& DiffList[1].Count > 0)
			{
				resultPainter(Params, DiffList);
				resultShower(Params, DiffList);
			}
		}
		private void copyImageBtn_Click(object sender, RoutedEventArgs e)
		{
			Canvas surface;
			if (resultCanvas.Visibility == System.Windows.Visibility.Visible)
			{
				surface = resultCanvas;
			}
			else
			{
				surface = resultCompareCanvas;
			}

			if (surface.Children.Count > 0)
			{
				// Save current canvas transform
				Transform transform = surface.LayoutTransform;
				// reset current transform (in case it is scaled or rotated)
				surface.LayoutTransform = null;
				// Get the size of canvas
				Size size = new Size(surface.ActualWidth * 1.1, surface.ActualHeight * 1.2);

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

				// Create a white background render bitmap
				int dWidth = (int)size.Width;
				int dHeight = (int)size.Height;
				int dStride = dWidth * 4;
				byte[] pixels = new byte[dHeight * dStride];
				for (int i = 0; i < pixels.Length; i++)
				{
					pixels[i] = 0xFF;
				}
				BitmapSource bg = BitmapSource.Create(
					dWidth,
					dHeight,
					96,
					96,
					PixelFormats.Pbgra32,
					null,
					pixels,
					dStride
				);

				// Adding those two render bitmap to the same drawing visual
				DrawingVisual dv = new DrawingVisual();
				DrawingContext dc = dv.RenderOpen();
				dc.DrawImage(bg, new Rect(size));
				dc.DrawImage(renderBitmap, new Rect(size));
				dc.Close();

				// Render the result
				RenderTargetBitmap resultBitmap =
					new RenderTargetBitmap(
					(int)size.Width,
					(int)size.Height,
					96d,
					96d,
					PixelFormats.Pbgra32
				);
				resultBitmap.Render(dv);

				try
				{
					Clipboard.SetImage(resultBitmap);
					CopyFailRetryCount = 0;
				}
				catch
				{
					if (CopyFailRetryCount++ < 10)
					{
						this.copyImageBtn_Click(null, null);
					}
					else { return; }
				}
				finally
				{
					// Restore previously saved layout
					surface.LayoutTransform = transform;
				}
			}
		}
		System.Windows.Forms.SaveFileDialog saveFileDlg = new System.Windows.Forms.SaveFileDialog();
		private void exportImageBtn_Click(object sender, RoutedEventArgs e)
		{
			saveFileDlg.Filter = "图片(*.png)|*.png|所有文件(*.*)|*.*";
			saveFileDlg.OverwritePrompt = true;
			saveFileDlg.FileName = "CompareResultImage";
			var saveResult = saveFileDlg.ShowDialog();
			if (saveResult == System.Windows.Forms.DialogResult.OK)
			{
				Canvas surface;
				if (resultCanvas.Visibility == System.Windows.Visibility.Visible)
				{
					surface = resultCanvas;
				}
				else
				{
					surface = resultCompareCanvas;
				}
				try
				{
					if (ExportImage != null)
					{
						ExportImage(surface, saveFileDlg.FileName, new Size(resultCanvas.ActualWidth * 1.1, resultCanvas.ActualHeight * 1.2));
					}
				}
				catch
				{
					MessageBox.Show("保存失败！请检查写入权限，文件名和是否尝试覆盖只读文件");
				}
			}
		}
		private void copyResultBtn_Click(object sender, RoutedEventArgs e)
		{
			if (resultBox.Text != null && resultBox.Text.Length > 0)
			{
				try
				{
					Clipboard.SetText(resultBox.Text);
					CopyFailRetryCount = 0;
				}
				catch
				{
					if (CopyFailRetryCount++ < 10)
					{
						this.copyResultBtn_Click(null, null);
					}
					else { return; }
				}
			}
		}
		private void toggleCanvasBtn_Click(object sender, RoutedEventArgs e)
		{
			if (resultCanvas.Visibility == System.Windows.Visibility.Visible)
			{
				resultCanvas.Visibility = System.Windows.Visibility.Hidden;
				resultCompareCanvas.Visibility = System.Windows.Visibility.Visible;
				if (Params != null && Params.Count > 0)
				{
					resultPainter(Params, DiffList);
				}
			}
			else
			{
				resultCanvas.Visibility = System.Windows.Visibility.Visible;
				resultCompareCanvas.Visibility = System.Windows.Visibility.Hidden;
				if (Params != null && Params.Count > 0)
				{
					resultPainter(Params, DiffList);
				}
			}
		}

		private void resultPainter(List<List<double>> paramList, List<List<double>> diffList)
		{
			if (paramList.Count < 3 || GetAxisItems == null)
			{
				return;
			}
			List<double> positions = paramList[0];
			List<double> rayleighYList = paramList[1];
			List<double> iterationYList = paramList[2];
			List<double> absoluteDiffList = diffList[0];
			List<double> relativeDiffList = diffList[1];

			resultCanvas.Children.Clear();
			Brush fillBlue = (Brush)FindResource("ResultLineGradientBlue");
			Brush fillRed = (Brush)FindResource("ResultLineGradientRed");
			double height = resultCanvas.ActualHeight;
			double width = resultCanvas.ActualWidth;
			double axisGap = 18;

			Polyline rayleighLine = polylineGenerator(positions, rayleighYList, new Size(width - axisGap, height - axisGap));
			rayleighLine.Stroke = fillBlue;
			Polyline iterationLine = polylineGenerator(positions, iterationYList, new Size(width - axisGap, height - axisGap));
			iterationLine.Stroke = fillRed;
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
			TextBlock rayleighLegendHeader = new TextBlock();
			Line rayleighLegend = new Line();
			rayleighLegend.Stroke = fillBlue;
			TextBlock iterationLegendHeader = new TextBlock();
			Line iterationLegend = new Line();
			iterationLegend.Stroke = fillRed;

			Canvas.SetLeft(yHeader, width * 0.382);
			Canvas.SetLeft(rayleighLine, axisGap);
			Canvas.SetLeft(iterationLine, axisGap);
			Canvas.SetBottom(xAxisHeader, axisGap + 3);
			Canvas.SetRight(xAxisHeader, 0);
			Canvas.SetTop(yAxisHeader, -axisGap);

			Canvas.SetLeft(rayleighLegendHeader, axisGap + 10);
			Canvas.SetTop(iterationLegendHeader, 20);
			Canvas.SetLeft(iterationLegendHeader, axisGap + 10);

			yHeader.Text = "雷利法与迭代法振型";
			xAxisHeader.Text = "Position(m)";
			yAxisHeader.Text = "Y(1)";
			rayleighLegendHeader.Text = "雷利法";
			iterationLegendHeader.Text = "振型迭代法";
			rayleighLegend.X1 = axisGap + 10;
			rayleighLegend.X2 = axisGap + 70;
			rayleighLegend.Y1 = 18;
			rayleighLegend.Y2 = 18;
			iterationLegend.X1 = axisGap + 10;
			iterationLegend.X2 = axisGap + 70;
			iterationLegend.Y1 = 38;
			iterationLegend.Y2 = 38;

			xAxisItems = GetAxisItems(positions, AxisType.X, new Point(axisGap, height - axisGap), width - axisGap, showMin:false);
			yAxisItems = GetAxisItems(iterationYList, AxisType.Y, new Point(axisGap, height - axisGap), height - axisGap);

			resultCanvas.Children.Add(xAxis);
			resultCanvas.Children.Add(yAxis);
			resultCanvas.Children.Add(xAxisHeader);
			resultCanvas.Children.Add(yAxisHeader);
			resultCanvas.Children.Add(yHeader);
			resultCanvas.Children.Add(rayleighLine);
			resultCanvas.Children.Add(iterationLine);
			resultCanvas.Children.Add(rayleighLegendHeader);
			resultCanvas.Children.Add(iterationLegendHeader);
			resultCanvas.Children.Add(rayleighLegend);
			resultCanvas.Children.Add(iterationLegend);
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

			//Compare Result
			resultCompareCanvas.Children.Clear();
			height = resultCompareCanvas.ActualHeight;
			width = resultCompareCanvas.ActualWidth;
			double absoluteMax = absoluteDiffList.Max(e => Math.Abs(e));
			double relativeMax = relativeDiffList.Max(e => Math.Abs(e));
			double absoluteRange = 0;
			double relativeRange = 0;
			List<double> yCompAxisList = new List<double>();

			if (absoluteMax > relativeMax)
			{
				absoluteRange = height - axisGap;
				relativeRange = (height - axisGap) * (relativeMax / absoluteMax);
				yCompAxisList = absoluteDiffList.Select(e => e * 100).ToList();
			}
			else
			{
				absoluteRange = (height - axisGap) * (absoluteMax / relativeMax);
				relativeRange = height - axisGap;
				yCompAxisList = relativeDiffList.Select(e => e * 100).ToList();
			}
			Polyline absoluteLine = polylineGenerator(
				positions, 
				absoluteDiffList.Select(e => Math.Abs(e)).ToList(),
				new Size(width - axisGap, absoluteRange)
			);
			absoluteLine.Stroke = fillBlue;
			Polyline relativeLine = polylineGenerator(positions, 
				relativeDiffList.Select(e => Math.Abs(e)).ToList(),
				new Size(width - axisGap, relativeRange)
			);
			relativeLine.Stroke = fillRed;
			List<AxisItem> xCompAxisItems = new List<AxisItem>();
			List<AxisItem> yCompAxisItems = new List<AxisItem>();

			Polyline xCompAxis = new Polyline();
			Polyline yCompAxis = new Polyline();
			xCompAxis.Points.Add(new Point(0, height - axisGap));
			xCompAxis.Points.Add(new Point(width, height - axisGap));
			xCompAxis.Stroke = new SolidColorBrush(Colors.Black);
			yCompAxis.Points.Add(new Point(axisGap, 0));
			yCompAxis.Points.Add(new Point(axisGap, height));
			yCompAxis.Stroke = new SolidColorBrush(Colors.Black);
			TextBlock yCompHeader = new TextBlock();
			TextBlock xCompAxisHeader = new TextBlock();
			xCompAxisHeader.FontSize = 12;
			TextBlock yCompAxisHeader = new TextBlock();
			yCompAxisHeader.FontSize = 12;
			TextBlock absoluteLegendHeader = new TextBlock();
			Line absoluteLegend = new Line();
			absoluteLegend.Stroke = fillBlue;
			TextBlock relativeLegendHeader = new TextBlock();
			Line relativeLegend = new Line();
			relativeLegend.Stroke = fillRed;

			Canvas.SetLeft(absoluteLine, axisGap);
			Canvas.SetBottom(absoluteLine, axisGap);
			Canvas.SetLeft(relativeLine, axisGap);
			Canvas.SetBottom(relativeLine, axisGap);
			Canvas.SetLeft(yCompHeader, width * 0.382);
			Canvas.SetBottom(xCompAxisHeader, axisGap + 3);
			Canvas.SetRight(xCompAxisHeader, 0);
			Canvas.SetTop(yCompAxisHeader, -axisGap);

			Canvas.SetLeft(absoluteLegendHeader, axisGap + 10);
			Canvas.SetTop(relativeLegendHeader, 20);
			Canvas.SetLeft(relativeLegendHeader, axisGap + 10);

			yCompHeader.Text = "振型绝对与相对差值";
			xCompAxisHeader.Text = "Position(m)";
			yCompAxisHeader.Text = "差值(%)";
			absoluteLegendHeader.Text = "绝对差值";
			relativeLegendHeader.Text = "相对差值";
			absoluteLegend.X1 = axisGap + 10;
			absoluteLegend.X2 = axisGap + 70;
			absoluteLegend.Y1 = 18;
			absoluteLegend.Y2 = 18;
			relativeLegend.X1 = axisGap + 10;
			relativeLegend.X2 = axisGap + 70;
			relativeLegend.Y1 = 38;
			relativeLegend.Y2 = 38;

			xCompAxisItems = GetAxisItems(positions, AxisType.X, new Point(axisGap, height - axisGap), width - axisGap, showMin:false);
			if (yCompAxisList.Max() != 0)
			{
				yCompAxisItems = GetAxisItems(yCompAxisList, AxisType.Y, new Point(axisGap, height - axisGap), height - axisGap, showMax: false);
			}
			resultCompareCanvas.Children.Add(xCompAxis);
			resultCompareCanvas.Children.Add(yCompAxis);
			resultCompareCanvas.Children.Add(xCompAxisHeader);
			resultCompareCanvas.Children.Add(yCompAxisHeader);
			resultCompareCanvas.Children.Add(yCompHeader);
			resultCompareCanvas.Children.Add(absoluteLine);
			resultCompareCanvas.Children.Add(relativeLine);
			resultCompareCanvas.Children.Add(absoluteLegendHeader);
			resultCompareCanvas.Children.Add(relativeLegendHeader);
			resultCompareCanvas.Children.Add(absoluteLegend);
			resultCompareCanvas.Children.Add(relativeLegend);
			foreach (var item in xCompAxisItems)
			{
				resultCompareCanvas.Children.Add(item.Label);
				resultCompareCanvas.Children.Add(item.Mark);
			}
			foreach (var item in yCompAxisItems)
			{
				resultCompareCanvas.Children.Add(item.Label);
				resultCompareCanvas.Children.Add(item.Mark);
			}
		}
		private void resultShower(List<List<double>> paramList, List<List<double>> diffList)
		{
			resultBox.Text = resultGenerator(paramList, diffList);
		}
		private void resultCanvas_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			if (Params != null && Params.Count > 0 )
			{
				resultPainter(Params, DiffList);
			}
		}
		//UI END

		//Data
		private Polyline polylineGenerator(List<double> positions, List<double> yList, Size size)
		{
			Polyline polyline = new Polyline();
			polyline.Points = GetPointsFromY(positions, yList, size.Height, size.Width);
			return polyline;
		}
		private PointCollection GetPointsFromY(List<double> positions, List<double> yList, double canvasHeight, double canvasWidth)
		{
			var points = new List<Point>(yList.Count);
			double heightScale = canvasHeight / yList.Max();
			double widthScale = canvasWidth / positions.Max();

			points.Add(new Point(0, canvasHeight));
			for (int i = 0; i < yList.Count; i++)
			{
				points.Add(new Point(positions[i] * widthScale, canvasHeight - yList[i] * heightScale));
			}
			return new PointCollection(points);
		}
		private List<List<double>> ParamsAnaly(List<List<double>> paramList)
		{
			List<double> positions = paramList[0];
			List<double> rayleighYList = paramList[1];
			List<double> iterationYList = paramList[2];
			List<List<double>> diffList = new List<List<double>>(2);
			List<double> absoluteDiffList = new List<double>(positions.Count);
			List<double> relativeDiffList = new List<double>(positions.Count);

			for (int i = 0; i < rayleighYList.Count; i++)
			{
				absoluteDiffList.Add(rayleighYList[i] - iterationYList[i]);
				if (rayleighYList[i] == 0.0)
				{
					relativeDiffList.Add(0);
				}
				else
				{
					relativeDiffList.Add((rayleighYList[i] - iterationYList[i]) / rayleighYList[i]);
				}
			}
			diffList.Add(absoluteDiffList);
			diffList.Add(relativeDiffList);
			return diffList;
		}
		private string resultGenerator(List<List<double>> paramList, List<List<double>> diffList)
		{
			List<double> positions = paramList[0];
			List<double> rayleighYList = paramList[1];
			List<double> iterationYList = paramList[2];
			List<double> absoluteDiffList = diffList[0];
			List<double> relativeDiffList = diffList[1];
			StringBuilder result = new StringBuilder();
			double rayleighFreq = paramList[3][0];
			double iterationFreq = paramList[4][0];

			result
				.Append("叶片固有频率计算结果\r\n")
				.Append("阶数\t雷利法(Hz)\t迭代法(Hz)\t差值(Hz)\t\t相对差值\r\n")
				.Append("一阶").Append("\t")
				.Append(rayleighFreq.ToString("F6")).Append("\t") //rayleigh freq
				.Append(iterationFreq.ToString("F6")).Append("\t") //iteration freq
				.Append((rayleighFreq - iterationFreq).ToString("F6")).Append("\t");
			if (rayleighFreq == 0)
			{
				result.Append("\r\n\r\n");
			}
			else
			{
				result.Append(((rayleighFreq - iterationFreq) / rayleighFreq).ToString("P4")).Append("\r\n\r\n");
			}

			result
				.Append("叶片归一化振型计算结果\r\n")
				.Append("位置(m)\t雷利法\t\t迭代法\t\t差值\t\t相对差值");
			for (int i = 0; i < positions.Count; i++)
			{
				result.Append("\r\n")
					.Append(positions[i].ToString("F4")).Append("\t")
					.Append(rayleighYList[i].ToString("F8")).Append("\t")
					.Append(iterationYList[i].ToString("F8")).Append("\t")
					.Append(absoluteDiffList[i].ToString("P6")).Append("\t")
					.Append(relativeDiffList[i].ToString("P6"));
			}
			return result.ToString();
		}
		//Data end
	}
}
