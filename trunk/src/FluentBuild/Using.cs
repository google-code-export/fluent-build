using FluentBuild.Compilation;
using FluentBuild.Core;

namespace FluentBuild
{
    public static class Using
    {
        /// <summary>
        /// Creates a BuildTask using the C# compiler
        /// </summary>
        public static TargetType Csc
        {
            get { return new TargetType(new BuildTask("csc.exe")); }
        }

        /// <summary>
        /// Creates a BuildTask using the VB compiler
        /// </summary>
        public static TargetType Vbc
        {
            get { return new TargetType(new BuildTask("vbc.exe")); }
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