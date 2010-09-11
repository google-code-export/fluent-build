using System;
using System.IO;

namespace FluentBuild.Compilation.ResGen
{
    public class Resgen
    {
        //TODO: test this
        private static FileSet _files;
        private static string _outputFolder;
        private string _prefix;
        private WindowsSdkFinder _sdkFinder = new WindowsSdkFinder();

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

        public FileSet Execute()
        {
            if (Environment.ExitCode != 0)
                return null;

            try
            {
                if (!_sdkFinder.IsWindowsSdkInstalled())
                {
                    MessageLogger.Write("RESGEN", "could not find the Windows SDK which contains resgen.exe which is required to build resources");
                    return null;
                }


                string resGenExecuteable = Path.Combine(_sdkFinder.PathToHighestVersionedSdk(), "bin\\resgen.exe");
                MessageLogger.WriteDebugMessage("Found ResGen at: " + resGenExecuteable);
                var outputFiles = new FileSet();
                foreach (string resourceFileName in _files.Files)
                {
                    string outputFileName = _prefix + Path.GetFileNameWithoutExtension(resourceFileName) + ".resources";
                    outputFileName = Path.Combine(_outputFolder, outputFileName);
                    Run.Executeable(resGenExecuteable).WithArguments("\"" + resourceFileName + "\"").WithArguments("\"" + outputFileName + "\"").Execute();
                    outputFiles.Include(outputFileName);
                }
                return outputFiles;
            }
            catch (Exception ex)
            {
                Environment.ExitCode = 1;
                MessageLogger.Write("ERROR", ex.ToString());
            }
            return null;
        }
    }
}