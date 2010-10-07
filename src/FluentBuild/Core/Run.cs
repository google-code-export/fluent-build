using FluentBuild.Runners;
using FluentBuild.Runners.UnitTesting;
using FluentBuild.Runners.Zip;

namespace FluentBuild.Core
{
    /// <summary>
    /// Runs an execuable. It may later be expaned to have other run tasks (e.g. nunit, code analysis, etc.)
    /// </summary>
    public class Run
    {
        public static UnitTestFrameworkRun UnitTestFramework
        {
            get { return new UnitTestFrameworkRun(); }
        }

        public static Zip Zip
        {
            get { return new Zip(); }
        }

        public static ILMerge ILMerge
        {
            get { return new ILMerge(); }
        }

        /// <summary>
        /// Creates an Executable object based on a string path
        /// </summary>
        /// <param name="executeablePath">Path to the executable</param>
        /// <returns>an Executable object</returns>
        public static Executeable Executeable(string executeablePath)
        {
            return new Executeable(executeablePath);
        }

        /// <summary>
        /// Builds an Executable object based on a build artifact
        /// </summary>
        /// <param name="executeablePath">The build artifact</param>
        /// <returns>an Executable object</returns>
        public static Executeable Executeable(BuildArtifact executeablePath)
        {
            return new Executeable(executeablePath.ToString());
        }

        public static void Debugger()
        {
            System.Diagnostics.Debugger.Break();
            System.Diagnostics.Debugger.Launch();
        }
    }
}