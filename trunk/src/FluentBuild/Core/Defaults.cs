using FluentBuild.Utilities;

namespace FluentBuild.Core
{
    ///<summary>
    /// Defaults for the fluent build runner
    ///</summary>
    public static class Defaults
    {
        ///<summary>
        /// Sets the behavior of what to do when an error occurs. The default is to fail.
        ///</summary>
        public static OnError OnError = OnError.Fail;

        ///<summary>
        /// Sets the .NET Framework version to use. The default is .NET 4.0
        ///</summary>
        public static IFrameworkVersion FrameworkVersion;

        static Defaults()
        {
            FrameworkVersion = Utilities.FrameworkVersion.NET4_0.Full ;
        }
    }
}