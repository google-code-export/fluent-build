using System.IO;
using System.Xml.Linq;
using FluentBuild.BuildFileConverter.Parsing;
using FluentBuild.BuildFileConverter.Structure;

namespace FluentBuild.BuildFileConverter
{
    public class ConvertFile
    {
        private readonly string _pathToNantFile;
        private readonly string _pathToOutputFile;

        public ConvertFile(string pathToNantFile, string pathToOutputFile)
        {
            _pathToNantFile = pathToNantFile;
            _pathToOutputFile = pathToOutputFile;
        }

        public void Generate()
        {
            var parser = new NantBuildFileParser();
            BuildProject buildProject = parser.ParseDocument(XDocument.Load(_pathToNantFile));
            var outputGenerator = new OutputGenerator(buildProject);
            string output = outputGenerator.CreateOutput();
            using (var fs = new StreamWriter(_pathToOutputFile + "\\default.cs"))
            {
                fs.Write(output);
            }
        }
    }
}