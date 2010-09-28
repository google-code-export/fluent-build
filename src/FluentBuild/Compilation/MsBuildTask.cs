using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using FluentBuild.Core;
using FluentBuild.Runners;

namespace FluentBuild.Compilation
{
    public class MsBuildTask
    {
        internal readonly string _projectOrSolutionFilePath;
        private readonly IExecuteable _executeable;
        internal readonly NameValueCollection _properties;
        internal readonly IList<string> _targets;
        internal string _configuration;
        internal string _outdir;
        
        //TODO: overloads to take BuildFolder/Artifact

        public MsBuildTask(string projectOrSolutionFilePath, IExecuteable executeable)
        {
            _projectOrSolutionFilePath = projectOrSolutionFilePath;
            _executeable = executeable;
            _targets = new List<string>();
            _properties = new NameValueCollection();
        }

        public MsBuildTask(string projectOrSolutionFilePath) : this(projectOrSolutionFilePath, new Executeable())
        {
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
            return this;
        }

        //actually a property. Just making it a bit easier to consume common items
        //might want to have Configuration.Debug .Release .Custom("myconfig")
        public MsBuildTask Configuration(string configuration)
        {
            _configuration = configuration;
            return this;
        }


        internal string[] BuildArgs()
        {
            var args = new List<String>();
            args.Add(_projectOrSolutionFilePath);

            if (!String.IsNullOrEmpty(_outdir))
            {
                var tempDir = _outdir;
                if (!tempDir.Trim().EndsWith("\\"))
                    tempDir += "\\";

                args.Add("/p:OutDir=" + tempDir);
            }
            if (!String.IsNullOrEmpty(_configuration))
                args.Add("/p:Configuration=" + _configuration);

            foreach (string target in _targets)
            {
                args.Add("/target:" + target);
            }
            return args.ToArray();
        }

        public void Execute()
        {
            string pathToMsBuild = Environment.GetEnvironmentVariable("windir") + @"\Microsoft.Net\Framework\" +
                                   Defaults.FrameworkVersion.FullVersion + @"\MsBuild.exe";
            
            _executeable.Executable(pathToMsBuild).WithArguments(BuildArgs()).Execute();
        }
    }
}

