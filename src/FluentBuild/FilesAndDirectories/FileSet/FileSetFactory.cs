using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FluentBuild.FilesAndDirectories.FileSet
{
    //Filesets often become created in code so this allows the mocking of filesets
    public interface IFileSetFactory
    {
        Core.IFileSet Create();
    }

    public class FileSetFactory : IFileSetFactory
    {
        public Core.IFileSet Create()
        {
            return new Core.FileSet();
        }
    }
}
