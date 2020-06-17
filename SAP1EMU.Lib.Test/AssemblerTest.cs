using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SAP1EMU.Assembler;

namespace SAP1EMU.Lib.Test
{

    [TestClass]
    public class AssemblerTest
    {
        #region Assembler Parsing Test - Valid Code Section
        [TestMethod]
        public void TestParseList_Valid_Code_1()
        {
            List<string> asm = new List<string> 
            { 
                "LDA 0xF", 
                "...", 
                "0xF 0xF" };
            List<string> expected_bin = new List<string>
            {
                "00001111",
                "00000000",
                "00000000",
                "00000000",
                "00000000",
                "00000000",
                "00000000",
                "00000000",
                "00000000",
                "00000000",
                "00000000",
                "00000000",
                "00000000",
                "00000000",
                "00000000",
                "11111111",

            };

            List<string> compiled_bin = Assemble.Parse(asm);

            for (int i = 0; i <= 15; i++)
            {
                Assert.AreEqual(expected_bin[i], compiled_bin[i]);

            }
        }


        [TestMethod]
        public void TestParseList_Valid_Code_2()
        {
            List<string> asm = new List<string> 
            { 
                "HLT 0xF", 
                "HLT 0xF", 
                "HLT 0xF", 
                "...", 
                "0xF 0xF", 
                "0xF 0xF" 
            };
            List<string> expected_bin = new List<string>
            {
                "11111111",
                "11111111",
                "11111111",
                "00000000",
                "00000000",
                "00000000",
                "00000000",
                "00000000",
                "00000000",
                "00000000",
                "00000000",
                "00000000",
                "00000000",
                "00000000",
                "11111111",
                "11111111",

            };

            List<string> compiled_bin = Assemble.Parse(asm);

            for (int i = 0; i <= 15; i++)
            {
                Assert.AreEqual(expected_bin[i], compiled_bin[i]);

            }
        }



        [TestMethod]
        public void TestParseList_Valid_Code_3()
        {
            List<string> asm = new List<string>
            {
                "LDA 0xD",
                "ADD 0xE",
                "ADD 0xF",
                "OUT 0x0",
                "HLT 0x0",
                "...",
                "0x2 0x8",
                "0x0 0xF",
                "0x0 0xD" 
            };
            List<string> expected_bin = new List<string>
            {
                "00001101",
                "00011110",
                "00011111",
                "11100000",
                "11110000",
                "00000000",
                "00000000",
                "00000000",
                "00000000",
                "00000000",
                "00000000",
                "00000000",
                "00000000",
                "00101000",
                "00001111",
                "00001101",

            };

            List<string> compiled_bin = Assemble.Parse(asm);

            for (int i = 0; i <= 15; i++)
            {
                Assert.AreEqual(expected_bin[i], compiled_bin[i]);

            }
        }


        [TestMethod]
        public void TestParseList_Valid_Code_4()
        {
            List<string> asm = new List<string>
            {
                "LDA 0xD",
                "ADD 0xE",
                "ADD 0xF",
                "OUT 0x0",
                "OUT 0x0",
                "OUT 0x0",
                "OUT 0x0",
                "OUT 0x0",
                "OUT 0x0",
                "OUT 0x0",
                "OUT 0x0",
                "HLT 0x0",
                "...",
                "0x2 0x8",
                "0x0 0xF",
                "0x0 0xD"
            };
            List<string> expected_bin = new List<string>
            {
                "00001101",
                "00011110",
                "00011111",
                "11100000",
                "11100000",
                "11100000",
                "11100000",
                "11100000",
                "11100000",
                "11100000",
                "11100000",
                "11110000",
                "00000000",
                "00101000",
                "00001111",
                "00001101",

            };

            List<string> compiled_bin = Assemble.Parse(asm);

            for (int i = 0; i <= 15; i++)
            {
                Assert.AreEqual(expected_bin[i], compiled_bin[i]);

            }
        }


        // On this test I added a ... and 16 lines of code.
        // It should ignore the ... because there is no room to pad wth 0's
        [TestMethod]
        public void TestParseList_Valid_Code_5()
        {
            List<string> asm = new List<string>
            {
                "LDA 0xD",
                "ADD 0xE",
                "ADD 0xF",
                "OUT 0x0",
                "OUT 0x0",
                "OUT 0x0",
                "OUT 0x0",
                "OUT 0x0",
                "OUT 0x0",
                "OUT 0x0",
                "OUT 0x0",
                "HLT 0x0",
                "HLT 0x0",
                "...",
                "0x2 0x8",
                "0x0 0xF",
                "0x0 0xD"
            };
            List<string> expected_bin = new List<string>
            {
                "00001101",
                "00011110",
                "00011111",
                "11100000",
                "11100000",
                "11100000",
                "11100000",
                "11100000",
                "11100000",
                "11100000",
                "11100000",
                "11110000",
                "11110000",
                "00101000",
                "00001111",
                "00001101",

            };

            List<string> compiled_bin = Assemble.Parse(asm);

            for (int i = 0; i <= 15; i++)
            {
                Assert.AreEqual(expected_bin[i], compiled_bin[i]);

            }
        }




        [TestMethod]
        public void TestParseList_Valid_Code_6()
        {
            List<string> asm = new List<string>
            {
                "..."
            };
            List<string> expected_bin = new List<string>
            {
                "00000000",
                "00000000",
                "00000000",
                "00000000",
                "00000000",
                "00000000",
                "00000000",
                "00000000",
                "00000000",
                "00000000",
                "00000000",
                "00000000",
                "00000000",
                "00000000",
                "00000000",
                "00000000",

            };

            List<string> compiled_bin = Assemble.Parse(asm);

            for (int i = 0; i <= 15; i++)
            {
                Assert.AreEqual(expected_bin[i], compiled_bin[i]);

            }
        }


        [TestMethod]
        public void TestParseList_Valid_Code_7()
        {
            List<string> asm = new List<string>
            {
                "NOP 0x0",
                "NOP 0x0",
                "NOP 0x0",
                "NOP 0x0",
                "NOP 0x0",
                "NOP 0x0",
                "NOP 0x0",
                "NOP 0x0",
                "NOP 0x0",
                "NOP 0x0",
                "NOP 0x0",
                "NOP 0x0",
                "NOP 0x0",
                "NOP 0x0",
                "NOP 0x0",
                "NOP 0x0",
            };
            List<string> expected_bin = new List<string>
            {
                "00000000",
                "00000000",
                "00000000",
                "00000000",
                "00000000",
                "00000000",
                "00000000",
                "00000000",
                "00000000",
                "00000000",
                "00000000",
                "00000000",
                "00000000",
                "00000000",
                "00000000",
                "00000000",

            };

            List<string> compiled_bin = Assemble.Parse(asm);

            for (int i = 0; i <= 15; i++)
            {
                Assert.AreEqual(expected_bin[i], compiled_bin[i]);

            }
        }
        #endregion


        #region Assembler Parsing Test - Invaild Code Section


        [TestMethod]
        public void TestParseList_Invalid_Code_1()
        {
            List<string> asm = new List<string>
            {
                "XXX 0xF",
                "...",
                "0xF 0xF"
            };

            try
            {
                // This should fail and throw an execption, if it doesn't it will fail
                _ = Assemble.Parse(asm);
                Assert.Fail();
            }
            catch (ParseException)
            {
                Assert.IsTrue(true);
            }
        }

        [TestMethod]
        public void TestParseList_Invalid_Code_2()
        {
            List<string> asm = new List<string>
            {
                "...",
                "jjj 0xF"
            };

            try
            {
                // This should fail and throw an execption, if it doesn't it will fail
                _ = Assemble.Parse(asm);
                Assert.Fail();
            }
            catch (ParseException)
            {
                Assert.IsTrue(true);
            }
        }


        [TestMethod]
        public void TestParseList_Invalid_Code_3()
        {
            List<string> asm = new List<string>
            {
                "LDA0x1",
                "LDA0x1",
                "LDA0x1",
                "LDA0x1",
                "LDA0x1",
                "..."
            };

            try
            {
                // This should fail and throw an execption, if it doesn't it will fail
                _ = Assemble.Parse(asm);
                Assert.Fail();
            }
            catch (ParseException)
            {
                Assert.IsTrue(true);
            }
        }


        #endregion


        // Malvino Op Code Loop Detection Test **************************************
        [TestMethod]
        public void Test_MalvinoCodes_1()
        {
            List<string> asm = new List<string>
            {
                "LDA 0xF",
                "OUT 0x0",
                "HLT 0x0",
                "...    ",
                "0xA 0xA"
            };
            try
            {
                Assemble.Parse(asm, "Malvino");
            }
            catch(Exception e)
            {
                Assert.Fail(e.ToString());
            }
        }


        [TestMethod]
        public void Test_MalvinoCodes_2()
        {
            List<string> asm = new List<string>
            {
                "LDA 0xF",
                "STA 0xE",
                "OUT 0x0",
                "HLT 0x0",
                "...    ",
                "0x0 0x0",
                "0xA 0xA"
            };
            try
            {
                Assemble.Parse(asm, "Malvino");
                Assert.Fail();
            }
            catch (Exception e)
            {
        
            }
        }
        // **************************************************************************


    }
}
