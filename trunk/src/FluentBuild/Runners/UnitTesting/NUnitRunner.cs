using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using FluentBuild.Core;
using FluentBuild.Utilities;

namespace FluentBuild.Runners.UnitTesting
{
    public class NUnitRunner : Failable<NUnitRunner>
    {
        internal string _fileToTest;
        internal NameValueCollection _parameters;
        internal string _pathToConsoleRunner;
        internal string _workingDirectory;
        private IExecuteable _executable;
        private readonly IFileFinder _fileFinder;

        public NUnitRunner(IExecuteable executeable, IFileFinder fileFinder)
        {
            _executable = executeable;
            _fileFinder = fileFinder;
            _parameters = new NameValueCollection();
        }

        public NUnitRunner() : this (new Executeable(), new FileFinder())
        {

        }

        public NUnitRunner WorkingDirectory(string path)
        {
            _workingDirectory = path;
            return this;
        }

        public NUnitRunner FileToTest(string path)
        {
            _fileToTest = path;
            return this;
        }


        /// <summary>
        /// Manually sets the path to nunit-console.exe. If this is not set then the runner will try and find the file on its own by searching the current folder and its subfolders.
        /// </summary>
        /// <param name="path">Path to nunit-console.exe</param>
        /// <returns></returns>
        public NUnitRunner PathToNunitConsoleRunner(string path)
        {
            _pathToConsoleRunner = path;
            return this;
        }

        public NUnitRunner XmlOutputTo(string path)
        {
            return AddParameter("xml", path);
        }

        public NUnitRunner AddParameter(string name, string value)
        {
            _parameters.Add(name, value);
            return this;
        }

        public NUnitRunner AddParameter(string name)
        {
            _parameters.Add(name, string.Empty);
            return this;
        }


        internal string[] BuildArgs()
        {
            var args = new List<string>();
            args.Add(_fileToTest);
            foreach (string key in _parameters.Keys)
            {
                if (_parameters[key]==string.Empty)
                    args.Add(string.Format("/{0}", key));
                else
                    args.Add(string.Format("/{0}:{1}", key, _parameters[key]));
                
            }
            args.Add("/nologo");
            args.Add("/nodots");
            //args.Add("/labels"););
            return args.ToArray();
        }

        public void Execute()
        {
            if (String.IsNullOrEmpty(_pathToConsoleRunner))
            {
              _pathToConsoleRunner = _fileFinder.Find("nunit-console.exe");
                if (_pathToConsoleRunner == null)
                    throw new FileNotFoundException("Could not automatically find nunit-console.exe. Please specify it manually using NunitRunner.PathToNunitConsoleRunner");
            }

            var executeable = _executable.Executable(_pathToConsoleRunner).WithArguments(BuildArgs());
            if (OnError == OnError.Fail)
                executeable = executeable.FailOnError;
            else if (OnError == OnError.Continue)
                executeable = executeable.ContinueOnError;

            if (!String.IsNullOrEmpty(_workingDirectory))
                executeable = executeable.InWorkingDirectory(_workingDirectory);
            executeable.Execute();
        }

        public NUnitRunner FileToTest(BuildArtifact buildArtifact)
        {
            return FileToTest(buildArtifact.ToString());
        }
    }
}