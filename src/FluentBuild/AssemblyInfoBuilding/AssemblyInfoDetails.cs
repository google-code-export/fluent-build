using System;
using System.Collections.Generic;
using System.IO;
using FluentBuild.Core;

namespace FluentBuild.AssemblyInfoBuilding
{
    /// <summary>
    /// Sets the lines for an assembly info file
    /// </summary>
    public class AssemblyInfoDetails
    {
        internal readonly List<String> Imports = new List<string>();
        internal readonly IAssemblyInfoBuilder AssemblyInfoBuilder;

        internal string AssemblyCopyright;
        internal string AssemblyDescription;
        internal string AssemblyTitle;
        internal Version AssemblyVersion;
        internal bool AssemblyClsCompliant;
        internal bool ClsCompliantSet;
        internal bool AssemblyComVisible;
        internal bool ComVisibleSet;
        internal string AssemblyCompany;
        internal string AssemblyProduct;

        internal AssemblyInfoDetails(IAssemblyInfoBuilder assemblyInfoBuilder)
        {
            AssemblyInfoBuilder = assemblyInfoBuilder;
        }

        /// <summary>
        /// Import a namespace. Will generate a using namespace; in C# and imports namespace in VB
        /// </summary>
        /// <param name="namespaces">The namespaces to import</param>
        /// <remarks>Duplicate namespace imports will be automatically ignored</remarks>
        /// <returns></returns>
        public AssemblyInfoDetails Import(params string[] namespaces)
        {
            foreach (string import in namespaces)
                ImportDropIfDuplicate(import);
            return this;
        }

        private void ImportDropIfDuplicate(string @namespace)
        {
            if (!Imports.Contains(@namespace.Trim()))
                Imports.Add(@namespace.Trim());
        }

        /// <summary>
        /// Explicitly states if this assembly is visible to COM clients. 
        /// If the attribute is missing then the assembly is COM visible.
        /// </summary>
        /// <param name="visible">sets com visibility</param>
        /// <returns></returns>
        public AssemblyInfoDetails ComVisible(bool visible)
        {
            ImportDropIfDuplicate("System.Runtime.InteropServices");
            ComVisibleSet = true;
            AssemblyComVisible = visible;
            return this;
        }

        /// <summary>
        /// States if the assembly is CLS Compliant. CLS compliant means that all classes only
        /// expose features that are common accross all .NET languages.
        /// </summary>
        /// <remarks>
        /// Things that make an assembly non-cls compliant:
        /// Exposing unsigned types.
        /// Unsafe types (e.g. pointers) should not be exposed.
        /// Operators should not be overloaded
        /// Two types or methods should not be included that differ only by case. e.g. doWork and DOWORK.
        /// </remarks>
        /// <param name="compliant">sets cls compliant</param>
        /// <returns></returns>
        public AssemblyInfoDetails ClsCompliant(bool compliant)
        {
            ImportDropIfDuplicate("System");
            ClsCompliantSet = true;
            AssemblyClsCompliant = compliant;
            return this;
        }

        /// <summary>
        /// Sets the assembly version.
        /// </summary>
        /// <param name="value">a version in the format of Major.Minor.[Build].[Revision]</param>
        /// <returns></returns>
        public AssemblyInfoDetails Version(string value)
        {
            return Version(new Version(value));
        }


        /// <summary>
        /// Sets the assembly version.
        /// </summary>
        /// <param name="value">a version object</param>
        /// <returns></returns>
        public AssemblyInfoDetails Version(Version value)
        {
            ImportDropIfDuplicate("System.Reflection");
            AssemblyVersion = value;
            return this;
        }

        /// <summary>
        /// Sets the title attribute of the assembly
        /// </summary>
        /// <param name="value">The title to use</param>
        /// <returns></returns>
        public AssemblyInfoDetails Title(string value)
        {
            ImportDropIfDuplicate("System.Reflection");
            AssemblyTitle = value;
            return this;
        }

        /// <summary>
        /// Sets the description attribute of the assembly
        /// </summary>
        /// <param name="value">The description to set</param>
        /// <returns></returns>
        public AssemblyInfoDetails Description(string value)
        {
            ImportDropIfDuplicate("System.Reflection");
            AssemblyDescription = value;
            return this;
        }

        /// <summary>
        /// Sets the copyright attribute of the assembly
        /// </summary>
        /// <param name="value">The copyright to set</param>
        /// <returns></returns>
        public AssemblyInfoDetails Copyright(string value)
        {
            ImportDropIfDuplicate("System.Reflection");
            AssemblyCopyright = value;
            return this;
        }

        /// <summary>
        /// Sets the company attribute of the assembly
        /// </summary>
        /// <param name="value">The company to set</param>
        /// <returns></returns>
        public AssemblyInfoDetails Company(string value)
        {
            AssemblyCompany = value;
            return this;
        }

        /// <summary>
        /// Sets the product attribute of the assembly
        /// </summary>
        /// <param name="value">The product to set</param>
        /// <returns></returns>
        public AssemblyInfoDetails Product(string value)
        {
            AssemblyProduct = value;
            return this;
        }

        /// <summary>
        /// Execute the generation of the assembly info file and output it.
        /// </summary>
        /// <param name="artifactLocation">The destination artifact location</param>
        public void OutputTo(BuildArtifact artifactLocation)
        {
            OutputTo(artifactLocation.ToString());
        }

        /// <summary>
        /// Execute the generation of the assembly info file and output it.
        /// </summary>
        /// <param name="filePath">The destination file path location</param>
        public void OutputTo(string filePath)
        {
            using (var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            using (var sw = new StreamWriter(fs))
            {
                sw.Write(AssemblyInfoBuilder.Build(this));
            }
        }
 
    }
}