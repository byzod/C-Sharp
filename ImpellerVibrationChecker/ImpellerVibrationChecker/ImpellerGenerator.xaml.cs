/* Copyright (C) 2012  Byzod
 * Contact me : byzzod@gmail.com
 * 
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/. */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ImpellerVibrationChecker
{
	/// <summary>
	/// ImpellerGenerator.xaml 的交互逻辑
	/// </summary>
	public partial class ImpellerGenerator : Window
	{
		public event Action<string> ResultApply;
		public ImpellerGenerator()
		{
			InitializeComponent();
			InitializeArgs();
		}

		private void InitializeArgs()
		{
			Impeller = new ImpellerData();
			ImpellerArgs.DataContext = Impeller.InnerImpeller;
			shapeChoice.SelectedIndex = 1;
			IsParamModifiedByUser = true;
		}

		//Properties
		public ImpellerData Impeller { get; set; }
		public bool IsParamModifiedByUser { get; set; }
		//Properties END

		//UI
		private void shapeThumbnail_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			shapeThumbnailPaint();
		}
		private void shapeThumbnailPaint()
		{
			shapeThumbnail.Children.Clear();
			Brush fill;
			try
			{
				fill = (Brush)FindResource("ResultLineGradientBlue");
			}
			catch
			{
				fill = new SolidColorBrush(Colors.BlueViolet);
			}
			switch (shapeChoice.SelectedIndex)
			{
				case 0://圆
					Ellipse circle = new Ellipse();
					circle.Height = shapeThumbnail.ActualHeight * 0.9;
					circle.Width = circle.Height;
					circle.Fill = fill;
					Canvas.SetLeft(circle, (shapeThumbnail.ActualWidth - circle.Width) / 2);
					Canvas.SetTop(circle, shapeThumbnail.ActualHeight * 0.05);
					shapeThumbnail.Children.Add(circle);
					break;
				case 1://矩形
					Rectangle rect = new Rectangle();
					rect.Width = shapeThumbnail.ActualWidth * 0.9;
					rect.Height = shapeThumbnail.ActualHeight * 0.8;
					rect.Fill = fill;
					Canvas.SetLeft(rect, (shapeThumbnail.ActualWidth - rect.Width) / 2);
					Canvas.SetTop(rect, shapeThumbnail.ActualHeight * 0.1);
					shapeThumbnail.Children.Add(rect);
					break;
				case 2://直角三角形
					Polygon tri = new Polygon();
					double length = shapeThumbnail.ActualHeight * 2 / 3 * Math.Sqrt(3);
					tri.Points = new PointCollection{
						new Point(shapeThumbnail.ActualWidth * 0.05, shapeThumbnail.ActualHeight * 0.1),
						new Point(shapeThumbnail.ActualWidth * 0.05, shapeThumbnail.ActualHeight * 0.9),
						new Point(shapeThumbnail.ActualWidth * 0.95, shapeThumbnail.ActualHeight * 0.9)
					};
					tri.Fill = fill;
					shapeThumbnail.Children.Add(tri);
					break;
				default:
					break;
			}
		}
		private void shapeChoice_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			switch (shapeChoice.SelectedIndex)
			{
				case 0://圆
					startSecParam1Header.Text = "半径(m)";
					startSecParam2Header.Text = null;
					startSecParam2.Visibility = System.Windows.Visibility.Hidden;
					endSecParam1Header.Text = "半径(m)";
					endSecParam2Header.Text = null;
					endSecParam2.Visibility = System.Windows.Visibility.Hidden;
					break;
				case 1://矩形
					startSecParam1Header.Text = "宽(m)";
					startSecParam2Header.Text = "高(m)";
					startSecParam2.Visibility = System.Windows.Visibility.Visible;
					endSecParam1Header.Text = "宽(m)";
					endSecParam2Header.Text = "高(m)";
					endSecParam2.Visibility = System.Windows.Visibility.Visible;
					break;
				case 2://三角形
					startSecParam1Header.Text = "宽(m)";
					startSecParam2Header.Text = "高(m)";
					startSecParam2.Visibility = System.Windows.Visibility.Visible;
					endSecParam1Header.Text = "宽(m)";
					endSecParam2Header.Text = "高(m)";
					endSecParam2.Visibility = System.Windows.Visibility.Visible;
					break;
				default:
					break;
			}
			shapeThumbnailPaint();
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

		private void generate_Click(object sender, RoutedEventArgs e)
		{
			double sPosition = 0.0;
			double ePosition = 1.0;
			double sParam1 = 1;
			double sParam2 = 1;
			double eParam1 = 1;
			double eParam2 = 1;
			double density = 1;
			int count = 1;
			StringBuilder impellerDataString = new StringBuilder();

			try
			{
				sPosition = Convert.ToDouble(startPosition.Text);
				ePosition = Convert.ToDouble(endPosition.Text);
				sParam1 = Convert.ToDouble(startSecParam1.Text);
				sParam2 = Convert.ToDouble(startSecParam2.Text);
				eParam1 = Convert.ToDouble(endSecParam1.Text);
				eParam2 = Convert.ToDouble(endSecParam2.Text);
				density = Convert.ToDouble(impellerDensity.Text);
				count = Convert.ToInt32(sectionCount.Text);
			}
			catch
			{
				MessageBox.Show("参数有误，请检查后再运行");
				return;
			}
			ShapeType shape = ShapeType.Rect;
			switch (shapeChoice.SelectedIndex)
			{
				case 0://圆
					shape = ShapeType.Circle;
					break;
				case 1://矩形
					shape = ShapeType.Rect;
					break;
				case 2://三角形
					shape = ShapeType.Triangle;
					break;
				default:
					break;
			}
			List<FullSection> genSecs = GenerateSections(
				shape,
				sPosition,
				ePosition,
				sParam1,
				sParam2,
				eParam1,
				eParam2,
				density,
				count
			);
			if (genSecs != null)
			{
				Impeller.Sections = genSecs;
				impellerDataString
					.Append(ParseImpellerArgsToString()).Append("\r\n\r\n")
					.Append(ParseSectionsToString());

				resultBox.Text = impellerDataString.ToString();
			}
			else
			{
				MessageBox.Show("生成截面失败，请检查参数是否有误");
				return;
			}
		}
		private void appendSec_Click(object sender, RoutedEventArgs e)
		{
			double sPosition = 0.0;
			double ePosition = 1.0;
			double sParam1 = 1;
			double sParam2 = 1;
			double eParam1 = 1;
			double eParam2 = 1;
			double density = 1;
			int count = 1;
			StringBuilder impellerDataString = new StringBuilder();

			try
			{
				sPosition = Convert.ToDouble(startPosition.Text);
				ePosition = Convert.ToDouble(endPosition.Text);
				sParam1 = Convert.ToDouble(startSecParam1.Text);
				sParam2 = Convert.ToDouble(startSecParam2.Text);
				eParam1 = Convert.ToDouble(endSecParam1.Text);
				eParam2 = Convert.ToDouble(endSecParam2.Text);
				density = Convert.ToDouble(impellerDensity.Text);
				count = Convert.ToInt32(sectionCount.Text);
			}
			catch
			{
				MessageBox.Show("参数有误，请检查后再运行");
				return;
			}
			ShapeType shape = ShapeType.Rect;
			switch (shapeChoice.SelectedIndex)
			{
				case 0://圆
					shape = ShapeType.Circle;
					break;
				case 1://矩形
					shape = ShapeType.Rect;
					break;
				case 2://三角形
					shape = ShapeType.Triangle;
					break;
				default:
					break;
			}
			List<FullSection> genSecs = GenerateSections(
				shape,
				sPosition,
				ePosition,
				sParam1,
				sParam2,
				eParam1,
				eParam2,
				density,
				count
			);

			if (genSecs != null)
			{
				Impeller.Sections = Impeller.Sections.Concat(genSecs).ToList();
				impellerDataString
					.Append(ParseImpellerArgsToString()).Append("\r\n\r\n")
					.Append(ParseSectionsToString());

				resultBox.Text = impellerDataString.ToString();
			}
			else
			{
				MessageBox.Show("生成截面失败，请检查参数是否有误");
				return;
			}
		}
		private void copy_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				Clipboard.SetText(resultBox.Text);
			}
			catch
			{
				copy_Click(null, null);
			}
		}
		System.Windows.Forms.SaveFileDialog saveFileDlg = new System.Windows.Forms.SaveFileDialog();
		private void export_Click(object sender, RoutedEventArgs e)
		{
			saveFileDlg.Filter = "叶片文件(*.txt)|*.txt|所有文件(*.*)|*.*";
			saveFileDlg.OverwritePrompt = true;
			saveFileDlg.FileName = "GeneratedImpellerData";
			var saveResult = saveFileDlg.ShowDialog();
			if (saveResult == System.Windows.Forms.DialogResult.OK)
			{
				File.WriteAllText(saveFileDlg.FileName, resultBox.Text);
			}
		}
		private void apply_Click(object sender, RoutedEventArgs e)
		{
			if (ResultApply != null)
			{
				try
				{
					ResultApply(resultBox.Text);
				}
				catch
				{
					apply_Click(null, null);
				}
			}
		}
		//UI END

		//DATA
		public enum ShapeType
		{
			Circle = 0,
			Rect = 1,
			Triangle = 2
		}
		private List<FullSection> GenerateSections(ShapeType shape, double startPosition, double endPosition, double startParam1, double startParam2, double endParam1, double endParam2, double density, int count)
		{
			if (endPosition <= startPosition
				|| count < 2)
			{
				return null;
			}
			var sections = new List<FullSection>(count);
			var positions = new List<double>(count);
			var inertiaMoments = new List<double>(count);
			var areas = new List<double>(count);
			var masses = new List<double>(count);
			double param1Step = (endParam1 - startParam1) / (count - 1);
			double param2Step = (endParam1 - startParam1) / (count - 1);
			double pieceLength = Math.Abs(endPosition - startPosition) / (count - 1);

			//Positions
			for (int i = 0; i < count; i++)
			{
				positions.Add(startPosition + i * pieceLength);
			}

			//Areas
			switch (shape)
			{
				case ShapeType.Circle:
					for (int i = 0; i < count; i++)
					{
						areas.Add(Math.PI * Math.Pow(startParam1 + i * param1Step, 2));
					}
					break;
				case ShapeType.Rect:
					for (int i = 0; i < count; i++)
					{
						areas.Add((startParam1 + i * param1Step) * (startParam2 + i * param2Step));
					}
					break;
				case ShapeType.Triangle:
					for (int i = 0; i < count; i++)
					{
						areas.Add((startParam1 + i * param1Step) * (startParam2 + i * param2Step) / 2);
					}
					break;
				default:
					break;
			}

			//Masses
			for (int i = 0; i < count; i++)
			{
				if (i == 0)
				{
					masses.Add(density * (pieceLength * (areas[1] + areas[0]) / 2));
				}
				else
				{
					masses.Add(density * (pieceLength * (areas[i] + areas[i - 1]) / 2));
				}
			}

			//Inertia Moments
			double width = 0;
			double height = 0;
			switch (shape)
			{
				case ShapeType.Circle:
					for (int i = 0; i < count; i++)
					{
						//pi*d^4 / 32
						inertiaMoments.Add(Math.PI * Math.Pow((startParam1 + i * param1Step) * 2, 4) / 32);
					}
					break;
				case ShapeType.Rect:
					for (int i = 0; i < count; i++)
					{
						//dh^3/12 + hd^3/12
						width = startParam1 + i * param1Step;
						height = startParam2 + i * param2Step;
						inertiaMoments.Add((width * Math.Pow(height, 3) + height * Math.Pow(width, 3)) / 12);
					}
					break;
				case ShapeType.Triangle:
					for (int i = 0; i < count; i++)
					{
						//dh^3/36 + hd^3/36
						width = startParam1 + i * param1Step;
						height = startParam2 + i * param2Step;
						inertiaMoments.Add((width * Math.Pow(height, 3) + height * Math.Pow(width, 3)) / 36);
					}
					break;
				default:
					break;
			}

			//Sections generate
			for (int i = 0; i < count; i++)
			{
				sections.Add(new FullSection(positions[i], inertiaMoments[i], areas[i], masses[i]));
			}

			return sections;
		}

		private string ParseImpellerArgsToString()
		{
			string impellerArgsString = "[Impeller]";
			impellerArgsString += "\r\nHeight=" + impellerHeight.Text;
			impellerArgsString += "\r\nDensity=" + impellerDensity.Text;
			impellerArgsString += "\r\nE=" + impellerE.Text;
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

		private void resultBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			if (resultBox.Text != null && resultBox.Text.Length > 0)
			{
				copy.IsEnabled = true;
				export.IsEnabled = true;
				apply.IsEnabled = true;
			}
			else
			{
				copy.IsEnabled = false;
				export.IsEnabled = false;
				apply.IsEnabled = false;
			}
		}

		//DATA END
	}
}
