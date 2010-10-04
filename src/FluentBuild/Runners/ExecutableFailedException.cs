using System;

namespace FluentBuild.Runners
{
    public class ExecutableFailedException : Exception
    {
        public ExecutableFailedException(string message) : base(message)
        {
            
        }


    }
}