namespace FluentBuild.Runners
{
    public class UnitTestFrameworkRun
    {
        public NUnitRunner NUnit { get { return new NUnitRunner();} }
    }
}