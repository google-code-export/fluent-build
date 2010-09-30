using System;
using System.Collections.Generic;
using System.Text;
using FluentBuild.Core;
using FluentBuild.Runners;

namespace FluentBuild.Compilation
{
    ///<summary>
    /// A task around builds that will execute a compiler to generate an assembly.
    ///</summary>
    public class BuildTask
    {
        private readonly List<string> _references = new List<string>();
        private readonly List<Resource> _resources = new List<Resource>();
        private readonly List<string> _sources = new List<string>();
        internal readonly string Compiler;
        private bool _includeDebugSymbols;
        private string _outputFileLocation;

        internal BuildTask() : this("")
        {
        }

        protected internal BuildTask(string compiler)
        {
            Target = new Target(this);
            Compiler = compiler;
        }

        /// <summary>
        /// Set the output file type
        /// </summary>
        public Target Target { get; set; }

        protected internal string TargetType { get; set; }

        /// <summary>
        /// Sets if Debug Symbols are generated. Defaults to False.
        /// </summary>
        public BuildTask IncludeDebugSymbols
        {
            get
            {
                _includeDebugSymbols = true;
                return this;
            }
        }

        internal string Args
        {
            get
            {
                var sources = new StringBuilder();
                foreach (string source in _sources)
                {
                    sources.Append(" \"" + source + "\"");
                }

                var references = new StringBuilder();
                foreach (string reference in _references)
                {
                    references.Append(" /reference:");
                    references.Append("\"" + reference + "\"");
                }

                var resources = new StringBuilder();
                foreach (Resource res in _resources)
                {
                    //res.ToString() does the work of converting the resource to a string
                    resources.AppendFormat(" /resource:{0}", res);
                }

                string args = String.Format("/out:\"{0}\" {1} /target:{2} {3} {4}", _outputFileLocation, resources,
                                            TargetType, references, sources);
                if (_includeDebugSymbols)
                    args += " /debug";

                return args;
            }
        }

        /// <summary>
        /// Sets the output file location
        /// </summary>
        /// <param name="outputFileLocation">The path to output the file to</param>
        /// <returns></returns>
        public BuildTask OutputFileTo(string outputFileLocation)
        {
            _outputFileLocation = outputFileLocation;
            return this;
        }

        /// <summary>
        /// Sets the output file location
        /// </summary>
        /// <param name="artifact">The BuildArtifact to output the file to</param>
        /// <returns></returns>
        public BuildTask OutputFileTo(BuildArtifact artifact)
        {
            return OutputFileTo(artifact.ToString());
        }

        /// <summary>
        /// Adds a reference to be included in the build
        /// </summary>
        /// <param name="fileNames">a param array of string paths to the reference</param>
        /// <returns></returns>
        public BuildTask AddRefences(params string[] fileNames)
        {
            _references.AddRange(fileNames);
            return this;
        }

        /// <summary>
        /// Adds a reference to be included in the build
        /// </summary>
        /// <param name="artifacts">a param array of BuildArtifacts to the reference</param>
        /// <returns></returns>
        public BuildTask AddRefences(params BuildArtifact[] artifacts)
        {
            foreach (BuildArtifact buildArtifact in artifacts)
            {
                _references.Add(buildArtifact.ToString());
            }
            return this;
        }

        /// <summary>
        /// Adds a single resource to be included in the build
        /// </summary>
        /// <param name="fileName">a resource file to include</param>
        /// <returns></returns>
        public BuildTask AddResource(string fileName)
        {
            AddResource(fileName, null);
            return this;
        }
        
        /// <summary>
        /// Adds a single resource to be included in the build
        /// </summary>
        /// <param name="fileName">a resource file to include</param>
        /// <param name="identifier">the identifier that code uses to refer to the resource</param>
        /// <returns></returns>
        public BuildTask AddResource(string fileName, string identifier)
        {
            _resources.Add(new Resource(fileName, identifier));
            return this;
        }

        ///<summary>
        /// Adds a fileset of resources to be included in the build
        ///</summary>
        ///<param name="fileSet">The fileset containing the resouces</param>
        ///<returns></returns>
        public BuildTask AddResources(FileSet fileSet)
        {
            foreach (string file in fileSet.Files)
            {
                _resources.Add(new Resource(file));
            }
            return this;
        }


        ///<summary>
        /// Executes the compliation with the provided parameters
        ///</summary>
        public void Execute()
        {
            string compilerWithoutExtentions = Compiler.Substring(0, Compiler.IndexOf("."));
            MessageLogger.Write(compilerWithoutExtentions,
                                String.Format("Compiling {0} files to '{1}'", _sources.Count, _outputFileLocation));
            string compileMessage = "Compile Using: " + 
                                    Defaults.FrameworkVersion.GetPathToFrameworkInstall() + "\\" + Compiler + " " +
                                    Args.Replace("/", Environment.NewLine + "/");
            MessageLogger.WriteDebugMessage(compileMessage);
            //necessary to cast currently as method is internal so can not be exposed via an interface
            var executeable =
                (Executeable)
                Run.Executeable(Defaults.FrameworkVersion.GetPathToFrameworkInstall() + "\\" +
                                Compiler).WithArguments(Args);
            executeable.Execute(compilerWithoutExtentions);
            MessageLogger.WriteDebugMessage("Done Compiling");
        }

        ///<summary>
        ///Adds in the source files to compile. This method is additive. It can be called multiple times without issue.
        ///</summary>
        ///<param name="fileset">A FileSet containing the files to be compiled.</param>
        ///<returns></returns>
        public BuildTask AddSources(FileSet fileset)
        {
            _sources.AddRange(fileset.Files);
            return this;
        }
    }
}