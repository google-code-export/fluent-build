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
        public AssemblyInfoDetails CSharp
        {
            get { return new AssemblyInfoDetails(new CSharpAssemblyInfoBuilder());  }
        }

        /// <summary>
        /// Generate using Visual Basic
        /// </summary>
        public AssemblyInfoDetails VisualBasic
        {
            get { return new AssemblyInfoDetails(new VisualBasicAssemblyInfoBuilder()); }
        }
    }
}