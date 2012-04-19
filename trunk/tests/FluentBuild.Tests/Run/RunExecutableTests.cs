using System;
using System.ComponentModel;
using FluentBuild.Runners;
using NUnit.Framework;

namespace FluentBuild.Tests.Run
{
    [TestFixture]
    public class RunExecutableTests :TestBase
    {
        [Test]
        public void ShouldRunCmdPrompt()
        {
            string pathtocmd = Environment.GetEnvironmentVariable("windir") + @"\system32\cmd.exe";
           Task.Run.Executable(x=>x.ExecutablePath(pathtocmd).WithArguments(@"/K 'cd \'"));
        }

        [Test, ExpectedException(typeof(Win32Exception))]
        public void ShouldFailIfFileDoesNotExist()
        {
            string pathtocmd = Environment.GetEnvironmentVariable("windir") + @"\afilethatdoesnotexist.exe";
            Task.Run.Executable(x=>x.ExecutablePath(pathtocmd));
        }

        [Test]
        public void ShouldNotThrowErrorIfFileDoesNotExistAndContinueOnErrorIsSet()
        {
            string pathtocmd = Environment.GetEnvironmentVariable("windir") + @"\afilethatdoesnotexist.exe";
            Task.Run.Executable(x=>x.ExecutablePath(pathtocmd).ContinueOnError2());
            Assert.Fail("This needs to be fixed");
        }

        [Test, ExpectedException(typeof(ApplicationException))]
        public void ShouldFailOnNonZeroErrorCode()
        {
            string pathtocmd = Environment.GetEnvironmentVariable("windir") + @"\system32\cmd.exe";
            Task.Run.Executable(x=>x.ExecutablePath(pathtocmd).WithArguments("/c copy c:\\temp\\nothing.txt c:\\temp\\nothing2.txt"));
        }

        [Test]
        public void ShouldContinueOnNonZeroErrorCode()
        {
            string pathtocmd = Environment.GetEnvironmentVariable("windir") + @"\system32\cmd.exe";
           Task.Run.Executable(x=>x.ExecutablePath(pathtocmd).ContinueOnError.WithArguments("/c copy c:\\temp\\nothing.txt c:\\temp\\nothing2.txt"));
        }

        [Test]
        public void ShouldContinueOnErrorCodeOne()
        {
            string pathtocmd = Environment.GetEnvironmentVariable("windir") + @"\system32\cmd.exe";
            Task.Run.Executable(x=>x.ExecutablePath(pathtocmd).WithArguments("/c copy c:\\temp\\nothing.txt c:\\temp\\nothing2.txt").SucceedOnNonZeroErrorCodes());
        }
    }
}