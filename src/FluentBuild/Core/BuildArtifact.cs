using FluentBuild.FilesAndDirectories;
using FluentBuild.Utilities;

namespace FluentBuild.Core
{

    public class BuildArtifact
    {
        private readonly IFileSystemWrapper _fileSystemWrapper;
        private readonly string _path;
        

        public BuildArtifact(string path) : this(new FileSystemWrapper(), path)
        {
        }

        public BuildArtifact(IFileSystemWrapper fileSystemWrapper, string path)
        {
            _fileSystemWrapper = fileSystemWrapper;
            _path = path;
        }

        public void Delete()
        {
            Delete(Defaults.OnError);
        }

        public void Delete(OnError onError)
        {
            OnErrorActionExecutor.DoAction(onError, _fileSystemWrapper.DeleteFile, _path);
        }

        public RenameBuildArtifact Rename
        {
            get { return new RenameBuildArtifact(this); }
        }

        public CopyBuildArtifcat Copy
        {
            get {
                return new CopyBuildArtifcat(this);
            }
        }

        public override string ToString()
        {
            return _path;
        }
    }
}