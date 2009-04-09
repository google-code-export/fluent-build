//using System;
//using System.Collections.Generic;
//using System.IO;
//using NUnit.Framework;
//using NUnit.Framework.SyntaxHelpers;

//namespace FluentBuild.Tests.Functional
//{
//    [TestFixture]
//    public class FileSystemUtilityTests
//    {
//        #region Setup/Teardown

//        [SetUp]
//        public void Setup()
//        {
//            /*
//             * creates a folder with this structure
//             * FileSystemUtilityTests\test1.cs
//             * FileSystemUtilityTests\test2.cs
//             * FileSystemUtilityTests\test1.vb
//             * FileSystemUtilityTests\test2.vb
//             * FileSystemUtilityTests\dir1\test1.cs
//             * FileSystemUtilityTests\dir1\test2.cs
//             * FileSystemUtilityTests\dir1\test1.vb
//             * FileSystemUtilityTests\dir1\test2.vb
//             * FileSystemUtilityTests\dir2\test1.cs
//             * FileSystemUtilityTests\dir2\test2.cs
//             * FileSystemUtilityTests\dir2\test1.vb
//             * FileSystemUtilityTests\dir2\test2.vb
//             */


//            string folder = Environment.GetEnvironmentVariable("TEMP");
//            if (String.IsNullOrEmpty(folder))
//                Assert.Fail("Need a temp directory to run this test");

//            rootFolder = Path.Combine(folder, "FileSystemUtilityTests");
//            if (Directory.Exists(rootFolder))
//                Directory.Delete(rootFolder, true);

//            Directory.CreateDirectory(rootFolder);
//            CreateTestFilesIn(rootFolder);
//            Directory.CreateDirectory(Path.Combine(rootFolder, "dir1"));
//            CreateTestFilesIn(Path.Combine(rootFolder, "dir1"));
//            Directory.CreateDirectory(Path.Combine(rootFolder, "dir2"));
//            CreateTestFilesIn(Path.Combine(rootFolder, "dir2"));
//        }

//        #endregion

//        private string rootFolder;

//        private void CreateTestFilesIn(string folder)
//        {
//            File.Create(Path.Combine(folder, "auto1.cs")).Dispose();
//            File.Create(Path.Combine(folder, "auto2.cs")).Dispose();

//            File.Create(Path.Combine(folder, "test1.vb")).Dispose();
//            File.Create(Path.Combine(folder, "test2.vb")).Dispose();

//            File.Create(Path.Combine(folder, "test1.cs")).Dispose();
//            File.Create(Path.Combine(folder, "test2.cs")).Dispose();
//        }

//        /*
//       *     c:\temp\*.cs        
//       *     c:\temp\
//       *     c:\temp\*.*
//       *     c:\temp\**\*.cs
//       *     c:\temp\auto*.cs
//       *     c:\temp\**\auto*.cs
//       */
//        [Test]
//        public void GetAllFilesMatching_Name_And_WildCard()
//        {
//            var util = new FileSystemUtility();
//            IList<string> matching = util.GetAllFilesMatching(Path.Combine(rootFolder, "auto*.cs"));
//            Assert.That(matching.Count, Is.EqualTo(2));
//        }

//        [Test]
//        public void GetAllFilesMatching_Recursive_Name_And_WildCard()
//        {
//            var util = new FileSystemUtility(null);
//            IList<string> matching = util.GetAllFilesMatching(rootFolder + "\\**\\auto*.cs");
//            Assert.That(matching.Count, Is.EqualTo(6));
//        }


//        [Test]
//        public void GetAllFilesMatching_Just_A_File()
//        {
//            var util = new FileSystemUtility();
//            IList<string> matching = util.GetAllFilesMatching(Path.Combine(rootFolder, "test1.cs"));
//            Assert.That(matching.Count, Is.EqualTo(1));
//        }

//        [Test]
//        public void GetAllFilesMatching_JustDirectory()
//        {
//            var util = new FileSystemUtility();
//            IList<string> matching = util.GetAllFilesMatching(rootFolder);
//            Assert.That(matching.Count, Is.EqualTo(6));
//        }

//        [Test]
//        public void GetAllFilesMatching_Start_Dot_Start_Filter()
//        {
//            var util = new FileSystemUtility();
//            IList<string> matching = util.GetAllFilesMatching(rootFolder + "\\*.*");
//            Assert.That(matching.Count, Is.EqualTo(6));
//        }

//        [Test]
//        public void GetAllFilesMatching_Start_Dot_CS_Filter()
//        {
//            var util = new FileSystemUtility();
//            IList<string> matching = util.GetAllFilesMatching(rootFolder + "\\*.cs");
//            Assert.That(matching.Count, Is.EqualTo(4));
//        }

//        [Test]
//        public void GetAllFilesMatching_Recursive_Start_Dot_CS_Filter()
//        {
//            var util = new FileSystemUtility();
//            IList<string> matching = util.GetAllFilesMatching(rootFolder + "\\**\\*.cs");
//            Assert.That(matching.Count, Is.EqualTo(12));
//        }
//    }
//}