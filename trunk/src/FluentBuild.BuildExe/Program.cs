using System;
using System.IO;
using System.Reflection;
using FluentBuild.ApplicationProperties;
using FluentBuild.Core;

namespace FluentBuild.BuildExe
{
    internal class CommandLineParser
    {
        private string _fileToRun;
        private string _classToRun;

        public CommandLineParser(string[] args)
        {
            //what about when build class is passed in
            _fileToRun = args[0];
            for (int i = 1; i < args.Length; i++)
            {
                ParseArg(args[i]);
            }
        }

        private void ParseArg(string arg)
        {
            arg = arg.Substring(1); //drop the preceeding - or / character
            var type = arg.Substring(0, arg.IndexOf(":")); //get the type
            var data = arg.Substring(arg.IndexOf(":")); //get the value

            string name;
            string value;
            if (data.IndexOf("=") > 0)
            {
                name = data.Substring(0, data.IndexOf("="));
                value = data.Substring(data.IndexOf("="));
            }
            else
            {
                name = data;
                value = String.Empty;
            }
                
            switch (type.ToUpper())
            {
                case "P":
                    Properties.CommandLineProperties.Add(name, value);
                    break;
                case "C":
                    _classToRun = data;
                    break;
                default:
                    throw new ArgumentException("Do not understand type");
                    
            }
        }
    }

    internal class Program
    {

        private static void Main(string[] args)
        {
            
            if (args.Length == 0)
            {
                Console.WriteLine("Usage: fb.exe BuildFileOrSource [-c:BuildClass] [-p:property=value] [-p:property]");
                Console.WriteLine();
                Console.WriteLine("BuildFileOrSource: the dll that contains the precompiled build file OR the path to the source folder than contains build files (fb.exe will compile the build file for you)");
                Console.WriteLine("c: The class to run. If none is specified then \"Default\" is assumed");
                Console.WriteLine("p: properties to pass to the build script. These can be accessed via Properties.CommandLine in your build script. ");
                return;
            }

            AppDomain.CurrentDomain.UnhandledException += UnhandledException;
            MessageLogger.ShowDebugMessages = true;

            var classToRun = "Default";
            if (args.Length > 1)
                classToRun = args[1];

            string pathToAssembly = Path.Combine(Environment.CurrentDirectory, args[0]);
            if (Path.GetExtension(args[0]).ToLower() != "dll")
            {
                Console.WriteLine("building task from sources");
                pathToAssembly = BuildAssemblyFromSources(pathToAssembly);
            }

            ExecuteBuildTask(pathToAssembly, classToRun);
        }

        static void UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Environment.ExitCode = 1;
            MessageLogger.Write("ERROR", "An unexpected error has occurred. Details:" + e.ExceptionObject);
            Environment.Exit(1);
        }

        /// <summary>
        /// Builds an assembly from a source folder. Currently this only works with .cs files
        /// </summary>
        /// <param name="path">The path to the source files</param>
        /// <returns>returns the path to the compiled assembly</returns>
        private static string BuildAssemblyFromSources(string path)
        {
            Console.WriteLine("Press enter key to start");
            Console.ReadLine();
            MessageLogger.WriteDebugMessage("Sources found in: " + path);
            var fileset = new FileSet();
            fileset.Include(path + "\\**\\*.cs");

            string startPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase).Replace("file:\\", "");

            string dllReference = Path.Combine(startPath, "FluentBuild.dll");
            MessageLogger.WriteDebugMessage("Adding in reference to the FluentBuild DLL from: " + dllReference);
            string outputAssembly = Path.Combine(path, "build.dll");
            MessageLogger.WriteDebugMessage("Output Assembly: " + outputAssembly);
            Build.UsingCsc.AddSources(fileset).AddRefences(dllReference).OutputFileTo(outputAssembly).IncludeDebugSymbols.Target.Library.Execute();
            return outputAssembly;
        }

        /// <summary>
        /// Executes a DLL.
        /// </summary>
        /// <param name="path">The path to the DLL that has a class that implements IBuild</param>
        /// <param name="classToRun"></param>
        private static void ExecuteBuildTask(string path, string classToRun)
        {
            MessageLogger.WriteDebugMessage("Executing DLL build from " + path);
            Assembly assemblyInstance = Assembly.LoadFile(path);
            Type[] types = assemblyInstance.GetTypes();
            foreach (Type t in types)
            {
                if (t.Name == classToRun)
                {
                    StartRun(assemblyInstance, t);
                }
                
            }
        }

        private static void StartRun(Assembly assemblyInstance, Type t)
        {
                var build = (BuildFile)assemblyInstance.CreateInstance(t.FullName);
                MessageLogger.WriteHeader("Execute");
                MessageLogger.Write("EXECUTE", "Running Class: " + t.FullName);
                build.InvokeNextTask();

            //Type[] interfaces = t.GetInterfaces();
            //foreach (Type i in interfaces)
            //{
            //    if (i.FullName == typeof(IBuild).FullName)
            //    {
            //        var build = (IBuild)assemblyInstance.CreateInstance(t.FullName);
            //        MessageLogger.WriteHeader("Execute");
            //        MessageLogger.Write("EXECUTE", "Running Class: " + t.FullName);
            //        build.Execute();
            //    }
            //}
        }
    }
}