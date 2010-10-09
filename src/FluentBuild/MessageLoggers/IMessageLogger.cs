namespace FluentBuild.MessageLoggers
{
    internal interface IMessageLogger
    {
        void WriteHeader(string header);
        void WriteDebugMessage(string message);
        void Write(string type, string message);
        void WriteError(string type, string message);
        void WriteWarning(string type, string message);
    }
}