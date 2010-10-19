using System;
using System.IO;
using System.Reflection;
using System.Text;
using FluentBuild.Core;
using FluentBuild.Utilities;

namespace FluentBuild.BuildExe
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Usage: fb.exe BuildFileOrSource [-c:BuildClass] [-p:property=value] [-p:property] -v:Verbosity");
                Console.WriteLine();
                Console.WriteLine(
                    "BuildFileOrSource: the dll that contains the precompiled build file OR the path to the source folder than contains build files (fb.exe will compile the build file for you)");
                Console.WriteLine("c: The class to run. If none is specified then \"Default\" is assumed");
                Console.WriteLine("p: properties to pass to the build script. These can be accessed via Properties.CommandLine in your build script. ");
                Console.WriteLine("v: verbosity of output. Can be None, TaskNamesOnly, TaskDetails, Full");
                Environment.Exit(1);
            }

            

            AppDomain.CurrentDomain.UnhandledException += UnhandledException;
            MessageLogger.Verbosity = VerbosityLevel.TaskDetails;

            //creates a new parser and parses args
            var parser = new CommandLineParser(args);

            var argString = new StringBuilder();
            foreach (var s in args)
            {
                argString.Append(" /" + s);
            }

            MessageLogger.Write("INIT", "running fb.exe " + argString.ToString());

            string pathToAssembly;
            if (parser.SourceBuild)
            {
                Console.WriteLine("building task from sources");
                if (!Directory.Exists(parser.PathToBuildSources))
                {
                    Console.WriteLine("Could not find sources at: " + parser.PathToBuildSources);
                    Environment.Exit(1);
                }
                pathToAssembly = BuildAssemblyFromSources(parser.PathToBuildSources);
            }
            else
            {

                pathToAssembly = parser.PathToBuildDll;
            }

            if (!File.Exists(pathToAssembly))
            {
                Console.WriteLine("Could not find compiled build script at: " + parser.PathToBuildSources);
                Environment.Exit(1);
            }

            ExecuteBuildTask(pathToAssembly, parser.ClassToRun);
        }

        private static void UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Environment.ExitCode = 1;
            var exceptionObject = e.ExceptionObject as Exception;
            MessageLogger.WriteError("An unexpected error has occurred. Details:" + exceptionObject.ToString());
            Environment.Exit(1);
        }

        /// <summary>
        /// Builds an assembly from a source folder. Currently this only works with .cs files
        /// </summary>
        /// <param name="path">The path to the source files</param>
        /// <returns>returns the path to the compiled assembly</returns>
        private static string BuildAssemblyFromSources(string path)
        {
            MessageLogger.WriteDebugMessage("Sources found in: " + path);
            var fileset = new FileSet();
            fileset.Include(path + "\\**\\*.cs");

            string startPath =
                Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase).Replace("file:\\", "");

            string dllReference = Path.Combine(startPath, "FluentBuild.dll");
            MessageLogger.WriteDebugMessage("Adding in reference to the FluentBuild DLL from: " + dllReference);
            string outputAssembly = Path.Combine(path, "build.dll");
            MessageLogger.WriteDebugMessage("Output Assembly: " + outputAssembly);
            Build.UsingCsc.Target.Library.AddSources(fileset).AddRefences(dllReference).OutputFileTo(outputAssembly).
                IncludeDebugSymbols.Execute();
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
            
            MessageLogger.Write("INFO", "Using framework " + Defaults.FrameworkVersion.ToString());
            Assembly assemblyInstance = Assembly.LoadFile(path);
            Type[] types = assemblyInstance.GetTypes();
            bool classfound = false;
            foreach (Type t in types)
            {
                if ((t.Name == classToRun) && t.IsSubclassOf(typeof (BuildFile)))
                {
                    classfound = true;
                    StartRun(assemblyInstance, t);
                    return;
                }
            }

            if (!classfound)
            {
                Console.WriteLine(String.Format("Could not find class {0} that inherits from FluentBuild.BuildFile",
                                                classToRun));
                Environment.Exit(1);
            }

            Environment.Exit(0);
        }

        private static void StartRun(Assembly assemblyInstance, Type t)
        {
            var build = (BuildFile) assemblyInstance.CreateInstance(t.FullName);
            MessageLogger.WriteHeader("Execute");
            MessageLogger.Write("EXECUTE", "Running Class: " + t.FullName);
            if (build.TaskCount == 0)
            {
                Console.WriteLine(
                    "No tasks were found. Make sure that you add a task in your build classes constructor via AddTask()");
                Environment.Exit(1);
            }
            build.InvokeNextTask();
        }
    }
}