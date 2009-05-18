namespace FluentBuild
{
    public class Target
    {
        private readonly BuildTask buildTask;

        protected internal Target(BuildTask buildTask)
        {
            this.buildTask = buildTask;
        }
        
        public BuildTask Library
        {
            get
            {
                buildTask.TargetType = "library";
                return buildTask;
            }
        }

        public BuildTask WindowsExecutable
        {
            get
            {
                buildTask.TargetType = "winexe";
                return buildTask;
            }
        }

        public BuildTask Executable
        {
            get
            {
                buildTask.TargetType = "exe";
                return buildTask;
            }
        }

        public BuildTask Module
        {
            get
            {
                buildTask.TargetType = "module";
                return buildTask;
            }
        }
    }
}