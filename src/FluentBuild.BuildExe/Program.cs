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

            MessageLogger.ShowDebugMessages = true;
    
            string pathToAssembly = Path.Combine(Environment.CurrentDirectory, args[0]);
            if (Path.GetExtension(args[0]).ToLower() != "dll")
            {
                Console.WriteLine("building task from sources");
                pathToAssembly = BuildAssemblyFromSources(pathToAssembly);
            }

            ExecuteBuildTask(pathToAssembly);
        }

        private static string BuildAssemblyFromSources(string path)
        {
            MessageLogger.WriteDebugMessage("Sources found in: " + path);
            var fileset = new FileSet();
            fileset.Include(path + "/**/*.cs");

            string startPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase).Replace("file:\\", "");

            string dllReference = Path.Combine(startPath, "FluentBuild.dll");
            MessageLogger.WriteDebugMessage("Adding in reference to the FluentBuild DLL from: " + dllReference);
            string outputAssembly = Path.Combine(path, "build.dll");
            MessageLogger.WriteDebugMessage("Output Assembly: " + outputAssembly);
            Build.UsingCsc.AddSources(fileset).AddRefences(dllReference).OutputFileTo(outputAssembly).Target.Library.Execute();
            return outputAssembly;
        }

        private static void ExecuteBuildTask(string path)
        {
            MessageLogger.WriteDebugMessage("Executing DLL build from " + path);
            Assembly assemblyInstance = Assembly.LoadFile(path);
            Type[] types = assemblyInstance.GetTypes();
            foreach (Type t in types)
            {
                Type[] interfaces = t.GetInterfaces();
                foreach (Type i in interfaces)
                {
                    if (i.FullName == typeof (IBuild).FullName)
                    {
                        var build = (IBuild) assemblyInstance.CreateInstance(t.FullName);
                        MessageLogger.WriteHeader("Execute");
                        build.Execute();
                    }
                }
            }
        }
    }
}