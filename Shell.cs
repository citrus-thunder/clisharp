using System;
using System.Collections.Generic;

namespace CLISharp{
    public class Shell
    {
        protected delegate void funcdel(Shell s, params string[] args);
        private bool toExit = false;
        public string PromptPrefix{get;set;} = "> ";
        public string WelcomeMessage{get;set;} = "Welcome. Please enter a command. Use \"exit\" or \"quit\" to close the program";
        public bool ShowMessage{get;set;} = true;
        private Dictionary<string,funcdel> Functions = new Dictionary<string,funcdel>
        {
            {"test",Test},
            {"exit",Exit},
            {"quit",Exit}
        };
        public void Run()
        {
            string input = null;
            if(ShowMessage)
            {
                Console.WriteLine(WelcomeMessage);
            }
            do
            {
                Console.Write(PromptPrefix);
                input = Console.ReadLine();
                Execute(input);
            }
            while(!toExit);
        }
        private static void Test(Shell s, params string[] args)
        {
            Console.WriteLine($"Args for command {args[0]}: ");
            for (var i = 1; i < args.Length;i++)
            {
                Console.Write($"{args[i]}, ");
            }
            Console.WriteLine();
        }
        private static void Exit(Shell s, params string[] args)
        {
            Console.WriteLine("Goodbye.");
            s.toExit = true;
        }
        public void Execute(string input)
        {
            string[] ss = input.Split(' ');
            input = input.ToLower();
            if (Functions.ContainsKey(ss[0]))
            {
                Functions[ss[0]](this,ss);
            }
            else
            {
                Console.WriteLine($"Function '{input}' not recognized. Sorry!");
            }
        }
        protected void RegisterFunctions(Dictionary<string,funcdel> func)
        {
            foreach (var function in func)
            {
                Functions.Add(function.Key.ToLower(),function.Value);
            }
        }
    }
}