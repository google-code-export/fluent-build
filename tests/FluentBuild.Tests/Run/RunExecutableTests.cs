using System;
using System.ComponentModel;
using FluentBuild.Runners;
using NUnit.Framework;

namespace FluentBuild.Tests.Run
{
    [TestFixture]
    public class RunExecutableTests :TestBase
    {
        //[Test]
        //public void ShouldRunCmdPrompt()
        //{
        //    Console.WriteLine("ShouldRunCmdPrompt");
        //    string pathtocmd = Environment.GetEnvironmentVariable("windir") + @"\system32\cmd.exe";
        //   Task.Run.Executable(x=>x.ExecutablePath(pathtocmd).WithArguments(@"/C 'cd \'"));
        //}

        [Test, ExpectedException(typeof(Win32Exception))]
        public void ShouldFailIfFileDoesNotExist()
        {
            Console.WriteLine("ShouldFailIfFileDoesNotExist");
            string pathtocmd = Environment.GetEnvironmentVariable("windir") + @"\afilethatdoesnotexist.exe";
            Task.Run.Executable(x=>x.ExecutablePath(pathtocmd));
        }

        [Test]
        public void ShouldNotThrowErrorIfFileDoesNotExistAndContinueOnErrorIsSet()
        {
            Console.WriteLine("ShouldNotThrowErrorIfFileDoesNotExistAndContinueOnErrorIsSet");
            string pathtocmd = Environment.GetEnvironmentVariable("windir") + @"\afilethatdoesnotexist.exe";
            Task.Run.Executable(x=>x.ExecutablePath(pathtocmd).ContinueOnError);
         //   Assert.Fail("This needs to be fixed");
        }

        [Test, ExpectedException(typeof(ApplicationException))]
        public void ShouldFailOnNonZeroErrorCode()
        {
            Console.WriteLine("ShouldFailOnNonZeroErrorCode");
            string pathtocmd = Environment.GetEnvironmentVariable("windir") + @"\system32\cmd.exe";
            Task.Run.Executable(x=>x.ExecutablePath(pathtocmd).WithArguments("/c copy c:\\temp\\nothing.txt c:\\temp\\nothing2.txt"));
        }

        [Test]
        public void ShouldSucceedOnNonZeroErrorCodeWhenSet()
        {
            Console.WriteLine("ShouldSucceedOnNonZeroErrorCodeWhenSet");
            string pathtocmd = Environment.GetEnvironmentVariable("windir") + @"\system32\cmd.exe";
            var errorCode = Task.Run.Executable(x => x.ExecutablePath(pathtocmd).WithArguments("/c copy c:\\temp\\nothing.txt c:\\temp\\nothing2.txt").SucceedOnNonZeroErrorCodes());
            Assert.That(errorCode, Is.Not.EqualTo(0));
        }

        [Test]
        public void ShouldContinueOnNonZeroErrorCode()
        {
            Console.WriteLine("ShouldContinueOnNonZeroErrorCode");
            string pathtocmd = Environment.GetEnvironmentVariable("windir") + @"\system32\cmd.exe";
            var errorCode = Task.Run.Executable(x=>x.ExecutablePath(pathtocmd).ContinueOnError.WithArguments("/c copy c:\\temp\\nothing.txt c:\\temp\\nothing2.txt"));
            Assert.That(errorCode,Is.Not.EqualTo(0));
        }

        [Test]
        public void ShouldContinueOnErrorCodeOne()
        {
            Console.WriteLine("ShouldContinueOnErrorCodeOne");
            string pathtocmd = Environment.GetEnvironmentVariable("windir") + @"\system32\cmd.exe";
            Task.Run.Executable(x=>x.ExecutablePath(pathtocmd).WithArguments("/c copy c:\\temp\\nothing.txt c:\\temp\\nothing2.txt").SucceedOnNonZeroErrorCodes());
        }
    }
}