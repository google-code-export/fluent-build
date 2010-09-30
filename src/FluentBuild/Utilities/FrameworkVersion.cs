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

    public interface IFrameworkVersion
    {
        string GetPathToSdk();
        string GetPathToFrameworkInstall();
    }

    public class FrameworkVersion : IFrameworkVersion 
    {
        //public static CustomFrameworkVersion Custom { get { return new CustomFrameworkVersion();} }
        
        public static DesktopFrameworkType NET4_0 = new DesktopFrameworkType("v4.0.30319", new[] {""}
                                                                                         ,new[]{ @"SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full\InstallPath" ,@"SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Client\InstallPath" ,@"SOFTWARE\Microsoft\.NETFramework\InstallRoot"});
        public static IFrameworkVersion NET3_5 = new FrameworkVersion("v3.5", new[]{@"SOFTWARE\Microsoft\Microsoft SDKs\Windows\v7.0A\InstallationFolder", @"SOFTWARE\Microsoft\Microsoft SDKs\Windows\v7.0\InstallationFolder", @"SOFTWARE\Microsoft\Microsoft SDKs\Windows\v6.1\InstallationFolder", @"SOFTWARE\Microsoft\Microsoft SDKs\Windows\v6.0A\WinSDKNetFxTools\InstallationFolder"}
                                                                            , new[]{@"SOFTWARE\Microsoft\NET Framework Setup\NDP\v3.5\InstallPath", @"SOFTWARE\Microsoft\.NETFramework\InstallRoot"});

        public static IFrameworkVersion NET3_0 = new FrameworkVersion("v3.0", new[]{@"SOFTWARE\Microsoft\.NETFramework\sdkInstallRootv2.0"}
                                                                            , new[]{@"SOFTWARE\Microsoft\.NETFramework\InstallRoot"});

        public static IFrameworkVersion NET2_0 = new FrameworkVersion("v2.0.50727", new[] {@"SOFTWARE\Microsoft\.NETFramework\sdkInstallRootv2.0"}
                                                                                  , new[] {@"SOFTWARE\Microsoft\.NETFramework\InstallRoot"});

        public static IFrameworkVersion NET1_1 = new FrameworkVersion("v1.1.4322", new[] {@"SOFTWARE\Microsoft\.NETFramework\sdkInstallRootv1.1"}
                                                                                 , new[] {@"SOFTWARE\Microsoft\.NETFramework\InstallRoot"});

        public static IFrameworkVersion NET1_0 = new FrameworkVersion("v1.0.3705", new[] {@"SOFTWARE\Microsoft\.NETFramework\sdkInstallRoot"}
                                                                                 , new[] {@"SOFTWARE\Microsoft\.NETFramework\InstallRoot"});

        private readonly string[] _frameworkInstallRoot;
        private readonly string _fullVersion;
        private readonly IRegistryKeyValueFinder _registryKeyValueFinder;
        private readonly string[] _sdkInstallRoot;

        internal FrameworkVersion(IRegistryKeyValueFinder registryKeyValueFinder, string fullVersion,
                                  string[] sdkInstallRoot, string[] frameworkInstallRoot)
        {
            _registryKeyValueFinder = registryKeyValueFinder;
            _fullVersion = fullVersion;
            _sdkInstallRoot = sdkInstallRoot;
            _frameworkInstallRoot = frameworkInstallRoot;
        }

        internal FrameworkVersion(string fullVersion, string[] sdkInstallRoot, string[] frameworkInstallRoot)
            : this(new RegistryKeyValueFinder(), fullVersion, sdkInstallRoot, frameworkInstallRoot)
        {
        }

        public string GetPathToSdk()
        {
            string pathToSdk = _registryKeyValueFinder.FindFirstValue(_sdkInstallRoot);
            if (pathToSdk == null)
                throw new SdkNotFoundException();
            return pathToSdk;
        }

        public string GetPathToFrameworkInstall()
        {
            string pathToFrameworkInstall = _registryKeyValueFinder.FindFirstValue(_frameworkInstallRoot);
            if (pathToFrameworkInstall == null)
                throw new FrameworkNotFoundException();
            //TODO: may have to append version # if not 3.5 or 4.0
            return pathToFrameworkInstall;
        }
    }
}