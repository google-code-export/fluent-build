using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;

namespace FluentBuild
{
    internal class WindowsSdkFinder
    {
        private const string registryKeyToSdks = "SOFTWARE\\Microsoft\\Microsoft SDKs\\Windows";

        public static bool IsWindowsSDKInstalled()
        {
            RegistryKey key = Registry.LocalMachine.OpenSubKey(registryKeyToSdks);
            if (key == null)
                return false;
            return true;
        }

        public static string PathToHighestVersionedSDK()
        {
            if(!IsWindowsSDKInstalled())
              throw new ApplicationException("Windows SDK is not installed!");

            RegistryKey key = Registry.LocalMachine.OpenSubKey(registryKeyToSdks);
          
            string path = "";
            Version highestVersionFound = new Version(0, 0);

            foreach (var keyName in key.GetSubKeyNames())
            {
                RegistryKey versionKey = key.OpenSubKey(keyName);
                if(versionKey==null)
                    throw new ApplicationException("A registry key vanished while it was being read");

                var keyVersion = new Version(versionKey.GetValue("ProductVersion").ToString());
                if ((keyVersion > highestVersionFound) || (highestVersionFound.Major==0))
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
