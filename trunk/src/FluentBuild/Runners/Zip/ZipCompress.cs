using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using FluentBuild.Core;
using ICSharpCode.SharpZipLib.Zip;

namespace FluentBuild.Runners.Zip
{
    ///<summary>
    /// Compresses a file or folder
    ///</summary>
    public class ZipCompress
    {
        internal int CompressionLevel;
        private string _password;
        private string _file;
        private string _path;

        internal ZipCompress()
        {
        }

        ///<summary>
        /// Determine the compression level to use from 1(lowest) to 9(highest)
        ///</summary>
        public OneThroughNine UsingCompressionLevel { get { return new OneThroughNine(this);} }

        ///<summary>
        /// Sets the password to protect the zip file with
        ///</summary>
        ///<param name="password">The password to use</param>
        
        public ZipCompress UsingPassword(string password)
        {
            _password = password;
            return this;
        }

        ///<summary>
        /// Sets ZipCompress to compress a single file
        ///</summary>
        ///<param name="file">The file to compress</param>
        public ZipCompress SourceFile(BuildArtifact file)
        {
            return SourceFile(file.ToString());
        }

        ///<summary>
        /// Sets ZipCompress to compress a single file
        ///</summary>
        ///<param name="file">The file to compress</param>
        public ZipCompress SourceFile(string file)
        {
            _file = file;
            return this;
        }

        ///<summary>
        /// Sets ZipCompress to compress an entire folder
        ///</summary>
        ///<param name="path">The folder to compress</param>
        public ZipCompress SourceFolder(BuildFolder path)
        {
            return SourceFolder(path.ToString());
        }

        ///<summary>
        /// Sets ZipCompress to compress an entire folder
        ///</summary>
        ///<param name="path">The folder to compress</param>
        public ZipCompress SourceFolder(string path)
        {
            _path = path;
            return this;
        }

        private IList<String> GetFiles()
        {
            //only one should be set
            if (String.IsNullOrEmpty(_file) && (String.IsNullOrEmpty(_path)))
                throw new ArgumentException("sourceFile OR sourceFolder must be set");

            if (!String.IsNullOrEmpty(_file) && (!String.IsNullOrEmpty(_path)))
                throw new ArgumentException("sourceFile OR sourceFolder must be set");

            if (!string.IsNullOrEmpty(_file))
                return new List<String> {_file};

            return Directory.GetFiles(_path, "*.*", SearchOption.AllDirectories).ToList();
        }


        ///<summary>
        /// The location to place the output
        ///</summary>
        ///<param name="zipFilePath">path to the output file</param>
        public void To(BuildArtifact zipFilePath)
        {
            To(zipFilePath.ToString());
        }

        ///<summary>
        /// The location to place the output
        ///</summary>
        ///<param name="zipFilePath">path to the output file</param>
        public void To(string zipFilePath)
        {
            using (var zipOut = new ZipOutputStream(File.Create(zipFilePath)))
            {
                zipOut.SetLevel(CompressionLevel);
                zipOut.Password = _password;

                foreach (string fileName in GetFiles())
                {
                    var fileInfo = new FileInfo(fileName);
                    //strip of the base folder 
                    //this will keep folders preserved
                    var path = fileName.Replace(_path, "");
                    if (path.StartsWith("\\"))
                        path = path.Substring(1); //removes the leading \

                    var entry = new ZipEntry(path);
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