using System;
using System.Linq;
using System.Collections.Generic;

namespace CLISharp
{
	public class Shell
	{
		/// <summary>
		/// Shell.Run() has been called and the REP loop has begun
		/// </summary>
		public event EventHandler Started;

		/// <summary>
		/// Shell.Exit() has been called and the REP loop is ending
		/// </summary>
		public event EventHandler Exited;

		private bool toExit = false;

		/// <summary>
		/// String that prepends user input on each line
		/// </summary>
		/// <value></value>
		public string PromptPrefix { get; set; } = "> ";

		/// <summary>
		/// Message text shown in the console when the shell is launched
		/// </summary>
		/// <value></value>
		public string WelcomeMessage { get; set; } = "";

		/// <summary>
		/// Message text shown in the console when the shell is exited
		/// </summary>
		/// <value></value>
		public string ExitMessage { get; set; } = "";

		private List<ShellFunction> _functions = new List<ShellFunction>();

		public Shell()
		{
			AddFunction("help", GetHelp)
				.AddAlias("?")
				.SetHelp("Shows the Help Dialog");

			AddFunction("exit", Exit)
				.AddAlias("quit")
				.SetHelp("Exits the application");
		}

		/// <summary>
		/// Creates a new ShellFunction and adds it to the Shell
		/// </summary>
		/// <param name="name">
		/// The name of the ShellFunction. 
		/// Used as the command to call the ShellFunction from the console</param>
		public ShellFunction AddFunction(string name) => AddFunction(new ShellFunction(name));

		/// <summary>
		/// Creates a new ShellFunction and adds it to the Shell
		/// </summary>
		/// <param name="name">
		/// The name of the ShellFunction. 
		/// Used as the command to call the ShellFunction from the console
		/// </param>
		/// <param name="action">
		/// The Action executed when the ShellFunction is called by the Shell
		/// </param>
		public ShellFunction AddFunction(string name, Action<string[]> action) => AddFunction(new ShellFunction(name, action));

		/// <summary>
		/// Adds an existing ShellFunction to the Shell
		/// </summary>
		/// <param name="function">The ShellFunction to add to the Shell</param>
		public ShellFunction AddFunction(ShellFunction function)
		{
			ShellFunction ret = null;
			if (!_functions.Where(f => f.Name == function.Name || f.Aliases.Contains(function.Name)).Any())
			{
				_functions.Add(function);
				ret = function;
			}
			return ret;
		}

		/// <summary>
		/// Returns a reference to a ShellFunction by specified name or alias
		/// </summary>
		/// <param name="name">Name or Alias of ShellFunction to get</param>
		/// <returns></returns>
		public ShellFunction GetFunction(string name)
		{
			return _functions.Where(f => f.Name == name || f.Aliases.Contains(name)).FirstOrDefault();
		}

		/// <summary>
		/// Begins the Shell's read-eval-print loop
		/// </summary>
		public void Run()
		{
			string input = null;
			if (WelcomeMessage != null && WelcomeMessage.Length > 0)
			{
				Console.WriteLine(WelcomeMessage);
			}
			Started?.Invoke(this, new EventArgs());
			
			do
			{
				Console.Write(PromptPrefix);
				input = Console.ReadLine();
				Execute(input);
			}
			while (!toExit);
			
			Exited?.Invoke(this, new EventArgs());
		}

		/// <summary>
		/// Ends the Shell's read-eval-print loop
		/// </summary>
		/// <param name="args"></param>
		private void Exit(string[] args)
		{
			if (ExitMessage != null && ExitMessage.Length > 0)
			{
				Console.WriteLine(ExitMessage);
			}
			toExit = true;
		}

		/// <summary>
		/// Executes a ShellFunction with the given name or alias
		/// </summary>
		/// <param name="input">Name or Alias of the ShellFunction to execute</param>
		/// <remarks>
		/// Shell.Run() calls this method every time a command is
		/// submitted to the Shell through the CLI. This only needs 
		/// to be called directly when executing a ShellFunction
		/// outside of the REP loop
		/// </remarks>
		public void Execute(string input)
		{
			input = input.ToLower();
			string[] ss = input.Split(' ');
			var function = GetFunction(ss[0]);
			if (function != null)
			{
				function.Function(ss);
			}
			else
			{
				Console.WriteLine($"Function '{input}' not recognized. Sorry!");
			}
		}

		/// <summary>
		/// Displays the Help Text for the
		/// specified ShellFunction, or, if
		/// no ShellFunction specified, all
		/// ShellFunctions with help text.
		/// </summary>
		/// <param name="args"></param>
		private void GetHelp(string[] args)
		{
			if (args.Length <= 1)
			{
				DisplayHelp();
			}
			else
			{
				DisplayHelp(args[1]);
			}
		}

		/// <summary>
		/// Prints ShellFunction help text to the console
		/// </summary>
		/// <param name="funcKey">
		/// Name or Alias of ShellFunction to display help 
		/// text for
		/// </param>
		private void DisplayHelp(string funcKey)
		{
			string res = String.Empty;
			var func = GetFunction(funcKey);
			if (func != null)
			{
				var text = func.HelpText;
				if (text != String.Empty && text != null)
				{
					var aliases = String.Empty;
					if (func.Aliases.Count > 0)
					{
						aliases = " [";
						for (var i = 0; i < func.Aliases.Count; i++)
						{
							aliases += func.Aliases[i];
							if (i < func.Aliases.Count - 1)
							{
								aliases += ", ";
							}
						}
						aliases += "]";
					}
					res = $"{func.Name}{aliases}: {text}";
				}
			}
			else
			{
				res = $"Function \"{funcKey}\" not found.";
			}
			if (res != String.Empty) Console.WriteLine(res);
		}

		/// <summary>
		/// Prints ShellFunction help text to the console
		/// for all ShellFunctions set in the Shell that
		/// have specified help text.
		/// </summary>
		private void DisplayHelp()
		{
			foreach (var func in _functions)
			{
				DisplayHelp(func.Name);
			}
		}
	}
}