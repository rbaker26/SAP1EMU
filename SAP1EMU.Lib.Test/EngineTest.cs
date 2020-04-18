using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using SAP1EMU.Engine;
namespace SAP1EMU.Lib.Test
{
    [TestClass]
    public class EngineTest
    {

        // LDA Tests 1-3 ************************************************************
        #region LDA Tests 1-3
        // Test_LDA_PROG_1 **********************************************************
        /// <summary>
        /// This will run the following program
        /// 
        /// 0x0 LDA 0xF
        /// 0x1 OUT 0xX
        /// 0x2 HLT 0xX
        /// 0x3 NOP 0xX
        /// 0x4 NOP 0xX
        /// 0x5 NOP 0xX
        /// 0x6 NOP 0xX
        /// 0x7 NOP 0xX
        /// 0x8 NOP 0xX
        /// 0x9 NOP 0xX
        /// 0xA NOP 0xX
        /// 0xB NOP 0xX
        /// 0xC NOP 0xX
        /// 0xD NOP 0xX
        /// 0xE NOP 0xX
        /// 0xf 0x0 0x1
        /// 
        /// The expected result is OReg: 00000001
        /// </summary>
        [TestMethod]
        public void Test_LDA_PROG_1()
        {
            string expectedResult = "00000001";
            List<string> program = new List<string>()
            {
                    "00001111",
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
                    "00000000",
                    "00000000",
                    "00000000",
                    "00000000",
                    "00000001",
            };

            EngineProc engine = new EngineProc();


            engine.Init(new RAMProgram(program));
            engine.Run();

            string output = engine.GetOutput();

            Assert.AreEqual(expectedResult, output);
        }
        // **************************************************************************


        // Test_LDA_PROG_2 **********************************************************
        /// <summary>
        /// This will run the following program
        /// 
        /// 0x0 LDA 0xF
        /// 0x1 OUT 0xX
        /// 0x2 HLT 0xX
        /// 0x3 NOP 0xX
        /// 0x4 NOP 0xX
        /// 0x5 NOP 0xX
        /// 0x6 NOP 0xX
        /// 0x7 NOP 0xX
        /// 0x8 NOP 0xX
        /// 0x9 NOP 0xX
        /// 0xA NOP 0xX
        /// 0xB NOP 0xX
        /// 0xC NOP 0xX
        /// 0xD NOP 0xX
        /// 0xE NOP 0xX
        /// 0xf 0xA 0xA
        /// 
        /// The expected result is OReg: 1010 1010
        /// </summary>
        [TestMethod]
        public void Test_LDA_PROG_2()
        {
            string expectedResult = "10101010";
            List<string> program = new List<string>()
            {
                    "00001111",
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
                    "00000000",
                    "00000000",
                    "00000000",
                    "00000000",
                    "10101010",
            };

            EngineProc engine = new EngineProc();


            engine.Init(new RAMProgram(program));
            engine.Run();

            string output = engine.GetOutput();

            Assert.AreEqual(expectedResult, output);
        }
        // **************************************************************************



        // Test_LDA_PROG_3 **********************************************************
        /// <summary>
        /// This will run the following program
        /// 
        /// 0x0 LDA 0xF
        /// 0x1 OUT 0xX
        /// 0x2 HLT 0xX
        /// 0x3 NOP 0xX
        /// 0x4 NOP 0xX
        /// 0x5 NOP 0xX
        /// 0x6 NOP 0xX
        /// 0x7 NOP 0xX
        /// 0x8 NOP 0xX
        /// 0x9 NOP 0xX
        /// 0xA NOP 0xX
        /// 0xB NOP 0xX
        /// 0xC NOP 0xX
        /// 0xD NOP 0xX
        /// 0xE NOP 0xX
        /// 0xf 0x5 0x5
        /// 
        /// The expected result is OReg: 0101 0101
        /// </summary>
        [TestMethod]
        public void Test_LDA_PROG_3()
        {
            string expectedResult = "01010101";
            List<string> program = new List<string>()
            {
                    "00001111",
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
                    "00000000",
                    "00000000",
                    "00000000",
                    "00000000",
                    "01010101",
            };

            EngineProc engine = new EngineProc();


            engine.Init(new RAMProgram(program));
            engine.Run();

            string output = engine.GetOutput();

            Assert.AreEqual(expectedResult, output);
        }
        // **************************************************************************
        #endregion
        // **************************************************************************

        // ADD Test 1-# *************************************************************
        #region ADD Test 1-#

        // Test_LDA_PROG_1 **********************************************************
        /// <summary>
        /// This will run the following program
        /// 
        /// 0x0 LDA 0xE
        /// 0x1 ADD 0xA
        /// 0x2 OUT 0xX
        /// 0x3 HLT 0xX
        /// 0x4 NOP 0xX
        /// 0x5 NOP 0xX
        /// 0x6 NOP 0xX
        /// 0x7 NOP 0xX
        /// 0x8 NOP 0xX
        /// 0x9 NOP 0xX
        /// 0xA NOP 0xX
        /// 0xB NOP 0xX
        /// 0xC NOP 0xX
        /// 0xD NOP 0xX
        /// 0xE 0x0 0x1
        /// 0xF 0x0 0x1
        /// 
        /// The expected result is OReg: 0000 0010
        /// </summary>
        [TestMethod]
        public void Test_ADD_PROG_1()
        {
            string expectedResult = "00000010";
            List<string> program = new List<string>()
            {
                    "00001110",
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
                    "00000000",
                    "00000000",
                    "00000001",
                    "00000001",
            };

            EngineProc engine = new EngineProc();


            engine.Init(new RAMProgram(program));
            engine.Run();

            string output = engine.GetOutput();

            Assert.AreEqual(expectedResult, output);
        }
        // **************************************************************************



        // Test_LDA_PROG_2 **********************************************************
        /// <summary>
        /// This will run the following program
        /// 
        /// 0x0 LDA 0xE
        /// 0x1 ADD 0xA
        /// 0x2 ADD 0xA
        /// 0x3 ADD 0xA
        /// 0x4 ADD 0xA
        /// 0x5 ADD 0xA
        /// 0x6 ADD 0xA
        /// 0x7 ADD 0xA
        /// 0x8 ADD 0xA
        /// 0x9 ADD 0xA
        /// 0xA ADD 0xA
        /// 0xB ADD 0xA
        /// 0xC OUT 0xX
        /// 0xD HLT 0xX
        /// 0xE 0x0 0x1
        /// 0xF 0x0 0x1
        /// 
        /// The expected result is OReg: 0000 1100
        /// </summary>
        [TestMethod]
        public void Test_ADD_PROG_2()
        {
            string expectedResult = "00001100";
            List<string> program = new List<string>()
            {
                    "00001110",
                    "00011111",
                    "00011111",
                    "00011111",
                    "00011111",
                    "00011111",
                    "00011111",
                    "00011111",
                    "00011111",
                    "00011111",
                    "00011111",
                    "00011111",
                    "11100000",
                    "11110000",
                    "00000001",
                    "00000001",
            };

            EngineProc engine = new EngineProc();


            engine.Init(new RAMProgram(program));
            engine.Run();

            string output = engine.GetOutput();

            Assert.AreEqual(expectedResult, output);
        }
        // **************************************************************************

        #endregion
        // **************************************************************************

    }
}
