using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.Collections.Generic;

namespace SAP1EMU.Lib.Test
{
    [TestClass]
    public class ProgramTest
    {
        [TestMethod]
        public void TestCtorReg()
        {
            // Test 2
            try
            {
                List<string> RamContentsData = new List<string>
                {
                    "10101010",
                    "01010101",
                    "10101010",
                    "01010101",
                    "10101010",
                    "01010101",
                    "10101010"
                };
                RAMProgram program = new RAMProgram(RamContentsData);

                List<string> RamContentsResults = program.RamContents;

                if (RamContentsResults.Count != 15)
                {
                    Assert.Fail();
                }
                // Make sure the program copied correctly
                for (int i = 0; i < RamContentsData.Count; i++)
                {
                    Assert.IsTrue(RamContentsResults[i] == RamContentsData[i], $"At index {i}, RamContentsResults != RamContentsData");
                }
                // Make sure the end of the program is padded with 0's
                for (int i = RamContentsData.Count; i < 15; i++)
                {
                    Assert.IsTrue(RamContentsResults[i] == "00000000");
                }
            }
            catch (ArgumentOutOfRangeException e)
            {
                Assert.Fail(e.ToString());
            }
        }

        [TestMethod]
        public void TestCtorOverflow()
        {
            try
            {
                List<string> RamContentsData = new List<string>
                {
                    "10101010",
                    "01010101",
                    "10101010",
                    "01010101",
                    "10101010",
                    "01010101",
                    "10101010",
                    "01010101",
                    "10101010",
                    "01010101",
                    "10101010",
                    "01010101",
                    "10101010",
                    "01010101",
                    "01010101",
                    "01010101",
                    "01010101"
                };

                RAMProgram program = new RAMProgram(RamContentsData);

                Assert.Fail("Ctor did not catch overflow");
            }
            catch (ArgumentOutOfRangeException e)
            {
                System.Console.Out.WriteLine(e);
                Assert.IsTrue(true);
            }
        }
    }
}