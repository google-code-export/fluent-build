namespace Build
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var build = new MainBuildTask();
            build.Execute();
        }
    }
}