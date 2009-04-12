using System;
using System.Collections.Generic;
using System.IO;

namespace FluentBuild
{
    public class AssemblyInfoDetails
    {
        internal readonly List<String> _imports = new List<string>();
        internal readonly IAssemblyInfoBuilder AssemblyInfoBuilder;

        internal string _assemblyCopyright;
        internal string _assemblyDescription;
        internal string _assemblyTitle;
        internal string _assemblyVersion;
        internal bool _clsCompliant;
        internal bool _clsCompliantSet;
        internal bool _comVisible;
        internal bool _comVisibleSet;

        // internal string _applicationName;
        // internal string _company;
        // internal string _productName;
        //public AssemblyInfoDetails AssemblyProduct(string productName)
        //{
        //    _productName = productName;
        //    return this;
        //}

        //public AssemblyInfoDetails AssemblyCompany(string company)
        //{
        //    _company = company;
        //    return this;
        //}

        //public AssemblyInfoDetails ApplicationName(string value)
        //{
        //    _applicationName = value;
        //    return this;
        //}

        internal AssemblyInfoDetails(IAssemblyInfoBuilder assemblyInfoBuilder)
        {
            AssemblyInfoBuilder = assemblyInfoBuilder;
        }

        public AssemblyInfoDetails Import(params string[] files)
        {
            foreach (string import in files)
                ImportDropIfDuplicate(import);
            return this;
        }

        private void ImportDropIfDuplicate(string file)
        {
            if (!_imports.Contains(file.Trim()))
                _imports.Add(file.Trim());
        }


        public AssemblyInfoDetails ComVisible(bool visible)
        {
            ImportDropIfDuplicate("System.Runtime.InteropServices");
            _comVisibleSet = true;
            _comVisible = visible;
            return this;
        }

        public AssemblyInfoDetails ClsCompliant(bool compliant)
        {
            ImportDropIfDuplicate("System");
            _clsCompliantSet = true;
            _clsCompliant = compliant;
            return this;
        }

        public AssemblyInfoDetails AssemblyVersion(string value)
        {
            ImportDropIfDuplicate("System.Reflection");
            _assemblyVersion = value;
            return this;
        }

        public AssemblyInfoDetails AssemblyTitle(string value)
        {
            ImportDropIfDuplicate("System.Reflection");
            _assemblyTitle = value;
            return this;
        }


        public AssemblyInfoDetails AssemblyDescription(string value)
        {
            ImportDropIfDuplicate("System.Reflection");
            _assemblyDescription = value;
            return this;
        }


        public AssemblyInfoDetails AssemblyCopyright(string value)
        {
            ImportDropIfDuplicate("System.Reflection");
            _assemblyCopyright = value;
            return this;
        }
           
        public void OutputTo(string filePath)
        {
            using (var fs = new FileStream(filePath, FileMode.CreateNew, FileAccess.Write))
            using (var sw = new StreamWriter(fs))
            {
                sw.Write(AssemblyInfoBuilder.Build(this));
            }
        }
    }
}