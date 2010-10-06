using System;

namespace FluentBuild.MessageLoggers
{
    internal class TeamCityMessageLogger : IMessageLogger
    {
        private static void WriteTeamCityMessage(string message)
        {
            WriteTeamCityMessage(message, string.Empty, "NORMAL");
        }

        private static void WriteTeamCityMessage(string message, string error, string type)
        {
            message = TeamCityEscapeCharacters(message);
            error = TeamCityEscapeCharacters(error);
            Console.WriteLine(String.Format("##teamcity[message text='{0}' errorDetails='{1}' status='{2}']", message, error, type));
        }

        private static string TeamCityEscapeCharacters(string data)
        {
            return data.Replace("|", "||").Replace("'", "|'").Replace("\n", "|n").Replace("\r", "|r").Replace("]", "|]");
        }

        private string _currentHeader;
        public  void WriteHeader(string header)
        {
            if (!String.IsNullOrEmpty(_currentHeader))
                Console.WriteLine(String.Format("##teamcity[blockClosed name='{0}']",_currentHeader));
            
            _currentHeader = header;
            Console.WriteLine(String.Format("##teamcity[blockOpened name='{0}']", header));

            //WriteTeamCityMessage(header);
        }

        public  void WriteDebugMessage(string message)
        {
            Write("DEBUG", message);
        }

        public  void Write(string type, string message)
        {
            string outputMessage = String.Format("[{0}] {1}", type, message);
            WriteTeamCityMessage(outputMessage);
        }

        public  void WriteError(string message)
        {
            WriteTeamCityMessage(message, message, "ERROR");
        }
    }
}