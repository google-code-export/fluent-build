﻿using System.IO;
using FluentBuild.Core;
using FluentBuild.Runners;
using FluentBuild.Utilities;
using FluentFs.Core;

namespace FluentBuild.Compilation
{
    internal class Resgen
    {
        private readonly IActionExcecutor _actionExcecutor;
        internal FileSet Files;
        internal string OutputFolder;
        internal string Prefix;

        public Resgen(IActionExcecutor actionExcecutor)
        {
            _actionExcecutor = actionExcecutor;
        }

        public Resgen() : this(new ActionExcecutor())
        {
        }

        public Resgen GenerateFrom(FileSet fileset)
        {
            Files = fileset;
            return this;
        }

        public Resgen OutputTo(string folder)
        {
            OutputFolder = folder;
            return this;
        }

        public Resgen PrefixOutputsWith(string prefix)
        {
            Prefix = prefix;
            return this;
        }

        internal string GetPathToResGenExecutable()
        {
            string executable = Path.Combine(Defaults.FrameworkVersion.GetPathToSdk(), "bin\\resgen.exe");
            Defaults.Logger.WriteDebugMessage("Found ResGen at: " + executable);
            return executable;
        }

        public FileSet Execute()
        {
            string resGenExecutable = GetPathToResGenExecutable();

            var outputFiles = new FileSet();
            foreach (string resourceFileName in Files.Files)
            {
                string outputFileName = Prefix + Path.GetFileNameWithoutExtension(resourceFileName) + ".resources";
                outputFileName = Path.Combine(OutputFolder, outputFileName);
                outputFiles.Include(outputFileName);
                _actionExcecutor.Execute<Executable>(x=>x.ExecutablePath(resGenExecutable).WithArguments("\"" + resourceFileName + "\"").WithArguments("\"" + outputFileName + "\""));
            }
            return outputFiles;
        }
    }
}