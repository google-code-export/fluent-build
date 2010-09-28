using System;
using FluentBuild.Core;

namespace FluentBuild.BuildExe
{
    internal class CommandLineParser
    {
        private string _fileToRun;
        private string _classToRun;

        public CommandLineParser(string[] args)
        {
            //what about when build class is passed in
            _fileToRun = args[0];
            for (int i = 1; i < args.Length; i++)
            {
                ParseArg(args[i]);
            }
        }

        private void ParseArg(string arg)
        {
            arg = arg.Substring(1); //drop the preceeding - or / character
            var type = arg.Substring(0, arg.IndexOf(":")); //get the type
            var data = arg.Substring(arg.IndexOf(":")); //get the value

            string name;
            string value;
            if (data.IndexOf("=") > 0)
            {
                name = data.Substring(0, data.IndexOf("="));
                value = data.Substring(data.IndexOf("="));
            }
            else
            {
                name = data;
                value = String.Empty;
            }
                
            switch (type.ToUpper())
            {
                case "P":
                    Properties.CommandLineProperties.Add(name, value);
                    break;
                case "C":
                    _classToRun = data;
                    break;
                default:
                    throw new ArgumentException("Do not understand type");
                    
            }
        }
    }
}