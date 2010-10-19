namespace FluentBuild.Tests.Build.Samples.Simple.C_
{
    public class Propgram
    {
        //entry point for if this is built as a console application
        private static void Main(string[] args)
        {
            Basic hello = new Basic();
            hello.Hello();
        }
    }
}