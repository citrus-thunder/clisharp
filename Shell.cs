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
		private Dictionary<string, ShellFunction> Functions = new Dictionary<string, ShellFunction>
				{
            {"help",new ShellFunction(GetHelp)},
            {"?", new ShellFunction(GetHelp)},
            {"exit",new ShellFunction(Exit)},
						{"quit",new ShellFunction(Exit)}
				};
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
		private static void Exit(Shell s, params string[] args)
		{
			if (s.ShowExitMessage)
			{
				Console.WriteLine(s.ExitMessage);
			}
			s.toExit = true;
		}
		public void Execute(string input)
		{
			string[] ss = input.Split(' ');
			input = input.ToLower();
			if (Functions.ContainsKey(ss[0]))
			{
				Functions[ss[0]].Execute(this, ss);
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
    private static void GetHelp(Shell s, string[] args)
    {
      if (args.Length <= 1)
      {
        s.DisplayHelp();
      }
      else
      {
        s.DisplayHelp(args[1]);
      }
    }
		private void DisplayHelp(string funcKey)
		{
      string res = String.Empty;
      if (Functions.ContainsKey(funcKey))
      {
        string text = Functions[funcKey].HelpText;
        if(text != String.Empty && text != null)
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