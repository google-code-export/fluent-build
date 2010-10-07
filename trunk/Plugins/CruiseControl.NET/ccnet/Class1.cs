using System;
using Exortech.NetReflector;
using ThoughtWorks.CruiseControl.Core;

namespace ccnet.fluentbuild.plugin
{
    [ReflectorType("fluentbuild")]
    public class Class1 :ITask
    {
        [ReflectorProperty("executable", Required = true)] public string Executable;
        [ReflectorProperty("buildFile", Required = true)] public string BuildFile;
        [ReflectorProperty("baseDirectory", Required = false)] public string BaseDirectory;
        [ReflectorProperty("buildArgs", Required = false)] public string BuildArgs;
        [ReflectorProperty("target", Required = false)] public string Target;

        public void Run(IIntegrationResult result)
        {
            throw new NotImplementedException();
        }
    }
}
