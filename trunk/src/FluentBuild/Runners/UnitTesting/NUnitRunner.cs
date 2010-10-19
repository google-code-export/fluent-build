using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using FluentBuild.Core;
using FluentBuild.MessageLoggers.MessageProcessing;
using FluentBuild.Utilities;

namespace FluentBuild.Runners.UnitTesting
{
    ///<summary>
    /// Runs nunit against an assembly
    ///</summary>
    public class NUnitRunner : Failable<NUnitRunner>
    {
        internal string _fileToTest;
        internal NameValueCollection _parameters;
        internal string _pathToConsoleRunner;
        internal string _workingDirectory;
        private IExecuteable _executable;
        private readonly IFileFinder _fileFinder;

        internal NUnitRunner(IExecuteable executeable, IFileFinder fileFinder)
        {
            _executable = executeable;
            _fileFinder = fileFinder;
            _parameters = new NameValueCollection();
        }

        internal NUnitRunner() : this (new Executeable(), new FileFinder())
        {

        }

        ///<summary>
        /// Sets the working directory
        ///</summary>
        ///<param name="path">The working directory for nunit-console</param>
        ///<returns></returns>
        public NUnitRunner WorkingDirectory(string path)
        {
            _workingDirectory = path;
            return this;
        }

        ///<summary>
        /// The assembly to run nunit against
        ///</summary>
        ///<param name="path">path to the assembly</param>
        ///<returns></returns>
        public NUnitRunner FileToTest(string path)
        {
            _fileToTest = path;
            return this;
        }

        ///<summary>
        /// The assembly to run nunit against
        ///</summary>
        ///<param name="buildArtifact">build artifact that represents the path to the assembly to test</param>
        ///<returns></returns>
        public NUnitRunner FileToTest(BuildArtifact buildArtifact)
        {
            return FileToTest(buildArtifact.ToString());
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

        ///<summary>
        /// Allows you to set the output to be an xml file.
        ///</summary>
        ///<param name="path">path for the output</param>
        ///<returns></returns>
        public NUnitRunner XmlOutputTo(string path)
        {
            return AddParameter("xml", path);
        }

        ///<summary>
        /// Adds a parameter for nunit
        ///</summary>
        ///<param name="name">The name of the parameter</param>
        ///<param name="value">The value of the parameter</param>
        ///<returns></returns>
        public NUnitRunner AddParameter(string name, string value)
        {
            _parameters.Add(name, value);
            return this;
        }

        ///<summary>
        /// Adds a named parameter to nunit
        ///</summary>
        ///<param name="name">The name of the parameter</param>
        ///<returns></returns>
        public NUnitRunner AddParameter(string name)
        {
            _parameters.Add(name, string.Empty);
            return this;
        }


        internal string[] BuildArgs()
        {
            var args = new List<string> {_fileToTest};
            foreach (string key in _parameters.Keys)
            {
                if (_parameters[key]==string.Empty)
                    args.Add(string.Format("/{0}", key));
                else
                    args.Add(string.Format("/{0}:{1}", key, _parameters[key]));
                
            }
            args.Add("/nologo");
            args.Add("/nodots");
            args.Add("/xmlconsole");
            //args.Add("/labels"););
            return args.ToArray();
        }

        ///<summary>
        /// Attempts to find and then run nunit-console
        ///</summary>
        ///<exception cref="FileNotFoundException">Occurs when nunit-console.exe can not be found</exception>
        public void Execute()
        {
            if (String.IsNullOrEmpty(_pathToConsoleRunner))
            {
              _pathToConsoleRunner = _fileFinder.Find("nunit-console.exe");
                if (_pathToConsoleRunner == null)
                    throw new FileNotFoundException("Could not automatically find nunit-console.exe. Please specify it manually using NunitRunner.PathToNunitConsoleRunner");
            }

            var executeable = _executable.Executable(_pathToConsoleRunner).WithArguments(BuildArgs());
            //if (OnError == OnError.Fail)
            //    executeable = executeable.FailOnError;
            //else if (OnError == OnError.Continue)
            //    executeable = executeable.ContinueOnError;

            if (!String.IsNullOrEmpty(_workingDirectory))
                executeable = executeable.InWorkingDirectory(_workingDirectory);
            
            //don't throw an errors
            var returnCode = executeable.WithMessageProcessor(new NunitMessageProcessor()).Execute();

            //if it returned non-zero then just exit (as a test failed)
            if (returnCode != 0 && OnError == OnError.Fail)
            {
                Environment.Exit(1);
            }
        }

    }
}