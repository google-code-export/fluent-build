using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using FluentBuild.Core;
using ICSharpCode.SharpZipLib.Zip;
using NUnit.Framework;

namespace FluentBuild.Runners.Zip
{
    [TestFixture]
    public class ZipCompressTests
    {

        [Test]
        public void TestSomething()
        {
            var bothStringsAreSet = true ^ true;
            var firstStringIsSet = true ^ false;
            var secondStringIsSet = false ^ true;
            var noStringsAreSet = false ^ false;

            Assert.IsFalse(bothStringsAreSet, "Both strings are set");
            Assert.IsFalse(noStringsAreSet, "no strings are set");
            Assert.IsTrue(firstStringIsSet, "first string is set");
            Assert.IsTrue(secondStringIsSet, "second string is set");
        }
    }

    public class ZipCompress
    {
        internal int compressionLevel;
        private string _password;
        private string _file;
        private string _path;

        internal ZipCompress()
        {
        }

        public OneThroughNine UsingCompressionLevel { get { return new OneThroughNine(this);} }

        public ZipCompress UsingPassword(string password)
        {
            _password = password;
            return this;
        }

        public ZipCompress SourceFile(BuildArtifact file)
        {
            return SourceFile(file.ToString());
        }

        public ZipCompress SourceFile(string file)
        {
            _file = file;
            return this;
        }

        public ZipCompress SourceFolder(BuildFolder path)
        {
            return SourceFolder(path.ToString());
        }

        public ZipCompress SourceFolder(string path)
        {
            _path = path;
            return this;
        }

        private IList<String> GetFiles()
        {
            //only one should be set
            if (String.IsNullOrEmpty(_file) ^ (String.IsNullOrEmpty(_path)))
                throw new ApplicationException("Either sourceFile OR sourceFolder must be set. Both can not be set and neither can be left empty.");

            if (!string.IsNullOrEmpty(_file))
                return new List<String> {_file};

            return Directory.GetFiles(_path).ToList();
        }


        public void To(BuildArtifact zipFilePath)
        {
            To(zipFilePath.ToString());
        }

        public void To(string zipFilePath)
        {
            using (var zipOut = new ZipOutputStream(File.Create(zipFilePath)))
            {
                foreach (string fileName in GetFiles())
                {
                    var fileInfo = new FileInfo(fileName);
                    var entry = new ZipEntry(fileInfo.Name);
                    FileStream sReader = File.OpenRead(fileName);
                    var buff = new byte[Convert.ToInt32(sReader.Length)];
                    sReader.Read(buff, 0, (int) sReader.Length);
                    entry.DateTime = fileInfo.LastWriteTime;
                    entry.Size = sReader.Length;
                    sReader.Close();
                    zipOut.PutNextEntry(entry);
                    zipOut.Write(buff, 0, buff.Length);
                }
                zipOut.Finish();
                zipOut.Close();
            }
        }

    }
}