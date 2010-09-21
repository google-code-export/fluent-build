namespace FluentBuild.Core
{
    public class FrameworkVersion
    {
        public static FrameworkVersion NET4_0 = new FrameworkVersion("v4.0.30319");

        public static FrameworkVersion NET3_5 = new FrameworkVersion("v3.5");

        public static FrameworkVersion NET3_0 = new FrameworkVersion("v3.0");


        public static FrameworkVersion NET2_0 = new FrameworkVersion("v2.0.50727");


        public static FrameworkVersion NET1_1 = new FrameworkVersion("v1.1.4322");


        public static FrameworkVersion NET1_0 = new FrameworkVersion("v1.0.3705");
        private readonly string _fullVersion;

        internal string FullVersion { get { return _fullVersion; } }

        private FrameworkVersion(string fullVersion)
        {
            _fullVersion = fullVersion;
        }
    }
}