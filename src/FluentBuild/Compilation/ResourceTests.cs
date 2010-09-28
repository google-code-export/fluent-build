using System;
using NUnit.Framework;


namespace FluentBuild.Compilation
{
    [TestFixture]
    public class ResourceTests
    {
        [Test]
        public void CreateShouldBuildProperly()
        {
            var res = new Resource("value", "name");
            Assert.That(res.Identifier, Is.EqualTo("name"));
            Assert.That(res.FilePath, Is.EqualTo("value"));
        }

        [Test]
        public void IfEmptyNameThenJustValueShouldBeReturned()
        {
            var res = new Resource("value", String.Empty);
            Assert.That(res.ToString(), Is.EqualTo("value"));
        }

        [Test]
        public void ShouldCreateQuotedString()
        {
            var res = new Resource("value", "name");
            Assert.That(res.ToString(), Is.EqualTo("\"value\",name"));
        }
    }
}