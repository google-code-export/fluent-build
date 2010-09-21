using FluentBuild.Utilities;

namespace FluentBuild.Core
{
    public static class Defaults
    {
        public static OnError OnError = OnError.Fail;
        public static FrameworkVersion FrameworkVersion = FrameworkVersion.NET4_0;

        static Defaults()
        {
            FrameworkVersion = FrameworkVersion.NET4_0;
        }
    }
}