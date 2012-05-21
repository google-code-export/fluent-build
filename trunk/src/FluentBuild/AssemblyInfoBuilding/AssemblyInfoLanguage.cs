using System;

namespace FluentBuild.AssemblyInfoBuilding
{
    ///<summary>
    /// Determines which language to use to generate an assembly info file
    ///</summary>
    public class AssemblyInfoLanguage
    {
        private readonly IActionExcecutor _executor;

        internal AssemblyInfoLanguage(IActionExcecutor executor)
        {
            _executor = executor;
        }

        internal AssemblyInfoLanguage() : this(new ActionExcecutor())
        {
        }

        /// <summary>
        /// Generate using C#
        /// </summary>
        public void CSharp(Action<AssemblyInfoDetails> args)
        {
            _executor.Execute(args, new CSharpAssemblyInfoBuilder());
        }

        /// <summary>
        /// Generate using Visual Basic
        /// </summary>
        public void VisualBasic(Action<AssemblyInfoDetails> args)
        {
            _executor.Execute(args, new VisualBasicAssemblyInfoBuilder());
        }
    }
}