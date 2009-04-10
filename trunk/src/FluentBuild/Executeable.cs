using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace FluentBuild
{
    public class Executeable
    {
        private readonly List<String> _args = new List<string>();
        internal readonly string _executeablePath;
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
            foreach (string argument in _args)
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

        internal void Execute(string prefix)
        {
            var startInfo = new ProcessStartInfo(_executeablePath);
            startInfo.UseShellExecute = false;
            startInfo.Arguments = CreateArgumentString();

            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;

            if (!String.IsNullOrEmpty(_workingDirectory))
                startInfo.WorkingDirectory = _workingDirectory;

            Process process = Process.Start(startInfo);
            if (process == null)
                throw new ApplicationException("process did not start?");

            using (StreamReader output = process.StandardOutput)
                using (StreamReader error = process.StandardError)
                {
                    process.WaitForExit();
                    Environment.ExitCode += process.ExitCode;
                    var textColor = ConsoleColor.BuildColor.Default;
                    
                    if (process.ExitCode != 0)
                    {
                      textColor = ConsoleColor.BuildColor.Red;
                    }

                    foreach (var line in output.ReadToEnd().Split(Environment.NewLine.ToCharArray()))
                    {
                        if (line.Trim().Length > 0)
                        {
                            ConsoleColor.SetColor(textColor);
                            if (line.Contains("warning") || line.Contains("Warning"))
                                ConsoleColor.SetColor(ConsoleColor.BuildColor.Yellow);
                            MessageLogger.Write(prefix, line);
                        }
                    }

                    ConsoleColor.SetColor(textColor);
                    foreach (var line in error.ReadToEnd().Split(Environment.NewLine.ToCharArray()))
                    {
                        if (line.Trim().Length > 0)
                            MessageLogger.Write(prefix, line);
                    }
                   
                }

            ConsoleColor.SetColor(ConsoleColor.BuildColor.Default);
        }

        public void Execute()
        {
           Execute("exec");
        }
    }
}