using System;
using FluentBuild.Runners.UnitTesting;

namespace FluentBuild.Runners
{
    public class UnitTestFrameworkArgs
    {
        public void Nunit(Action<INUnitRunner> args)
        {
            var implementation = new NUnitRunner();
            args(implementation);
            implementation.InternalExecute();
        }

//        public void MsTest(Ms args)
//        {
//            
//        }
    }
}