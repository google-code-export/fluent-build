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
           Task.Run.Executable(new Executable(pathtocmd).WithArguments(@"/K 'cd \'"));
        }

        [Test, ExpectedException(typeof(Win32Exception))]
        public void ShouldFailIfFileDoesNotExist()
        {
            string pathtocmd = Environment.GetEnvironmentVariable("windir") + @"\afilethatdoesnotexist.exe";
            Task.Run.Executable(new Executable(pathtocmd));
        }

        [Test]
        public void ShouldNotThrowErrorIfFileDoesNotExistAndContinueOnErrorIsSet()
        {
            string pathtocmd = Environment.GetEnvironmentVariable("windir") + @"\afilethatdoesnotexist.exe";
           Task.Run.Executable(new Executable(pathtocmd).ContinueOnError);
        }

        [Test, ExpectedException(typeof(ApplicationException))]
        public void ShouldFailOnNonZeroErrorCode()
        {
            string pathtocmd = Environment.GetEnvironmentVariable("windir") + @"\system32\cmd.exe";
            Task.Run.Executable(new Executable(pathtocmd).WithArguments("/c copy c:\\temp\\nothing.txt c:\\temp\\nothing2.txt"));
        }

        [Test]
        public void ShouldContinueOnNonZeroErrorCode()
        {
            string pathtocmd = Environment.GetEnvironmentVariable("windir") + @"\system32\cmd.exe";
           Task.Run.Executable(new Executable(pathtocmd).ContinueOnError.WithArguments("/c copy c:\\temp\\nothing.txt c:\\temp\\nothing2.txt"));
        }

        [Test]
        public void ShouldContinueOnErrorCodeOne()
        {
            string pathtocmd = Environment.GetEnvironmentVariable("windir") + @"\system32\cmd.exe";
            Task.Run.Executable(new Executable(pathtocmd).WithArguments("/c copy c:\\temp\\nothing.txt c:\\temp\\nothing2.txt").SucceedOnNonZeroErrorCodes());
        }
    }
}