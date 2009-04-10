using System;
using System.IO;

namespace FluentBuild.Tokenization
{
    public class TokenReplacer
    {
        internal string Input;
        internal string Token;

        public TokenReplacer(string input)
        {
            Input = input;
        }

        public TokenWith ReplaceToken(string token)
        {
            Token = token;
            return new TokenWith(this);
        }

        public void To(string destination)
        {
            if (File.Exists(destination))
                throw new ApplicationException("File already exists. Delete it first");
            File.WriteAllText(destination, Input);
        }

        public override string ToString()
        {
            return Input;
        }
    }
}