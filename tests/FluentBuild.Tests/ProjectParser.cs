using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using NUnit.Framework;

namespace FluentBuild.Tests
{
    public class ProjectParser
    {
        private readonly string _path;

        public ProjectParser(string path)
        {
            _path = path;
        }

        public IList<String> GetReferences()
        {
            var files = Directory.GetFiles(_path, "*.csproj");

            if (!files.Any())
                throw new FileNotFoundException("Could not find a project file in " + _path);
            
            var xDocument = XDocument.Load(files[0]);
            var ns = ((XElement)xDocument.FirstNode).Attribute("xmlns").Value;
            var elements = xDocument.Descendants("{" + ns + "}HintPath");
            
            if (!elements.Any())
            {
                throw new ApplicationException("Could not find a HintPath section for any references");
            }

            var references = new List<String>();

            foreach (var hintPath in elements)
            {
                if (hintPath.Value.Contains(@":\")) //if an absolute path (i.e. contains a drive letter)
                    references.Add(hintPath.Value);
                else
                    references.Add(Path.GetFullPath(Environment.CurrentDirectory + hintPath.Value)); //resolve the path
            }
            
            return references;
        }
    }
}
