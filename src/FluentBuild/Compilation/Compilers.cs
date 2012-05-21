﻿using System;
using FluentBuild.Core;

namespace FluentBuild.Compilation
{
    public class Compilers
    {
        private readonly IActionExcecutor _actionExcecutor;

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

        internal Compilers() : this(new ActionExcecutor())
        {
        }

        internal Compilers(IActionExcecutor actionExcecutor)
        {
            _actionExcecutor = actionExcecutor;
        }

        /// <summary>
        /// Creates a BuildTask using MSBuild
        /// </summary>
        public void MsBuild(Action<MsBuildTask> args)
        {
            _actionExcecutor.Execute(args);
        }
    }
}