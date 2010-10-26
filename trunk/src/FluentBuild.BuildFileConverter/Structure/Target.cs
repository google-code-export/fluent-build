using System;
using System.Collections.Generic;
using System.Text;
using FluentBuild.BuildFileConverter.Parsing;

namespace FluentBuild.BuildFileConverter.Structure
{
    public class Target
    {
        public String Name { get; set; }
        public string Body { get; set; }
        public IList<ITaskParser> Tasks { get; set; }

        public Target()
        {
            Tasks = new List<ITaskParser>();
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach (var taskParser in Tasks)
            {
                sb.AppendLine(taskParser.GererateString());
            }
            return sb.ToString();
        }
    }
}