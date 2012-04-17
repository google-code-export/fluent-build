using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentBuild.Core;
using FluentBuild.Utilities;
using FluentFs.Core;
using NUnit.Framework;
using File = System.IO.File;

namespace FluentBuild.Tests.Build
{

    [TestFixture]
    public class FrameworkVersionTests : TestBase
    {
        [Test]
        public void TestFramework40_Full()
        {
            Defaults.FrameworkVersion = FrameworkVersion.NET4_0.Full;
            Build();
        }

        [Test]
        public void TestFramework40_Client()
        {
            Defaults.FrameworkVersion = FrameworkVersion.NET4_0.Client;
            Build();
        }

        [Test]
        public void TestFramework3_5()
        {
            Defaults.FrameworkVersion = FrameworkVersion.NET3_5;
            Build();
        }

        [Test]
        public void TestFramework3_0()
        {
            Defaults.FrameworkVersion = FrameworkVersion.NET3_0;
            Build();
        }

        [Test]
        public void TestFramework2_0()
        {
            Defaults.FrameworkVersion = FrameworkVersion.NET2_0;
            Build();
        }

        public void Build()
        {
            //Using Resource
            var sources = new FileSet();
            sources.Include(Settings.PathToSamplesFolder + "\\Build\\Simple\\C#\\*.cs");

            var outputFileLocation = rootFolder + "\\temp.dll";
            Task.Build(Using.Csc
                .Target.Library
                .AddSources(sources)
                .OutputFileTo(outputFileLocation));
            Assert.That(File.Exists(outputFileLocation));
        }
    }
}
