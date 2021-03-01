using System;
using System.Collections.Generic;

namespace CLISharp
{
	public class Shell
	{
		//protected delegate void funcdel(Shell s, params string[] args);
		private bool toExit = false;
		public string PromptPrefix { get; set; } = "> ";
		public string WelcomeMessage { get; set; } = "Welcome. Please enter a command. Use \"exit\" or \"quit\" to close the program";
		public string ExitMessage { get; set; } = "Goodbye";
		public bool ShowWelcomeMessage { get; set; } = true;
		public bool ShowExitMessage { get; set; } = true;
		private Dictionary<string, ShellFunction> Functions;

		public Shell()
		{
			Functions = new Dictionary<string, ShellFunction>()
			{
				{"help",new ShellFunction(GetHelp)},
				{"?", new ShellFunction(GetHelp)},
				{"exit",new ShellFunction(Exit)},
				{"quit",new ShellFunction(Exit)}
			};
		}

		public ShellFunction AddFunction(string name) => AddFunction(name, new ShellFunction());

		public ShellFunction AddFunction(string name, ShellFunction function)
		{
			ShellFunction ret = null;
			if (!Functions.ContainsKey(name))
			{
				Functions.Add(name, function);
				ret = function;
			}
			return ret;
		}

		public ShellFunction AddAlias(string currentname, string alias)
		{
			ShellFunction res = null;

			if (Functions.ContainsKey(currentname))
			{
				res = Functions[currentname];
				Functions.Add(alias, Functions[currentname]);
			}

			return res;
		}

		public ShellFunction GetFunction(string name)
		{
			return Functions[name];
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
			string[] ss = input.Split(' ');
			input = input.ToLower();
			if (Functions.ContainsKey(ss[0]))
			{
				Functions[ss[0]].Function(ss);
			}
			else
			{
				Console.WriteLine($"Function '{input}' not recognized. Sorry!");
			}
		}

		protected void RegisterFunctions(Dictionary<string, ShellFunction> func)
		{
			foreach (var function in func)
			{
				Functions.Add(function.Key.ToLower(), function.Value);
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
			if (Functions.ContainsKey(funcKey))
			{
				string text = Functions[funcKey].HelpText;
				if (text != String.Empty && text != null)
				{
					res = $"{funcKey}: {text}";
				}
			}
			else
			{
				res = $"Function '{funcKey}' not found.";
			}
			if (res != String.Empty) Console.WriteLine(res);
		}

		private void DisplayHelp()
		{
			foreach (var func in Functions)
			{
				DisplayHelp(func.Key);
			}
		}
	}
}