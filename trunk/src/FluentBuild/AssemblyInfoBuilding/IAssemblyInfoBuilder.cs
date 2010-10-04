namespace FluentBuild.AssemblyInfoBuilding
{
    internal interface IAssemblyInfoBuilder
    {
        string Build(AssemblyInfoDetails details);
    }
}