using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using FluentBuild.Core;
using FluentBuild.Utilities;
using ConsoleColor = FluentBuild.Utilities.ConsoleColor;

namespace FluentBuild.Runners
{
    public interface IExecuteable : IFailable<IExecuteable>
    {
        IExecuteable WithArguments(params string[] arguments);
        IExecuteable InWorkingDirectory(string directory);
        IExecuteable InWorkingDirectory(BuildFolder directory);
        void Execute();
        IExecuteable Executable(string path);
    }

    public class Executeable : Failable<IExecuteable>, IExecuteable 
    {
        private readonly object ErrorLock;
        private readonly object OutputLock;
        private readonly List<String> _args;
        private readonly StringBuilder error;
        private readonly StringBuilder output;
        internal string _executeablePath;
        internal string _workingDirectory;
        private IColorizedOutputDisplay _colorizedOutputDisplay;

        public Executeable() : this(new ColorizedOutputDisplay())
    {
        
    }

        internal Executeable(IColorizedOutputDisplay colorizedOutputDisplay)
        {
            ErrorLock = new object();
            OutputLock = new object();
            _args = new List<string>();
            error = new StringBuilder();
            output = new StringBuilder();
            _colorizedOutputDisplay = colorizedOutputDisplay;
        }

        public Executeable(string executeablePath) : this()
        {
            _executeablePath = executeablePath;
        }

        #region IExecuteable Members

        public IExecuteable WithArguments(params string[] arguments)
        {
            _args.AddRange(arguments);
            return this;
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

        public void Execute()
        {
            Execute("exec");
        }

        public IExecuteable Executable(string path)
        {
            _executeablePath = path;
            return this;
        }

        #endregion

        internal string CreateArgumentString()
        {
            var sb = new StringBuilder();
            foreach (string argument in _args)
            {
                sb.AppendFormat(" {0}", argument);
            }
            return sb.ToString();
        }

        internal IProcessWrapper CreateProcess()
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

            return new ProcessWrapper(process);
        }

        internal string Execute(string prefix)
        {
            MessageLogger.WriteDebugMessage("executing " + _executeablePath + CreateArgumentString());

            using (IProcessWrapper process = CreateProcess())
            {
                try
                {
                    process.Start();
                    process.BeginOutputReadLine();

                    if (!process.WaitForExit(50000))
                    {
                        MessageLogger.WriteDebugMessage("TIMEOUT!");
                        process.Kill();
                        Thread.Sleep(1000); //wait one second so that the process has time to exit
                        
                        if (this.OnError == OnError.Fail) //exit code should only be set if we want the application to fail on error
                            Environment.ExitCode = 1; //set our ExitCode to non-zero so consumers know we errored
                    }
                    _colorizedOutputDisplay.Display(prefix, output.ToString(), error.ToString(), process.ExitCode == 0);

                    if (process.ExitCode != 0)
                        throw new ApplicationException("Exectable returned non-zero exit code");
                }
                catch (Exception e)
                {
                    if (OnError == OnError.Fail)
                        throw;
                    Debug.Write(prefix, "An error occurred running a process but FailOnError was set to false. Error: " + e);
                }
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
    }
}