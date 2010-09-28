using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using FluentBuild.Core;
using ICSharpCode.SharpZipLib.Zip;

namespace FluentBuild.Runners.Zip
{
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
            if (String.IsNullOrEmpty(_file) && (String.IsNullOrEmpty(_path)))
                throw new ApplicationException("sourceFile OR sourceFolder must be set");

            if (!String.IsNullOrEmpty(_file) && (!String.IsNullOrEmpty(_path)))
                throw new ApplicationException("sourceFile OR sourceFolder must be set");

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