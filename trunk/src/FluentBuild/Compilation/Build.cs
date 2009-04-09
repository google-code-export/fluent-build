namespace FluentBuild
{
    public static class Build
    {
        public static BuildTask UsingCsc
        {
            get { return new BuildTask("csc.exe"); }
        }

        public static BuildTask UsingVbc
        {
            get { return new BuildTask("vbc.exe"); }
        }
    }
}