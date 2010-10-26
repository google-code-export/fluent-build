using System.Xml.Linq;
using NUnit.Framework;
using Rhino.Mocks;

namespace FluentBuild.BuildFileConverter.Parsing
{
    [TestFixture]
    public class TargetParserTests
    {
        private IParserResolver _resolver;
        private TargetParser _subject;
        private XElement _targetXml;

        [SetUp]
        public void Setup()
        {
            _resolver = MockRepository.GenerateStub<IParserResolver>();
            _subject = new TargetParser(_resolver);
            _resolver.Stub(x => x.Resolve("call")).Return(new UnkownTypeParser());
            _targetXml = XElement.Parse("<target name=\"basic\"><call target=\"mainbuild\"/></target>");   
        }

        [Test]
        public void ShouldParseTarget()
        {
            var target = _subject.Parse(_targetXml);
            
            Assert.That(target.Name, Is.EqualTo("basic"));
            Assert.That(target.Body, Is.EqualTo(_targetXml.ToString()));
        }

        [Test]
        public void ShouldResolveParser()
        {
            var target = _subject.Parse(_targetXml);
            _resolver.AssertWasCalled(x=>x.Resolve("call"));
        }


    }
}