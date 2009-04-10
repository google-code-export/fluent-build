using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

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


    }
}

