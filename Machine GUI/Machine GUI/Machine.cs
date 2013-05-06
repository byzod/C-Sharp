using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace Machine_GUI
{
	class Machine
	{
		public List<string> Inputs { get; set; }
		public List<string> Outputs { get; set; }
		public bool IsInputFilePath { get; set; }
		public bool IsOuputFilePath { get; set; }
		public string ProblemID { get; set; }

		/// <summary>
		/// Initialize a new virtual Machine
		/// </summary>
		public Machine()
		{
			this.Inputs = new List<string>(1);
			this.Outputs = new List<string>(1);
			this.IsInputFilePath = true;
			this.IsOuputFilePath = true;
			this.ProblemID = "";
		}

		/// <summary>
		/// Awake teh Machine with override arguments
		/// </summary>
		/// <param name="args">Arguments to pass as shell command</param>
		/// <returns>Output lines</returns>
		public List<string> AwakeMachine(string args)
		{
			List<string> result = new List<string>(1);

			// Check Machine existence
			if (File.Exists("Machine.exe"))
			{
				// Awake Machine
				Process p = new Process();
				p.StartInfo = new ProcessStartInfo();
				p.StartInfo.FileName = "Machine.exe";
				p.StartInfo.Arguments = args;
				p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
				p.StartInfo.RedirectStandardOutput = true;
				p.StartInfo.UseShellExecute = false;
				p.StartInfo.CreateNoWindow = true;
				p.Start();

				// Redirect output
				StreamReader sr = p.StandardOutput;

				// Read all output lines
				while (!sr.EndOfStream)
				{
					result.Add(sr.ReadLine());
				}
			}
			else
			{
				throw (new FileNotFoundException("Machine.exe not found", "Machine.exe"));
			}

			return result;
		}

		/// <summary>
		/// Awake teh Machine
		/// </summary>
		/// <returns>Output lines</returns>
		public List<string> AwakeMachine()
		{
			string args = "";
			string input = "";
			string output = "";
			string id = " -p " + this.ProblemID;

			input = this.IsInputFilePath ? " -if " : " -is ";
			output = this.IsOuputFilePath ? " -ofc " : " -oc ";

			foreach (var item in this.Inputs)
			{
				if (item.Length > 0)
				{
					input += " \"" + item + "\" ";
				}
			}

			foreach (var item in this.Outputs)
			{
				if (item.Length > 0)
				{
					output += " \"" + item + "\" ";
				}
			}

			args = id + input + output;

			return AwakeMachine(args);
		}
	}
}
