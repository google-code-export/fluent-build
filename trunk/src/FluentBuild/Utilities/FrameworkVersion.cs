using FluentBuild.FrameworkFinders;

namespace FluentBuild.Utilities
{
    //CF1.0
    //SDK: SOFTWARE\Microsoft\.NETCompactFramework\sdkInstallRoot
    //Desktop framework:SOFTWARE\Microsoft\.NETFramework\sdkInstallRootv1.1

    //CF2.0
    //SDK: SOFTWARE\Microsoft\.NETCompactFramework\v2.0.0.0\InstallRoot\
    //Desktop framework:SOFTWARE\Microsoft\.NETFramework\sdkInstallRootv2.0

    //SL2
    //SDK:SOFTWARE\Microsoft\Microsoft SDKs\Windows\v6.0A\WinSDKNetFxTools\InstallationFolder
    //    SOFTWARE\Microsoft\Microsoft SDKs\Windows\v6.1\InstallationFolder

    //SL3
    //SDK: 

    //SL4
    //SDK:


    ///<summary>
    /// .NET Framework version
    ///</summary>
    public interface IFrameworkVersion
    {
        ///<summary>
        /// Gets the path to the .NET/Windows SDK for the desired version
        ///</summary>
        ///<returns>path to the SDK</returns>
        ///<exception cref="SdkNotFoundException">Thrown if the SDK can not be found</exception>
        string GetPathToSdk();

        ///<summary>
        /// Gets the path to the .NET framework install directory
        ///</summary>
        ///<returns>The path to where the .NET Framework was installed</returns>
        /// <exception cref="FrameworkNotFoundException">Thrown when the .NET Framework path can not be found</exception>
        string GetPathToFrameworkInstall();
    }

    ///<summary>
    /// The .NET Framework version
    ///</summary>
    public class FrameworkVersion : IFrameworkVersion
    {
        //public static CustomFrameworkVersion Custom { get { return new CustomFrameworkVersion();} }

        public static DesktopFrameworkType NET4_0 = new DesktopFrameworkType(new Desktop4_0ClientFrameworkFinder(),
                                                                             new Desktop4_0FullFrameworkFinder());

        public static IFrameworkVersion NET3_5 = new FrameworkVersion(new Desktop3_5Finder());

        public static IFrameworkVersion NET3_0 = new FrameworkVersion(new Desktop3_0Finder());

        public static IFrameworkVersion NET2_0 = new FrameworkVersion(new Desktop2_0Finder());
        private readonly IFrameworkFinder _frameworkFinder;

        //public static IFrameworkVersion NET1_1 = new FrameworkVersion("v1.1.4322", new[] {@"SOFTWARE\Microsoft\.NETFramework\sdkInstallRootv1.1"}
        //                                                                         , new[] {@"SOFTWARE\Microsoft\.NETFramework\InstallRoot"});

        //public static IFrameworkVersion NET1_0 = new FrameworkVersion("v1.0.3705", new[] {@"SOFTWARE\Microsoft\.NETFramework\sdkInstallRoot"}
        //                                                                         , new[] {@"SOFTWARE\Microsoft\.NETFramework\InstallRoot"});


        internal FrameworkVersion(IFrameworkFinder frameworkFinder)
        {
            _frameworkFinder = frameworkFinder;
        }

        #region IFrameworkVersion Members

        public string GetPathToSdk()
        {
            string pathToSdk = _frameworkFinder.PathToSdk();
            if (pathToSdk == null)
                throw new SdkNotFoundException(_frameworkFinder.SdkSearchPathsUsed);
            return pathToSdk;
        }

        public string GetPathToFrameworkInstall()
        {
            string pathToFrameworkInstall = _frameworkFinder.PathToFrameworkInstall();
            if (pathToFrameworkInstall == null)
                throw new FrameworkNotFoundException(_frameworkFinder.FrameworkSearchPaths);
            return pathToFrameworkInstall;
        }

        #endregion
    }
}