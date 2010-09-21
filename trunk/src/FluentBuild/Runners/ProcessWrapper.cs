using System;
using System.Diagnostics;

namespace FluentBuild.Runners
{

    public interface IProcessWrapper : IDisposable
    {
        bool Start();
        void BeginOutputReadLine();
        bool WaitForExit(int milliseconds);
        void Kill();
        int ExitCode { get; }
    }


    public class ProcessWrapper:IProcessWrapper
    {
        private readonly Process _process;

        public ProcessWrapper(Process process)
        {
            _process = process;
        }

        public bool Start()
        {
            return _process.Start();
        }

        public void BeginOutputReadLine()
        {
            _process.BeginOutputReadLine();
        }

        public bool WaitForExit(int milliseconds)
        {
            return _process.WaitForExit(milliseconds);
        }

        public void Kill()
        {
            _process.Kill();
        }

        public int ExitCode
        {
            get { return _process.ExitCode; }
        }

        public void Dispose()
        {
            _process.Dispose();
        }
    }
}

