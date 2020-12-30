using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.Collections.Generic;
using System.Linq;

namespace SAP1EMU.SAP2.Lib.Test
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

                // Make sure the program copied correctly
                for (int i = 0; i < RamContentsData.Count; i++)
                {
                    Assert.IsTrue(RamContentsResults[i] == RamContentsData[i], $"At index {i}, RamContentsResults != RamContentsData");
                }
                // Make sure the end of the program is padded with 0's
                for (int i = RamContentsData.Count; i < 0xFFFF; i++)
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
                Random random = new Random();
                List<string> RamContentsData = new List<string>();

                // Just fill up ram with random data for the heck of it
                for (int i = 0; i <= 0xFFFF; i++)
                {
                    RamContentsData.Add(string.Join("", Convert.ToString(random.Next(), 2).PadLeft(8).Take(8)));
                }

                RAMProgram program = new RAMProgram(RamContentsData);

                Assert.Fail("Ctor did not catch overflow");
            }
            catch (ArgumentOutOfRangeException e)
            {
                Console.Out.WriteLine(e);
                Assert.IsTrue(true);
            }
        }
    }
}