namespace FluentBuild.Utilities
{
    public class DesktopFrameworkType
    {
        private readonly string _frameworkVersion;
        private readonly string[] _sdkInstalledRoot;
        private readonly string[] _frameworkInstalled;

        public DesktopFrameworkType(string frameworkVersion, string[] sdkInstalledRoot, string[] frameworkInstalled)
        {
            _frameworkVersion = frameworkVersion;
            _sdkInstalledRoot = sdkInstalledRoot;
            _frameworkInstalled = frameworkInstalled;
        }

        public FrameworkVersion Client
        {
            get { return new FrameworkVersion(_frameworkVersion, _sdkInstalledRoot, _frameworkInstalled ); }
        }

        public FrameworkVersion Full
        {
            get { return new FrameworkVersion(_frameworkVersion, _sdkInstalledRoot, _frameworkInstalled); }
        }
    }
}