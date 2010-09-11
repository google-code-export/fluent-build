using System;
using System.IO;

namespace FluentBuild.Tokenization
{
    public class TokenReplacer
    {
        private readonly IFileSystemWrapper _fileSystemWrapper;
        internal string Input;
        internal string Token;

        public TokenReplacer(string input) : this(new FileSystemWrapper(), input)
        {
        }

        public TokenReplacer(IFileSystemWrapper fileSystemWrapper, string input)
        {
            _fileSystemWrapper = fileSystemWrapper;
            Input = input;
        }

        public TokenWith ReplaceToken(string token)
        {
            Token = token;
            return new TokenWith(this);
        }

        public void To(string destination)
        {
            if (_fileSystemWrapper.FileExists(destination))
                throw new ApplicationException("File already exists. Delete it first");
            _fileSystemWrapper.WriteAllText(destination, Input);
        }

        public override string ToString()
        {
            return Input;
        }
    }
}