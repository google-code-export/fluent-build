using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace FluentBuild
{
    public interface IExecuteable
    {
        IExecuteable WithArguments(params string[] arguments);
        IExecuteable InWorkingDirectory(string directory);
        IExecuteable InWorkingDirectory(BuildFolder directory);
        void Execute();
        IExecuteable Executable(string path);
    }

    public class Executeable : IExecuteable
    {
        private readonly object ErrorLock;
        private readonly object OutputLock;
        private readonly List<String> _args;
        internal string _executeablePath;
        private readonly StringBuilder error;
        private readonly StringBuilder output; 
        internal string _workingDirectory;

        public Executeable()
        {
            ErrorLock = new object();
            OutputLock = new object();
            _args = new List<string>();
            error = new StringBuilder();
            output = new StringBuilder();
        }

        public Executeable(string executeablePath) : this()
        {
            _executeablePath = executeablePath;
        }

        public IExecuteable WithArguments(params string[] arguments)
        {
            _args.AddRange(arguments);
            return this;
        }

        internal string CreateArgumentString()
        {
            var sb = new StringBuilder();
            foreach (string argument in _args)
            {
                sb.AppendFormat(" {0}", argument);
            }
            return sb.ToString();
        }

        public IExecuteable InWorkingDirectory(string directory)
        {
            _workingDirectory = directory;
            return this;
        }

        public IExecuteable InWorkingDirectory(BuildFolder directory)
        {
            _workingDirectory = directory.ToString();
            return this;
        }

        internal Process CreateProcess()
        {
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
//            process.PriorityClass = ProcessPriorityClass.Idle;

            return process;
        }


        internal string Execute(string prefix)
        {
            MessageLogger.WriteDebugMessage("executing " + _executeablePath + CreateArgumentString());

            using (var process = CreateProcess())
            {
                process.Start();
                process.BeginOutputReadLine();

                if (!process.WaitForExit(50000))
                {
                    MessageLogger.WriteDebugMessage("TIMEOUT!");
                    process.Kill();
                    Thread.Sleep(1000); //wait one second so that the process has time to exit
                    Environment.ExitCode = 1; //set our ExitCode to non-zero so consumers know we errored
                }
                else
                {
                    Environment.ExitCode += process.ExitCode;
                }
                DisplayOutput(prefix, process.ExitCode);
            }
            return output.ToString();
        }

        //lock objects in case events fire out of order
        private void process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            lock (OutputLock)
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

        public IExecuteable Executable(string path)
        {
            _executeablePath = path;
            return this;
        }
    }
}