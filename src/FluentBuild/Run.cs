using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace FluentBuild
{
    public class Run
    {
        public static Executeable Executeable(string executeablePath)
        {
            return new Executeable(executeablePath);
        }

        public static Executeable Executeable(BuildArtifact executeablePath)
        {
            return new Executeable(executeablePath.ToString());
        }
    }

    public class Executeable
    {
        internal readonly string _executeablePath;
        private readonly List<String> _args = new List<string>();
        private string _workingDirectory;

        public Executeable(string executeablePath)
        {
            _executeablePath = executeablePath;
        }

        public Executeable WithArguments(params string[] arguments)
        {
            _args.AddRange(arguments);
            return this;
        }

        private string CreateArgumentString()
        {
            var sb = new StringBuilder();
            foreach (var argument in _args)
            {
                sb.AppendFormat(" {0}", argument);
            }
            return sb.ToString();
        }

        public Executeable InWorkingDirectory(string directory)
        {
            _workingDirectory = directory;
            return this;
        }

        public void Execute()
        {
            MessageLogger.Write("exec", String.Format("Running '{0} {1}", _executeablePath, CreateArgumentString()));
            var startInfo = new ProcessStartInfo(_executeablePath);
            startInfo.UseShellExecute = false;
            startInfo.Arguments = CreateArgumentString();

            if (!String.IsNullOrEmpty(_workingDirectory))
                startInfo.WorkingDirectory = _workingDirectory;

            Process process = Process.Start(startInfo);
            if (process != null)
                process.WaitForExit();
        }
    }
}