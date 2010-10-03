using System;
using System.Collections.Generic;
using System.Text;
using FluentBuild.Utilities;
using System.Linq;

namespace FluentBuild.FrameworkFinders
{
    //check all folders in
    //pull Version
    //greater than 3.5 can pull InstallPath from folder
    //highest version wins.

    //seek out SDK for set version.

    //if information can not be found then fail?
    //SDK is not required for msbuild so maybe failing should be done only when the 
    //property is used.
    //SDK has sn, xsd.exe, wsdl.exe, svcutil.exe, resgen.exe

    //32 bit vs 64

    //need custom framework version so that people can implement it before an official release is out

    //Silverlight:
    //C:\Program Files (x86)\Microsoft Silverlight\4.0.50826.0

    //need friendly name
    //sdkdirectory
    //framework directoy
    //clr version#

    //FrameworkVersion.Desktop.v1_1.ServicePack.1
    //FrameworkVersion.Silverlight
    //FrameworkVersion.Compact
    //FrameworkVersion.Mono / Moonlight
        


    public abstract class DefaultFinder : IFrameworkFinder
    {
        protected DefaultFinder()
        {
            PossibleSdkInstallKeys = new List<string>();
            PossibleFrameworkInstallKeys = new List<string>();
        }

        public string PathToSdk()
        {
            var finder = new RegistryKeyValueFinder();
            var foundValue = finder.FindFirstValue(PossibleSdkInstallKeys.ToArray());
            if (string.IsNullOrEmpty(foundValue.Key))
                return null;
            return foundValue.Value;
        }

        protected IList<string> PossibleSdkInstallKeys { get; set; }

        public virtual string PathToFrameworkInstall()
        {
            var baseInstallPath = @"SOFTWARE\Microsoft\.NETFramework\InstallRoot";
            PossibleFrameworkInstallKeys.Add(baseInstallPath);
            var finder = new RegistryKeyValueFinder();
            var foundValue = finder.FindFirstValue(PossibleFrameworkInstallKeys.ToArray());
            if (string.IsNullOrEmpty(foundValue.Key))
                return null;
            if (foundValue.Key == baseInstallPath)
                return foundValue.Value + "\\" + FrameworkFolderVersionName;
            return foundValue.Value;
        }

        protected abstract string FrameworkFolderVersionName { get; }

        private string CreateCommaSeperatedList(IList<string> input)
        {
            var sb = new StringBuilder();
            foreach (var paths in input)
            {
                sb.Append(paths);
                sb.Append(", ");
            }
            sb.Remove(sb.Length - 2, 2);
            return sb.ToString();
        }

        public string SdkSearchPathsUsed
        {
            get { return CreateCommaSeperatedList(PossibleSdkInstallKeys); }
           
        }

        public string FrameworkSearchPaths
        {
            get { return CreateCommaSeperatedList(PossibleFrameworkInstallKeys); }
          
        }

        protected IList<string> PossibleFrameworkInstallKeys { get; set; }

    }
}