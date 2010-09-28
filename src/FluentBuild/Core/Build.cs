using FluentBuild.Compilation;

namespace FluentBuild.Core
{
    ///<summary>
    /// Builds an assembly
    ///</summary>
    public static class Build
    {
        /// <summary>
        /// Creates a BuildTask using the C# compiler
        /// </summary>
        public static BuildTask UsingCsc
        {
            get { return new BuildTask("csc.exe"); }
        }

        /// <summary>
        /// Creates a BuildTask using the VB compiler
        /// </summary>
        public static BuildTask UsingVbc
        {
            get { return new BuildTask("vbc.exe"); }
        }

        /// <summary>
        /// Creates a BuildTask using MSBuild
        /// </summary>
        public static MsBuildTask UsingMsBuild(string projectOrSolutionFilePath)
        {
            return new MsBuildTask(projectOrSolutionFilePath);
        }
    }
}