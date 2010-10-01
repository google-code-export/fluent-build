using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentBuild.Core;
using NUnit.Framework;
using Rhino.Mocks;

namespace FluentBuild.Utilities
{
    ///<summary />	[TestFixture]
    public class FrameworkVersionTests
    {

        [Test, ExpectedException(typeof(SdkNotFoundException))]
        public void GetPathToSdk_ShouldThrowExceptionIfNoSdkFound()
        {
            var registryKeyValueFinder = MockRepository.GenerateMock<IRegistryKeyValueFinder>();
            var keysToCheck = new[] {@"SOFTWARE\Microsoft\.NETFramework\sdkInstallRoot", @"SOFTWARE\garbage"};
            var framework = new FrameworkVersion(registryKeyValueFinder, "v0.0.0.0", keysToCheck, null);
            registryKeyValueFinder.Stub(x => x.FindFirstValue(keysToCheck)).Return(null);
            framework.GetPathToSdk();
        }

        ///<summary />	[Test]
        public void GetPathToSdk_ShouldCallOutToRegistryKeyFinder()
        {
            var registryKeyValueFinder = MockRepository.GenerateMock<IRegistryKeyValueFinder>();
            var keysToCheck = new[] {@"SOFTWARE\Microsoft\.NETFramework\sdkInstallRoot", @"SOFTWARE\garbage"};
            var framework = new FrameworkVersion(registryKeyValueFinder, "v0.0.0.0", keysToCheck, null);
            registryKeyValueFinder.Stub(x => x.FindFirstValue(keysToCheck)).Return("c:\\temp");
            framework.GetPathToSdk();
            registryKeyValueFinder.AssertWasCalled(x => x.FindFirstValue(keysToCheck));
        }

        ///<summary />	[Test]
        public void GetPathToFrameworkInstall_ShouldCallOutToRegistryKeyFinder()
        {
            var registryKeyValueFinder = MockRepository.GenerateMock<IRegistryKeyValueFinder>();
            var keysToCheck = new[] { @"SOFTWARE\Microsoft\.NETFramework\sdkInstallRoot", @"SOFTWARE\garbage" };
            var framework = new FrameworkVersion(registryKeyValueFinder, "v0.0.0.0", null, keysToCheck);
            registryKeyValueFinder.Stub(x => x.FindFirstValue(keysToCheck)).Return("c:\\temp");
            framework.GetPathToFrameworkInstall();
            registryKeyValueFinder.AssertWasCalled(x => x.FindFirstValue(keysToCheck));
        }

        [Test, ExpectedException(typeof(FrameworkNotFoundException))]
        public void GetPathToFrameworkInstall_ShouldThrowExceptionIfNoSdkFound()
        {
            var registryKeyValueFinder = MockRepository.GenerateMock<IRegistryKeyValueFinder>();
            var keysToCheck = new[] { @"SOFTWARE\Microsoft\.NETFramework\sdkInstallRoot", @"SOFTWARE\garbage" };
            var framework = new FrameworkVersion(registryKeyValueFinder, "v0.0.0.0", null, keysToCheck);
            registryKeyValueFinder.Stub(x => x.FindFirstValue(keysToCheck)).Return(null);
            framework.GetPathToFrameworkInstall();
        }

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
            


            
        

            //Core:
            //HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\NET Framework Setup\NDP\            
            //HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\.NETFramework\InstallRoot = path to framework install

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
        
    }
}
