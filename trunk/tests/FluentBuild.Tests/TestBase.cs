using System;
using System.IO;
using NUnit.Framework;

namespace FluentBuild.Tests
{
    public class TestBase
    {
        protected string rootFolder;
        
        [SetUp]
        protected void SetupTestFolder()
        {
            string folder = Environment.GetEnvironmentVariable("TEMP");
            if (String.IsNullOrEmpty(folder))
                Assert.Fail("Need a temp directory to run this test");

            rootFolder = Path.Combine(folder, "FluentBuild");

            if (Directory.Exists(rootFolder))
                Directory.Delete(rootFolder, true);
            Directory.CreateDirectory(rootFolder);
        }
    }
}