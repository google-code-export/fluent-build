using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;
using NAnt.Core;

namespace FluentBuild.BuildFileConverter
{
    public class NantConverterStub
    {
        private readonly string _pathToNantFile;
        private readonly string _pathToOutputFile;

        public NantConverterStub(string pathToNantFile, string pathToOutputFile)
        {
            _pathToNantFile = pathToNantFile;
            _pathToOutputFile = pathToOutputFile;
        }

        public void Parse()
        {
            var doc = new XmlDocument();
            doc.Load(_pathToNantFile);
            var project = new NAnt.Core.Project(doc, Level.Warning, 0);
            var method = project.GetType().GetMethod("InitializeProjectDocument", BindingFlags.NonPublic | BindingFlags.Instance);
            method.Invoke(project, new object[] {doc});

            var targetCollection = project.Targets;
            foreach (Target target in targetCollection)
            {   
                Console.WriteLine(target.Name + " " + target.Description);
                //target.DependsListString 
                //target.Properties is a list of DictionaryEntries that has all the properties the build file has defined (i.e. dir_compile, dir_tools, etc.)
                Console.WriteLine(target.GetType().FullName);
                break;
            }
        }
    }
}
