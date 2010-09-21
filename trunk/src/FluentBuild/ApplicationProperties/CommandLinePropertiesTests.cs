﻿using FluentBuild.Core;
using NUnit.Framework;

namespace FluentBuild.ApplicationProperties
{
    [TestFixture]
    public class CommandLinePropertiesTests
    {
        [Test]
        public void ShouldConstructWithProperties()
        {
            Assert.That(Properties.CommandLineProperties.Properties, Is.Not.Null);
        }

        [Test]
        public void ShouldGetAndSetProperly()
        {
            var value = "testvalue";
            var name = "testname";
            Properties.CommandLineProperties.Add(name, value);
            Assert.That(Properties.CommandLineProperties.Properties[name], Is.EqualTo(value));
        }

    }
}