namespace CLISharp
{
  public class ShellFunction
  {
    public delegate void funcdel(Shell s, params string[] args);
    private funcdel function;
    public string HelpText{get;private set;}
    public void SetHelp(string h)
    {
      HelpText = h;
    }
    public ShellFunction(funcdel f)
    {
      function = f;
    }
    public ShellFunction(funcdel f, string desc)
    {
      function = f;
      HelpText = desc;
    }
    public void Execute(Shell s, string[] ss)
    {
      function(s,ss);
    }
  }
}