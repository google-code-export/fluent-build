using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FluentBuild.FilesAndDirectories.FileSet
{
    //Filesets often become created in code so this allows the mocking of filesets
    ///<summary>
    /// Creates a Fileset
    ///</summary>
    internal interface IFileSetFactory
    {
        ///<summary>
        /// Builds a fileset
        ///</summary>
        ///<returns>a fileset</returns>
        Core.IFileSet Create();
    }

    ///<summary>
    /// Generates a new fileset
    ///</summary>
    internal class FileSetFactory : IFileSetFactory
    {
        public Core.IFileSet Create()
        {
            return new Core.FileSet();
        }
    }
}
