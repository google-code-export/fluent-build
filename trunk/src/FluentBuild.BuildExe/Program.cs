using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using FluentBuild.Compilation;
using FluentBuild.Core;
using FluentFs.Core;
using Directory = System.IO.Directory;
using File = System.IO.File;

namespace FluentBuild.BuildExe
{
    public class Compiler
    {
        /// <summary>
        /// Builds an assembly from a source folder. Currently this only works with .cs files
        /// </summary>
        /// <param name="path">The path to the source files</param>
        /// <returns>returns the path to the compiled assembly</returns>
        public static string BuildAssemblyFromSources(string path)
        {
            Defaults.Logger.WriteDebugMessage("Sources found in: " + path);
            var fileset = new FileSet();
            fileset.Include(path + "\\**\\*.cs");

            string startPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase).Replace("file:\\", "");

            string dllReference = Path.Combine(startPath, "FluentBuild.dll");
            Defaults.Logger.WriteDebugMessage("Adding in reference to the FluentBuild DLL from: " + dllReference);
            string tempPath = Environment.GetEnvironmentVariable("TEMP") + "\\FluentBuild\\" + DateTime.Now.Ticks;
            Directory.CreateDirectory(tempPath);
            string outputAssembly = Path.Combine(tempPath, "build.dll");
            Defaults.Logger.WriteDebugMessage("Output Assembly: " + outputAssembly);
            Task.Build(Using.Csc.Target.Library.AddSources(fileset).AddRefences(dllReference).OutputFileTo(outputAssembly).IncludeDebugSymbols);
            return outputAssembly;
        }

        public static IEnumerable<Type> FindBuildClasses(string path)
        {
            Defaults.Logger.WriteDebugMessage("Executing DLL build from " + path);

            Defaults.Logger.Write("INFO", "Using framework " + Defaults.FrameworkVersion);
            Assembly assemblyInstance = Assembly.LoadFile(path);
            Type[] types = assemblyInstance.GetTypes();
            return types.Where(t => t.IsSubclassOf(typeof (BuildFile)));
        }
    }

    public class Program
    {
        private static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Usage: fb.exe BuildFileOrSource [-c:BuildClass] [-m:Method] [-p:property=value] [-p:property] -v:Verbosity");
                Console.WriteLine();
                Console.WriteLine(
                    "BuildFileOrSource: the dll that contains the precompiled build file OR the path to the source folder than contains build files (fb.exe will compile the build file for you)");
                Console.WriteLine("c: The class to run. If none is specified then \"Default\" is assumed");
                Console.WriteLine("p: properties to pass to the build script. These can be accessed via Properties.CommandLine in your build script. ");
                Console.WriteLine("v: verbosity of output. Can be None, TaskNamesOnly, TaskDetails, Full");
                Console.WriteLine("m: Method to run. Allows a user to execute specific methods in the build. If specified only the method will run. Multiple specifications are allowed.");
                Environment.Exit(1);
            }


            AppDomain.CurrentDomain.UnhandledException += UnhandledException;
            Defaults.Logger.Verbosity = VerbosityLevel.TaskDetails;

            //creates a new parser and parses args
            var parser = new CommandLineParser(args);

            var argString = new StringBuilder();
            foreach (string s in args)
            {
                argString.Append(" /" + s);
            }

            Defaults.Logger.Write("INIT", "running fb.exe " + argString);

            string pathToAssembly;
            if (parser.SourceBuild)
            {
                Defaults.Logger.Write("INIT", "building task from sources");
                if (!Directory.Exists(parser.PathToBuildSources))
                {
                    Defaults.Logger.WriteError("ERROR", "Could not find sources at: " + parser.PathToBuildSources);
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

            ExecuteBuildTask(pathToAssembly, parser.ClassToRun, parser.MethodsToRun);
        }

        private static void UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Environment.ExitCode = 1;
            var exceptionObject = e.ExceptionObject as Exception;
            Defaults.Logger.WriteError("ERROR", "An unexpected error has occurred. Details:" + exceptionObject);
            Environment.Exit(1);
        }

        /// <summary>
        /// Builds an assembly from a source folder. Currently this only works with .cs files
        /// </summary>
        /// <param name="path">The path to the source files</param>
        /// <returns>returns the path to the compiled assembly</returns>
        public static string BuildAssemblyFromSources(string path)
        {
            //TODO: once FluentFs is merged it can be removed here
            //but look at what is done so that we can have external DLLs referenced from the command line?
            Defaults.Logger.WriteDebugMessage("Sources found in: " + path);
            var fileset = new FileSet();
            fileset.Include(path + "\\**\\*.cs");

            string startPath =
                Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase).Replace("file:\\", "");

            string fluentBuilddll = Path.Combine(startPath, "FluentBuild.dll");
            var fluentFs = new FluentFs.Core.File(Path.Combine(startPath, "FluentFs.dll"));
            Defaults.Logger.WriteDebugMessage("Adding in reference to the FluentBuild DLL from: " + fluentBuilddll);
            string tempPath = Environment.GetEnvironmentVariable("TEMP") + "\\FluentBuild\\" + DateTime.Now.Ticks;
            Directory.CreateDirectory(tempPath);
            string outputAssembly = Path.Combine(tempPath, "build.dll");
            
            
            Defaults.Logger.WriteDebugMessage("Output Assembly: " + outputAssembly);

            var references = new List<String>() { fluentBuilddll};
            if (File.Exists(fluentFs.ToString()))
            {
                fluentFs.Copy.To(tempPath);
                references.Add(fluentFs.ToString());
            }
            
            Task.Build(Using.Csc.Target.Library.AddSources(fileset).AddRefences(references.ToArray()).OutputFileTo(outputAssembly).IncludeDebugSymbols);
            return outputAssembly;
        }

        /// <summary>
        /// Executes a DLL.
        /// </summary>
        /// <param name="path">The path to the DLL that has a class that implements IBuild</param>
        /// <param name="classToRun"></param>
        /// <param name="methodsToRun"></param>
        private static void ExecuteBuildTask(string path, string classToRun, IList<string> methodsToRun)
        {
            Defaults.Logger.WriteDebugMessage("Executing DLL build from " + path);

            Defaults.Logger.Write("INFO", "Using framework " + Defaults.FrameworkVersion);
            Assembly assemblyInstance = Assembly.LoadFile(path);
            Type[] types = assemblyInstance.GetTypes();
            bool classfound = false;
            foreach (Type t in types)
            {
                if ((t.Name == classToRun) && t.IsSubclassOf(typeof (BuildFile)))
                {
                    classfound = true;
                    StartRun(assemblyInstance, t, methodsToRun);
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

        private static void StartRun(Assembly assemblyInstance, Type t, IList<string> methodsToRun)
        {
            var build = (BuildFile) assemblyInstance.CreateInstance(t.FullName);
            Defaults.Logger.WriteHeader("Execute");
            Defaults.Logger.Write("EXECUTE", "Running Class: " + t.FullName);
            if (build.TaskCount == 0)
            {
                Console.WriteLine("No tasks were found. Make sure that you add a task in your build classes constructor via AddTask()");
                Environment.Exit(1);
            }
            if (methodsToRun.Count != 0)
            {
                if (!DoAllMethodsExistInType(t, methodsToRun))
                {
                    Console.WriteLine("Methods that were specified could not be found in the build file. Ensure the method is Public and spelled correctly");
                    Environment.Exit(1);
                }
                build.ClearTasks();

                foreach (string method in methodsToRun)
                {
                    string methodToInvoke = method;
                    var a = new Action(delegate { t.InvokeMember(methodToInvoke, BindingFlags.Default | BindingFlags.InvokeMethod, null, build, null); });
                    build.AddTask(method, a);
                }
            }

            build.InvokeNextTask();
        }

        public static bool DoAllMethodsExistInType(Type type, IList<string> methodsToRun)
        {
            MethodInfo[] methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance);
            foreach (string methodToRun in methodsToRun)
            {
                bool found = false;
                foreach (MethodInfo method in methods)
                {
                    if (method.Name == methodToRun)
                        found = true;
                }
                if (found == false)
                    return false;
            }
            return true;
        }
    }
}