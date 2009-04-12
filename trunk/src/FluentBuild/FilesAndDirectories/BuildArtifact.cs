namespace FluentBuild
{
    public class BuildArtifact
    {
        private readonly string path;

        public BuildArtifact(string path)
        {
            this.path = path;
        }

        public CopyBuildArtifcat Copy
        {
            get {
                return new CopyBuildArtifcat(this);
            }
        }

        public override string ToString()
        {
            return path;
        }
    }
}