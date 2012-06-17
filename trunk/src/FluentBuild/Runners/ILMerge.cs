using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using FluentBuild.Core;
using FluentBuild.Utilities;

namespace FluentBuild.Runners
{
    public interface IILMerge
    {
        ///<summary>
        /// Sets the path to the ILMerge.exe executable
        ///</summary>
        ///<param name="path">path to ILMerge.exe</param>
        ILMerge ExecutableLocatedAt(string path);

        ///<summary>
        /// Sets the output location
        ///</summary>
        ///<param name="destination">path to output file</param>
        ILMerge OutputTo(string destination);

        ///<summary>
        /// Sets the output location
        ///</summary>
        ///<param name="destination">path to output file</param>
        ILMerge OutputTo(FluentFs.Core.File destination);

        ///<summary>
        /// Adds a source file to merge in
        ///</summary>
        ///<param name="source">path to the file to merge in</param>
        ILMerge AddSource(string source);

        ///<summary>
        /// Adds a source file to merge in
        ///</summary>
        ///<param name="source">path to the file to merge in</param>
        ILMerge AddSource(FluentFs.Core.File source);
    }

    ///<summary>
    /// Merges assemblies together
    ///</summary>
    public class ILMerge : IILMerge
    {
        internal string Destination;
        internal IList<String> Sources;
        private string _exePath;
        private readonly IFileSystemHelper _fileSystemHelper;
        private string _framework;

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
        ///<exception cref="FileNotFoundException">If the path to the executable was not set or can not be found automatically.</exception>
        [Obsolete("This has been replaced with Task.Run.ILMerge(args)")]
         public void Execute()
         {
            InternalExecute();
         }

        internal void InternalExecute()
        {
            Task.Run.Executable(x=>x.ExecutablePath(FindExecutable()).WithArguments(BuildArgs()));
        }

        internal string FindExecutable()
        {
            if (!string.IsNullOrEmpty(_exePath))
                return _exePath;

            var tmp =_fileSystemHelper.Find("ILMerge.exe");

            if (tmp == null)
                throw new FileNotFoundException("Could not automatically find ILMerge.exe. Please specify it manually using ILMerge.ExecutableLocatedAt");

            return tmp;
        }

        ///<summary>
        /// Sets the path to the ILMerge.exe executable
        ///</summary>
        ///<param name="path">path to ILMerge.exe</param>
        public ILMerge ExecutableLocatedAt(string path)
        {
            _exePath = path;
            return this;
        }

        internal ILMerge(IFileSystemHelper fileSystemHelper)
        {
            _fileSystemHelper = fileSystemHelper;
            Sources = new List<string>();
        }

        public ILMerge() : this(new FileSystemHelper())
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
        public ILMerge OutputTo(FluentFs.Core.File destination)
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
        public ILMerge AddSource(FluentFs.Core.File source)
        {
            return AddSource(source.ToString());
        }

        /*
        /// <summary>
        /// Sets the target platform for ILMerge to set the outputed assembly to
        /// </summary>
        /// <param name="framework"></param>
        /// <returns></returns>
        /// <examle>v4,c:\Windows\Microsoft.NET\Framework\v4.0.30319</examle>
        public ILMerge TargetPlatform(string framework)
        {
            _framework = framework;
            return this;
        }
       */
    }
}
