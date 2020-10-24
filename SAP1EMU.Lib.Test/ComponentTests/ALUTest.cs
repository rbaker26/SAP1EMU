using Microsoft.VisualStudio.TestTools.UnitTesting;

using SAP1EMU.Lib.Components;

using System;

namespace SAP1EMU.Lib.Test.ComponentTests
{
    [TestClass]
    public class ALUTest
    {
        [TestMethod]
        public void TestALUSum1()
        {
            for (int i = 0; i < 256; i++)
            {
                string temp = Convert.ToString(i, 2);
                if (temp.Length <= 8)
                {
                    temp = temp.PadLeft(8, '0');
                }
                else
                {
                    temp = temp.Substring(temp.Length - 1 - 8, 8);
                }

                int result = (int)(Convert.ToUInt32(ALU.Compute("0000", temp), 2));

                string sresult = Convert.ToString(result, 2);
                sresult = sresult.PadLeft(8, '0');

                string expectedResult = Convert.ToString(i, 2);
                if (expectedResult.Length <= 8)
                {
                    expectedResult = expectedResult.PadLeft(8, '0');
                }
                else
                {
                    expectedResult = expectedResult.Substring(expectedResult.Length - 1 - 8, 8);
                }
                Assert.AreEqual(expectedResult, sresult);

                //System.Console.WriteLine(result + " = " + sresult);
            }
        }

        [TestMethod]
        public void TestALUSum2()
        {
            for (int i = 0; i < 256; i += 4)
            {
                string temp = Convert.ToString(i, 2);
                if (temp.Length <= 8)
                {
                    temp = temp.PadLeft(8, '0');
                }
                else
                {
                    temp = temp.Substring(temp.Length - 1 - 8, 8);
                }

                int result = (int)(Convert.ToUInt32(ALU.Compute("00000001", temp), 2));

                string sresult = Convert.ToString(result, 2);
                sresult = sresult.PadLeft(8, '0');

                string expectedResult = Convert.ToString((i + 1), 2);
                if (expectedResult.Length <= 8)
                {
                    expectedResult = expectedResult.PadLeft(8, '0');
                }
                else
                {
                    expectedResult = expectedResult.Substring(expectedResult.Length - 1 - 8, 8);
                }
                Assert.AreEqual(expectedResult, sresult);

                //System.Console.WriteLine(result + " = " + sresult + " = " + expectedResult);
            }
        }

        [TestMethod]
        public void TestALUSum3()
        {
            //TODO Set ADD Bit

            // Addition *******************************************************
            Assert.AreEqual("00000010", ALU.Compute("00000001", "00000001"));
            Assert.AreEqual("00000011", ALU.Compute("00000010", "00000001"));
            Assert.AreEqual("00000100", ALU.Compute("00000011", "00000001"));

            Assert.AreEqual("00000010", ALU.Compute("00000001", "00000001"));
            Assert.AreEqual("00000011", ALU.Compute("00000001", "00000010"));
            Assert.AreEqual("00000100", ALU.Compute("00000001", "00000011"));

            Assert.AreEqual("11111111", ALU.Compute("11111111", "00000000"));
            Assert.AreEqual("11111111", ALU.Compute("00000000", "11111111"));
            Assert.AreEqual("10000000", ALU.Compute("01111111", "00000001"));
            Assert.AreEqual("10000000", ALU.Compute("00000001", "01111111"));

            Assert.AreEqual("00000000", ALU.Compute("11111111", "00000001"));
            Assert.AreEqual("00000000", ALU.Compute("00000001", "11111111"));

            Assert.AreEqual("11111110", ALU.Compute("11111111", "11111111"));
            //*****************************************************************
        }
    }
}