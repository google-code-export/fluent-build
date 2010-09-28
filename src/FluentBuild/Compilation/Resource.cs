using System;

namespace FluentBuild.Compilation
{
    public class Resource
    {
        public string FileName { get; private set; }
        public string Identifier { get; private set; }

        public Resource(string fileName, string identifier)
        {
            this.FileName = fileName;
            this.Identifier = identifier;
        }

        public Resource(string fileName)
        {
            this.FileName = fileName;
        }
        
        public override string ToString()
        {
            if (Identifier == String.Empty)
                return FileName;
            return string.Format("\"{0}\",{1}", FileName, Identifier);
        }
    }
}