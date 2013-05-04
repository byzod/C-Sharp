using System;
using System.Text;
using System.IO;

namespace Machine
{
	/// <summary>
	/// Provides instance that handles input and output of files
	/// </summary>
	class FileIO
	{
		/// <summary>
		/// Indicate the given file state
		/// </summary>
		public enum FileState
		{
			Accessible = 0,
			CanWrite = 1,
			CanRead = 2,
			Inaccessible = 3,
			NotExists = 4,
			Unknow = 5
		}

		#region Methods

		/// <summary>
		/// Get the file exists/read/write attributes of given file path
		/// </summary>
		/// <param name="filePath">File path</param>
		/// <returns>FileState enum</returns>
		internal FileState GetFileState(string filePath)
		{
			FileState state = FileState.Unknow;
			if ( File.Exists( filePath ) )
			{
				bool canWrite = false;
				bool canRead = false;
				FileAttributes fileAttr = File.GetAttributes(filePath);
				if ( (fileAttr & FileAttributes.ReadOnly) != FileAttributes.ReadOnly )
				{
					canWrite = true;
				}
				using ( StreamReader sr = File.OpenText(filePath) )
				{
					// TODO : only read a small chunk instead of read a whole line
					if (sr.ReadLine() != null)
					{
						canRead = true;
					}
					sr.Close();
				}

				if (canRead && canWrite)
				{
					state = FileState.Accessible;
				}
				else
				{
					if (!canRead && !canWrite)
					{
						state = FileState.Inaccessible;
					}
					else if (canWrite)
					{
						state = FileState.CanWrite;
					}
					else // canRead = true
					{
						state = FileState.CanRead;
					}
				}
			}
			else
			{
				state = FileState.NotExists;
			}

			return state;
		}

		/// <summary>
		/// Read all bytes, return empty byte array on error
		/// </summary>
		/// <param name="filePath">File path</param>
		/// <returns>Read bytes array</returns>
		internal byte[] ReadAllBytes(string filePath)
		{
			byte[] bytes;
			try
			{
				bytes = File.ReadAllBytes(filePath);
			}
			catch (Exception)
			{
				bytes = new byte[] { };
			}
			return bytes;
		}
		
		/// <summary>
		///  Read all lines, return empty string array on error. Only supports UTF-8 files
		/// </summary>
		/// <param name="filePath">File path</param>
		/// <returns>Read string array</returns>
		internal string[] ReadAllLines(string filePath)
		{
			string[] lines;
			try
			{
				lines = File.ReadAllLines(filePath, Encoding.UTF8);
			}
			catch (Exception)
			{
				lines = new string[] { };
			}

			return lines;
		}

		/// <summary>
		/// Write all bytes to file
		/// </summary>
		/// <param name="filePath">File path</param>
		/// <param name="content">Byte array to write</param>
		internal void Write(string filePath, byte[] content)
		{
			File.WriteAllBytes(filePath, content);
		}

		/// <summary>
		/// Write all bytes to file
		/// </summary>
		/// <param name="filePath">File path</param>
		/// <param name="content">String array to write</param>
		internal void Write(string filePath, string[] content)
		{
			File.WriteAllLines(filePath, content, Encoding.UTF8);
		}

		#endregion Methods

	}
}
