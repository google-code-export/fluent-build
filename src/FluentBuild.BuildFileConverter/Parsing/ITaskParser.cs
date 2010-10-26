using System.Xml.Linq;

namespace FluentBuild.BuildFileConverter.Parsing
{
    public interface ITaskParser
    {
        void Parse(XElement data);
        string GererateString();
    }
}