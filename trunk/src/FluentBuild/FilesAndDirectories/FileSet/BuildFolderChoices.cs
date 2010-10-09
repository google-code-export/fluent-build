namespace FluentBuild.FilesAndDirectories.FileSet
{
    ///<summary>
    /// Allows the user to pick certain attributes to add to a folder
    ///</summary>
    public class BuildFolderChoices : Core.FileSet
    {
        private readonly bool _isInclusion;

        internal BuildFolderChoices(Core.FileSet fileset, IFileSystemUtility utility, bool isInclusion) : base(utility)
        {
            _isInclusion = isInclusion;
            PendingInclude = fileset.PendingInclude;
            PendingExclude = fileset.PendingExclude;
            Inclusions = fileset.Inclusions;
            Exclusions = fileset.Exclusions;
        }

        ///<summary>
        /// Modifies the current include to have a \\**\\ added to the end
        ///</summary>
        public BuildFolderChoices RecurseAllSubDirectories
        {
            get
            {
                if (_isInclusion)
                    PendingInclude = PendingInclude + "\\**\\";
                else
                    PendingExclude = PendingExclude + "\\**\\";
                return this;
            }
        }

        ///<summary>
        /// Applies a filter to use when searching for files
        ///</summary>
        ///<param name="filter">A wildcard filter (e.g. *.cs)</param>
        public Core.FileSet Filter(string filter)
        {
            if (_isInclusion)
                PendingInclude = PendingInclude + "\\" + filter;
            else
                PendingExclude = PendingExclude + "\\" + filter;
            ProcessPendings();
            return this;
        }
    }
}