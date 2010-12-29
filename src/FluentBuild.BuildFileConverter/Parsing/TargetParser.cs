using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using FluentBuild.BuildFileConverter.Structure;

namespace FluentBuild.BuildFileConverter.Parsing
{
    public class TargetParser
    {
        private readonly IParserResolver _resolver;
        private readonly ITargetRepository _targetRepository;

        public TargetParser(IParserResolver resolver, ITargetRepository targetRepository)
        {
            _resolver = resolver;
            _targetRepository = targetRepository;
        }

        public TargetParser() : this(new ParserResolver(), new TargetRepository())
        {
            
        }

        public Target Parse(XElement element)
        {
            var target = new Target();
            target.Name = element.Attribute("name").Value.Replace(".", "_");
            target.Body = element.ToString();

            if (element.Attribute("depends") != null)
            {
                foreach (var name in Regex.Split(element.Attribute("depends").Value, "[ ,]"))
                {
                    if (name.Trim().Length > 0 && name != ",")
                        target.DependsOn.Add(_targetRepository.Resolve(name));
                }
            }

            foreach (var childNode in element.Elements())
            {
                var parser = _resolver.Resolve(childNode.Name.ToString());
                parser.Parse(childNode);
                target.Tasks.Add(parser);
            }

            _targetRepository.Register(target);
            return target;
        }
    }
}