﻿using System;
using System.IO;
using FluentBuild.Core;

namespace FluentBuild.BuildExe
{
    internal class CommandLineParser
    {
        public CommandLineParser(string[] args)
        {
            ClassToRun = "Default";
            //what about when build class is passed in
            if (Path.GetExtension(args[0]).ToLower() != "dll")
            {
                SourceBuild = true;
                PathToBuildSources = GetFullPathIfRelative(args[0]);
            }
            else
            {
                PathToBuildDll = GetFullPathIfRelative(args[0]);
            }

            for (int i = 1; i < args.Length; i++)
            {
                ParseArg(args[i]);
            }
        }

        public string PathToBuildDll { get; set; }
        public string PathToBuildSources { get; set; }
        public bool SourceBuild { get; private set; }
        public string ClassToRun { get; set; }

        public string GetFullPathIfRelative(string path)
        {
            //check if we have a full path like c:\temp
            if (path.IndexOf(":") == 1)
                return path;

            return Path.Combine(Environment.CurrentDirectory, path);
        }

        private void ParseArg(string arg)
        {
            arg = arg.Substring(1); //drop the preceeding - or / character
            string type = arg.Substring(0, arg.IndexOf(":")); //get the type
            string data = arg.Substring(arg.IndexOf(":") + 1); //get the value

            string name;
            string value;
            if (data.IndexOf("=") > 0)
            {
                name = data.Substring(0, data.IndexOf("="));
                value = data.Substring(data.IndexOf("=") + 1);
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
                    ClassToRun = data;
                    break;
                case "V":
                    DetermineVerbosity(data);
                    break;
                case "L":
                    Core.MessageLogger.SetLogger(data);
                    break;
                default:
                    throw new ArgumentException("Do not understand type");
            }
        }

        private void DetermineVerbosity(string data)
        {
            switch (data.ToUpper())
            {
                case "FULL":
                    MessageLogger.Verbosity = VerbosityLevel.Full;
                    break;
                case "NONE":
                    MessageLogger.Verbosity = VerbosityLevel.None;
                    break;
                case "TASKDETAILS":
                    MessageLogger.Verbosity = VerbosityLevel.TaskDetails;
                    break;
                case "TASKNAMESONLY":
                    MessageLogger.Verbosity = VerbosityLevel.TaskNamesOnly;
                    break;
                default:
                    Console.WriteLine("Could not determine verbosity level");
                    Environment.Exit(1);
                    break;
            }
        }
    }
}