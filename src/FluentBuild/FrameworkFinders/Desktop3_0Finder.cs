using FluentBuild.Utilities;

namespace FluentBuild.FrameworkFinders
{
    public class Desktop3_0Finder : DefaultFinder
    {
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