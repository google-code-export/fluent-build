using System;

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
        public void Library(Action<BuildTask> args)
        {
            args(_buildTask);
            _buildTask.TargetType = "library";
            _buildTask.InternalExecute();
        }

        /// <summary>
        /// Builds a windows executable
        /// </summary>
        public void WindowsExecutable(Action<BuildTask> args)
        {
            args(_buildTask);
            _buildTask.TargetType = "winexe";
            _buildTask.InternalExecute();
        }

        /// <summary>
        /// Builds a console application
        /// </summary>
        public void Executable(Action<BuildTask> args)
        {
            args(_buildTask);
            _buildTask.TargetType = "exe";
            _buildTask.InternalExecute();
        }

        /// <summary>
        /// Builds a module
        /// </summary>
        public void Module(Action<BuildTask> args)
        {
            args(_buildTask);
            _buildTask.TargetType = "module";
            _buildTask.InternalExecute();
        }
    }
}