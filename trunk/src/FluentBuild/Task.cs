using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using FluentBuild.Compilation;
using FluentBuild.Core;
using FluentBuild.Runners.Zip;

namespace FluentBuild
{
    public static class Task
    {
        /// <summary>
        /// Executes a build task
        /// </summary>
        /// <param name="args">The compiler to use. e.g. Using.Csc.Target.Library.</param>
        public static void Build(BuildTask args)
        {
            args.InternalExecute();
        }

        public static RunArgs Run
        {
            get { return new RunArgs();  }
        }
    }
}
