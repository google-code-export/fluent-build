using System.IO;
using FluentBuild.Core;
using FluentBuild.Runners;
using FluentFs.Core;

namespace FluentBuild.Compilation
{
    internal class Resgen
    {
        private readonly IExecutable _exeRunner;
        internal FileSet Files;
        internal string OutputFolder;
        internal string Prefix;

        internal Resgen(IExecutable exeRunner)
        {
            _exeRunner = exeRunner;
        }

        public Resgen() : this(new Executable())
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
            MessageLogger.WriteDebugMessage("Found ResGen at: " + executable);
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

                IExecutable executable = _exeRunner.ExecutablePath(resGenExecutable);
                IExecutable withArguments = executable.WithArguments("\"" + resourceFileName + "\"");
                IExecutable arguments = withArguments.WithArguments("\"" + outputFileName + "\"");
                arguments.Execute();
            }
            return outputFiles;
        }
    }
}