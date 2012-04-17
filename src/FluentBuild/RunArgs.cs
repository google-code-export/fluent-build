using System;
using FluentBuild.Runners;

namespace FluentBuild
{
    public class RunArgs
    {
        public void Zip(Action<IZipOptions> args)
        {
            var implementation = new ZipOptions();
            args(implementation);
        }

        public object Executable( object args)
        {
            return null;
        }

        public void Debugger()
        {
            System.Diagnostics.Debugger.Break();
            System.Diagnostics.Debugger.Launch();
        }

        public void ILMerge(Action<IILMerge> args)
        {
            var implementation = new ILMerge();
            args(implementation);
            implementation.InternalExecute();
        }

        public UnitTestFrameworkArgs UnitTestFramework { get { return new UnitTestFrameworkArgs();  }}
    }
}