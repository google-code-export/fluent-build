using System.Collections.Specialized;
using System.ComponentModel;

namespace FluentBuild.ApplicationProperties
{
    
    public  class CommandLineProperties
    {
        private static readonly NameValueCollection _properties = new NameValueCollection();

        internal CommandLineProperties()
        {
        }

        internal NameValueCollection Properties
        {
            get { return _properties; }
        }

        public string GetProperty(string name)
        {
            return _properties[name];
        }


        public void Add(string name, string value)
        {
            _properties.Add(name, value);
        }
    }
}