using FluentBuild.Utilities;

namespace FluentBuild.FrameworkFinders
{
    ///<summary>
    /// Determines the location of Framework 3.0 components.
    ///</summary>
    public class Desktop3_0Finder : DefaultFinder
    {
        ///<summary>
        /// Creates the finder.
        ///</summary>
        public Desktop3_0Finder()
        {
            PossibleSdkInstallKeys.Add(@"SOFTWARE\Microsoft\.NETFramework\sdkInstallRootv2.0");
        }

        protected override string FrameworkFolderVersionName
        {
            get { return "v3.0"; }
        }

    }
}