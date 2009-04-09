using System;
using System.Collections.Generic;
using System.Text;

namespace FluentBuild
{
    public class AssemblyInfo
    {
        private readonly List<String> _imports = new List<string>();
        private string _applicationName;
        private string _assemblyCopyright;
        private string _assemblyDescription;
        private string _assemblyTitle;
        private string _assemblyVersion;
        private bool _clsCompliant;
        private bool _clsCompliantSet;
        private bool _comVisible;
        private bool _comVisibleSet;
        private string _company;
        private string _productName;

        public AssemblyInfo Import(params string[] files)
        {
            _imports.AddRange(files);
            return this;
        }

        public AssemblyInfo AssemblyProduct(string productName)
        {
            _productName = productName;
            return this;
        }

        public AssemblyInfo AssemblyCompany(string company)
        {
            _company = company;
            return this;
        }

        public AssemblyInfo ComVisible(bool visible)
        {
            _comVisibleSet = true;
            _comVisible = visible;
            return this;
        }

        public AssemblyInfo ClsCompliant(bool compliant)
        {
            _clsCompliantSet = true;
            _clsCompliant = compliant;
            return this;
        }

        public AssemblyInfo AssemblyVersion(string value)
        {
            _assemblyVersion = value;
            return this;
        }

        public AssemblyInfo AssemblyTitle(string value)
        {
            _assemblyTitle = value;
            return this;
        }


        public AssemblyInfo AssemblyDescription(string value)
        {
            _assemblyDescription = value;
            return this;
        }


        public AssemblyInfo AssemblyCopyright(string value)
        {
            _assemblyCopyright = value;
            return this;
        }


        public AssemblyInfo ApplicationName(string value)
        {
            _applicationName = value;
            return this;
        }

        //TODO implement other language support + have this external
        public void OutputTo(string filePath)
        {
            var sb = new StringBuilder();
            foreach (var import in _imports)
            {
                sb.AppendFormat("using {0};{1}", import, Environment.NewLine);
            }

            if (_comVisibleSet)
                sb.AppendFormat("[assembly: ComVisible({0})]{1}", _comVisible, Environment.NewLine);

            if (_clsCompliantSet)
                sb.AppendFormat("[assembly: ClsCompliant({0})]{1}", _clsCompliant, Environment.NewLine);

            sb.AppendFormat("[assembly: AssemblyVersionAttribute(\"{0}\")]{1}", _assemblyVersion, Environment.NewLine);
            sb.AppendFormat("[assembly: AssemblyTitleAttribute(\"{0}\")]{1}", _assemblyTitle, Environment.NewLine);
            sb.AppendFormat("[assembly: AssemblyDescriptionAttribute(\"{0}\")]{1}", _assemblyDescription, Environment.NewLine);
            sb.AppendFormat("[assembly: AssemblyCopyrightAttribute(\"{0}\")]{1}", _assemblyCopyright, Environment.NewLine);
            sb.AppendFormat("[assembly: ApplicationNameAttribute(\"{0}\")]{1}", _applicationName, Environment.NewLine);
            sb.AppendFormat("[assembly: ApplicationCompanyAttribute(\"{0}\")]{1}", _company , Environment.NewLine);
            sb.AppendFormat("[assembly: ApplicationProductNameAttribute(\"{0}\")]{1}", _productName, Environment.NewLine);

            Console.WriteLine(sb.ToString());
        }


    }
}