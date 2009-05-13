using System;
using System.IO;
using System.Reflection;
using FluentBuild.BuildFile;

namespace FluentBuild.BuildExe
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Usage: fb.exe buildassembly.dll");
                Console.WriteLine("OR");
                Console.WriteLine("fb.exe PathToSources");
                return;
            }
            Console.WriteLine("Press any key to start");
            Console.ReadKey();

            string pathToAssembly=Path.Combine(Environment.CurrentDirectory, args[0]);
            if (System.IO.Path.GetExtension(args[0]).ToLower() != "dll")
            {
                Console.WriteLine("building task from sources");
                pathToAssembly = BuildAssemblyFromSources(args[0]);
            }

            ExecuteBuildTask(pathToAssembly);

        }

        private static string BuildAssemblyFromSources(string path)
        {
            var fileset = new FileSet();
            fileset.Include(path + "/**/*.cs");
            FluentBuild.Build.UsingCsc.AddSources(fileset).AddRefences(Path.Combine(Environment.CurrentDirectory, "FluentBuild.Build.dll")).OutputFileTo("build.dll").Target.Library.Execute();
            return System.IO.Path.Combine(path, "build.dll");
        }

        private static void ExecuteBuildTask(string path)
        {
            Assembly assemblyInstance = Assembly.LoadFile(path);
            Type[] types = assemblyInstance.GetTypes();
            foreach (Type t in types)
            {
                Type[] interfaces = t.GetInterfaces();
                foreach (Type i in interfaces)
                {
                    if (i.FullName == typeof(IBuild).FullName)
                    {
                        var build = (IBuild)assemblyInstance.CreateInstance(t.FullName);
                        MessageLogger.WriteHeader("Execute");
                        build.Execute();
                    }
                }
            }

        }
    }
}