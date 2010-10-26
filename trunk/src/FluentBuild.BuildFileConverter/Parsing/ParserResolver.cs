namespace FluentBuild.BuildFileConverter.Parsing
{
    public interface IParserResolver
    {
        ITaskParser Resolve(string name);
    }

    public class ParserResolver : IParserResolver
    {
        public ITaskParser Resolve(string name)
        {
            switch (name)
            {
                //case "csc":
                //    return new CSCParser();
                //    break;
                default:
                    return new UnkownTypeParser();
            }
        }
    }
}