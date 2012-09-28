using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace FluentBuild.Tests.Run
{
    [TestFixture]
    class MSTests
    {
        [Test, ExpectedException(typeof(ApplicationException))]
        public void RunAllTests()
        {
            Task.Run.UnitTestFramework.MSTest(x => x.PathToConsoleRunner(@"C:\Program Files (x86)\Microsoft Visual Studio 10.0\Common7\IDE\mstest.exe")
                .TestContainer(@"C:\projects\fluent-build\FluentBuild.MSTests\bin\Debug\FluentBuild.MSTests.dll"));
        }

        [Test]
        public void RunOnlyPassingTest()
        {
            Task.Run.UnitTestFramework.MSTest(x => x.PathToConsoleRunner(@"C:\Program Files (x86)\Microsoft Visual Studio 10.0\Common7\IDE\mstest.exe")
                .TestContainer(@"C:\projects\fluent-build\FluentBuild.MSTests\bin\Debug\FluentBuild.MSTests.dll")
                .Test("FluentBuild.MSTests.BasicTests.SimplePass"));
        }

        [Test, ExpectedException(typeof(ApplicationException))]
        public void RunOnlyFailingTest()
        {
            Task.Run.UnitTestFramework.MSTest(x => x.PathToConsoleRunner(@"C:\Program Files (x86)\Microsoft Visual Studio 10.0\Common7\IDE\mstest.exe")
                .TestContainer(@"C:\projects\fluent-build\FluentBuild.MSTests\bin\Debug\FluentBuild.MSTests.dll")
                .Test("FluentBuild.MSTests.BasicTests.SimpleFail"));
        }

        [Test]
        public void RunOnlyIgnoredTest()
        {
            Task.Run.UnitTestFramework.MSTest(x => x.PathToConsoleRunner(@"C:\Program Files (x86)\Microsoft Visual Studio 10.0\Common7\IDE\mstest.exe")
                .TestContainer(@"C:\projects\fluent-build\FluentBuild.MSTests\bin\Debug\FluentBuild.MSTests.dll")
                .Test("FluentBuild.MSTests.BasicTests.IgnoredTest"));
        }

    }
}
