namespace FluentBuild
{
    public class FrameworkVersion
    {
        internal static string frameworkVersion = "v4.0.30319";

        public static void NET4_0()
        {
            frameworkVersion = "v4.0.30319";
        }

        public static void NET3_5()
        {
            frameworkVersion = "v3.5";
        }

        public static void NET3_0()
        {
            frameworkVersion = "v3.0";
        }

        public static void NET2_0()
        {
            frameworkVersion = "v2.0.50727";
        }

        public static void NET1_1()
        {
            frameworkVersion = "v1.1.4322";
        }

        public static void NET1_0()
        {
            frameworkVersion = "v1.0.3705";
        }
    }
}