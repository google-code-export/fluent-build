using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using FluentBuild.Utilities;

namespace FluentBuild.Runners.UnitTesting
{
    public class MSTestRunner: FailableInternalExecutable<MSTestRunner>
    {
        protected internal string testContainer;
        protected internal string testMetadata;
        protected internal string runConfig;
        protected internal string resultsFile;
        protected internal bool unique;
        protected internal string[] test;
        protected internal string[] testList;
        protected internal string pathToConsoleRunner;
        protected internal string workingDirectory;
        private IExecutable _executable;

        public MSTestRunner(IExecutable executable)
        {
            _executable = executable;
        }

        public MSTestRunner() : this(new Executable())
        {
        }

        public MSTestRunner WorkingDirectory(string path)
        {
            workingDirectory = path;
            return this;
        }

        public MSTestRunner WorkingDirectory(FluentFs.Core.File path)
        {
            return WorkingDirectory(path.ToString());
        }

        public MSTestRunner TestContainer(string path)
        {
            this.testContainer = path;
            return this;
        }

        public MSTestRunner TestContainer(FluentFs.Core.File path)
        {
            return TestContainer(path.ToString());
        }

        public MSTestRunner TestMetadata(string path)
        {
            this.testMetadata = path;
            return this;
        }

        public MSTestRunner TestMetadata(FluentFs.Core.File path)
        {
            return TestMetadata(path.ToString());
        }

        public MSTestRunner Test(params string[] testNames)
        {
            this.test = testNames;
            return this;
        }

        public MSTestRunner TestList(params string[] testLists)
        {
            this.testList = testLists;
            return this;
        }

        public MSTestRunner RunConfig(string path)
        {
            this.runConfig = path;
            return this;
        }

        public MSTestRunner RunConfig(FluentFs.Core.File path)
        {
            return RunConfig(path.ToString());
        }

        public MSTestRunner ResultsFile(string path)
        {
            this.resultsFile = path;
            return this;
        }

        public MSTestRunner ResultsFile(FluentFs.Core.File path)
        {
            return ResultsFile(path.ToString());
        }

        public MSTestRunner Unique()
        {
            this.unique = true;
            return this;
        }

        public MSTestRunner PathToConsoleRunner(string path)
        {
            this.pathToConsoleRunner = path;
            return this;
        }

        public MSTestRunner PathToConsoleRunner(FluentFs.Core.File path)
        {
            this.pathToConsoleRunner = path.ToString();
            return this;
        }

        internal void AddArgsFromArray(string[] items, string prefix, List<string> args)
        {
            if (items == null || items.Count() == 0)
                return;
                
            foreach (var arg in items)
                {
                    args.Add("/"+prefix+":" + arg);
                }
            
        }

        internal string[] BuildArgs()
        {
            var args = new List<string>();
            if (!string.IsNullOrEmpty(this.testContainer))
                args.Add("/testcontainer:" + testContainer);

            if (!string.IsNullOrEmpty(testMetadata))
            {
                    args.Add("/testmetadata:" + testMetadata);
            }

            AddArgsFromArray(testList, "testList", args);
            AddArgsFromArray(test, "test", args);

            if (!string.IsNullOrEmpty(this.runConfig))
                args.Add("/runconfig:" + runConfig);

            if (!string.IsNullOrEmpty(this.resultsFile))
                args.Add("/resultsfile:" + resultsFile);

            if (this.unique)
                args.Add("/unique");

            args.Add("/nologo");
            return args.ToArray();
        }


        internal override void InternalExecute()
        {
            if (String.IsNullOrEmpty(pathToConsoleRunner))
                throw new FileNotFoundException("Could not automatically find mstest.exe. Please specify it manually using PathToConsoleRunner");

            var executable = _executable.ExecutablePath(pathToConsoleRunner).WithArguments(BuildArgs());
            if (!String.IsNullOrEmpty(workingDirectory))
                executable = executable.InWorkingDirectory(workingDirectory);
            
            //don't throw any errors
            //.WithMessageProcessor()
            var returnCode = executable.Execute();
            //if it returned non-zero then just exit (as a test failed)
            if (returnCode != 0 && base.OnError == OnError.Fail)
            {
                BuildFile.SetErrorState();
                Defaults.Logger.WriteError("ERROR", "MSTest returned non-zero error code");
            }
        }
    }
}
