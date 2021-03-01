using System;

namespace CLISharp
{
  public class ShellFunction
  {

    public Action<string[]> Function {get; set;}
    public string HelpText{get; set;}

    public ShellFunction()
    {
      Function = (string[] s) => {};
    }
    public ShellFunction(Action<string[]> a)
    {
      Function = a;
    }
    public ShellFunction(Action<string[]> a, string desc) : this(a)
    {
      HelpText = desc;
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
  }
}