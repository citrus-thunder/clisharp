# CLISharp
## A Simple C# CLI Library
CLISharp provides a simple toolset for creating REPL shell applications, useful for simple programs and testing libraries.

----

## Creating and Running a Shell
To create a shell, simply create a new `Shell` object. Running the shell is as simple as calling `Run()` on the shell object.
```csharp
using CLISharp;

class Program
{
	static void Main(string[] args)
	{
		var s = new Shell();
		s.Run();
	}
}
```
## Built-In Shell Functions
A shell has a handful of built-in functions as soon as it is instantiated.

* `help`, `?`: Shows help text for any functions the Shell has access to which have their help information defined.
* `exit`, `quit`: Exits the shell, returning control to the calling method.

## Adding New Functions
While the base `Shell` class has some basic functionality built in, it will be much more useful once given access to new commands.

Doing so looks like this:
```csharp
// Creates a new Shell object
var s = new Shell();

// Adds a "greet" command to the Shell, which will be run whenever the user types "greet" into the CLI
s.AddFunction("greet")
	.SetFunction((string[] args) => {Console.WriteLine("Hello!")})
	.SetHelp("Greets the User");

// Begins the Shell Read-Evaluate-Print loop process.
s.Run();
```
A Shell can run any method with an `Action<string[]>` compatible signature, so adding a function can also look something like this:
```csharp
// ...
s.AddFunction("greet")
	.SetFunction(Greet)
	.SetHelp("Greets the User");
// ...

// elsewhere in the code
void Greet(string[] args)
{
	Console.WriteLine("Hello!");
}
```
## Customizing the Shell
Several aspects of the Shell's behavior can be changed through properties and events.

### Shell Properties

* `WelcomeMessage`: Sets the message that shows upon the shell's launch.
* `ExitMessage`: Sets the message that shows when the shell is closed.
* `PromptPrefix`: Sets what characters prepend the user's commands on each line.

### Shell Events

* `Started`: Invoked when `Run()` is called and the Shell begins the REP loop.
* `Exited`: Invoked when `Exit()` is called and the Shell leaves the REP loop.

Shells can further be customized by creating a new Shell class derived from `CLISharp.Shell` and extending the Shell's capabilities there.

## Tips and Tricks

* `AddFunction()` has a few helpful overloads, allowing for a number of ways to create functions. Be sure to check them out and use whichever methods work best for you.
* A ShellFunction can create and run a new Shell, allowing you to nest Shells that have their own separate functions and behaviors. When a nested Shell is exited, the application will resume in the calling Shell right where it left off.
* The `string[] args` parameter passed to ShellFunctions contains an array of all text entered by the user separated by space. Use this to collect and parse user-provided arguments in your ShellFunctions.

----

## Notes on Future Updates and Collaboration
I created this simple tool as a way to test libraries and create very simple command-line utilities -- think contact books and specialized calculators. The tool already satisfies its scope, and is therefore considered complete.

Some future work is planned on adding integrated support for handling arguments and options passed to shell functions, but other than that you can expect only occasional updates to this tool.

Enhancements and fixes are welcome, but anybody who wishes to expand the scope of this application beyond its current capabilities are encouraged to fork the project and continue their extension as a separate repository.