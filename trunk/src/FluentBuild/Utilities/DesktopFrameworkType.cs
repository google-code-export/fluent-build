namespace FluentBuild.Utilities
{
    ///<summary>
    /// Indicates if the type is client or full (used in .NET 4.0 and higher)
    ///</summary>
    public class DesktopFrameworkType
    {
        private readonly string _frameworkVersion;
        private readonly string[] _sdkInstalledRoot;
        private readonly string[] _frameworkInstalled;

        internal DesktopFrameworkType(string frameworkVersion, string[] sdkInstalledRoot, string[] frameworkInstalled)
        {
            _frameworkVersion = frameworkVersion;
            _sdkInstalledRoot = sdkInstalledRoot;
            _frameworkInstalled = frameworkInstalled;
        }

        ///<summary>
        /// Creates a FrameworkVersion object for the client type
        ///</summary>
        public FrameworkVersion Client
        {
            get { return new FrameworkVersion(_frameworkVersion, _sdkInstalledRoot, _frameworkInstalled ); }
        }

        ///<summary>
        /// Creates a FrameworkVersion object for the full type
        ///</summary>
        public FrameworkVersion Full
        {
            get { return new FrameworkVersion(_frameworkVersion, _sdkInstalledRoot, _frameworkInstalled); }
        }
    }
}