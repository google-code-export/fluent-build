using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluentBuild.MSTests
{
    [TestClass]
    public class BasicTests
    {
        [TestMethod]
        public void SimplePass()
        {
            Assert.IsTrue(1==1);
        }

        
        [TestMethod]
        public void SimpleFail()
        {
            Assert.Fail();
        }


        [Ignore]
        public void IgnoredTest()
        {
            Assert.Fail();
        }
    }
}
