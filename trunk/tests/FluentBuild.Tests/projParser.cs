using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using NUnit.Framework;

namespace FluentBuild.Tests
{
    [TestFixture]
    class projParser
    {
        [Test]
        public void ShouldFindProjectFile()
        {
            var files = Directory.GetFiles(@"C:\Projects\fluent-build\src\FluentBuild.BuildUI", "*.csproj");
            Assert.That(files.Count(), Is.GreaterThan(0));
        }

        [Test]
        public void ShouldOpenFile()
        {
            var files = Directory.GetFiles(@"C:\Projects\fluent-build\src\FluentBuild.BuildUI", "*.csproj");

            var xDocument = XDocument.Load(files[0]);
            var ns = ((XElement) xDocument.FirstNode).Attribute("xmlns").Value;
            var elements = xDocument.Descendants("{"+ns+"}HintPath");
            Assert.That(elements.Count(), Is.GreaterThan(0));
        }

        [Test]
        public void HintPathShouldGetResolved()
        {
            var files = Directory.GetFiles(@"C:\Projects\fluent-build\src\FluentBuild.BuildUI", "*.csproj");

            var xDocument = XDocument.Load(files[0]);
            var ns = ((XElement)xDocument.FirstNode).Attribute("xmlns").Value;
            var elements = xDocument.Descendants("{" + ns + "}HintPath");

            //need to internalize the compiler for fb.exe and fb.ui.exe to do this work. 

            //Path.GetFullPath("base" + "..\relative");
        }
    }
}
