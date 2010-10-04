namespace FluentBuild.FilesAndDirectories.FileSet
{
    ///<summary>
    /// Allows the user to pick certain attributes to add to a folder
    ///</summary>
    public class BuildFolderChoices : Core.FileSet
    {
        private readonly Core.FileSet _fileset;

        internal BuildFolderChoices(Core.FileSet fileset)
        {
            _fileset = fileset;
        }

        ///<summary>
        /// Modifies the current include to have a \\**\\ added to the end
        ///</summary>
        public BuildFolderChoices RecurseAllSubDirectories
        {
            get
            {
                _fileset.PendingInclude = _fileset.PendingInclude + "\\**\\";
                return this;
            }
        }

        ///<summary>
        /// Applies a filter to use when searching for files
        ///</summary>
        ///<param name="filter">A wildcard filter (e.g. *.cs)</param>
        public BuildFolderChoices Filter(string filter)
        {
            _fileset.PendingInclude = _fileset.PendingInclude + "\\" + filter;
            return this;
        }
    }
}