using System;
using FluentBuild.Utilities;

namespace FluentBuild.Runners.UnitTesting
{
    public class UnitTestFrameworkArgs
    {
        internal readonly IActionExcecutor _actionExcecutor;

        public UnitTestFrameworkArgs(IActionExcecutor actionExcecutor)
        {
            _actionExcecutor = actionExcecutor;
        }

        public UnitTestFrameworkArgs() : this(new ActionExcecutor())
        {
        }

        public void Nunit(Func<NUnitRunner,object> args)
        {
            _actionExcecutor.ExecuteFailable(args);
        }

//        public void MsTest(Ms args)
//        {
//            
//        }
    }
}