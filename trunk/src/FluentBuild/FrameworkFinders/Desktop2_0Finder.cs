using System;
using FluentBuild.Utilities;

namespace FluentBuild.FrameworkFinders
{
    public class Desktop2_0Finder : DefaultFinder
    {
        public Desktop2_0Finder()
        {
            
            PossibleSdkInstallKeys.Add(@"SOFTWARE\Microsoft\.NETFramework\sdkInstallRootv2.0");
        }

        protected override string FrameworkFolderVersionName
        {
            get { return "v2.0.50727"; }
        }
    }
}