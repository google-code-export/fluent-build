﻿using NUnit.Framework;

namespace FluentBuild.Runners.Zip
{
    ///<summary />
    public class ZipCompressTests
    {

        ///<summary />
        public void TestSomething()
        {
            var bothStringsAreSet = true ^ true;
            var firstStringIsSet = true ^ false;
            var secondStringIsSet = false ^ true;
            var noStringsAreSet = false ^ false;

            Assert.IsFalse(bothStringsAreSet, "Both strings are set");
            Assert.IsFalse(noStringsAreSet, "no strings are set");
            Assert.IsTrue(firstStringIsSet, "first string is set");
            Assert.IsTrue(secondStringIsSet, "second string is set");
        }
    }
}