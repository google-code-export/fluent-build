using System;

namespace FluentBuild
{
    internal class MessageLogger
    {
        public static void Write(string type, string message)
        {
            Console.WriteLine(String.Format("   [{0}] {1}",type, message));
        }
    }
}