using System;
using System.Collections.Generic;

namespace CLISharp
{
	public class ShellFunction
	{
		private string _name;
		public string Name
		{
			get => _name;
			set
			{
				_name = GetSanitizedName(value);
			}
		}
		public List<string> Aliases {get; set;} = new List<string>();
		public Action<string[]> Function { get; set; }
		public string HelpText { get; set; }

		public ShellFunction(string name)
		{
			Function = (string[] s) => { };
			Name = name;
		}

		public ShellFunction(string name, Action<string[]> a) : this(name)
		{
			Function = a;
		}

		public ShellFunction(string name, Action<string[]> a, string desc) : this(name, a)
		{
			HelpText = desc;
		}

		public static string GetSanitizedName(string name)
		{
			return name.ToLower().Replace(' ', '_').Trim();
		}

		public ShellFunction SetFunction(Action<string[]> function)
		{
			Function = function;
			return this;
		}

		public ShellFunction SetHelp(string help)
		{
			HelpText = help;
			return this;
		}

		public ShellFunction AddAlias(params string[] aliases)
		{
			foreach (var a in aliases)
			{
				var newAlias = GetSanitizedName(a);
				if (!Aliases.Contains(newAlias))
				{
					Aliases.Add(newAlias);
				}	
			}
			return this;
		}
	}
}