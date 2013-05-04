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
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ImpellerVibrationChecker
{
	/// <summary>
	/// About.xaml 的交互逻辑
	/// </summary>
	public partial class About : Window
	{
		public About()
		{
			InitializeComponent();
			this.EmailAddress = "byzzod@gmail.com";
			email.DataContext = this;
		}
		public string EmailAddress { get; set; }

		private void button1_Click(object sender, RoutedEventArgs e)
		{
			this.Close();
		}

		private void email_MouseDown(object sender, MouseButtonEventArgs e)
		{
			try
			{
				System.Diagnostics.Process.Start("mailto://" + EmailAddress);
			}
			catch { System.Diagnostics.Process.Start("https://plus.google.com/110715073765465568673"); }
		}

		private void email_MouseHover(object sender, MouseEventArgs e)
		{
			switch (e.RoutedEvent.Name)
			{
				case "MouseEnter":
					this.Cursor = Cursors.Hand;
					break;
				case "MouseLeave":
					this.Cursor = Cursors.Arrow;
					break;
				default:
					break;
			}

		}
	}
}
