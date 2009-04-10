using System;
using FluentBuild.BuildFile;

namespace Build
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Environment.ExitCode = 0;
            Console.WriteLine("args");
            foreach (string arg in args)
            {
                Console.WriteLine(arg);
            }
            var build = new MainBuildTask();
            build.Execute();
        }
    }
}