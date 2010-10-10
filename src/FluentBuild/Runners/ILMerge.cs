using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using FluentBuild.Core;
using FluentBuild.Utilities;

namespace FluentBuild.Runners
{
    ///<summary>
    /// Merges assemblies together
    ///</summary>
    public class ILMerge
    {
        internal string Destination;
        internal IList<String> Sources;
        private string _exePath;
        private readonly IFileFinder _fileFinder;

        internal string[] BuildArgs()
        {
            var args = new List<String>();
            args.AddRange(Sources);
            args.Add("/OUT:" + Destination);

            //for some reson ILMerge does not work properly with 4.0 
            //so forcing the version and path to assemlies fixes this
            if (Defaults.FrameworkVersion.FriendlyName == FrameworkVersion.NET4_0.Full.FriendlyName)
                args.Add("/targetplatform:v4," + Defaults.FrameworkVersion.GetPathToFrameworkInstall());
            args.Add("/ndebug"); //no pdb generated
            return args.ToArray();
        }

        ///<summary>
        /// Executes the ILMerge assembly
        ///</summary>
        ///<exception cref="FileNotFoundException">If the path to the executeable was not set or can not be found automatically.</exception>
        public void Execute()
        {
            Run.Executeable(FindExecuteable()).WithArguments(BuildArgs()).Execute();
        }

        internal string FindExecuteable()
        {
            if (!string.IsNullOrEmpty(_exePath))
                return _exePath;

            var tmp =_fileFinder.Find("ILMerge.exe");

            if (tmp == null)
                throw new FileNotFoundException("Could not automatically find ILMerge.exe. Please specify it manually using ILMerge.ExecuteableLocatedAt");

            return tmp;
        }

        ///<summary>
        /// Sets the path to the ILMerge.exe executeable
        ///</summary>
        ///<param name="path">path to ILMerge.exe</param>
        public ILMerge ExecuteableLocatedAt(string path)
        {
            _exePath = path;
            return this;
        }

        internal ILMerge(IFileFinder fileFinder)
        {
            _fileFinder = fileFinder;
            Sources = new List<string>();
        }

        internal ILMerge() : this(new FileFinder())
        {
            
        }

        ///<summary>
        /// Sets the output location
        ///</summary>
        ///<param name="destination">path to output file</param>
        public ILMerge OutputTo(string destination)
        {
            Destination = destination;
            return this;
        }


        ///<summary>
        /// Sets the output location
        ///</summary>
        ///<param name="destination">path to output file</param>
        public ILMerge OutputTo(BuildArtifact destination)
        {
            return OutputTo(destination.ToString());
        }
        
        ///<summary>
        /// Adds a source file to merge in
        ///</summary>
        ///<param name="source">path to the file to merge in</param>
        public ILMerge AddSource(string source)
        {
            Sources.Add(source);
            return this;
        }

        ///<summary>
        /// Adds a source file to merge in
        ///</summary>
        ///<param name="source">path to the file to merge in</param>
        public ILMerge AddSource(BuildArtifact source)
        {
            return AddSource(source.ToString());
        }
    }
}
