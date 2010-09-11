using System;

namespace FluentBuild
{
    public class MessageLogger
    {
        //TODO: write tests for this
        public static bool ShowDebugMessages;

        public static void WriteHeader(string header)
        {
            Console.WriteLine(header);
        }

        public static void WriteDebugMessage(string message)
        {
            if (ShowDebugMessages)
                Write("DEBUG", message);
        }

        public static void Write(string type, string message)
        {
            string outputMessage = String.Format("[{0}] {1}", type, message);
            do
            {
                int length = 0;
                if (outputMessage.Length > Console.WindowWidth - 20)
                    length = Console.WindowWidth - 20;
                else
                    length = outputMessage.Length;

                //ugly indentation code. Better way to do it but brain hurts
                if (!outputMessage.Substring(0, length).Contains(String.Format("[{0}]", type)))
                {
                    Console.WriteLine("\t".PadRight(type.Length+4, ' ') + outputMessage.Substring(0, length));
                }
                else
                {
                    Console.WriteLine("\t" + outputMessage.Substring(0, length));
                }
                outputMessage = outputMessage.Substring(length);
            } while (outputMessage.Trim().Length > 0);
        }

        public static void BlankLine()
        {
            Console.WriteLine();
        }
    }
}
