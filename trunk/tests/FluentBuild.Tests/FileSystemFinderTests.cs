using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;


namespace FluentBuild.Tests
{
    [TestFixture]
    public class FileSystemFinderTests
    {
        [Test]
        public void FindFile()
        {
            var finder = new FluentBuild.Utilities.FileSystemHelper();
            string find = finder.Find("MSBuild.exe", @"C:\Windows\Microsoft.NET\");
            Assert.That(find, Is.Not.Null);
        }
    }
}
