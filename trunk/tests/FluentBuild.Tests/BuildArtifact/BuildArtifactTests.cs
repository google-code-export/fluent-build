﻿using System;
using System.IO;
using FluentBuild.Core;
using FluentBuild.Utilities;
using NUnit.Framework;

namespace FluentBuild.Tests
{
    [TestFixture]
    public class BuildArtifactTests : TestBase
    {
        private BuildArtifact _artifact;

        [SetUp]
        public void Setup()
        {
            _artifact = new FluentBuild.Core.BuildArtifact(rootFolder + "\\test.txt");
            File.Create(_artifact.ToString()).Dispose();
        }

        [Test]
        public void ShouldRenameFile()
        {
            _artifact.Rename.To("test2.txt");
            Assert.That(File.Exists(rootFolder + "\\test2.txt"));
        }

        [Test, ExpectedException(typeof(FileNotFoundException))]
        public void ShouldFailToRenameIfFileDoesNotExist()
        {
            File.Delete(_artifact.ToString());
            _artifact.Rename.FailOnError.To("test2.txt");
            Assert.That(File.Exists(rootFolder + "\\test2.txt"));
        }

        [Test]
        public void ShouldSucceedIfFileDoesNotExist()
        {
            File.Delete(_artifact.ToString());
            _artifact.Rename.ContinueOnError.To("test2.txt");
        }


        [Test]
        public void ShouldCopyFile()
        {
            _artifact.Copy.To(rootFolder + "\\test2.txt");
            Assert.That(File.Exists(rootFolder + "\\test.txt"));
            Assert.That(File.Exists(rootFolder + "\\test2.txt"));
        }

        [Test, ExpectedException(typeof(FileNotFoundException))]
        public void ShouldFailToCopyFileIfFileDoesNotExist()
        {
            File.Delete(_artifact.ToString());
            _artifact.Copy.FailOnError.To(rootFolder + "\\test2.txt");
        }

        [Test]
        public void ShouldNotThrowErrorIfFileDoesNotExist()
        {
            File.Delete(_artifact.ToString());
            _artifact.Copy.ContinueOnError.To(rootFolder + "\\test2.txt");
        }

        [Test]
        public void ShouldDeleteFile()
        {
            _artifact.Delete();
            Assert.That(!File.Exists(rootFolder + "\\test.txt"));
        }

        [Test, ExpectedException(typeof(IOException))]
        public void ShouldFailToDeleteFileIfFileInUse()
        {
            using (var fs = new StreamReader(_artifact.ToString()))
            {
                _artifact.Delete(OnError.Fail);
            }
        }

        [Test]
        public void ShouldNotThrowErrorIfFileInUse()
        {
            using (var fs = new StreamReader(_artifact.ToString()))
            {
                _artifact.Delete(OnError.Continue);
            }
        }
    }
}