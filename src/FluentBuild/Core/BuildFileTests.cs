using NUnit.Framework;

namespace FluentBuild.Core
{
    [TestFixture]
    public class BuildFileTests
    {
        [Test]
        public void TestThatQueueGetsProcessed()
        {
            var subject = new BuildFile();
            bool methodCalled = false;
            
            subject.AddTask(delegate { methodCalled = true; });
            Assert.That(subject.tasks.Count, Is.EqualTo(1));
            subject.InvokeNextTask();
            Assert.IsTrue(methodCalled);
            Assert.That(subject.tasks.Count, Is.EqualTo(0));
        }

    }
}