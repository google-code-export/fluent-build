using System.Collections.Generic;
using NUnit.Framework;

namespace FluentBuild.Tests.BuildExeTexts
{
    [TestFixture]
    public class ProgramTests
    {
        public class Sample
        {
            public void DoStuff()
            {
                
            }

            public void DoStuff2()
            {
                
            }
        }

        [Test]
        public void ShouldFindAllMethods()
        {
            Assert.That(BuildExe.Program.DoAllMethodsExistInType(typeof (Sample), new List<string>() {"DoStuff", "DoStuff2"}), Is.True);
        }


        [Test]
        public void ShouldNotFindAllMethods()
        {
            Assert.That(BuildExe.Program.DoAllMethodsExistInType(typeof(Sample), new List<string>() { "DoStuff", "DoStuff2", "NonexistantMethod" }), Is.False);
        }

    }
}