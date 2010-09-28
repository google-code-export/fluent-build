namespace FluentBuild.Compilation
{
    /// <summary>
    /// Determines the type of assembly to build
    /// </summary>
    public class Target
    {
        private readonly BuildTask _buildTask;

        protected internal Target(BuildTask buildTask)
        {
            _buildTask = buildTask;
        }

        /// <summary>
        /// Builds a library assembly (i.e. a dll)
        /// </summary>
        public BuildTask Library
        {
            get
            {
                _buildTask.TargetType = "library";
                return _buildTask;
            }
        }

        /// <summary>
        /// Builds a windows executable
        /// </summary>
        public BuildTask WindowsExecutable
        {
            get
            {
                _buildTask.TargetType = "winexe";
                return _buildTask;
            }
        }

        /// <summary>
        /// Builds a console application
        /// </summary>
        public BuildTask Executable
        {
            get
            {
                _buildTask.TargetType = "exe";
                return _buildTask;
            }
        }

        /// <summary>
        /// Builds a module
        /// </summary>
        public BuildTask Module
        {
            get
            {
                _buildTask.TargetType = "module";
                return _buildTask;
            }
        }
    }
}