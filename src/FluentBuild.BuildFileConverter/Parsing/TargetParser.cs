using System.Xml.Linq;
using FluentBuild.BuildFileConverter.Structure;

namespace FluentBuild.BuildFileConverter.Parsing
{
    public class TargetParser
    {
        private readonly IParserResolver _resolver;

        public TargetParser(IParserResolver resolver)
        {
            _resolver = resolver;
        }

        public TargetParser() : this(new ParserResolver())
        {
            
        }

        public Target Parse(XElement element)
        {
            var target = new Target();
            target.Name = element.Attribute("name").Value.Replace(".", "_");
            target.Body = element.ToString();
            
            foreach (var childNode in element.Elements())
            {
                var parser = _resolver.Resolve(childNode.Name.ToString());
                parser.Parse(childNode);
                target.Tasks.Add(parser);
            }
            return target;
        }
    }
}