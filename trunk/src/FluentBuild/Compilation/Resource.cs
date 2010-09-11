using System;

namespace FluentBuild
{
    public class Resource
    {
        //TODO: test this
        public string Value { get; private set; }
        public string Name { get; private set; }

        public Resource(string value, string name)
        {
            this.Value = value;
            this.Name = name;
        }

        public Resource(string value)
        {
            this.Value = value;
        }

        
        
        public override string ToString()
        {
            if (Name == String.Empty)
                return Value;
            return string.Format("\"{0}\",{1}", Value, Name);
        }
    }
}