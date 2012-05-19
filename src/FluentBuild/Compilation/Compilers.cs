using System;
using FluentBuild.Core;

namespace FluentBuild.Compilation
{
    public class Compilers
    {
        /// <summary>
        /// Creates a BuildTask using the C# compiler
        /// </summary>
        public TargetType Csc
        {
            get { return new TargetType(new BuildTask("csc.exe")); }
        }

        /// <summary>
        /// Creates a BuildTask using the VB compiler
        /// </summary>
        public TargetType Vbc
        {
            get { return new TargetType(new BuildTask("vbc.exe")); }
        }

        /// <summary>
        /// Creates a BuildTask using MSBuild
        /// </summary>
        public void MsBuild(Action<MsBuildTask> args)
        {
            var executor = new ActionExcecutor<MsBuildTask>();
            executor.Execute(args);
        }
    }
}