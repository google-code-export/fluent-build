using System;
using Microsoft.Win32;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Rhino.Mocks;

namespace FluentBuild
{
    [TestFixture]
    public class WindowsSdkFinderTests
    {
        private IRegistryWrapper _mockRegistrySeeker;
        private WindowsSdkFinder _sdkFinder;

        [SetUp]
        public void Setup()
        {
            _mockRegistrySeeker = MockRepository.GenerateStub<IRegistryWrapper>();
            _sdkFinder = new WindowsSdkFinder(_mockRegistrySeeker);
        }

        [Test]
        public void IsWindowsSdkInstalledShouldFailIfNoKeyIsFound()
        {
            _mockRegistrySeeker.Stub(x => x.OpenLocalMachineKey(WindowsSdkFinder.RegistryKeyToSdks)).Return(null);
            Assert.That(_sdkFinder.IsWindowsSdkInstalled(), Is.False);
        }

        [Test]
        public void IsWindowsSdkInstalledShouldPassIfKeyIsFound()
        {
            _mockRegistrySeeker.Stub(x => x.OpenLocalMachineKey(WindowsSdkFinder.RegistryKeyToSdks)).Return(new RegistryKeyWrapper());
            Assert.That(_sdkFinder.IsWindowsSdkInstalled(), Is.True);
        }

        [Test, ExpectedException(typeof(ApplicationException))]
        public void PathToHighestVersionedSdkShouldFailIfSdkIsNotInstalled()
        {
            _mockRegistrySeeker.Stub(x => x.OpenLocalMachineKey(WindowsSdkFinder.RegistryKeyToSdks)).Return(null);
            _sdkFinder.PathToHighestVersionedSdk();
        }

        [Test]
        public void PathToHighestVersionedSdkShouldFindHighestVersion()
        {
            var mockKey = MockRepository.GenerateStub<IRegistryKeyWrapper>();
            _mockRegistrySeeker.Stub(x => x.OpenLocalMachineKey(WindowsSdkFinder.RegistryKeyToSdks)).Return(mockKey);
            
            mockKey.Stub(x => x.GetSubKeyNames()).Return(new System.Collections.Generic.List<String>()
                                                             {"BLAH"});
            
            var mockVersionKey = MockRepository.GenerateStub<IRegistryKeyWrapper>();
            mockKey.Stub(x => x.OpenSubKey("BLAH")).Return(mockVersionKey);
            mockVersionKey.Stub(x => x.GetValue("ProductVersion")).Return("1.0");
            mockVersionKey.Stub(x => x.GetValue("InstallationFolder")).Return("c:\\");
            _sdkFinder.PathToHighestVersionedSdk();
        }


        [Test, ExpectedException(typeof(ApplicationException))]
        public void PathToHighestVersion_ShouldFailIfRegistryKeyDeletedWhileItIsBeingRead()
        {
            var mockKey = MockRepository.GenerateStub<IRegistryKeyWrapper>();
            _mockRegistrySeeker.Stub(x => x.OpenLocalMachineKey(WindowsSdkFinder.RegistryKeyToSdks)).Return(mockKey);

            mockKey.Stub(x => x.GetSubKeyNames()).Return(new System.Collections.Generic.List<String>() { "BLAH" });

            var mockVersionKey = MockRepository.GenerateStub<IRegistryKeyWrapper>();
            mockKey.Stub(x => x.OpenSubKey("BLAH")).Return(null);

            _sdkFinder.PathToHighestVersionedSdk();
        }


    }
}