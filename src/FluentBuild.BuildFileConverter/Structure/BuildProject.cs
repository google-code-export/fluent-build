using System.Collections.Generic;
using System.Collections.Specialized;

namespace FluentBuild.BuildFileConverter.Structure
{    
    public class BuildProject
    {
        public readonly Dictionary<string, Property> Properties;
        public readonly List<Target> Targets;
        public readonly NameValueCollection Unkown;

        public BuildProject()
        {
            Properties = new Dictionary<string, Property>();
            Targets = new List<Target>();
            Unkown = new NameValueCollection();
        }

        public void AddProperty(Property property)
        {
            Properties.Add(property.Name, property);
        }
    }
}