namespace FluentBuild.Runners.UnitTesting
{
    public class UnitTestFrameworkRun
    {
        public NUnitRunner NUnit { get { return new NUnitRunner();} }
    }
}