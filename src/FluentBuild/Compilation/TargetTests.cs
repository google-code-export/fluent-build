using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;


namespace FluentBuild.Compilation
{
    ///<summary />	[TestFixture]
    public class TargetTests
    {
        private BuildTask _buildTask;
        private Target _target;

        ///<summary />	[SetUp]
        public void Setup()
        {
            _buildTask = new BuildTask(null);
            _target = new Target(_buildTask);
        }

        ///<summary />	[Test]
        public void CreateExe()
        {
            BuildTask buildTask = _target.Executable;
            Assert.That(buildTask, Is.Not.Null);
            Assert.That(buildTask.TargetType, Is.EqualTo("exe"));
        }

        ///<summary />	[Test]
        public void CreateLibrary()
        {
            BuildTask buildTask = _target.Library;
            Assert.That(buildTask, Is.Not.Null);
            Assert.That(buildTask.TargetType, Is.EqualTo("library"));
        }

        ///<summary />	[Test]
        public void CreateModule()
        {
            BuildTask buildTask = _target.Module;
            Assert.That(buildTask, Is.Not.Null);
            Assert.That(buildTask.TargetType, Is.EqualTo("module"));
        }

        ///<summary />	[Test]
        public void CreateWindowsExe()
        {
            BuildTask buildTask = _target.WindowsExecutable;
            Assert.That(buildTask, Is.Not.Null);
            Assert.That(buildTask.TargetType, Is.EqualTo("winexe"));
        }
    }
}
