using System;
using FluentBuild.Utilities;

namespace FluentBuild.Publishing
{
    public class Publish
    {
        internal readonly IActionExcecutor _excecutor;

        public Publish(IActionExcecutor excecutor)
        {
            _excecutor = excecutor;
        }

        public Publish() : this(new ActionExcecutor())
        {
        }

        public void ToGoogleCode(Action<GoogleCode> args)
        {
            _excecutor.Execute(args);
        }

        public void Ftp(Action<Ftp> args)
        {
            _excecutor.Execute(args);
        }
    }
}