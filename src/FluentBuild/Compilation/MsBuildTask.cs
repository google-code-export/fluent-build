using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace FluentBuild.Compilation
{
    public class MsBuildTask
    {
        private readonly string _projectOrSolutionFilePath;
        private IList<string> _targets;
        private NameValueCollection _properties;
        private string _outdir;
        private string _configuration;

        //TODO: testing
        //TODO: overloads to take BuildFolder/Artifact

        public MsBuildTask(string projectOrSolutionFilePath)
        {
            _projectOrSolutionFilePath = projectOrSolutionFilePath;
            _targets = new List<string>();
            _properties = new NameValueCollection();
        }


        public MsBuildTask AddTarget(string target)
        {
            _targets.Add(target);
            return this;
        }

        
        public MsBuildTask SetProperty(string name, string value)
        {
            _properties.Add(name, value);
            return this;
        }
        //actually a property. Just making it a bit easier to consume common items
        public MsBuildTask OutDir(string path)
        {
            //output dir must have trailing slash. Might as well do it for the user
            _outdir = path;
            if (!_outdir.Trim().EndsWith("\\"))
                _outdir += "\\";
            return this;
        }

        //actually a property. Just making it a bit easier to consume common items
        //might want to have Configuration.Debug .Release .Custom("myconfig")
        public MsBuildTask Configuration(string configuration)
        {
            _configuration = configuration;
            return this;
        }


        private string[] BuildArgs()
        {
            var args = new List<String>();
            args.Add(_projectOrSolutionFilePath);
            if (_outdir != string.Empty )
                args.Add("/p:OutDir=" + _outdir);
            if (_configuration != string.Empty)
                args.Add("/p:Configuration=" + _configuration);

                foreach (var target in _targets)
                {
                    args.Add("/target:" + target);
                }
            return args.ToArray();
        }

        public void Execute()
        {
            var pathToMsBuild = Environment.GetEnvironmentVariable("windir") + @"\Microsoft.Net\Framework\" + FrameworkVersion.frameworkVersion + @"\MsBuild.exe";
            Run.Executeable(pathToMsBuild).WithArguments(BuildArgs()).Execute();
        }

          
    }
}