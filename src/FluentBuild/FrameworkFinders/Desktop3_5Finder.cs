namespace FluentBuild.FrameworkFinders
{
    public class Desktop3_5Finder : DefaultFinder
    {
        public Desktop3_5Finder()
        {
           PossibleFrameworkInstallKeys.Add(@"SOFTWARE\Microsoft\NET Framework Setup\NDP\v3.5\InstallPath");
            

            PossibleSdkInstallKeys.Add(@"SOFTWARE\Microsoft\Microsoft SDKs\Windows\v7.0A\InstallationFolder");
            PossibleSdkInstallKeys.Add(@"SOFTWARE\Microsoft\Microsoft SDKs\Windows\v7.0\InstallationFolder");
            PossibleSdkInstallKeys.Add(@"SOFTWARE\Microsoft\Microsoft SDKs\Windows\v6.1\InstallationFolder");
            PossibleSdkInstallKeys.Add(@"SOFTWARE\Microsoft\Microsoft SDKs\Windows\v6.0A\WinSDKNetFxTools\InstallationFolder");
        }

        protected override string FrameworkFolderVersionName
        {
            get { return "v3.5"; }
        }
    }
}