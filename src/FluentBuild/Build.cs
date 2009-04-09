using System;
using System.Collections.Generic;
using System.Text;

namespace FluentBuild
{
    public class Build
    {
        private readonly string compiler;
        private bool _includeDebugSymbols;
        private string _outputFileLocation;
        private readonly List<string> _references = new List<string>();
        private readonly List<string> _sources = new List<string>();
        private Target _target;

        public Build() : this("")
        {
        }

        protected internal Build(string compiler)
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

        public Build IncludeDebugSymbols
        {
            get
            {
                _includeDebugSymbols = true;
                return this;
            }
        }

        public Build OutputFileTo(string outputFileLocation)
        {
            _outputFileLocation = outputFileLocation;
            return this;
        }

        public void ExcludeSource(string s)
        {
            throw new NotImplementedException();
        }

        public Build AddRefences(params string[] fileNames)
        {
            _references.AddRange(fileNames);
            return this;
        }

        public void Execute()
        {
            MessageLogger.Write(compiler, String.Format("Compiling {0} files to '{1}'", _sources.Count, _outputFileLocation));
            var sources = new StringBuilder();
            foreach (string source in _sources)
            {
                sources.Append(" " + source);
            }

            var references = new StringBuilder();
            foreach (string reference in _references)
            {
                references.Append(" /reference:");
                references.Append(reference);
            }

            string args = String.Format("/out:{1} /target:{2} {3} {0}", sources, _outputFileLocation, "library", references);
            if (_includeDebugSymbols)
                args += " /debug";
            ProcessUtility.StartProcess(@"c:\Windows\Microsoft.NET\Framework\v3.5\" + compiler, args);
        }

        public Build AddSources(FileSet fileset)
        {
            _sources.AddRange(fileset.Files);
            return this;
        }
    }
}