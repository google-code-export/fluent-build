using System.Reflection;
using System.Threading;
using FluentBuild.Core;
using FluentBuild.MessageLoggers;

namespace FluentBuild.BuildUI
{
    public class Runner
    {
        private readonly string _target;
        private readonly string _assemblyPath;

        public Runner(string target, string assemblyPath)
        {
            _target = target;

            _assemblyPath = assemblyPath;
        }

        private void DoRun()
        {
            Defaults.Logger.Verbosity = VerbosityLevel.TaskDetails;

            Assembly assemblyInstance = Assembly.LoadFile(_assemblyPath);
            var build = (BuildFile)assemblyInstance.CreateInstance(_target);
            if (build != null) build.InvokeNextTask();
        }

        public void Run()
        {
            var th = new Thread(DoRun);
            th.Start();
            
        }
    }
}