using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FluentBuild
{
    internal class CSharpAssemblyInfoBuilder : IAssemblyInfoBuilder
    {
       public string Build(AssemblyInfoDetails details)
       {
           var sb = new StringBuilder();
           details._imports.Sort();
           foreach (var import in details._imports)
           {
               sb.AppendFormat("using {0};{1}", import, Environment.NewLine);
           }

           if (details._comVisibleSet)
               sb.AppendFormat("[assembly: ComVisible({0})]{1}", details._comVisible, Environment.NewLine);

           if (details._clsCompliantSet)
               sb.AppendFormat("[assembly: ClsCompliant({0})]{1}", details._clsCompliant, Environment.NewLine);

           sb.AppendFormat("[assembly: AssemblyVersionAttribute(\"{0}\")]{1}", details._assemblyVersion, Environment.NewLine);
           sb.AppendFormat("[assembly: AssemblyTitleAttribute(\"{0}\")]{1}", details._assemblyTitle, Environment.NewLine);
           sb.AppendFormat("[assembly: AssemblyDescriptionAttribute(\"{0}\")]{1}", details._assemblyDescription, Environment.NewLine);
           sb.AppendFormat("[assembly: AssemblyCopyrightAttribute(\"{0}\")]{1}", details._assemblyCopyright, Environment.NewLine);
           //sb.AppendFormat("[assembly: ApplicationNameAttribute(\"{0}\")]{1}", details._applicationName, Environment.NewLine);
           sb.AppendFormat("[assembly: AssemblyCompany(\"{0}\")]{1}", details._company, Environment.NewLine);
           sb.AppendFormat("[assembly: AssemblyProduct(\"{0}\")]{1}", details._product, Environment.NewLine);
           return sb.ToString();
       }
    }
}
