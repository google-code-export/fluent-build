using System.Xml;
using System.Xml.Linq;
using FluentFs.Core;
using NUnit.Framework;

namespace FluentBuild.Publishing.NuGet
{
    [TestFixture]
    public class NuGetPublisherTests : TestBase
    {
        private NuGet.NuGetPublisher _subject;
        private NuGetOptionals _nuGetOptionals;

        [SetUp]
        public void SetUp()
        {
            _subject = new NuGetPublisher();
            _nuGetOptionals = _subject.DeployFolder(new Directory("somedir")).ProjectId("FluentBuild").Version("1.2.3.4").Description("Project 1").Authors("author1").ApiKey("123");
        }

        [Test]
        public void TestMandatoryFields()
        {
            Assert.That(_subject._deployFolder.ToString(), Is.EqualTo("somedir"));
            Assert.That(_subject._projectId, Is.EqualTo("FluentBuild"));
            Assert.That(_subject._version, Is.EqualTo("1.2.3.4"));
            Assert.That(_subject._description, Is.EqualTo("Project 1"));
            Assert.That(_subject._authors, Is.EqualTo("author1"));
            Assert.That(_subject._apiKey, Is.EqualTo("123"));
        }

        [Test]
        public void XML_TestAllFields()
        {
            var doc = new XmlDocument();
        
            var schema = _subject.CreateSchema();
            var xdoc = XDocument.Parse(schema);
            var ns = xdoc.Root.Name.Namespace;
            var metadata = xdoc.Element(ns + "package").Element(ns + "metadata");

            Assert.That(metadata.Element(ns + "id").Value, Is.EqualTo("FluentBuild"));
            
        }
    }
}