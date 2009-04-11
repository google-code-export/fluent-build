namespace FluentBuild
{
    internal interface IAssemblyInfoBuilder
    {
        string Build(AssemblyInfoDetails details);
    }
}