using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SAP1EMU.Engine;
using SAP1EMU.Lib;

namespace SAP1EMU.Engine.Test
{
    [TestClass]
    public class EngineTest
    {
        IDecoder _decoder = new InstructionDecoder();
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


            engine.Init(new RAMProgram(program), _decoder);
            engine.Run();

            string output = engine.GetOutputReg();

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


            engine.Init(new RAMProgram(program), _decoder);
            engine.Run();

            string output = engine.GetOutputReg();

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


            engine.Init(new RAMProgram(program), _decoder);
            engine.Run();

            string output = engine.GetOutputReg();

            Assert.AreEqual(expectedResult, output);
        }
        // **************************************************************************
        #endregion
        // **************************************************************************

        // ADD Test 1-3 *************************************************************
        #region ADD Test 1-3

        // Test_ADD_PROG_1 **********************************************************
        /// <summary>
        /// This will run the following program
        /// 
        /// 0x0 LDA 0xE
        /// 0x1 ADD 0xF
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


            engine.Init(new RAMProgram(program), _decoder);
            engine.Run();

            string output = engine.GetOutputReg();

            Assert.AreEqual(expectedResult, output);
        }
        // **************************************************************************



        // Test_ADD_PROG_2 **********************************************************
        /// <summary>
        /// This will run the following program
        /// 
        /// 0x0 LDA 0xE
        /// 0x1 ADD 0xF
        /// 0x2 ADD 0xF
        /// 0x3 ADD 0xF
        /// 0x4 ADD 0xF
        /// 0x5 ADD 0xF
        /// 0x6 ADD 0xF
        /// 0x7 ADD 0xF
        /// 0x8 ADD 0xF
        /// 0x9 ADD 0xF
        /// 0xA ADD 0xF
        /// 0xB ADD 0xF
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


            engine.Init(new RAMProgram(program), _decoder);
            engine.Run();

            string output = engine.GetOutputReg();

            Assert.AreEqual(expectedResult, output);
        }
        // **************************************************************************


        // Test_ADD_PROG_3 **********************************************************
        /// <summary>
        /// This will run the following program
        /// 
        /// 0x0 LDA 0xD
        /// 0x1 ADD 0xE
        /// 0x2 ADD 0xF
        /// 0x3 OUT 0xX
        /// 0x4 HLT 0xX
        /// 0x5 NOP 0xX
        /// 0x6 NOP 0xX
        /// 0x7 NOP 0xX
        /// 0x8 NOP 0xX
        /// 0x9 NOP 0xX
        /// 0xA NOP 0xX
        /// 0xB NOP 0xX
        /// 0xC NOP 0xX
        /// 0xD 0x2 0x8
        /// 0xE 0x0 0xF
        /// 0xF 0x0 0xD
        /// 
        /// The expected result is OReg: 0100 0100
        /// </summary>
        [TestMethod]
        public void Test_ADD_PROG_3()
        {
            string expectedResult = "01000100";
            List<string> program = new List<string>()
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

            EngineProc engine = new EngineProc();


            engine.Init(new RAMProgram(program), _decoder);
            engine.Run();

            string output = engine.GetOutputReg();

            Assert.AreEqual(expectedResult, output);
        }
        // **************************************************************************

        #endregion
        // **************************************************************************


        // SUB Test 1-3 *************************************************************
        #region SUB Test 1-3

        // Test_SUB_PROG_1 **********************************************************
        /// <summary>
        /// This will run the following program
        /// 
        /// 0x0 LDA 0xF
        /// 0x1 SUB 0xE
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
        /// 0xE 0x0 0x0
        /// 0xF 0x0 0x1
        /// 
        /// The expected result is OReg: 0000 0000
        /// </summary>
        [TestMethod]
        public void Test_SUB_PROG_1()
        {
            string expectedResult = "00000001";
            List<string> program = new List<string>()
            {
                    "00001111",
                    "00101110",
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
                    "00000001",
            };

            EngineProc engine = new EngineProc();


            engine.Init(new RAMProgram(program), _decoder);
            engine.Run();

            string output = engine.GetOutputReg();

            Assert.AreEqual(expectedResult, output);
        }
        // **************************************************************************



        // Test_SUB_PROG_2 **********************************************************
        /// <summary>
        /// This will run the following program
        /// 
        /// 0x0 LDA 0xF
        /// 0x1 SUB 0xE
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
        /// The expected result is OReg: 0000 0000
        /// </summary>
        [TestMethod]
        public void Test_SUB_PROG_2()
        {
            string expectedResult = "00000000";
            List<string> program = new List<string>()
            {
                    "00001111",
                    "00101110",
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


            engine.Init(new RAMProgram(program), _decoder);
            engine.Run();

            string output = engine.GetOutputReg();

            Assert.AreEqual(expectedResult, output);
        }
        // **************************************************************************


        // Test_SUB_PROG_3 **********************************************************
        /// <summary>
        /// This will run the following program
        /// 
        /// 0x0 LDA 0xF
        /// 0x1 SUB 0xE
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
        /// 0xE 0x9 0x0
        /// 0xF 0xF 0x0
        /// 
        /// The expected result is OReg: 0110 0000
        /// </summary>
        [TestMethod]
        public void Test_SUB_PROG_3()
        {
            string expectedResult = "01100000";
            List<string> program = new List<string>()
            {
                    "00001111",
                    "00101110",
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
                    "10010000",
                    "11110000",
            };

            EngineProc engine = new EngineProc();


            engine.Init(new RAMProgram(program), _decoder);
            engine.Run();

            string output = engine.GetOutputReg();

            Assert.AreEqual(expectedResult, output);
        }
        // **************************************************************************


        #endregion
        // **************************************************************************


        // Overflow Tests 1-3 *******************************************************
        #region Overflow Tests 1-3
        // Test_Overflow_PROG_1 **********************************************************
        /// <summary>
        /// This will run the following program
        /// 
        /// 0x0 LDA 0xF
        /// 0x1 ADD 0xE
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
        /// 0xF 0xF 0xF
        /// 
        /// The expected result is OReg: 0000 0000
        /// </summary>
        [TestMethod]
        public void Test_OVERFLOW_PROG_1()
        {
            string expectedResult = "00000000";
            List<string> program = new List<string>()
            {
                    "00001111",
                    "00011110",
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
                    "11111111",
            };

            EngineProc engine = new EngineProc();


            engine.Init(new RAMProgram(program), _decoder);
            engine.Run();

            string output = engine.GetOutputReg();

            Assert.AreEqual(expectedResult, output);
        }
        // **************************************************************************


        // Test_Overflow_PROG_2 **********************************************************
        /// <summary>
        /// This will run the following program
        /// 
        /// 0x0 LDA 0xF
        /// 0x1 ADD 0xE
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
        /// 0xE 0x0 0x2
        /// 0xF 0xF 0xF
        /// 
        /// The expected result is OReg: 0000 0001
        /// </summary>
        [TestMethod]
        public void Test_OVERFLOW_PROG_2()
        {
            string expectedResult = "00000001";
            List<string> program = new List<string>()
            {
                    "00001111",
                    "00011110",
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
                    "00000010",
                    "11111111",
            };

            EngineProc engine = new EngineProc();


            engine.Init(new RAMProgram(program), _decoder);
            engine.Run();

            string output = engine.GetOutputReg();

            Assert.AreEqual(expectedResult, output);
        }
        // **************************************************************************



        // Test_Overflow_PROG_3 **********************************************************
        /// <summary>
        /// This will run the following program
        /// 
        /// 0x0 LDA 0xF
        /// 0x1 ADD 0xE
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
        /// 0xE 0xF 0xF
        /// 0xF 0xF 0xF
        /// 
        /// The expected result is OReg: 1111 1110
        /// </summary>
        [TestMethod]
        public void Test_OVERFLOW_PROG_3()
        {
            string expectedResult = "11111110";
            List<string> program = new List<string>()
            {
                    "00001111",
                    "00011110",
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
                    "11111111",
                    "11111111",
            };

            EngineProc engine = new EngineProc();


            engine.Init(new RAMProgram(program), _decoder);
            engine.Run();

            string output = engine.GetOutputReg();

            Assert.AreEqual(expectedResult, output);
        }
        // **************************************************************************


        #endregion
        // **************************************************************************


        // Underflow Tests 1-3 ******************************************************
        #region Underflow Test 1-3

        // Test_Underflow_PROG_1 **********************************************************
        /// <summary>
        /// This will run the following program
        /// 
        /// 0x0 LDA 0xF
        /// 0x1 SUB 0xE
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
        /// 0xF 0x0 0x0
        /// 
        /// The expected result is OReg: 1111 1111
        /// </summary>
        [TestMethod]
        public void Test_UNDERFLOW_PROG_1()
        {
            string expectedResult = "11111111";
            List<string> program = new List<string>()
            {
                    "00001111",
                    "00101110",
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
                    "00000000",
            };

            EngineProc engine = new EngineProc();


            engine.Init(new RAMProgram(program), _decoder);
            engine.Run();

            string output = engine.GetOutputReg();

            Assert.AreEqual(expectedResult, output);
        }
        // **************************************************************************


        // Test_Underflow_PROG_2 **********************************************************
        /// <summary>
        /// This will run the following program
        /// 
        /// 0x0 LDA 0xF
        /// 0x1 SUB 0xE
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
        /// 0xE 0x0 0x2
        /// 0xF 0x0 0x0
        /// 
        /// The expected result is OReg: 1111 1110
        /// </summary>
        [TestMethod]
        public void Test_UNDERFLOW_PROG_2()
        {
            string expectedResult = "11111110";
            List<string> program = new List<string>()
            {
                    "00001111",
                    "00101110",
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
                    "00000010",
                    "00000000",
            };

            EngineProc engine = new EngineProc();


            engine.Init(new RAMProgram(program), _decoder);
            engine.Run();

            string output = engine.GetOutputReg();

            Assert.AreEqual(expectedResult, output);
        }
        // **************************************************************************


        // Test_Underflow_PROG_3 **********************************************************
        /// <summary>
        /// This will run the following program
        /// 
        /// 0x0 LDA 0xF
        /// 0x1 SUB 0xE
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
        /// 0xE 0xF 0xF
        /// 0xF 0x0 0x0
        /// 
        /// The expected result is OReg: 0000 0001
        /// </summary>
        [TestMethod]
        public void Test_UNDERFLOW_PROG_3()
        {
            string expectedResult = "00000001";
            List<string> program = new List<string>()
            {
                    "00001111",
                    "00101110",
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
                    "11111111",
                    "00000000",
            };

            EngineProc engine = new EngineProc();


            engine.Init(new RAMProgram(program), _decoder);
            engine.Run();

            string output = engine.GetOutputReg();

            Assert.AreEqual(expectedResult, output);
        }
        // **************************************************************************

        #endregion
        // **************************************************************************

        // STA Tests 1 **************************************************************
        [TestMethod]
        public void Test_STA_PROG_1()
        {
            string expectedResult = "11111111";
            List<string> program = new List<string>()
            {
                    "00001111",
                    "00111110",
                    "00001100",
                    "00001110",
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
                    "11111111",
            };

            EngineProc engine = new EngineProc();


            engine.Init(new RAMProgram(program), _decoder);
            engine.Run();

            string output = engine.GetOutputReg();

            Assert.AreEqual(expectedResult, output);
        }
        // **************************************************************************


        // JMP Tests 1 **************************************************************
        [TestMethod]
        public void Test_JMP_PROG_1()
        {
            string expectedResult = "11111111";
            List<string> program = new List<string>()
            {
                    "00001111",
                    "01000011",
                    "00001110",
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
                    "11111111",
            };

            EngineProc engine = new EngineProc();


            engine.Init(new RAMProgram(program), _decoder);
            engine.Run();

            string output = engine.GetOutputReg();

            Assert.AreEqual(expectedResult, output);
        }
        // **************************************************************************

        // JEQ Tests 1 **************************************************************
        [TestMethod]
        public void Test_JEQ_PROG_1()
        {
            string expectedResult = "00000000";
            List<string> program = new List<string>()
            {
                    "00001111",
                    "01010000",  // JEQ - shouldn't jump, if it does, it will infinite loop
                    "00001101",
                    "01010101",  // JEQ should jump
                    "00001110",  // should never be hit, A=RAM[14]
                    "11100000",
                    "11110000",
                    "00000000",
                    "00000000",
                    "00000000",
                    "00000000",
                    "00000000",
                    "00000000",
                    "00000000",
                    "10101010",
                    "11111111",
            };

            EngineProc engine = new EngineProc();


            engine.Init(new RAMProgram(program), _decoder);
            engine.Run();

            string output = engine.GetOutputReg();

            Assert.AreEqual(expectedResult, output);
        }
        // **************************************************************************


        // JNQ Tests 1 **************************************************************
        [TestMethod]
        public void Test_JNQ_PROG_1()
        {
            string expectedResult = "00000001";
            List<string> program = new List<string>()
            {
                "00001111",
                "01100011",
                "00001110", // This LDA should be skipped
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
                "11111111", // should never be loaded into A
                "00000001"
            };

            EngineProc engine = new EngineProc();


            engine.Init(new RAMProgram(program), _decoder);
            engine.Run();

            string output = engine.GetOutputReg();

            Assert.AreEqual(expectedResult, output);
        }
        // **************************************************************************


        // JLT Tests 1 **************************************************************
        [TestMethod]
        public void Test_JLT_PROG_1()
        {
            string expectedResult = "11111111";
            List<string> program = new List<string>()
            {
                "00001111",
                "01110011", // Should not jump, 1 > 0
                "00001110",
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
                "11111111",
                "00000001"
            };

            EngineProc engine = new EngineProc();


            engine.Init(new RAMProgram(program), _decoder);
            engine.Run();

            string output = engine.GetOutputReg();

            Assert.AreEqual(expectedResult, output);
        }
        // **************************************************************************


        // JLT Tests 2 **************************************************************
        [TestMethod]
        public void Test_JLT_PROG_2()
        {
            string expectedResult = "11111110";
            List<string> program = new List<string>()
            {
                "00001111",
                "01110011", // Should jump, -254 < 0
                "00001110",
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
                "11111111",
                "11111110"
            };

            EngineProc engine = new EngineProc();


            engine.Init(new RAMProgram(program), _decoder);
            engine.Run();

            string output = engine.GetOutputReg();

            Assert.AreEqual(expectedResult, output);
        }
        // **************************************************************************


        // JGT Tests 1 **************************************************************
        [TestMethod]
        public void Test_JGT_PROG_1()
        {
            string expectedResult = "00000001";
            List<string> program = new List<string>()
            {
                "00001111",
                "10000011",
                "00001110",
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
                "11111111",
                "00000001"
            };

            EngineProc engine = new EngineProc();


            engine.Init(new RAMProgram(program), _decoder);
            engine.Run();

            string output = engine.GetOutputReg();

            Assert.AreEqual(expectedResult, output);
        }
        // **************************************************************************


        // JGT Tests 2 **************************************************************
        [TestMethod]
        public void Test_JGT_PROG_2()
        {
            string expectedResult = "11111111";
            List<string> program = new List<string>()
            {
                "00001111",
                "10000011",
                "00001110",
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
                "11111111",
                "10000001"
            };

            EngineProc engine = new EngineProc();


            engine.Init(new RAMProgram(program), _decoder);
            engine.Run();

            string output = engine.GetOutputReg();

            Assert.AreEqual(expectedResult, output);
        }
        // **************************************************************************


        // JIC Test 1 ***************************************************************
        [TestMethod]
        public void Test_JIC_PROG_1()
        {
            string expectedResult = "00000000";
            List<string> program = new List<string>()
            {
                    "00001111", // LDA F
                    "00011110", // ADD E
                    "10010100", // JIC 4
                    "00001101", // LDA D (should miss)
                    "11100000",
                    "11110000",
                    "00000000",
                    "00000000",
                    "00000000",
                    "00000000",
                    "00000000",
                    "00000000",
                    "00000000",
                    "10101010",
                    "00000001",
                    "11111111",
            };

            EngineProc engine = new EngineProc();


            engine.Init(new RAMProgram(program), _decoder);
            engine.Run();

            string output = engine.GetOutputReg();

            Assert.AreEqual(expectedResult, output);
        }
        // **************************************************************************



        // Infinite Loop Detection Test *********************************************
        [TestMethod]
        public void Infinite_Loop_Test()
        {
            List<string> program = new List<string>()
            {
                    "00001111",
                    "01000000",  // JEQ - shouldn't jump, if it does, it will infinite loop
                    "11100000",
                    "11110000",  // JEQ should jump
                    "00000000",  // should never be hit, A=RAM[14]
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

            EngineProc engine = new EngineProc();

            bool caught = false;
            engine.Init(new RAMProgram(program), _decoder);
            try
            {
                engine.Run();
            }
            catch (EngineRuntimeException)
            {
                caught = true;
            }

            if (!caught)
            {
                Assert.Fail();
            }


        }
        // **************************************************************************


    }
}
