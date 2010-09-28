using System;

namespace FluentBuild.Compilation
{

    ///<summary>
    /// Represents a resource file
    ///</summary>
    internal class Resource
    {
        internal string FilePath { get; private set; }
        internal string Identifier { get; private set; }

        ///<summary>
        /// Creates a Resource for a file with a given identifier
        ///</summary>
        ///<param name="filePath">The path to the resource file</param>
        ///<param name="identifier">The identifier to be used during compilation</param>
        internal Resource(string filePath, string identifier)
        {
            this.FilePath = filePath;
            this.Identifier = identifier;
        }

        ///<summary>
        /// Creates a Resource for a file
        ///</summary>
        ///<param name="filePath">The path to the resource file</param>
        internal Resource(string filePath)
        {
            this.FilePath = filePath;
        }
        
        /// <summary>
        /// Creates a string representation of the path and identifier
        /// </summary>
        /// <returns>A string in the format of filePath[,identifier]</returns>
        public override string ToString()
        {
            if (Identifier == String.Empty)
                return FilePath;
            return string.Format("\"{0}\",{1}", FilePath, Identifier);
        }
    }
}