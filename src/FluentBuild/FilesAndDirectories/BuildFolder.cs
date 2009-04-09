using System;
using System.IO;

namespace FluentBuild
{
    public class BuildFolder
    {
        private readonly string _path;

        public BuildFolder(string path)
        {
            _path = path;
        }

        public BuildFolder SubFolder(string path)
        {
            return new BuildFolder(Path.Combine(_path, path));
        }

        public BuildFolder RecurseAllSubFolders()
        {
            return new BuildFolder(_path + "\\**\\");
        }

        public BuildFolder Delete()
        {
            MessageLogger.Write("delete", _path);
            if (Directory.Exists(_path))
                Directory.Delete(_path, true);
            return this;
        }

        public BuildFolder Create()
        {
            MessageLogger.Write("Make Directory", _path);
            Directory.CreateDirectory(_path);
            return this;
        }

        public override string ToString()
        {
            return _path;
        }

        public BuildArtifact FileName(string name)
        {
            return new BuildArtifact(Path.Combine(_path, name));
        }
    }
}