﻿using FluentBuild.AssemblyInfoBuilding;
using NUnit.Framework;

namespace FluentBuild.Core
{
    ///<summary />
    public class AssemblyInfoTests
    {
        ///<summary />
        public void MethodCallShouldNotThrowException()
        {
            AssemblyInfoLanguage assemblyInfoLanguage = AssemblyInfo.Language;
            Assert.That(assemblyInfoLanguage, Is.Not.Null);
        }
    }

}