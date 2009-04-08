using System;
using System.IO;

namespace FluentBuild.Sample
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var build = new MainBuild();
            build.Execute();
        }
    }
}