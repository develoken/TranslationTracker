using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Collections.Generic;
using TranslationTracker;

namespace UnitTest
{
    [TestClass]
    public class UnitTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            Database db = new Database();
            db.InsertItem("한국말", "English");
            db.InsertItem("아이템1", "Item1");
            db.InsertItem("아이템2", "Item2");
            db.InsertItem("아이템3", "Item3");
            db.InsertItem("아이템4", "Item4");

            Dictionary<string, string> dict1 = new Dictionary<string, string>(db.GetDictionary());

            db.SaveDatabase(Directory.GetCurrentDirectory() + "\\test.xml");
            db.LoadDatabase(Directory.GetCurrentDirectory() + "\\test.xml");

            Dictionary<string, string> dict2 = db.GetDictionary();

            CollectionAssert.AreEqual(dict1, dict2);
        }

        [TestMethod]
        public void TestMethod2()
        {
            ExcelReader reader = ExcelReader.Instance;
            reader.Load(Directory.GetCurrentDirectory() + "\\[KR]20140116_별자리캡슐.xls", "자리");
        }
    }
}
