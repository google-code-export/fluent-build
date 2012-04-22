namespace FluentBuild.Compilation
{
    ///<summary>
    /// Picks the Target Type
    ///</summary>
    public class TargetType
    {
        private readonly BuildTask _buildTask;

        internal TargetType(BuildTask buildTask)
        {
            _buildTask = buildTask;
        }

        ///<summary>
        /// Pick the target
        ///</summary>
        public Target Target
        {
            get { return new Target(_buildTask); }
        }
    }
}