using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentBuild.Core;

namespace FluentBuild.Runners
{
    public class ILMerge
    {
        private string _destination;
        private IList<String> sources;
        private string _exePath;

        public void Execute()
        {
            var args = new List<String>();
            args.AddRange(sources);
            args.Add("/OUT:" + _destination);
            args.Add("/lib:" + Defaults.FrameworkVersion.GetPathToFrameworkInstall());
            args.Add("/ndebug"); //nopdb
            //TODO autofind exepath
            Run.Executeable(_exePath).WithArguments(args.ToArray()).Execute();
        }

        public ILMerge ILMergeExecuteableLocatedAt(string path)
        {
            _exePath = path;
            return this;
        }

        public ILMerge()
        {
            sources = new List<string>();
        }

        public ILMerge OutputTo(string destination)
        {
            _destination = destination;
            return this;
        }

        public ILMerge OutputTo(BuildArtifact destination)
        {
            return OutputTo(destination.ToString());
        }
        
        public ILMerge AddSource(string source)
        {
            sources.Add(source);
            return this;
        }

        public ILMerge AddSource(BuildArtifact source)
        {
            return AddSource(source.ToString());
        }
    }
}
