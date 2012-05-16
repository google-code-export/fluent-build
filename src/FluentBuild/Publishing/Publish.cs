using System;

namespace FluentBuild.Publishing
{
    public class Publish
    {
        public void ToGoogleCode(Action<GoogleCode> args)
        {
            var concrete = new GoogleCode();
            args(concrete);
            concrete.Execute();
        }

        public void Ftp(Action<Ftp> args)
        {
            var concrete = new Ftp();
            args(concrete);
            concrete.Execute();
        }
    }
}