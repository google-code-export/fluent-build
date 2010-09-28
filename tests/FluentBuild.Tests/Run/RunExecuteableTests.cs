using System;
using System.ComponentModel;
using FluentBuild.Core;
using NUnit.Framework;

namespace FluentBuild.Tests
{
    [TestFixture]
    public class RunExecuteableTests
    {
        [Test]
        public void ShouldRunCmdPrompt()
        {
            string pathtocmd = Environment.GetEnvironmentVariable("windir") + @"\system32\cmd.exe";
            Run.Executeable(pathtocmd).WithArguments(@"/K 'cd \'").Execute();
        }

        [Test, ExpectedException(typeof(Win32Exception))]
        public void ShouldFailIfFileDoesNotExist()
        {
            string pathtocmd = Environment.GetEnvironmentVariable("windir") + @"\afilethatdoesnotexist.exe";
            Run.Executeable(pathtocmd).Execute();
        }

        [Test]
        public void ShouldNotThrowErrorIfFileDoesNotExistAndContinueOnErrorIsSet()
        {
            string pathtocmd = Environment.GetEnvironmentVariable("windir") + @"\afilethatdoesnotexist.exe";
            Run.Executeable(pathtocmd).ContinueOnError.Execute();
        }
    }
}