using System;
using FluentBuild.ApplicationProperties;
using NUnit.Framework;

namespace FluentBuild.Core
{
    [TestFixture]
    public class PropertiesTests
    {
        [Test]
        public void TeamCityShouldReturnProperObject()
        {
            Assert.That(Properties.TeamCity, Is.TypeOf<TeamCityProperties>());
        }

        [Test]
        public void CruiseControlShouldReturnProperObject()
        {
            Assert.That(Properties.CruiseControl, Is.TypeOf<CruiseControlProperties>());
        }

        [Test]
        public void CommandLineShouldReturnProperObject()
        {
            Assert.That(Properties.CommandLineProperties, Is.TypeOf<CommandLineProperties>());
        }

        [Test]
        public void CurrentDirectoryShouldBeProper()
        {
            Assert.That(Properties.CurrentDirectory, Is.EqualTo(Environment.CurrentDirectory));
        }
    }
}