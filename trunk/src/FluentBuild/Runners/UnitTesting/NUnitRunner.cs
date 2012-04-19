using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using FluentBuild.Core;
using FluentBuild.MessageLoggers.MessageProcessing;
using FluentBuild.Utilities;

namespace FluentBuild.Runners.UnitTesting
{
    public interface INUnitRunner
    {
        ///<summary>
        /// Sets the working directory
        ///</summary>
        ///<param name="path">The working directory for nunit-console</param>
        ///<returns></returns>
        NUnitRunner WorkingDirectory(string path);

        ///<summary>
        /// The assembly to run nunit against
        ///</summary>
        ///<param name="path">path to the assembly</param>
        ///<returns></returns>
        NUnitRunner FileToTest(string path);

        ///<summary>
        /// The assembly to run nunit against
        ///</summary>
        ///<param name="File">build artifact that represents the path to the assembly to test</param>
        ///<returns></returns>
        NUnitRunner FileToTest(FluentFs.Core.File buildArtifact);

        /// <summary>
        /// Manually sets the path to nunit-console.exe. If this is not set then the runner will try and find the file on its own by searching the current folder and its subfolders.
        /// </summary>
        /// <param name="path">Path to nunit-console.exe</param>
        /// <returns></returns>
        NUnitRunner PathToNunitConsoleRunner(string path);

        ///<summary>
        /// Allows you to set the output to be an xml file.
        ///</summary>
        ///<param name="path">path for the output</param>
        ///<returns></returns>
        NUnitRunner XmlOutputTo(string path);

        ///<summary>
        /// Adds a parameter for nunit
        ///</summary>
        ///<param name="name">The name of the parameter</param>
        ///<param name="value">The value of the parameter</param>
        ///<returns></returns>
        NUnitRunner AddParameter(string name, string value);

        ///<summary>
        /// Adds a named parameter to nunit
        ///</summary>
        ///<param name="name">The name of the parameter</param>
        ///<returns></returns>
        NUnitRunner AddParameter(string name);

        NUnitRunner FailOnError { get; }
        NUnitRunner ContinueOnError { get; }
    }

    ///<summary>
    /// Runs nunit against an assembly
    ///</summary>
    public class NUnitRunner : Failable<NUnitRunner>, INUnitRunner
    {
        internal string _fileToTest;
        internal NameValueCollection _parameters;
        internal string _pathToConsoleRunner;
        internal string _workingDirectory;
        private IExecutable _executable;
        private readonly IFileFinder _fileFinder;

        internal NUnitRunner(IExecutable executable, IFileFinder fileFinder)
        {
            _executable = executable;
            _fileFinder = fileFinder;
            _parameters = new NameValueCollection();
        }

        public NUnitRunner() : this (new Executable(), new FileFinder())
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
        ///<param name="File">build artifact that represents the path to the assembly to test</param>
        ///<returns></returns>
        public NUnitRunner FileToTest(FluentFs.Core.File buildArtifact)
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
        [Obsolete("This is replaced by Tasks.Run.UnitTestFramework(args)", false)]
        public void Execute()
        {
            InternalExecute();    
        }

        internal void InternalExecute()
        {
            if (String.IsNullOrEmpty(_pathToConsoleRunner))
            {
              _pathToConsoleRunner = _fileFinder.Find("nunit-console.exe");
                if (_pathToConsoleRunner == null)
                    throw new FileNotFoundException("Could not automatically find nunit-console.exe. Please specify it manually using NunitRunner.PathToNunitConsoleRunner");
            }

            var executable = _executable.ExecutablePath(_pathToConsoleRunner);
            executable = executable.WithArguments(BuildArgs());
            executable = executable.SucceedOnNonZeroErrorCodes();

            //var executable = executablePath.SucceedOnNonZeroErrorCodes();
            //if (OnError == OnError.Fail)
            //    executable = executable.FailOnError;
            //else if (OnError == OnError.Continue)
            //    executable = executable.ContinueOnError;

            if (!String.IsNullOrEmpty(_workingDirectory))
                executable = executable.InWorkingDirectory(_workingDirectory);
            
            //don't throw an errors
            var returnCode = executable.WithMessageProcessor(new NunitMessageProcessor()).Execute();

            //if it returned non-zero then just exit (as a test failed)
            if (returnCode != 0 && OnError == OnError.Fail)
            {
                BuildFile.SetErrorState();
                Defaults.Logger.WriteError("ERROR", "Nunit returned non-zero error code");
            }
        }

    }
}