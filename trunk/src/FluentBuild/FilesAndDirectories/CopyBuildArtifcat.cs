using System;
using System.IO;
using FluentBuild.Tokenization;

namespace FluentBuild
{
    public class CopyBuildArtifcat
    {
        private readonly BuildArtifact source;

        public CopyBuildArtifcat(BuildArtifact artifact)
        {
            source = artifact;
        }
        //TODO: test these overloads
        public void To(BuildArtifact artifactDestination)
        {
            To(artifactDestination.ToString());    
        }

        public void To(BuildFolder folderDestination)
        {
            To(folderDestination.ToString());
        }


        public void To(String destination)
        {
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
            
// ReSharper disable AssignNullToNotNullAttribute
            string dest = Path.Combine(destinationDirectory, destinationFileName);
// ReSharper restore AssignNullToNotNullAttribute
            MessageLogger.WriteDebugMessage("Copy from " + source + " to " + dest);
            
            File.Copy(source.ToString(), dest);    
        }

        public TokenWith ReplaceToken(string token)
        {
            return new TokenReplacer(File.ReadAllText(source.ToString())).ReplaceToken(token);
        }
    }
}