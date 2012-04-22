namespace FluentBuild.AssemblyInfoBuilding
{
    /// <summary>
    /// Allows the creation of assembly info files
    /// </summary>
    public class AssemblyInfo
    {
        /// <summary>
        /// select the language used to generate the assembly info file
        /// </summary>
        public AssemblyInfoLanguage Language
        {
            get { return new AssemblyInfoLanguage(); }
        }

        internal AssemblyInfo()
        {
        }
    }
}