using System;
using System.ComponentModel;
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
            Core.Run.Executable(pathtocmd).WithArguments(@"/K 'cd \'").Execute();
        }

        [Test, ExpectedException(typeof(Win32Exception))]
        public void ShouldFailIfFileDoesNotExist()
        {
            string pathtocmd = Environment.GetEnvironmentVariable("windir") + @"\afilethatdoesnotexist.exe";
            Core.Run.Executable(pathtocmd).Execute();
        }

        [Test]
        public void ShouldNotThrowErrorIfFileDoesNotExistAndContinueOnErrorIsSet()
        {
            string pathtocmd = Environment.GetEnvironmentVariable("windir") + @"\afilethatdoesnotexist.exe";
            Core.Run.Executable(pathtocmd).ContinueOnError.Execute();
        }

        [Test, ExpectedException(typeof(ApplicationException))]
        public void ShouldFailOnNonZeroErrorCode()
        {
            string pathtocmd = Environment.GetEnvironmentVariable("windir") + @"\system32\cmd.exe";
            Core.Run.Executable(pathtocmd).WithArguments("/c copy c:\\temp\\nothing.txt c:\\temp\\nothing2.txt").Execute();
        }
    }
}