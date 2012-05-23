using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using FluentFs.Core;
using Directory = System.IO.Directory;
using File = FluentFs.Core.File;

namespace FluentBuild.BuildUI
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

            string startPath =
                Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase).Replace("file:\\", "");

            string fluentBuildDll = Path.Combine(startPath, "FluentBuild.dll");
            var references = new List<String>() { fluentBuildDll };
            string fluentFsDll = Path.Combine(startPath, "FluentFs.dll");
            Defaults.Logger.WriteDebugMessage("Adding in reference to the FluentBuild DLL from: " + fluentBuildDll);
            if (System.IO.File.Exists(fluentFsDll))
            {
                Defaults.Logger.WriteDebugMessage("Adding in reference to the FluentFs DLL from: " + fluentFsDll);
                references.Add(fluentFsDll);
            }
            var tempPath = Environment.GetEnvironmentVariable("TEMP") + "\\FluentBuild\\" + DateTime.Now.Ticks.ToString();
            Directory.CreateDirectory(tempPath);
            string outputAssembly = Path.Combine(tempPath, "build.dll");
            Defaults.Logger.WriteDebugMessage("Output Assembly: " + outputAssembly);
            Task.Build.Csc.Target.Library(x => x.AddSources(fileset).AddRefences(references.ToArray()).IncludeDebugSymbols.OutputFileTo(outputAssembly));
            return outputAssembly;
        }

        public static IEnumerable<Type> FindBuildClasses(string path)
        {
            Defaults.Logger.WriteDebugMessage("Executing DLL build from " + path);

            Defaults.Logger.Write("INFO", "Using framework {0}", Defaults.FrameworkVersion.ToString());
            Assembly assemblyInstance = Assembly.LoadFile(path);
            Type[] types = assemblyInstance.GetTypes();
            return types.Where(t => t.IsSubclassOf(typeof(BuildFile)));
        }

    }
}