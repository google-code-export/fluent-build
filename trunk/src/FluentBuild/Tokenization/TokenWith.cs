using System;
using FluentBuild.Tokenization;

namespace FluentBuild
{
    public class TokenWith
    {
        private readonly TokenReplacer replacer;

        public TokenWith(TokenReplacer replacer)
        {
            this.replacer = replacer;
        }

        public TokenReplacer With(string value)
        {
            replacer.Input = replacer.Input.Replace(String.Format("@{0}@", replacer.Token), value);
            return replacer;
        }
    }
}