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

		/// <summary>
		/// Alternate names that can be used to call this ShellFunction
		/// </summary>
		/// <typeparam name="string"></typeparam>
		/// <returns></returns>
		public List<string> Aliases {get; set;} = new List<string>();

		/// <summary>
		/// The Action performed by this ShellFunction when called by the
		/// Shell
		/// </summary>
		/// <value></value>
		public Action<string[]> Function { get; set; }

		/// <summary>
		/// Help text displayed for this ShellFunction
		/// by the Shell's help command
		/// </summary>
		/// <value></value>
		public string HelpText { get; set; }

		/// <summary>
		/// Creates a new ShellFunction
		/// </summary>
		/// <param name="name">Name of the new ShellFunction</param>
		public ShellFunction(string name)
		{
			Function = (string[] s) => { };
			Name = name;
		}

		/// <summary>
		/// Creates a new ShellFunction
		/// </summary>
		/// <param name="name">Name of the new ShellFunction</param>
		/// <param name="a">
		/// Action executed by the ShellFunction when called by the
		/// Shell.
		/// </param>
		/// <returns></returns>
		public ShellFunction(string name, Action<string[]> a) : this(name)
		{
			Function = a;
		}

		/// <summary>
		/// Creates a new ShellFunction
		/// </summary>
		/// <param name="name">Name of the new ShellFunction</param>
		/// <param name="a">
		/// Action executed by the ShellFunction when called by the
		/// Shell.
		/// </param>
		/// <param name="desc">
		/// Help text for the ShellFunction
		/// </param>
		/// <returns></returns>
		public ShellFunction(string name, Action<string[]> a, string desc) : this(name, a)
		{
			HelpText = desc;
		}

		/// <summary>
		/// Returns a command-line safe name of the given string
		/// </summary>
		/// <param name="name">Name of the ShellFunction</param>
		/// <returns></returns>
		public static string GetSanitizedName(string name)
		{
			return name.ToLower().Replace(' ', '_').Trim();
		}

		/// <summary>
		/// Sets the Action that is executed by the ShellFunction
		/// when called by the Shell
		/// </summary>
		/// <param name="function"></param>
		/// <returns></returns>
		public ShellFunction SetFunction(Action<string[]> function)
		{
			Function = function;
			return this;
		}

		/// <summary>
		/// Sets the Help text for the ShellFunction
		/// </summary>
		/// <param name="help">Help text for the ShellFunction</param>
		/// <returns></returns>
		public ShellFunction SetHelp(string help)
		{
			HelpText = help;
			return this;
		}

		/// <summary>
		/// Adds a new Alias for the ShellFunction
		/// </summary>
		/// <param name="aliases">New Alias for the ShellFunction</param>
		/// <returns></returns>
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