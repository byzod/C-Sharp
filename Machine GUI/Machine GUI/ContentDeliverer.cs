using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Machine_GUI
{
	class ContentDeliverer
	{
		/// <summary>
		/// Deliver the required arguments to solver
		/// </summary>
		/// <param name="problemID">ID of problem</param>
		/// <param name="inputs">Input, only support 1 file now</param>
		/// <param name="outputs">Output, only support 1 file now</param>
		/// <param name="isInputFilePath">If is file mode</param>
		/// <param name="isOutputFilePath">If is file mode</param>
		/// <returns>Text content result</returns>
		public UITextContent GetResult(string problemID, List<string> inputs, List<string> outputs, bool isInputFilePath, bool isOutputFilePath)
		{
			Machine machine = new Machine();
			machine.ProblemID = problemID;
			machine.Inputs = inputs;
			machine.Outputs = outputs;
			machine.IsInputFilePath = isInputFilePath;
			machine.IsOuputFilePath = isOutputFilePath;

			UITextContent content = new UITextContent();

			if (isInputFilePath)
			{
				content.Input = System.IO.File.ReadAllLines(inputs.First()).ToList();
			}
			else
			{
				content.Input = inputs;
			}

			content.Output = machine.AwakeMachine();

			return content;
		}
	}

	/// <summary>
	/// UI update content
	/// </summary>
	class UITextContent
	{
		public List<string> Input { get; set; }
		public List<string> Output { get; set; }

		public UITextContent()
		{
			this.Input = new List<string>();
			this.Output = new List<string>();
		}
	}
}
