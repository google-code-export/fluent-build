namespace FluentBuild
{
    public class Run
    {
        public static Executeable Executeable(string executeablePath)
        {
            return new Executeable(executeablePath);
        }

        public static Executeable Executeable(BuildArtifact executeablePath)
        {
            return new Executeable(executeablePath.ToString());
        }
    }
}