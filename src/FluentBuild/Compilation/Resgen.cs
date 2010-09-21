using System;
using System.IO;
using FluentBuild.Core;
using FluentBuild.Runners;
using FluentBuild.Utilities;

namespace FluentBuild.Compilation
{
    public class Resgen
    {
        private readonly IWindowsSdkFinder _sdkFinder;
        internal FileSet _files;
        internal string _outputFolder;
        internal string _prefix;
        private IExecuteable _exeRunner;
        internal Resgen(IWindowsSdkFinder sdkFinder, IExecuteable exeRunner)
        {
            _sdkFinder = sdkFinder;
            _exeRunner = exeRunner;
        }

        public Resgen() : this(new WindowsSdkFinder(), new Executeable())
        {
        }

        public Resgen GenerateFrom(FileSet fileset)
        {
            _files = fileset;
            return this;
        }

        public Resgen OutputTo(string folder)
        {
            _outputFolder = folder;
            return this;
        }

        public Resgen PrefixOutputsWith(string prefix)
        {
            _prefix = prefix;
            return this;
        }

        internal string GetPathToResGenExecuteable()
        {
            if (!_sdkFinder.IsWindowsSdkInstalled())
                throw new ApplicationException(
                    "Could not find the Windows SDK which contains resgen.exe which is required to build resources");

            string resGenExecuteable = Path.Combine(_sdkFinder.PathToHighestVersionedSdk(), "bin\\resgen.exe");
            MessageLogger.WriteDebugMessage("Found ResGen at: " + resGenExecuteable);
            return resGenExecuteable;
        }

        public FileSet Execute()
        {
            string resGenExecuteable = GetPathToResGenExecuteable();

            var outputFiles = new FileSet();
            foreach (string resourceFileName in _files.Files)
            {
                string outputFileName = _prefix + Path.GetFileNameWithoutExtension(resourceFileName) + ".resources";
                outputFileName = Path.Combine(_outputFolder, outputFileName);
                outputFiles.Include(outputFileName);

                IExecuteable executeable = _exeRunner.Executable(resGenExecuteable);
                IExecuteable withArguments = executeable.WithArguments("\"" + resourceFileName + "\"");
                IExecuteable arguments = withArguments.WithArguments("\"" + outputFileName +"\"");
                arguments.Execute();
                
            }
            return outputFiles;
        }
    }
}