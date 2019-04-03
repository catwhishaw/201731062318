using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Program3;

namespace WordCountTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            string l_path = "C:/第三次博客作业/Test1.txt";
            int number = 3;
            int outPutNum = 5;
            string s_path = "C:/第三次博客作业/Test6.txt";

            Program program = new Program();
            program.TestMethod(l_path, s_path, number, outPutNum);
            Assert.AreEqual(0, program.characters);
            Assert.AreEqual(0, program.lines);
        }
    }
}
