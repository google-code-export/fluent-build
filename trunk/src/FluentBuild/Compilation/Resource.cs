using System;

namespace FluentBuild
{
    public class Resource
    {
        //TODO: test this
        private readonly string name;
        private readonly string value;

        public Resource(string value, string name)
        {
            this.value = value;
            this.name = name;
        }

        public Resource(string value)
        {
            this.value = value;
        }

        public string Value
        {
            get { return value; }
        }

        public string Name
        {
            get { return name; }
        }

        public override string ToString()
        {
            if (Name == String.Empty)
                return Value;
            return string.Format("\"{0}\",{1}", Value, Name);
        }
    }
}