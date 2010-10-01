namespace FluentBuild.Runners.UnitTesting
{
    ///<summary>
    /// Allows you to choose which unit testing framework to run
    ///</summary>
    public class UnitTestFrameworkRun
    {
        ///<summary>
        /// Selects NUnit as the unit test runner
        ///</summary>
        public NUnitRunner NUnit { get { return new NUnitRunner();} }

        internal UnitTestFrameworkRun()
        {
        }
    }
}