using System;
using System.Text;
using System.Xml.Linq;
using FluentBuild.BuildFileConverter.Structure;
using NUnit.Framework;

namespace FluentBuild.BuildFileConverter.Parsing
{
    [TestFixture]
    public class AsmInfoParserTests
    {
        #region Setup/Teardown

        [SetUp]
        public void SetUp()
        {
            _data = new StringBuilder();
            _data.AppendLine("<asminfo output=\"AssemblyInfo.cs\" language=\"CSharp\">");
            _data.AppendLine("<imports>");
            _data.AppendLine("<import namespace=\"System\" />");
            _data.AppendLine("<import namespace=\"System.Reflection\" />");
            _data.AppendLine("<import namespace=\"System.EnterpriseServices\" />");
            _data.AppendLine("<import namespace=\"System.Runtime.InteropServices\" />");
            _data.AppendLine("</imports>");
            _data.AppendLine("<attributes>");
            _data.AppendLine("<attribute type=\"ComVisibleAttribute\" value=\"false\" />");
            _data.AppendLine("<attribute type=\"CLSCompliantAttribute\" value=\"true\" />");
            _data.AppendLine("<attribute type=\"AssemblyVersionAttribute\" value=\"1.0.0.0\" />");
            _data.AppendLine("<attribute type=\"AssemblyTitleAttribute\" value=\"My fun assembly\" />");
            _data.AppendLine(
                "<attribute type=\"AssemblyDescriptionAttribute\" value=\"More fun than a barrel of monkeys\" />");
            _data.AppendLine(
                "<attribute type=\"AssemblyCopyrightAttribute\" value=\"Copyright (c) 2002, Monkeyboy, Inc.\" />");
            _data.AppendLine("<attribute type=\"ApplicationNameAttribute\" value=\"FunAssembly\" />");
            _data.AppendLine("</attributes>");
            _data.AppendLine("<references>");
            _data.AppendLine("<include name=\"System.EnterpriseServices.dll\" />");
            _data.AppendLine("</references>");
            _data.AppendLine("</asminfo>");
        }

        #endregion

        private StringBuilder _data;

        [Test]
        public void GenerateString()
        {
            var subject = new AsmInfoParser();
            subject.Parse(XElement.Parse(_data.ToString()), null);
            var expected = new StringBuilder();
            expected.AppendLine("AssemblyInfo.Language.CSharp");
            expected.AppendLine("\t.ComVisible(False)");
            expected.AppendLine("\t.ClsCompliant(True)");
            expected.AppendLine("\t.Copyright(\"Copyright (c) 2002, Monkeyboy, Inc.\")");
            expected.AppendLine("\t.Description(\"More fun than a barrel of monkeys\")");
            expected.AppendLine("\t.Title(\"My fun assembly\")");
            expected.AppendLine("\t.Version(\"1.0.0.0\")");
            expected.AppendLine("\t.OutputTo(\"AssemblyInfo.cs\");");
            Assert.That(subject.GererateString(), Is.EqualTo(expected.ToString()));
        }

        [Test]
        public void Parse()
        {
            var subject = new AsmInfoParser();
            subject.Parse(XElement.Parse(_data.ToString()), null);
            Assert.That(subject.ComVisible, Is.EqualTo(false));
            Assert.That(subject.ClsCompliant, Is.EqualTo(true));
            Assert.That(subject.Copyright, Is.EqualTo("Copyright (c) 2002, Monkeyboy, Inc."));
            Assert.That(subject.Description, Is.EqualTo("More fun than a barrel of monkeys"));
            //Assert.That(subject.Product, Is.EqualTo(true));
            Assert.That(subject.Title, Is.EqualTo("My fun assembly"));
            Assert.That(subject.Version, Is.EqualTo("1.0.0.0"));
            Assert.That(subject.OutputTo, Is.EqualTo("AssemblyInfo.cs"));
        }
    }

    public class AsmInfoParser : ITaskParser
    {
        public bool ComVisible { get; set; }

        public bool ClsCompliant { get; set; }

        public string Copyright { get; set; }

        public string Description { get; set; }

        public string Title { get; set; }

        public string Version { get; set; }

        public string OutputTo { get; set; }

        #region ITaskParser Members

        public void Parse(XElement data, BuildProject buildProject)
        {
            OutputTo = data.Attribute("output").Value;
            foreach (XElement element in data.Element("attributes").Elements())
            {
                switch (element.Attribute("type").Value)
                {
                    case "ComVisibleAttribute":
                        ComVisible = Convert.ToBoolean(element.Attribute("value").Value);
                        break;
                    case "CLSCompliantAttribute":
                        ClsCompliant = Convert.ToBoolean(element.Attribute("value").Value);
                        break;
                    case "AssemblyVersionAttribute":
                        Version = element.Attribute("value").Value;
                        break;
                    case "AssemblyTitleAttribute":
                        Title = element.Attribute("value").Value;
                        break;
                    case "AssemblyDescriptionAttribute":
                        Description = element.Attribute("value").Value;
                        break;
                    case "AssemblyCopyrightAttribute":
                        Copyright = element.Attribute("value").Value;
                        break;
                }
            }
        }

        public string GererateString()
        {
            var sb = new StringBuilder();
            sb.AppendLine("AssemblyInfo.Language.CSharp");
            sb.AppendFormat("\t.ComVisible({0}){1}", ComVisible, Environment.NewLine);
            sb.AppendFormat("\t.ClsCompliant({0}){1}", ClsCompliant, Environment.NewLine);
            sb.AppendFormat("\t.Copyright(\"{0}\"){1}", Copyright, Environment.NewLine);
            sb.AppendFormat("\t.Description(\"{0}\"){1}", Description, Environment.NewLine);
            sb.AppendFormat("\t.Title(\"{0}\"){1}", Title, Environment.NewLine);
            sb.AppendFormat("\t.Version(\"{0}\"){1}", Version, Environment.NewLine);
            sb.AppendFormat("\t.OutputTo(\"{0}\");{1}", OutputTo, Environment.NewLine);
            return sb.ToString();
        }

        #endregion
    }
}