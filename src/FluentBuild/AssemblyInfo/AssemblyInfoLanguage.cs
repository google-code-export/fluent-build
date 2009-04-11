namespace FluentBuild
{
    public class AssemblyInfoLanguage
    {
        public AssemblyInfoDetails CSharp
        {
            get { return new AssemblyInfoDetails(new CSharpAssemblyInfoBuilder());  }
        }

        public AssemblyInfoDetails VisualBasic
        {
            get { return new AssemblyInfoDetails(new VisualBasicAssemblyInfoBuilder()); }
        }
    }
}