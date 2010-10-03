using System;

namespace FluentBuild.FrameworkFinders
{
    public class Desktop4_0FullFrameworkFinder : DefaultFinder
    {
        public Desktop4_0FullFrameworkFinder()
        {
            PossibleSdkInstallKeys.Add(@"SOFTWARE\Microsoft\Microsoft SDKs\Windows\v7.0A\InstallationFolder");
            PossibleSdkInstallKeys.Add(@"SOFTWARE\Microsoft\Microsoft SDKs\Windows\v7.0\InstallationFolder");

            PossibleFrameworkInstallKeys.Add(@"SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full\InstallPath");

            
        }

        protected override string FrameworkFolderVersionName
        {
            get { return "v4.0.30319"; }
        }
    }
}