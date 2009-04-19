using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FluentBuild.BuildExe
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Usage: fb.exe folder");
                return; 
            }

            string folder = args[0];
            var fileset = new FileSet();
            fileset.Include(folder + "/**");
            FluentBuild.Build.UsingCsc.AddSources(fileset).OutputFileTo("build.dll").Target.Library.Execute();
        }
    }
}
