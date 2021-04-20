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
                var ramContentsData = new List<string>()
                {
                    "10101010",
                    "01010101",
                    "10101010",
                    "01010101",
                    "10101010",
                    "01010101",
                    "10101010" 
                };

                var program = new RAMProgram(ramContentsData);

                var ramContentsResults = program.RAMContents;

                // Make sure the program copied correctly
                for (int i = 0x0800; i < ramContentsData.Count; i++)
                {
                    Assert.IsTrue(ramContentsResults[i] == ramContentsData[i], $"At index {i}, RamContentsResults != RamContentsData");
                }
                // Make sure the end of the program is padded with 0's
                for (int i = ramContentsData.Count; i < ramContentsResults.Count; i++)
                {
                    Assert.IsTrue(ramContentsResults[i] == "00000000");
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
                var ramContentsData = new List<string>();

                // Just fill up ram with random data for the heck of it
                for (int i = 0; i <= 0xFFFF; i++)
                {
                    ramContentsData.Add(string.Join("", Convert.ToString(random.Next(), 2).PadLeft(8).Take(8)));
                }

                RAMProgram program = new RAMProgram(ramContentsData);

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