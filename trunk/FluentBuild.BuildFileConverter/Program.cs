using System;

namespace FluentBuild.BuildFileConverter
{
    internal class Program
    {
        /// <summary>
        /// Not at all meant for the real world. Just here to help get started by converting others scripts
        /// </summary>
        /// <param name="args"></param>
        

        private static void Main(string[] args)
        {
            Console.WriteLine("Does some initial conversion work on an nAnt build file to a fluent build file.");
            Console.WriteLine();
            Console.WriteLine("WARNING!! This is not a production tool. It is not meant to be used for anything more than testing");
            Console.WriteLine();
            if (args.Length != 2)
            {
                Console.WriteLine("Usage: BuildFileConverter.exe pathToNantFile pathToOutputFile");
            }
            else
            {
                new ConvertFile(args[0], args[1]);
            }
            
        }
    }
}