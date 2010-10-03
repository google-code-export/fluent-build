namespace FluentBuild.Utilities.MessageProcessing
{
    internal interface IMessageProcessor
    {
        void Display(string prefix, string output, string error, int exitCode);
    }
}