using System;
using System.Collections.Generic;
using System.Linq;

namespace Machine
{
	/// <summary>
	/// Provides instance of options handles the option parse and configurations
	/// </summary>
	class Options
	{
		#region Structures

		/// <summary>
		/// Mode of output
		/// </summary>
		public enum OutputMode
		{
			/// <summary>
			/// Outputs saved to file only
			/// </summary>
			FileOnly = 0,
			/// <summary>
			/// Outputs saved to file and displayed in console
			/// </summary>
			FileAndConsole = 1,
			/// <summary>
			/// Outputs only displayed in console
			/// </summary>
			ConsoleOnly = 2
		}
		/// <summary>
		/// Mode of input
		/// </summary>
		public enum InputMode
		{
			/// <summary>
			/// Read from file, file path must be provided
			/// </summary>
			File = 0,
			/// <summary>
			/// Read from string
			/// </summary>
			String = 1
		}
		/// <summary>
		/// Console display style
		/// </summary>
		public enum ConsoleDisplayMode
		{
			/// <summary>
			/// Display errors and warnings in console
			/// </summary>
			ErrorsAndWarnings = 0,
			/// <summary>
			/// Display errors in console
			/// </summary>
			ErrorsOnly = 1,
			/// <summary>
			/// Display warnings in console
			/// </summary>
			WarningsOnly = 2,
			/// <summary>
			/// Do not display errors nor warnings
			/// </summary>
			None = 3
		}

		#endregion Structures


		#region Properties

		/// <summary>
		/// Raw strings of command line arguments
		/// </summary>
		public List<string> RawArguments { get; set; }
		/// <summary>
		/// Should show help or not
		/// </summary>
		public bool ShouldShowHelp { get; set; }
		/// <summary>
		/// Should list all problem IDs or not
		/// </summary>
		public bool ShouldListProblemIDs { get; set; }
		/// <summary>
		/// Should load file in binary mode
		/// </summary>
		public bool IsBinaryLoad { get; set; }
		/// <summary>
		/// Should write file in binary mode
		/// </summary>
		public bool IsBinaryWrite { get; set; }
		/// <summary>
		/// Should display errors or warnings in console output
		/// </summary>
		public ConsoleDisplayMode ConsoleMode { get; set; }
		/// <summary>
		/// New line character
		/// </summary>
		//public string NewLine { get; set; }
		/// <summary>
		/// Arguments warnings
		/// </summary>
		public List<string> Warnings { get; set; }
		/// <summary>
		/// Errors
		/// </summary>
		public List<string> Errors { get; set; }
		/// <summary>
		/// Problem options
		/// </summary>
		public StringsOptionsItem Problem { get; set; }
		/// <summary>
		/// Input options
		/// </summary>
		public InputOptionsItem Input { get; set; }
		/// <summary>
		/// Output content as string[] array
		/// </summary>
		public OutputOptionsItem Output { get; set; }

		private static readonly string[] help = {
													"Usage:",
													"  Machine -i File_Path [File_Path2]... -p Problem_ID [Problem_args]",
													"          [-o File_Path [File_Path2] ...] [-Options [Args] ]...",
													"  ",
													"Example:",
													"  Machine -if \"file1\" \"file2\" -p \"example\" -oc",
													"  Command above will read two files with name \"file1\" and \"file2\",",
													"  call solver named \"example\", display the result only in console",
													"  ",
													"Options:",
													"  -i[f|s]    Input file paths",
													"               f: Treat arguments as file paths (default)",
													"               s: Treat arguments as string input. Each single argument",
													"                  separated by spaces will be treat as a line.",
													"  ",
													"  -p         Problem ID, the rest arguments after the first one are",
													"             all sent as problem parameters",
													"  ",
													"  -o[fc]     Output file paths, default path is the same as the inputs",
													"             Default options is -of. -ofc/-ocf are both O.K.",
													"               f: Output results to files",
													"               c: Output results to console",
													"  ",
													"  -l         List all available problem IDs",
													//"  ",
													//"  -l[w|m|u]  New line character",
													//"               w: Windows type, \\r\\n (default)",
													//"               m: Mac type, \\r",
													//"               u: Unix type, \\n",
													"  ",
													"  -c[ew]     Display errors or/and warnings in console. Default -cew",
													"             Use -c to display nothing. -cew/-cwe are both O.K.",
													"               e: Display errors",
													"               w: Display warnings",
													"  ",
													"  -h         Display this help",
												};
		/// <summary>
		/// Help
		/// </summary>
		public static string[] Help
		{
			get
			{
				return Options.help;
			}
		}

		#endregion Properties


		#region Constructors

		/// <summary>
		/// Default constructor
		/// </summary>
		public Options()
		{
			this.RawArguments = new List<string>();
			this.ShouldShowHelp = false;
			this.ShouldListProblemIDs = false;
			this.IsBinaryLoad = false;
			this.IsBinaryWrite = false;
			this.ConsoleMode = ConsoleDisplayMode.ErrorsAndWarnings;
			//this.NewLine = NewLines.Win;
			this.Warnings = new List<string>();
			this.Errors = new List<string>();
			this.Problem = new StringsOptionsItem();
			this.Input = new InputOptionsItem();
			this.Output = new OutputOptionsItem();
		}

		/// <summary>
		/// Initialize new options instance with args string list
		/// </summary>
		/// <param name="args">Arguments</param>
		public Options(List<string> args)
			: this()
		{
			this.Parse(args);
		}

		#endregion Constructors


		#region Methods

		/// <summary>
		/// Reset all properties
		/// </summary>
		private void Reset()
		{
			this.RawArguments = new List<string>();
			this.ShouldShowHelp = false;
			this.ShouldListProblemIDs = false;
			this.IsBinaryLoad = false;
			this.IsBinaryWrite = false;
			this.ConsoleMode = ConsoleDisplayMode.ErrorsAndWarnings;
			//this.NewLine = NewLines.Win;
			this.Warnings = new List<string>();
			this.Errors = new List<string>();
			this.Problem = new StringsOptionsItem();
			this.Input = new InputOptionsItem();
			this.Output = new OutputOptionsItem();
		}

		/// <summary>
		/// Check if the arguments are valid. Store errors in this.Errors
		/// </summary>
		public void Validate()
		{
			FileIO fileIO = new FileIO();

			// Check every menmber
			if (this.RawArguments == null || this.RawArguments.Count <= 0)
			{
				this.Errors.Add("Arguments is empty.");
			}
			if (this.Problem.ID == null || this.Problem.ID.Length <= 0)
			{
				this.Errors.Add("Problem ID must be set! Use -p to set the problem ID.");
			}
			if ((this.Input.Mode == InputMode.File && (this.Input.Paths.Count <= 0 || this.Input.Paths == null)))
			{
				this.Errors.Add("Input file path is empty! Use -if to set the input file paths.");
			}
			else
			{
				FileIO.FileState state;
				for (int i = 0; i < this.Input.Paths.Count; i++)
				{
					state = fileIO.GetFileState(Input.Paths[i]);
					if (!(state == FileIO.FileState.CanRead || state == FileIO.FileState.Accessible))
					{
						this.Errors.Add("Cannot open \"" + Input.Paths[i] + "\"");
					}
				}
			}
			if ((this.Output.Mode != OutputMode.ConsoleOnly && this.Input.Mode == InputMode.String)
				&& (this.Output.Paths.Count <= 0 || this.Output.Paths == null))
			{
				this.Errors.Add("Output file path is empty! Use -of to set the output file paths.");
			}
			else
			{
				FileIO.FileState state;
				for (int i = 0; i < this.Output.Paths.Count; i++)
				{
					state = fileIO.GetFileState(Output.Paths[i]);
					if (!(state == FileIO.FileState.CanWrite || state == FileIO.FileState.Accessible || state == FileIO.FileState.NotExists))
					{
						this.Errors.Add("Cannot write to \"" + Output.Paths[i] + "\"");
					}
				}
			}
		}

		/// <summary>
		/// Parse given args array to options object, will clear replace current object
		/// </summary>
		/// <param name="args">Arguments strings</param>
		public void Parse(List<string> args)
		{
			this.Reset();
			this.RawArguments = args;

			// Parse the first group
			//for (int i = 0; i < args.Count; i++)
			//{
			//    if (args[i] != null && args[i].Length > 0 && args[i][0] == '-')
			//    {
			//        string option = args[i].Substring(1).ToLower();
			//        if (option.Length <= 0)
			//        {
			//            option = " ";
			//        }
			//        List<string> optionArgs = new List<string>();
			//        switch (option[0])
			//        {
			//            case 'l':
			//                // If user set the line break...
			//                if (option.Length > 1)
			//                {
			//                    switch (option)
			//                    {
			//                        case "lw":
			//                            this.NewLine = NewLines.Win;
			//                            break;
			//                        case "lm":
			//                            this.NewLine = NewLines.Mac;
			//                            break;
			//                        case "lu":
			//                            this.NewLine = NewLines.Unix;
			//                            break;
			//                        default:
			//                            this.Warnings.Add((i + 1) + "# Unrecognized switch: \"" + option.Substring(1) + "\" in " + args[i]);
			//                            break;
			//                    }
			//                }
			//                else // ...else set it to windows type
			//                {
			//                    this.NewLine = NewLines.Win;
			//                }
			//                break;
			//            default:
			//                break;
			//        }
			//    }
			//}

			// Parse the second group
			for (int i = 0; i < args.Count; i++)
			{
				if ( args[i] != null && args[i].Length > 0 && args[i][0] == '-')
				{
					List<string> optionArgs = new List<string>();
					string option = args[i].Substring(1).ToLower();
					if (option.Length <= 0)
					{
						option = " ";
					}

					switch (option[0])
					{
						case 'i':
							// If user set the input mode...
							if (option.Length > 1)
							{
								switch (option)
								{
									case "if":
										this.Input.Mode = InputMode.File;
										break;
									case "is":
										this.Input.Mode = InputMode.String;
										break;
									default:
										this.Errors.Add("Invalid input switch: \"" + option.Substring(1) + "\" in " + args[i]);
										break;
								}
							}
							else // ...else set it to file mode
							{
								this.Input.Mode = InputMode.File;
							}

							if (this.Errors.Count <= 0)
							{
								// Then get all the file paths / strings
								optionArgs = this.GetAllOptionArgs(args, i + 1);
								// Skip the next optionArgs.Length args, becuase they are count as option args
								i += optionArgs.Count;
								this.Input.Content = new List<List<string>>(optionArgs.Count);
								IOHandler io = new IOHandler();

								if (this.Input.Mode == InputMode.File)
								{
									this.Input.Paths = optionArgs;
								}
                                else if (this.Input.Mode == InputMode.String)
                                {
                                    this.Input.Content.Add(new List<string>(optionArgs.Count));

                                    for (int j = 0; j < optionArgs.Count; j++)
                                    {
                                        this.Input.Content[0].Add(optionArgs[j]);
                                    }
                                }
							}

							break;
						case 'p':
							if (option.Length > 1)
							{
								this.Warnings.Add((i + 1) + "# Unrecognized option: " + args[i]);
							}
							else
							{
								// Then get all the problem parameters including problem id
								optionArgs = this.GetAllOptionArgs(args, i + 1);
								// Skip the next optionArgs.Length args, becuase they are count as option args
								i += optionArgs.Count;
								this.Problem.Args = new List<string>(optionArgs.Count);

								if (optionArgs.Count > 0)
								{
									this.Problem.ID = optionArgs[0];
									this.Problem.Args = new List<string>(optionArgs.Count - 1);
									for (int j = 1; j < optionArgs.Count; j++)
									{
										this.Problem.Args.Add(optionArgs[j]);
									}
								}
							}

							break;
						case 'o':
							// If user set the output mode...
							if (option.Length > 1)
							{
								if (option.Contains('f') && option.Contains('c')) // fc
								{
									this.Output.Mode = OutputMode.FileAndConsole;
								}
								else if (option.Contains('f')) // f
								{
									this.Output.Mode = OutputMode.FileOnly;
								}
								else if (option.Contains('c')) // c
								{
									this.Output.Mode = OutputMode.ConsoleOnly;
								}
								else
								{
									this.Errors.Add("Invalid output switch: \"" + option.Substring(1) + "\" in " + args[i]);
								}

								string unknownSwitches = option.Substring(1).Replace("f", "").Replace("c", "");
								if (unknownSwitches.Length > 0)
								{
									this.Warnings.Add((i + 1) + "# Unrecognized switch: \"" + unknownSwitches + "\" in " + args[i]);
								}
							}
							else // ...else set it to file only mode
							{
								this.Output.Mode = OutputMode.FileOnly;
							}

							if (this.Errors.Count <= 0)
							{
								// Then get all the file paths / strings
								optionArgs = this.GetAllOptionArgs(args, i + 1);
								// Skip the next optionArgs.Length args, becuase they are count as option args
								i += optionArgs.Count;

								this.Output.Paths = optionArgs;
							}

							break;
						case 'l':
							if (option.Length > 1)
							{
								this.Warnings.Add((i + 1) + "# Unrecognized option: " + args[i]);
							}
							else
							{
								this.ShouldListProblemIDs = true;
							}

							break;
						case 'c':
							// If user set the output mode with switches...
							if (option.Length > 1)
							{
								if (option.Contains('e') && option.Contains('w')) // ew
								{
									this.ConsoleMode = ConsoleDisplayMode.ErrorsAndWarnings;
								}
								else if (option.Contains('e')) // c
								{
									this.ConsoleMode = ConsoleDisplayMode.ErrorsOnly;
								}
								else if (option.Contains('w')) // w
								{
									this.ConsoleMode = ConsoleDisplayMode.WarningsOnly;
								}
								else
								{
									this.Errors.Add("Invalid console switch: \"" + option.Substring(1) + "\" in " + args[i]);
								}

								string unknownSwitches = option.Substring(1).Replace("e", "").Replace("w", "");
								if (unknownSwitches.Length > 0)
								{
									this.Warnings.Add((i + 1) + "# Unrecognized switch: \"" + unknownSwitches + "\" in " + args[i]);
								}
							}
							else // ...or only -c
							{
								this.ConsoleMode = ConsoleDisplayMode.None;
							}

							break;
						case 'h':
							if (option.Length > 1)
							{
								this.Warnings.Add((i + 1) + "# Unrecognized option: " + args[i]);
							}
							else
							{
								this.ShouldShowHelp = true;
							}

							break;
						default:
							this.Warnings.Add((i + 1) + "# Unrecognized option: " + args[i]);

							break;
					}

					// Write to input file dir if output paths is not set
					if (this.Input.Paths.Count > 0 && this.Output.Paths.Count < this.Input.Paths.Count)
					{
						for (int j = this.Output.Paths.Count; j < this.Input.Paths.Count; j++)
						{
							this.Output.Paths.Add(
								System.IO.Path.Combine(System.IO.Path.GetDirectoryName(this.Input.Paths[j]),
									System.IO.Path.GetFileNameWithoutExtension(this.Input.Paths[j]))
								+ ".out");
						}
					}
				}
				else
				{
					this.Warnings.Add((i + 1) + "# Unrecognized argument: " + args[i]);
				}
			}
		}

		/// <summary>
		/// Load content depend on parsed settings
		/// </summary>
		public void LoadContent()
		{
			IOHandler io = new IOHandler();
			if (this.Input.Mode == InputMode.File)
			{
				if (this.IsBinaryLoad)
				{
					this.Input.BinaryContent = io.LoadBin(this.Input.Paths);
				}
				else
				{
					this.Input.Content = io.Load(this.Input.Paths);
				}
			}
		}
		
		/// <summary>
		/// Get all arguments of certain option
		/// </summary>
		/// <param name="args">The whole args string list</param>
		/// <param name="startIndex">Option position</param>
		/// <returns>Arguments string list of option in the given position</returns>
		private List<string> GetAllOptionArgs(List<string> args, Int32 startIndex)
		{
			List<string> optionArgs = new List<string>(args.Count - startIndex); // Assume there won't be many arguments, so use RAM to gain performance
			Int32 position = startIndex;
			while (position < args.Count 
				&& (args[position] == null || args[position].Length <= 0 || args[position][0] != '-')) // Treat anything but options string which start with '-' as args
			{
				if (args[position] == null || args[position].Length <= 0)
				{
					optionArgs.Add("");
				}
				else
				{
					optionArgs.Add(args[position]);
				}
				position++;
			}
			return optionArgs;
		}

		#endregion Methods
	}
}
