namespace FluentBuild.Compilation
{
    ///<summary>
    /// Picks the Target Type
    ///</summary>
    public class TargetType
    {
        private readonly BuildTask _buildTask;
        private Target _target;

        internal TargetType(BuildTask buildTask)
        {
            _buildTask = buildTask;
        }

        ///<summary>
        /// Pick the target
        ///</summary>
        public Target Target
        {
            get
            {
                _target = new Target(_buildTask);
                return _target;
            }
        }
    }
}