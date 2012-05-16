using System;

namespace FluentBuild.AssemblyInfoBuilding
{
    ///<summary>
    /// Determines which language to use to generate an assembly info file
    ///</summary>
    public class AssemblyInfoLanguage
    {
        internal AssemblyInfoLanguage()
        {
        }

        /// <summary>
        /// Generate using C#
        /// </summary>
        public void CSharp(Action<AssemblyInfoDetails> args)
        {
            var concrete = new AssemblyInfoDetails(new CSharpAssemblyInfoBuilder());
            args(concrete);
            concrete.InternalExecute();
        }

        /// <summary>
        /// Generate using Visual Basic
        /// </summary>
        public void VisualBasic(Action<AssemblyInfoDetails> args)
        {
            var concrete = new AssemblyInfoDetails(new VisualBasicAssemblyInfoBuilder());
            args(concrete);
            concrete.InternalExecute();
        }
    }
}