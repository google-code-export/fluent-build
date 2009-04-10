using System;
using FluentBuild.BuildFile;

namespace Build
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            foreach (var arg in args)
            {
                Console.WriteLine(arg);
            }
            var build = new MainBuildTask();
            build.Execute();
        }
    }
}