using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace FluentBuild.Tests.Samples.Run.StdOutStdError
{
    //this is used to test stdout vs. stderror functionality
    public class Program
    {
        public static string ErrorOutput = "error output";
        public static string NormalOutput = "normal output";
        public static string NormalFlag = "normal";
        public static string ErrorFlag = "error";

        public static void Main(string[] args)
        {
            if (args[0] == NormalFlag)
            {
                Console.WriteLine(NormalOutput);
            }
            else
            {
                TextWriter errorWriter = Console.Error;
                errorWriter.WriteLine(ErrorOutput);
            }
            Environment.Exit(1);
        }
    }
}
