using System;

namespace FluentBuild
{
    public interface IWindowsSdkFinder
    {
        /// <summary>
        /// Determines if the Windows SDK is installed by checking for the existance of the SOFTWARE\\Microsoft\\Microsoft SDKs\\Windows registry key
        /// </summary>
        /// <returns>true if the key is found and false if the key is not found</returns>
        bool IsWindowsSdkInstalled();

        /// <summary>
        /// Finds the highest SDK version installed via its registry key
        /// </summary>
        /// <returns>The path to the hisghest SDK version</returns>
        string PathToHighestVersionedSdk();
    }

    /// <summary>
    /// A Utility class that is used to locate the Windows SDK so that the appropriate compiler can be run
    /// </summary>
    internal class WindowsSdkFinder : IWindowsSdkFinder
    {
        public const string RegistryKeyToSdks = "SOFTWARE\\Microsoft\\Microsoft SDKs\\Windows";
        private static IRegistryWrapper _seeker;

        public WindowsSdkFinder(IRegistryWrapper seeker)
        {
            _seeker = seeker;
        }

        public WindowsSdkFinder() : this(new RegistryWrapper())
        {
        }

        /// <summary>
        /// Determines if the Windows SDK is installed by checking for the existance of the SOFTWARE\\Microsoft\\Microsoft SDKs\\Windows registry key
        /// </summary>
        /// <returns>true if the key is found and false if the key is not found</returns>
        public bool IsWindowsSdkInstalled()
        {
            IRegistryKeyWrapper key = _seeker.OpenLocalMachineKey(RegistryKeyToSdks);
            if (key == null)
                return false;
            return true;
        }


        /// <summary>
        /// Finds the highest SDK version installed via its registry key
        /// </summary>
        /// <returns>The path to the hisghest SDK version</returns>
        public string PathToHighestVersionedSdk()
        {
            if (!IsWindowsSdkInstalled())
                throw new ApplicationException("Windows SDK is not installed!");

            IRegistryKeyWrapper key = _seeker.OpenLocalMachineKey(RegistryKeyToSdks);

            string path = "";
            var highestVersionFound = new Version(0, 0);

            foreach (string keyName in key.GetSubKeyNames())
            {
                IRegistryKeyWrapper versionKey = key.OpenSubKey(keyName);
                if (versionKey == null)
                    throw new ApplicationException("A registry key vanished while it was being read");

                var keyVersion = new Version(versionKey.GetValue("ProductVersion").ToString());
                if ((keyVersion > highestVersionFound) || (highestVersionFound.Major == 0))
                {
                    path = versionKey.GetValue("InstallationFolder").ToString();
                    highestVersionFound = keyVersion;
                }
                versionKey.Close();
            }
            key.Close();
            MessageLogger.WriteDebugMessage("Found windows SDK at: " + path);
            return path;
        }
    }
}