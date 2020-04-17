using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using SAP1EMU.Lib.Components;


namespace SAP1EMU.Lib.Test.ComponentTests
{
    [TestClass]
    public class RAMTest
    {
        [TestMethod]
        public void TestRamEmpty()
        {
            RAM ram = new RAM();
            try
            {
                // Should throw NullReferenceException bc list is empty.
                ram.GetWordAt("0000");
                Assert.Fail("RAM should be empty");
            }
            catch (NullReferenceException)
            {
                Assert.IsTrue(true);
            }
            catch (ArgumentOutOfRangeException)
            {
                Assert.IsTrue(true);
            }

            ram.ClearRAM();
        }

        [TestMethod]
        public void TestRamFull()
        {
            RAM ram = new RAM();

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
                    "01010101"
                };

                RAMProgram rmp = new RAMProgram(RamContentsData);
                ram.LoadProgram(rmp);

                Assert.IsTrue(true);
            }
            catch(Exception e)
            {
                Assert.Fail(e.ToString());
            }
            ram.ClearRAM();

        }


        [TestMethod]
        public void TestRamContentsEmpty()
        {
            RAM ram = new RAM();

            string[] ADDRS = new string[16] { "0000", "0001", "0010", "0011", "0100", "0101", "0110", "0111", "1000", "1001", "1010", "1011", "1100", "1101", "1110", "1111" };
            try
            {
                List<string> RamContentsData = new List<string>();

                RAMProgram rmp = new RAMProgram(RamContentsData);
                ram.LoadProgram(rmp);

                for(int i =0; i < 15; i++)
                {
                    // Since the RAMProgram was empty, the RAMProgam should fill will all zeros, so the RAM should be filled will all zeros.
                    Assert.IsTrue(string.Equals(ram.GetWordAt(ADDRS[i]), "00000000"));
                }
            }
            catch (Exception e)
            {
                Assert.Fail(e.ToString());
            }
            ram.ClearRAM();

        }



        [TestMethod]
        public void TestRamContentFull()
        {
            RAM ram = new RAM();

            string[] ADDRS = new string[16] { "0000", "0001", "0010", "0011", "0100", "0101", "0110", "0111", "1000", "1001", "1010", "1011", "1100", "1101", "1110", "1111" };

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
                    "10101010",
                    "01010101"
                };

                RAMProgram rmp = new RAMProgram(RamContentsData);
                ram.LoadProgram(rmp);


                // Check to make sure the RAM entries and the list<string> share the same values
                for(int i =0; i < 15; i++)
                {
                    Assert.IsTrue(Equals(RamContentsData[i], ram.GetWordAt(ADDRS[i])));
                }
                // Check to see it i == i+1, should be false
                for (int i = 0; i < 14; i++)
                {
                    Assert.IsFalse(Equals(RamContentsData[i], ram.GetWordAt(ADDRS[i+1])));
                }

            }
            catch (Exception e)
            {
                Assert.Fail(e.ToString());
            }
            ram.ClearRAM();

        }
    }


    
}
