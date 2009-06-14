using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;

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
            MessageLogger.WriteDebugMessage("executing " + _executeablePath + CreateArgumentString());
            var process = new Process();
            process.StartInfo.FileName = _executeablePath;
            process.StartInfo.Arguments = CreateArgumentString();

            //redirect to a stream so we can parse it and display it
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.ErrorDialog = false;

            if (!String.IsNullOrEmpty(_workingDirectory))
                process.StartInfo.WorkingDirectory = _workingDirectory;


            process.ErrorDataReceived += process_ErrorDataReceived;
            process.OutputDataReceived += process_OutputDataReceived;

            process.Start();
            process.PriorityClass = ProcessPriorityClass.Idle;
            process.BeginOutputReadLine();
            if (!process.WaitForExit(5000))
            {
                MessageLogger.WriteDebugMessage("TIMEOUT!");
                process.Kill();
                Thread.Sleep(1000);
                Environment.ExitCode = 1;
            }
            else
            {
                Environment.ExitCode += process.ExitCode;
            }
            DisplayOutput(prefix, process.ExitCode);
            process.Dispose();
        }

        //lock objects in case events fire out of order
        private object OutputLock = new object();
        private object ErrorLock = new object();
        private readonly StringBuilder output = new StringBuilder();
        private readonly StringBuilder error = new StringBuilder();


        private void process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            lock(OutputLock)
                output.AppendLine(e.Data);
        }

        private void process_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            lock (ErrorLock)
            output.AppendLine(e.Data);
        }

        private void DisplayOutput(string prefix, int exitCode)
        {
            ConsoleColor.BuildColor textColor = ConsoleColor.BuildColor.Default;

            if (exitCode != 0)
                    textColor = ConsoleColor.BuildColor.Red;

                foreach (string line in  output.ToString().Split(Environment.NewLine.ToCharArray()))
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
                foreach (string line in error.ToString().Split(Environment.NewLine.ToCharArray()))
                {
                    if (line.Trim().Length > 0)
                        MessageLogger.Write(prefix, line);
                }
             ConsoleColor.SetColor(ConsoleColor.BuildColor.Default);
        }
        
        public void Execute()
        {
            Execute("exec");
        }
    }
}