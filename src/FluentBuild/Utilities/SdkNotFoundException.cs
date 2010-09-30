using System;

namespace FluentBuild.Utilities
{
    public class SdkNotFoundException : ApplicationException
    {
        public override string Message
        {
            //TODO: fill in the paths
            get { return "Could not find the SDK by searching paths x. Please make sure it is installed."; }
        }
    }
}