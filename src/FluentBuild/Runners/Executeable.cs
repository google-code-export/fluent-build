using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using FluentBuild.Core;
using FluentBuild.Utilities;
using FluentBuild.Utilities.MessageProcessing;

namespace FluentBuild.Runners
{
    ///<summary>
    /// Represents an executable to be run
    ///</summary>
    public interface IExecuteable : IFailable<IExecuteable>
    {
        ///<summary>
        /// Sets the arguments to pass to the executable
        ///</summary>
        ///<param name="arguments">The arguments to pass</param>
        IExecuteable WithArguments(params string[] arguments);

        ///<summary>
        /// Sets the working directory
        ///</summary>
        ///<param name="directory">path to the working directory</param>
        IExecuteable InWorkingDirectory(string directory);

        ///<summary>
        /// Sets the working directory
        ///</summary>
        ///<param name="directory">path to the working directory</param>
        IExecuteable InWorkingDirectory(BuildFolder directory);

        ///<summary>
        /// Executes the executable with the provided options.
        ///</summary>
        void Execute();

        ///<summary>
        /// Sets the executeable to run
        ///</summary>
        ///<param name="path">path to the executable</param>
        IExecuteable Executable(string path);
    }

    ///<summary>
    /// An executable to be run
    ///</summary>
    public class Executeable : Failable<IExecuteable>, IExecuteable
    {
        private readonly IMessageProcessor _messageProcessor;
        private readonly object _errorLock;
        private readonly object _outputLock;
        private readonly List<String> _args;
        private readonly StringBuilder _error;
        private readonly StringBuilder _output;
        internal string ExecuteablePath;
        internal string WorkingDirectory;

        ///<summary>
        /// Instantiates a new executable
        ///</summary>
        public Executeable() : this(new DefaultMessageProcessor())
        {
        }

        internal Executeable(IMessageProcessor messageProcessor)
        {
            _messageProcessor = messageProcessor;
            _errorLock = new object();
            _outputLock = new object();
            _args = new List<string>();
            _error = new StringBuilder();
            _output = new StringBuilder();
        }

        ///<summary>
        /// instantiates an executeable with the path to the assembly specified
        ///</summary>
        ///<param name="executeablePath">Path to the executable to run</param>
        public Executeable(string executeablePath) : this()
        {
            ExecuteablePath = executeablePath;
        }

        #region IExecuteable Members

        public IExecuteable WithArguments(params string[] arguments)
        {
            _args.AddRange(arguments);
            return this;
        }

        public IExecuteable InWorkingDirectory(string directory)
        {
            WorkingDirectory = directory;
            return this;
        }

        public IExecuteable InWorkingDirectory(BuildFolder directory)
        {
            WorkingDirectory = directory.ToString();
            return this;
        }

        public void Execute()
        {
            Execute("exec");
        }

        public IExecuteable Executable(string path)
        {
            ExecuteablePath = path;
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
            process.StartInfo.FileName = ExecuteablePath;
            process.StartInfo.Arguments = CreateArgumentString();

            //redirect to a stream so we can parse it and display it
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.ErrorDialog = false;

            if (!String.IsNullOrEmpty(WorkingDirectory))
                process.StartInfo.WorkingDirectory = WorkingDirectory;

            process.ErrorDataReceived += ProcessErrorDataReceived;
            process.OutputDataReceived += ProcessOutputDataReceived;

            return new ProcessWrapper(process);
        }

        internal string Execute(string prefix)
        {
            MessageLogger.WriteDebugMessage("executing " + ExecuteablePath + CreateArgumentString());
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

                        if (OnError == OnError.Fail)
                            //exit code should only be set if we want the application to fail on error
                            Environment.ExitCode = 1; //set our ExitCode to non-zero so consumers know we errored
                    }
                    
                    _messageProcessor.Display(prefix, _output.ToString(), _error.ToString(), process.ExitCode);

                    if (process.ExitCode != 0)
                        throw new ApplicationException("Exectable returned non-zero exit code");
                }
                catch (Exception e)
                {
                    if (OnError == OnError.Fail)
                        throw;
                    Debug.Write(prefix,
                                "An error occurred running a process but FailOnError was set to false. Error: " + e);
                }
            }
            return _output.ToString();
        }

        //lock objects in case events fire out of order
        private void ProcessOutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            lock (_outputLock)
                _output.AppendLine(e.Data);
        }

        private void ProcessErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            lock (_errorLock)
                _output.AppendLine(e.Data);
        }
    }
}