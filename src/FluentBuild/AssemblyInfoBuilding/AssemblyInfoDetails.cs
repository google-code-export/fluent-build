using System;
using System.Collections.Generic;
using System.IO;
using FluentBuild.Core;

namespace FluentBuild.AssemblyInfoBuilding
{
    internal class AssemblyInfoItem
    {
        public AssemblyInfoItem(string name, bool isQuotedValue, string value)
        {
            Name = name;
            IsQuotedValue = isQuotedValue;
            Value = value;
        }

        public string Name { get; set; }
        public bool IsQuotedValue { get; set; }
        public string Value { get; set; }
    }

    /// <summary>
    /// Sets the lines for an assembly info file
    /// </summary>
    public class AssemblyInfoDetails
    {
        internal readonly IAssemblyInfoBuilder AssemblyInfoBuilder;
        internal readonly List<String> Imports = new List<string>();
        internal IList<AssemblyInfoItem> LineItems = new List<AssemblyInfoItem>();

//        internal string AssemblyCopyright;
//        internal string AssemblyDescription;
//        internal string AssemblyTitle;
//        internal Version AssemblyVersion;
//        internal bool AssemblyClsCompliant;
//        internal bool ClsCompliantSet;
//        internal bool AssemblyComVisible;
//        internal bool ComVisibleSet;
//        internal string AssemblyCompany;
//        internal string AssemblyProduct;
//        private bool CultureSet;
//        private string AssemblyCulture;
//        private bool DelaySignSet;
//        private bool AssemblyDelaySign;
//        private bool AssemblyFileVersionSet;
//        private string AssemblyFileVersion;
//        private bool AssemblyInformationalVersionSet;
//        private string AssemblyInformationalVersion;
//        private bool AssemblyKeyFileSet;
//        private string AssemblyKeyFile;
//        private bool AssemblyKeyNameSet;
//        private string AssemblyKeyName;
//        private bool AssemblyTrademarkSet;
//        private string AssemblyTrademark;


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
            LineItems.Add(new AssemblyInfoItem("ComVisible", false, visible.ToString().ToLower()));
            return this;
        }

        ///<summary>
        /// Sets the culture
        ///</summary>
        ///<param name="culture">The culture of the assembly</param>
        ///<returns></returns>
        public AssemblyInfoDetails Culture(string culture)
        {
            ImportDropIfDuplicate("System.Reflection");
            LineItems.Add(new AssemblyInfoItem("AssemblyCulture", true, culture));
            return this;
        }

        ///<summary>
        /// Sets if to delay sign the assembly
        ///</summary>
        ///<param name="delaySign">Wether to delay sign the assembly</param>
        ///<returns></returns>
        public AssemblyInfoDetails DelaySign(bool delaySign)
        {
            ImportDropIfDuplicate("System.Reflection");
            LineItems.Add(new AssemblyInfoItem("AssemblyDelaySign", false, delaySign.ToString().ToLower()));
            return this;
        }

        ///<summary>
        /// Sets the file version
        ///</summary>
        ///<param name="version"></param>
        ///<returns></returns>
        public AssemblyInfoDetails FileVersion(string version)
        {
            ImportDropIfDuplicate("System.Reflection");
            LineItems.Add(new AssemblyInfoItem("AssemblyFileVersion", true, version));
            return this;
        }


        ///<summary>
        /// Sets the informational version
        ///</summary>
        ///<param name="version"></param>
        ///<returns></returns>
        public AssemblyInfoDetails InformationalVersion(string version)
        {
            ImportDropIfDuplicate("System.Reflection");
            LineItems.Add(new AssemblyInfoItem("AssemblyInformationalVersion", true, version));
            return this;
        }

        ///<summary>
        /// Sets teh path to the Strong Named Key for an assembly
        ///</summary>
        ///<param name="path">The path to the snk file</param>
        ///<returns></returns>
        public AssemblyInfoDetails KeyFile(string path)
        {
            ImportDropIfDuplicate("System.Reflection");
            LineItems.Add(new AssemblyInfoItem("AssemblyKeyFile", true, path));
            return this;
        }


        ///<summary>
        /// Sets the KeyName
        ///</summary>
        ///<param name="name">The name of the key</param>
        ///<returns></returns>
        public AssemblyInfoDetails KeyName(string name)
        {
            ImportDropIfDuplicate("System.Reflection");
            LineItems.Add(new AssemblyInfoItem("AssemblyKeyName", true, name));
            return this;
        }

        ///<summary>
        /// Sets the trademark attribute
        ///</summary>
        ///<param name="trademark">The value representing the trademark</param>
        ///<returns></returns>
        public AssemblyInfoDetails Trademark(string trademark)
        {
            ImportDropIfDuplicate("System.Reflection");
            LineItems.Add(new AssemblyInfoItem("AssemblyTrademark", true, trademark));
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
            LineItems.Add(new AssemblyInfoItem("CLSCompliant", false, compliant.ToString().ToLower()));
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
            LineItems.Add(new AssemblyInfoItem("AssemblyVersionAttribute", true, value.ToString()));
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
            LineItems.Add(new AssemblyInfoItem("AssemblyTitleAttribute", true, value));
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
            LineItems.Add(new AssemblyInfoItem("AssemblyDescriptionAttribute", true, value));
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
            LineItems.Add(new AssemblyInfoItem("AssemblyCopyrightAttribute", true, value));
            return this;
        }

        /// <summary>
        /// Sets the company attribute of the assembly
        /// </summary>
        /// <param name="value">The company to set</param>
        /// <returns></returns>
        public AssemblyInfoDetails Company(string value)
        {
            LineItems.Add(new AssemblyInfoItem("AssemblyCompanyAttribute", true, value));
            return this;
        }

        /// <summary>
        /// Sets the product attribute of the assembly
        /// </summary>
        /// <param name="value">The product to set</param>
        /// <returns></returns>
        public AssemblyInfoDetails Product(string value)
        {
            LineItems.Add(new AssemblyInfoItem("AssemblyProductAttribute", true, value));
            return this;
        }


        ///<summary>
        /// Adds a custom attribute to the assemblyInfo file
        ///</summary>
        ///<param name="attributeNamespace">The namespace that the attribute exists in</param>
        ///<param name="name">The name of the attribute</param>
        ///<param name="isQuoted">Wether or not to quote the value when the file is generated</param>
        ///<param name="value">The value of the attribute</param>
        public AssemblyInfoDetails AddCustomAttribute(string attributeNamespace, string name, bool isQuoted, string value)
        {
            ImportDropIfDuplicate(attributeNamespace);
            LineItems.Add(new AssemblyInfoItem(name, isQuoted, value));
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