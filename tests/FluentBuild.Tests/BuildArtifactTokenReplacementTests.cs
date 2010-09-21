using System.IO;
using FluentBuild.Core;
using NUnit.Framework;


namespace FluentBuild.Tests
{
    [TestFixture]
    public class BuildArtifactTokenReplacementTests : TestBase
    {
        #region Setup/Teardown

        [SetUp]
        public void SetUp()
        {
            SetupTestFolder();
            using (FileStream stream = File.Create(Path.Combine(rootFolder, "test.txt")))
            {
                using (var sw = new StreamWriter(stream))
                {
                    sw.WriteLine("Hello @LastName@, @FirstName@. How are you?");
                }
            }
        }

        #endregion

        [Test]
        public void ReplaceToken()
        {
            var artifact = new BuildArtifact(Path.Combine(rootFolder, "test.txt"));
            string destination = Path.Combine(rootFolder, "test2.txt");
            artifact.Copy.ReplaceToken("LastName").With("Smith").ReplaceToken("FirstName").With("John").To(destination);

            Assert.That(File.Exists(destination));
            string[] strings = File.ReadAllLines(destination);
            Assert.That(strings[0], Is.EqualTo("Hello Smith, John. How are you?"));
        }
    }
}