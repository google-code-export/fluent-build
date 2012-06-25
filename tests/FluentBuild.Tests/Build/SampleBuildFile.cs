using System;
using NUnit.Framework;

namespace FluentBuild.Tests.Build
{
    internal class SampleBuildFile : BuildFile
    {
        public SampleBuildFile()
        {
            AddTask(Method1);
            ClearTasks();
            AddTask(Method2);
            AddTask(Crash);
            AddTask(Method3);
        }

        public bool Method1Run { get; set; }

        public bool Method3Run { get; set; }

        public bool Method2Run { get; set; }

        private void Method3()
        {
            Method3Run = true;
        }

        private void Method2()
        {
            Method2Run = true;
        }

        private void Crash()
        {
            SetErrorState();
        }

        private void Method1()
        {
            Method1Run = true;
        }
    }

    [TestFixture]
    public class SampleBuildFileTests
    {
        [Test]
        public void Start()
        {
            var file = new SampleBuildFile();
            file.InvokeNextTask();
            Assert.That(file.Method1Run, Is.False);
            Assert.That(file.Method2Run, Is.True);
            Assert.That(file.Method3Run, Is.False);
            Assert.That(Environment.ExitCode, Is.EqualTo(1));
        }
    }
}