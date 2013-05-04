using System;
using System.Collections.Generic;
using System.Linq;

namespace Machine
{
	/// <summary>
	/// Provides instance that handles inputs and outputs request
	/// </summary>
	class IOHandler
	{
		/// <summary>
		/// Initialize new instance of IO handler
		/// </summary>
		public IOHandler()
		{
			this.fileIO = new FileIO();
		}

		#region Members

		private FileIO fileIO;

		#endregion Members


		#region Methods

		/// <summary>
		/// Load binary from a file
		/// </summary>
		/// <param name="filePath">File path</param>
		/// <returns>Binary file content</returns>
		public List<byte[]> LoadBin(List<string> filePaths)
		{
			List<byte[]> files = new List<byte[]>(filePaths.Count);

			for (int i = 0; i < filePaths.Count; i++)
			{
				FileIO.FileState state = fileIO.GetFileState(filePaths[i]);
				if (state == FileIO.FileState.Accessible || state == FileIO.FileState.CanRead)
				{
					files.Add(fileIO.ReadAllBytes(filePaths[i]));
				} 
			}

			return files;
		}

		/// <summary>
		/// Load from files
		/// </summary>
		/// <param name="filePaths">File path</param>
		/// <returns>Lines of files</returns>
		public List<List<string>> Load(List<string> filePaths)
		{
			List<List<string>> files = new List<List<string>>(filePaths.Count);

			for (int i = 0; i < filePaths.Count; i++)
			{
				FileIO.FileState state = fileIO.GetFileState(filePaths[i]);
				if (state == FileIO.FileState.Accessible || state == FileIO.FileState.CanRead)
				{
					files.Add(fileIO.ReadAllLines(filePaths[i]).ToList());
				}
			}

			return files;
		}

		/// <summary>
		/// Read all string as lines
		/// </summary>
		/// <param name="input">Input string</param>
		/// <param name="newLine">Line break</param>
		/// <returns>Lines of input</returns>
		public List<string> Read(string input, string newLine)
		{
			List<string> inputLines = new List<string>();
			string[] newLines = new string[] { newLine };

			inputLines = input.Split(newLines, StringSplitOptions.None).ToList();

			return inputLines;
		}

		/// <summary>
		/// Write output to a file
		/// </summary>
		/// <param name="filePath">File path</param>
		/// <param name="outputContent">Output string array</param>
		public void Write(string filePath, List<string> outputContent)
		{
			if (outputContent != null)
			{
				FileIO.FileState state = fileIO.GetFileState(filePath);
				if (state == FileIO.FileState.Accessible || state == FileIO.FileState.CanWrite || state == FileIO.FileState.NotExists)
				{
					fileIO.Write(filePath, outputContent.ToArray());
				}
			}
		}

		/// <summary>
		/// Write binary output to a file
		/// </summary>
		/// <param name="filePath">File path</param>
		/// <param name="outputContent">Output byte array</param>
		public void Write(string filePath, byte[] outputContent)
		{
			if (outputContent != null)
			{
				FileIO.FileState state = fileIO.GetFileState(filePath);
				if (state == FileIO.FileState.Accessible || state == FileIO.FileState.CanWrite || state == FileIO.FileState.NotExists)
				{
					fileIO.Write(filePath, outputContent.ToArray());
				}
			}
		}

		/// <summary>
		/// Display the output to console
		/// </summary>
		/// <param name="output">Output string array</param>
		public void Show(string[] output)
		{
			for (int i = 0; i < output.Length; i++)
			{
				Console.WriteLine(output[i]);
			}
		}

		/// <summary>
		/// Display the output to console
		/// </summary>
		/// <param name="output">Output string collection</param>
		public void Show(IEnumerable<string> output)
		{
			foreach (string item in output)
			{
				Console.WriteLine(item);
			}
		}

		#endregion Methods
	}
}
