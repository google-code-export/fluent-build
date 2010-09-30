using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;

namespace FluentBuild.Publishing
{
    //TODO: all fields are required
    //TODO: make this fluent
    //This code was adapted from the nant-googlecode project http://code.google.com/p/nant-googlecode/ 
    public class GoogleCode
    {
        private static readonly byte[] NewLineAsciiBytes = Encoding.ASCII.GetBytes("\r\n");
        private static readonly string Boundary = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets Google user name to authenticate as (this is just the username part, don't include the @gmail.com part.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the Google Code password (not the same as the gmail password).
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the Google Code project name to upload to.
        /// </summary>
        public string ProjectName { get; set; }

        /// <summary>
        /// Gets or sets the local path of the file to upload.
        /// </summary>
        public string LocalFileName { get; set; }

        /// <summary>
        /// Gets or sets the file name that this file will be given on Google Code.
        /// </summary>
        public string TargetFileName { get; set; }

        /// <summary>
        /// Gets or sets the summary of the upload.
        /// </summary>
        public string Summary { get; set; }

        public void Upload()
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(String.Format("https://{0}.googlecode.com/files", ProjectName));
            request.Method = "POST";
            request.ContentType = String.Concat("multipart/form-data; boundary=" + Boundary);
            request.UserAgent = String.Concat("Google Code Upload Nant Task ", Assembly.GetExecutingAssembly().GetName().Version.ToString());
            request.Headers.Add("Authorization", String.Concat("Basic ", CreateAuthorizationToken(UserName, Password)));

            using (Stream stream = request.GetRequestStream())
            {
               
                WriteLine(stream, String.Concat("--", Boundary));
                WriteLine(stream, @"content-disposition: form-data; name=""summary""");
                WriteLine(stream, "");
                WriteLine(stream, Summary);

               
                WriteLine(stream, String.Concat("--", Boundary));
                WriteLine(stream, String.Format(@"content-disposition: form-data; name=""filename""; filename=""{0}""", TargetFileName));
                WriteLine(stream, "Content-Type: application/octet-stream");
                WriteLine(stream, "");
                WriteFile(stream, LocalFileName);
                WriteLine(stream, "");
                WriteLine(stream, String.Concat("--", Boundary, "--"));
            }

            request.GetResponse();
        }

        /// <summary>
        /// Writes the specified file to the specified stream.
        /// </summary>
        internal void WriteFile(Stream outputStream, string fileToWrite)
        {
            if (outputStream == null)
                throw new ArgumentNullException("outputStream");

            if (fileToWrite == null)
                throw new ArgumentNullException("fileToWrite");

            using (FileStream fileStream = new FileStream(LocalFileName, FileMode.Open))
            {
                byte[] buffer = new byte[1024];
                int count;
                while ((count = fileStream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    outputStream.Write(buffer, 0, count);
                }
            }
        }

        /// <summary>
        /// Writes the string to the specified stream and concatenates a newline.
        /// </summary>
        internal static void WriteLine(Stream outputStream, string valueToWrite)
        {
            if (valueToWrite == null)
                throw new ArgumentNullException("valueToWrite");

            List<byte> bytesToWrite = new List<byte>(Encoding.ASCII.GetBytes(valueToWrite));
            bytesToWrite.AddRange(NewLineAsciiBytes);
            outputStream.Write(bytesToWrite.ToArray(), 0, bytesToWrite.Count);
        }

        /// <summary>
        /// Creates the authorization token.
        /// </summary>
        internal static string CreateAuthorizationToken(string username, string password)
        {
            if (username == null)
                throw new ArgumentNullException("username");

            if (password == null)
                throw new ArgumentNullException("password");

            byte[] authBytes = Encoding.ASCII.GetBytes(String.Concat(username, ":", password));
            return Convert.ToBase64String(authBytes);
        }
    }
}
