using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Machine_GUI
{
	public partial class MainWindow : Form
	{
		/// <summary>
		/// Main window process
		/// </summary>
		public MainWindow()
		{
			InitializeComponent();
			Initialize();
		}

		/// <summary>
		/// Initialize UI
		/// </summary>
		public void Initialize()
		{
			Solvers solvers = new Solvers();
			try
			{
				solvers.GetIDs();
				problemSelector.Items.AddRange(solvers.SolverIDs.ToArray());
				problemSelector.SelectedIndex = 0;
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Error");
			}
		}

		// Run with inputs, output to outputBox
		private void runBtn_Click(object sender, EventArgs e)
		{
			// Prepare arguments
			List<string> input = new List<string>(1);
			List<string> output = new List<string>(1) { outputPathBox.Text };
			string problemID = "";

			if (problemSelector.SelectedIndex > -1 && problemSelector.Items.Count > 0)
			{
				problemID = problemSelector.Items[problemSelector.SelectedIndex].ToString();
			}

			if (isInputFilePath.Checked)
			{
				input.Add(inputPathBox.Text);
			}
			else
			{
				input = inputBox.Lines.ToList();
				for (int i = 0; i < input.Count; i++)
				{
					input[i] = input[i].Replace("\\", "\\\\").Replace("\"", "\\\"");
				}
			}

			// Run
			ContentDeliverer deliverer = new ContentDeliverer();
			UITextContent content = new UITextContent();
			try
			{
				content = deliverer.GetResult(problemID, input, output, isInputFilePath.Checked, isOutputFilePath.Checked);
			}
			catch(Exception ex)
			{
				content.Output.Add(ex.Message);
			}

			// Update UI
			inputBox.Text = "";
			outputBox.Text = "";

			string tempStr = "";

			tempStr = String.Join("\n", content.Input.ToArray());
			tempStr = (tempStr.Length > 2147483647) ? tempStr.Substring(0, 2147483647) : tempStr;
			inputBox.Text = tempStr.Replace("\\\"", "\"");

			tempStr = String.Join("\n", content.Output.ToArray());
			tempStr = (tempStr.Length > 2147483647) ? tempStr.Substring(0, 2147483647) : tempStr;
			outputBox.Text = tempStr;
		}

		// Lock path text box
		private void isInputFilePath_CheckedChanged(object sender, EventArgs e)
		{
			inputPathBox.Enabled = isInputFilePath.Checked;
		}

		// Lock path text box
		private void isOutputFilePath_CheckedChanged(object sender, EventArgs e)
		{
			outputPathBox.Enabled = isOutputFilePath.Checked;
		}

		// Pop file choose dialog
		private void inputPathBox_Click(object sender, EventArgs e)
		{
			openFileDialog1.ShowDialog();
			inputPathBox.Text = openFileDialog1.FileName;
		}

		// Pop file choose dialog
		private void outputPathBox_Click(object sender, EventArgs e)
		{
			saveFileDialog1.ShowDialog();
			outputPathBox.Text = saveFileDialog1.FileName;
		}
		
		// Clear paths
		private void clearPathBtn_Click(object sender, EventArgs e)
		{
			inputPathBox.Text = "";
			outputPathBox.Text = "";
		}
	}
}
