using System;
using System.Collections.Specialized;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace FluentBuild.BuildFileConverter
{
    public class ConvertFile
    {
        private readonly NameValueCollection _properties;
        private readonly NameValueCollection _tasks;
        private readonly StringBuilder _unknownContent;

        public ConvertFile(string pathToNantFile, string pathToOutputFile)
        {
            _properties = new NameValueCollection();
            _unknownContent = new StringBuilder();
            _tasks = new NameValueCollection();

            ParseFile(pathToNantFile);
            var output = CreateOutput();

            using (var fs = new StreamWriter(pathToOutputFile))
            {
                fs.Write(output);
            }
        }

        private string CreateOutput()
        {
            var sb = new StringBuilder();
            sb.AppendLine("using System;");
            sb.AppendLine("using FluentBuild;");
            sb.AppendLine();
            sb.AppendLine("namespace Build");
            sb.AppendLine("{");
            sb.AppendLine("\tpublic class Default : BuildFile");
            sb.AppendLine("\t{");
            foreach (string property in _properties.Keys)
            {
                var dataType = "string";
                if (property.Contains("dir") || property.Contains("path"))
                    dataType = "BuildFolder";
                if (_properties[property].LastIndexOf(".") == _properties[property].Length-4)
                    dataType = "BuildArtifact";

                sb.AppendFormat("\t\tprivate {0} {1};{2}", dataType, property, Environment.NewLine);
            }
            sb.AppendLine("\t\tpublic Default()");
            sb.AppendLine("\t\t{");
            foreach (string property in _properties.Keys)
            {
                sb.AppendFormat("\t\t\t{0}{1}", BuildPropertySetString(property, _properties[property]), Environment.NewLine);
            }

            sb.AppendLine();
            sb.Append("\t\t\t//TODO: these are generated in the order they are found in the file (which is probably not right)");
            sb.AppendLine();
            foreach (var key in _tasks.Keys)
            {
                sb.AppendFormat("\t\t\tAddTask({0});{1}", key, Environment.NewLine);
            }
            sb.AppendLine("\t\t}");

            foreach (string task in _tasks.Keys)
            {
                sb.AppendFormat("\t\tpublic void {0}(){1}", task, Environment.NewLine);
                sb.AppendLine("\t\t{");
                var replace = _tasks[task].Replace(">", ">\n");
                foreach (var line in replace.Split((char)(10)))
                {
                    sb.AppendFormat("\t\t\t//{0}{1}", line, Environment.NewLine);    
                }
                
                sb.AppendLine("\t\t}");
            }

            sb.AppendLine("\t}");//finish class
            sb.AppendLine("}");//finish namespace
            return sb.ToString();
        }

        private string BuildPropertySetString(string nameOfPropertyToSet, string valueToSet)
        {
            //check if the value contains a variable
            var regex = new Regex(@"\{[A-Za-z0-9_.]*\}");
            if (regex.IsMatch(valueToSet))
            {
                //What if the variable is "asdfom\${varname}\4ewgsa";
                //here there is a variable so we have to parse it out
                var indexOfEndOfVariable = valueToSet.IndexOf("}");
                var startIndex = valueToSet.IndexOf("{") +1;
                var variableName = valueToSet.Substring(startIndex, indexOfEndOfVariable-startIndex).Replace(".", "_");
                var data = valueToSet.Substring(indexOfEndOfVariable + 1);
                foreach (string property in _properties)
                {
                    //what about when a path is "{Variable}\nunit\nunit.exe"
                    if (variableName == property)
                    {
                        var valueForFunction = data;
                        //drop leading slash
                        if (valueForFunction.StartsWith("\\"))
                            valueForFunction = valueForFunction.Substring(1);

                        if (valueForFunction.StartsWith("/"))
                            valueForFunction = valueForFunction.Substring(1);

                        var methodToCall = "SubFolder";
                        //length must be greater than 3 so that it does not match folders like src. lib, etc,
                        //as LastIndexOf returns -1 if it can not find it
                        if ((valueForFunction.LastIndexOf(".") == valueForFunction.Length-4) && (valueForFunction.Length>3))
                        {
                            methodToCall = "File";
                        }

                        return String.Format("{0} = {1}.{2}(\"{3}\");", nameOfPropertyToSet, property,methodToCall, valueForFunction);
                    }
                }
            }
            return String.Format("{0} = \"{1}\";{2}", nameOfPropertyToSet, valueToSet, Environment.NewLine);
        }

        private void ParseFile(string pathToNantFile)
        {
            var doc = new XmlDocument();
            doc.Load(pathToNantFile);
            XmlElement projectNode = doc.DocumentElement; //get the project node
            foreach (XmlElement childNode in projectNode.ChildNodes)
            {
                if (childNode.Name == "property")
                {
                    _properties.Add(childNode.Attributes["name"].InnerText.Replace(".", "_"), childNode.Attributes["value"].InnerText);
                }
                else if (childNode.Name == "target")
                {
                    //may want to determine dependancies here
                    _tasks.Add(childNode.Attributes["name"].InnerText.Replace(".", "_"), childNode.InnerXml);
                }
                else
                {
                    _unknownContent.Append(childNode.OuterXml);
                }
            }
        }
    }
}