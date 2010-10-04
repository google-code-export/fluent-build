﻿using NUnit.Framework;

using Rhino.Mocks;

namespace FluentBuild.Database
{
    ///<summary />
    public class MsSqlUtilitiesTests
    {
        ///<summary />
        public void DoesDatabaseAlreadyExist_ShouldCallUnderlyingEngine()
        {
            var engine = MockRepository.GenerateStub<IMsSqlEngine>();
            var subject = new MsSqlUtilities(engine);
            subject.DoesDatabaseAlreadyExist();
            engine.AssertWasCalled(x=>x.DoesDatabaseAlreadyExist());
        }

        ///<summary />
        public void CreateOrUpdate_ShouldCreateProperType()
        {
            var engine = MockRepository.GenerateStub<IMsSqlEngine>();
            var subject = new MsSqlUtilities(engine);
            Assert.That(subject.CreateOrUpdate, Is.TypeOf(typeof(MsSqlCreateOrUpdate)));
        }
    }
}