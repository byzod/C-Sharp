using System;
using System.Linq;

namespace Machine
{
	class Program
	{
		static void Main(string[] args)
		{
			Options options = new Options();
			IOHandler io = new IOHandler();
			ProblemSolverHelper solverHelper = new ProblemSolverHelper();

			// Try parse options from args
			try
			{
				options.Parse(args.ToList());
				options.Validate();
			}
			catch (Exception exception)
			{
				options.Errors.Add(exception.Message);
			}

			// Treat empty args call as help request
			if (args.Length <= 0)
			{
				options.ShouldShowHelp = true;
			}

			// Continue only if no errors or show help instead if it's a help request
			if (options.Errors.Count == 0
				&& !(options.ShouldShowHelp || options.ShouldListProblemIDs))
			{
				// Main process
				ProblemSolverHelper.SolveState solveState = 
					solverHelper.CallSolver(
												options.Problem.ID.ToLower(), 
												ref options
											);

				if (solveState == ProblemSolverHelper.SolveState.SolverNotExist)
				{
					options.Errors.Add("Unrecognized problem ID: " + options.Problem.ID);
				}

				// Result output: console display
				if (options.Output.Mode == Options.OutputMode.ConsoleOnly 
					|| options.Output.Mode == Options.OutputMode.FileAndConsole)
				{
					if (options.IsBinaryWrite)
					{
						io.Show(new string[] { "[Binary content]" });
					}
					else
					{
						foreach (var output in options.Output.Content)
						{
							io.Show(output);
						}
					}
				}

				// Result output: file output
				if (options.Output.Mode == Options.OutputMode.FileAndConsole 
					|| options.Output.Mode == Options.OutputMode.FileOnly)
				{
					if (options.IsBinaryWrite)
					{
						for (int i = 0; i < options.Output.BinaryContent.Count; i++)
						{
							try
							{
								io.Write(options.Output.Paths[i], options.Output.BinaryContent[i]);
							}
							catch (Exception exception)
							{
								options.Errors.Add(exception.Message);
							}
						}
					}
					else
					{
						for (int i = 0; i < options.Output.Content.Count; i++)
						{
							try
							{
								io.Write(options.Output.Paths[i], options.Output.Content[i]);
							}
							catch (Exception exception)
							{
								options.Errors.Add(exception.Message);
							}
						}
					}
				}
			}
			
			// Show help in console
			if (options.ShouldShowHelp)
			{
				io.Show(Options.Help);
			}
			// Show problem IDs list in console
			if (options.ShouldListProblemIDs)
			{
				io.Show(solverHelper.ProblemSolvers.Keys);
			}

			if (options.Errors.Count > 0
				&& !(options.ShouldShowHelp || options.ShouldListProblemIDs)
				&& options.ConsoleMode != Options.ConsoleDisplayMode.WarningsOnly 
				&& options.ConsoleMode != Options.ConsoleDisplayMode.None)
			{
				// Show errors
				Console.WriteLine("Process aborted");
				Console.WriteLine("Error" + (options.Errors.Count > 1 ? "s" : "") + ":");
				foreach (string error in options.Errors)
				{
					Console.WriteLine("  " + error);
				}
			}

			if (options.Warnings.Count > 0
				&& !(options.ShouldShowHelp || options.ShouldListProblemIDs)
				&& options.ConsoleMode != Options.ConsoleDisplayMode.ErrorsOnly
				&& options.ConsoleMode != Options.ConsoleDisplayMode.None)
			{
				// Show warnings
				Console.WriteLine("Warning" + (options.Warnings.Count > 1 ? "s" : "") + ":");
				foreach (string waning in options.Warnings)
				{
					Console.WriteLine("  " + waning);
				}
			}
		}
	}
}
