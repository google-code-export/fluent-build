namespace FluentBuild
{
    public class Target
    {
        private readonly Build build;

        protected internal Target(Build build)
        {
            this.build = build;
        }
        
        public Build Library
        {
            get
            {
                build.TargetType = "Library";
                return build;
            }
        }

        public Build WindowsExecutable
        {
            get
            {
                build.TargetType = "winexe";
                return build;
            }
        }

        public Build Executable
        {
            get
            {
                build.TargetType = "exe";
                return build;
            }
        }

        public Build Module
        {
            get
            {
                build.TargetType = "module";
                return build;
            }
        }
    }
}