using System;
using System.ComponentModel;
using NUnit.Framework;

namespace FluentBuild.Tests.Run
{
    [TestFixture]
    public class RunExecuteableTests :TestBase
    {
        [Test]
        public void ShouldRunCmdPrompt()
        {
            string pathtocmd = Environment.GetEnvironmentVariable("windir") + @"\system32\cmd.exe";
            Core.Run.Executeable(pathtocmd).WithArguments(@"/K 'cd \'").Execute();
        }

        [Test, ExpectedException(typeof(Win32Exception))]
        public void ShouldFailIfFileDoesNotExist()
        {
            string pathtocmd = Environment.GetEnvironmentVariable("windir") + @"\afilethatdoesnotexist.exe";
            Core.Run.Executeable(pathtocmd).Execute();
        }

        [Test]
        public void ShouldNotThrowErrorIfFileDoesNotExistAndContinueOnErrorIsSet()
        {
            string pathtocmd = Environment.GetEnvironmentVariable("windir") + @"\afilethatdoesnotexist.exe";
            Core.Run.Executeable(pathtocmd).ContinueOnError.Execute();
        }
    }
}