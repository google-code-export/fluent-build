using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Rhino.Mocks;

namespace FluentBuild.Tokenization
{
    [TestFixture]
    public class TokenReplacerTests
    {
        [Test]
        public void Replace_ShouldReplaceToken()
        {
            const string input = "Hello @name@ how are you today?";
            const string name = "john";
            var replacement = new TokenReplacer(input);
            var results = replacement.ReplaceToken("name").With(name).ToString();
            Assert.That(results, Is.EqualTo("Hello john how are you today?"));
        }

        [Test]
        public void Replace_ShouldReplaceMultipleTokens()
        {
            const string input = "Hello @LastName@, @FirstName@ how are you today?";
            const string firstName = "John";
            const string lastName = "Smith";
            var replacement = new TokenReplacer(input);
            var results = replacement.ReplaceToken("FirstName").With(firstName).ReplaceToken("LastName").With(lastName).ToString();
            Assert.That(results, Is.EqualTo("Hello Smith, John how are you today?"));
        }

        [Test, ExpectedException(typeof(ApplicationException))]
        public void To_ShouldFailIfFileExists()
        {
            string destination = "c:\\temp";
            var fs = MockRepository.GenerateStub<IFileSystemWrapper>();
            fs.Stub(x => x.FileExists(destination)).Return(true);
            var replacer = new TokenReplacer(fs, "garbage");
            replacer.To(destination);
        }

        [Test]
        public void To_ShouldWriteOutFileIfFileDoesNotExist()
        {
            string destination = "c:\\temp\non.txt";
            string input = "garbage";

            var fs = MockRepository.GenerateStub<IFileSystemWrapper>();
            fs.Stub(x => x.FileExists(destination)).Return(false);
            
            var replacer = new TokenReplacer(fs, input);
            replacer.To(destination);

            fs.AssertWasCalled(x=>x.WriteAllText(destination, input));
        }

    }
}

