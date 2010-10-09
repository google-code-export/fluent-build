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
        private string _destination;
        private IList<String> sources;
        private string _exePath;
        private readonly IFileFinder _fileFinder;


        ///<summary>
        /// Executes the ILMerge assembly
        ///</summary>
        ///<exception cref="FileNotFoundException">If the path to the executeable was not set or can not be found automatically.</exception>
        public void Execute()
        {
            var args = new List<String>();
            args.AddRange(sources);
            args.Add("/OUT:" + _destination);

            //for some reson ILMerge does not work properly with 4.0 
            //so forcing the version and path to assemlies fixes this
            if (Defaults.FrameworkVersion.FriendlyName == FrameworkVersion.NET4_0.Full.FriendlyName)
                args.Add("/targetplatform:v4," + Defaults.FrameworkVersion.GetPathToFrameworkInstall());
            args.Add("/ndebug"); //no pdb generated
        
            if (string.IsNullOrEmpty(_exePath))
                _exePath = _fileFinder.Find("ILMerge.exe");

            if (_exePath == null)
                throw new FileNotFoundException("Could not automatically find ILMerge.exe. Please specify it manually using ILMerge.ExecuteableLocatedAt");

            Run.Executeable(_exePath).WithArguments(args.ToArray()).Execute();
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
            sources = new List<string>();
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
            _destination = destination;
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
            sources.Add(source);
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
