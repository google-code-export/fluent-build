using System;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace FluentBuild
{
    [TestFixture]
    public class ResourceTests
    {
        [Test]
        public void CreateShouldBuildProperly()
        {
            var res = new Resource("value", "name");
            Assert.That(res.Name, Is.EqualTo("name"));
            Assert.That(res.Value, Is.EqualTo("value"));
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