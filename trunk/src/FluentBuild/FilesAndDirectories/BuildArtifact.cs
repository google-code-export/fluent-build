using System;
using System.IO;
using FluentBuild.Tokenization;

namespace FluentBuild
{
    public class BuildArtifact
    {
        private readonly string path;
        private object copy;

        public BuildArtifact(string path)
        {
            this.path = path;
        }

        public CopyBuildArtifcat Copy
        {
            get {
                return new CopyBuildArtifcat(this);
            }
        }


        public override string ToString()
        {
            return path;
        }
    }

    public class CopyBuildArtifcat
    {
        private readonly BuildArtifact source;

        public CopyBuildArtifcat(BuildArtifact artifact)
        {
            source = artifact;
        }

        public void To(String destination)
        {
            // c:\temp\test.txt --> c:\temp\dir1
            // c:\temp\test.txt --> c:\temp\dir1\test2.txt
            // c:\temp\test.txt --> c:\temp\test2.txt
            
            string destinationFileName;
            string destinationDirectory;
            //if no filename in destination then get it from the source
            if (!Path.HasExtension(destination))
            {
                destinationFileName = Path.GetFileName(source.ToString());
                destinationDirectory = destination;
            }
            else
            {
                destinationFileName = Path.GetFileName(destination);
                destinationDirectory = Path.GetDirectoryName(destination);
            }
            File.Copy(source.ToString(), Path.Combine(destinationDirectory, destinationFileName));    
        }

        public TokenWith ReplaceToken(string token)
        {
            return new TokenReplacer(File.ReadAllText(source.ToString())).ReplaceToken(token);
        }
    }
}