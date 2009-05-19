using System;
using System.Collections.Generic;
using System.Text;

namespace FluentBuild
{
    public class BuildTask
    {
        private readonly List<string> _references = new List<string>();
        private readonly List<string> _sources = new List<string>();
        internal readonly string compiler;
        private bool _includeDebugSymbols;
        private string _outputFileLocation;
        private Target _target;
        private readonly List<Resource> _resources = new List<Resource>();

        public BuildTask() : this("")
        {
        }

        protected internal BuildTask(string compiler)
        {
            _target = new Target(this);
            this.compiler = compiler;
        }

        public Target Target
        {
            get { return _target; }
            set { _target = value; }
        }

        protected internal string TargetType { get; set; }

        public BuildTask IncludeDebugSymbols
        {
            get
            {
                _includeDebugSymbols = true;
                return this;
            }
        }

        public BuildTask OutputFileTo(string outputFileLocation)
        {
            _outputFileLocation = outputFileLocation;
            return this;
        }

        public BuildTask OutputFileTo(BuildArtifact artifact)
        {
            return OutputFileTo(artifact.ToString());
        }

        public BuildTask AddRefences(params string[] fileNames)
        {
            _references.AddRange(fileNames);
            return this;
        }

        public BuildTask AddRefences(params BuildArtifact[] artifact)
        {
            foreach (BuildArtifact buildArtifact in artifact)
            {
                _references.Add(buildArtifact.ToString());
            }
            return this;
        }

        public BuildTask AddResource(string value, string resourceName)
        {
            _resources.Add(new Resource(value, resourceName));
            return this;
        }

        public BuildTask AddResources(FileSet fileSet)
        {
            foreach (var file in fileSet.Files)
            {
                _resources.Add(new Resource(file)); 
            }
            return this;
        }

        public void Execute()
        {
            string compilerWithoutExtentions = compiler.Substring(0, compiler.IndexOf("."));
            MessageLogger.Write(compilerWithoutExtentions, String.Format("Compiling {0} files to '{1}'", _sources.Count, _outputFileLocation));
            string compileMessage = "Compile Using: " + @"c:\Windows\Microsoft.NET\Framework\" + FrameworkVersion.frameworkVersion + "\\" + compiler + " " + Args.Replace("/", Environment.NewLine + "/");
            MessageLogger.WriteDebugMessage(compileMessage);
            Run.Executeable(@"c:\Windows\Microsoft.NET\Framework\" + FrameworkVersion.frameworkVersion + "\\" + compiler).WithArguments(Args).Execute(compilerWithoutExtentions);
            MessageLogger.WriteDebugMessage("Done Compiling");
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
                    resources.AppendFormat(" /resource:{0}", res.ToString());
                } 

                string args = String.Format("/out:\"{0}\" {1} /target:{2} {3} {4}", _outputFileLocation, resources, TargetType, references, sources);
                if (_includeDebugSymbols)
                    args += " /debug";

                return args;
            }
        }

        public BuildTask AddSources(FileSet fileset)
        {
            _sources.AddRange(fileset.Files);
            return this;
        }
    }
}