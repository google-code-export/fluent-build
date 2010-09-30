using System.IO;
using FluentBuild.Core;
using FluentBuild.Runners;

namespace FluentBuild.Compilation
{
    internal class Resgen
    {
        private readonly IExecuteable _exeRunner;
        internal FileSet Files;
        internal string OutputFolder;
        internal string Prefix;

        internal Resgen(IExecuteable exeRunner)
        {
            _exeRunner = exeRunner;
        }

        public Resgen() : this(new Executeable())
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

        internal string GetPathToResGenExecuteable()
        {
            string resGenExecuteable = Path.Combine(Defaults.FrameworkVersion.GetPathToSdk(), "bin\\resgen.exe");
            MessageLogger.WriteDebugMessage("Found ResGen at: " + resGenExecuteable);
            return resGenExecuteable;
        }

        public FileSet Execute()
        {
            string resGenExecuteable = GetPathToResGenExecuteable();

            var outputFiles = new FileSet();
            foreach (string resourceFileName in Files.Files)
            {
                string outputFileName = Prefix + Path.GetFileNameWithoutExtension(resourceFileName) + ".resources";
                outputFileName = Path.Combine(OutputFolder, outputFileName);
                outputFiles.Include(outputFileName);

                IExecuteable executeable = _exeRunner.Executable(resGenExecuteable);
                IExecuteable withArguments = executeable.WithArguments("\"" + resourceFileName + "\"");
                IExecuteable arguments = withArguments.WithArguments("\"" + outputFileName + "\"");
                arguments.Execute();
            }
            return outputFiles;
        }
    }
}