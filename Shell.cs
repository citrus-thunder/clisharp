using System;
using System.Linq;
using System.Collections.Generic;

namespace CLISharp
{
	public class Shell
	{
		private bool toExit = false;
		public string PromptPrefix { get; set; } = "> ";
		public string WelcomeMessage { get; set; } = "Welcome. Please enter a command. Use \"exit\" or \"quit\" to close the program";
		public string ExitMessage { get; set; } = "Goodbye";
		public bool ShowWelcomeMessage { get; set; } = true;
		public bool ShowExitMessage { get; set; } = true;
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

		public ShellFunction AddFunction(string name) => AddFunction(new ShellFunction(name));

		public ShellFunction AddFunction(string name, Action<string[]> action) => AddFunction(new ShellFunction(name, action));

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

		public ShellFunction GetFunction(string name)
		{
			return _functions.Where(f => f.Name == name || f.Aliases.Contains(name)).FirstOrDefault();
		}

		public void Run()
		{
			string input = null;
			if (ShowWelcomeMessage)
			{
				Console.WriteLine(WelcomeMessage);
			}
			do
			{
				Console.Write(PromptPrefix);
				input = Console.ReadLine();
				Execute(input);
			}
			while (!toExit);
		}

		private void Exit(string[] args)
		{
			if (ShowExitMessage)
			{
				Console.WriteLine(ExitMessage);
			}
			OnExit();
			toExit = true;
		}

		protected virtual void OnExit()
		{

		}

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

		private void DisplayHelp()
		{
			foreach (var func in _functions)
			{
				DisplayHelp(func.Name);
			}
		}
	}
}