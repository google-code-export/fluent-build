namespace FluentBuild
{
    public static class CreateBuildTask
    {
        public static Build UsingCsc
        {
            get { return new Build("csc.exe"); }
        }

        public static Build UsingVbc
        {
            get { return new Build("vbc.exe"); }
        }
    }
}