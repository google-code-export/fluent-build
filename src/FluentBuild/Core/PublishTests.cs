using FluentBuild.Publishing;
using NUnit.Framework;

namespace FluentBuild.Core
{
    [TestFixture]
    public class PublishTests
    {
        [Test]
        public void ToGoogleCode()
        {
            Assert.That(Publish.ToGoogleCode, Is.TypeOf<GoogleCode>());
        }
    }
}