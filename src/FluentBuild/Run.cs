using FluentBuild.Runners;

namespace FluentBuild
{

    /// <summary>
    /// Runs an execuable. It may later be expaned to have other run tasks (e.g. nunit, code analysis, etc.)
    /// </summary>
    public class Run
    {
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

        public static UnitTestFrameworkRun UnitTestFramework
        {
            get { return new UnitTestFrameworkRun() ;}
        }
    }
}