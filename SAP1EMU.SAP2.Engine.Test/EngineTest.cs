using Microsoft.VisualStudio.TestTools.UnitTesting;

using SAP1EMU.SAP2.Engine;
using SAP1EMU.SAP2.Lib;
using System;
using System.Collections.Generic;
using System.IO;

namespace SAP1EMU.Engine.Test
{
    [TestClass]
    public class EngineTest
    {
        private IDecoder _decoder = new InstructionDecoder();
        // LDA Tests 1-3 ************************************************************

        #region MVI MOV 1-3

        // Test_MVI_MOV_1 **********************************************************
        /// <summary>
        /// This will run the following program
        ///
        /// 0x0 MVI A,0x5
        /// 0x1 MOV B,A
        /// 0x2 OUT 0x3
        /// 0x3 HLT
        ///
        /// The expected result is OReg: 00000101
        /// </summary>
        [TestMethod]
        public void Test_MVI_MOV_PROG_1()
        {
            string expectedResult = "00000101";
            List<string> program = new List<string>()
            {
                "00111110",
                "00000101",
                "01000111",
                "11010011",
                "00000011",
                "01110110"
            };

            EngineProc engine = new EngineProc();

            RAMProgram rp = new RAMProgram(program);

            engine.Init(rp, _decoder);
            engine.Run();

            string output = engine.GetOutputReg();

            using (StreamWriter file = new StreamWriter("MVI_MOV_PROG_1_FRAMES.txt"))
            {
                foreach (var frame in engine.FrameStack())
                {
                    file.WriteLine(frame);
                }
            }

            Assert.AreEqual(expectedResult, output);
        }

        // Test_MVI_MOV_2 **********************************************************
        /// <summary>
        /// This will run the following program
        ///
        /// 0x0 MVI A,0x1
        /// 0x1 MOV B,A
        /// 0x2 MVI B,0xF
        /// 0x3 MOV C,B
        /// 0x4 OUT 0x3
        /// 0x5 HLT
        ///
        /// The expected result is OReg: 00000001
        /// </summary>
        [TestMethod]
        public void Test_MVI_MOV_PROG_2()
        {
            string expectedResult = "00000001";
            List<string> program = new List<string>()
            {
                "00111110",
                "00000001",
                "01000111",
                "00000110",
                "00001111",
                "01001000",
                "11010011",
                "00000011",
                "01110110"
            };

            EngineProc engine = new EngineProc();

            RAMProgram rp = new RAMProgram(program);

            engine.Init(rp, _decoder);
            engine.Run();

            string output = engine.GetOutputReg();

            using (StreamWriter file = new StreamWriter("MVI_MOV_PROG_2_FRAMES.txt"))
            {
                foreach (var frame in engine.FrameStack())
                {
                    file.WriteLine(frame);
                }
            }

            Assert.AreEqual(expectedResult, output);
        }

        // Test_MVI_MOV_3 **********************************************************
        /// <summary>
        /// This will run the following program
        ///
        /// 0x0 MVI C,0x1
        /// 0x1 MOV A,C
        /// 0x2 OUT 0x4
        /// 0x3 HLT
        ///
        /// The expected result is OReg: 00000000
        /// Testing to see if B == A
        /// </summary>
        [TestMethod]
        public void Test_MVI_MOV_PROG_3()
        {
            string expectedResult = "00000000";
            List<string> program = new List<string>()
            {
                "00001110",
                "00000001",
                "01111001",
                "11010011",
                "00000100",
                "01110110"
            };

            EngineProc engine = new EngineProc();

            RAMProgram rp = new RAMProgram(program);

            engine.Init(rp, _decoder);
            engine.Run();

            string output = engine.GetOutputReg();

            using (StreamWriter file = new StreamWriter("MVI_MOV_PROG_3_FRAMES.txt"))
            {
                foreach (var frame in engine.FrameStack())
                {
                    file.WriteLine(frame);
                }
            }

            Assert.AreEqual(expectedResult, output);
        }


        #endregion

        #region LDA 1

        // Test_MVI_MOV_1 **********************************************************
        /// <summary>
        /// This will run the following program
        ///
        /// 0x0 MVI A,0x9
        /// 0x1 STA 0x1000
        /// 0x2 LDA 0x1000
        /// 0x3 HLT
        ///
        /// The expected result is OReg: 00001001
        /// </summary>
        [TestMethod]
        public void Test_LDA_PROG_1()
        {
            string expectedResult = "00001001";
            int addressChanged = 0x1000;

            List<string> program = new List<string>()
            {
                "00111110",
                "00001001",
                "00110010",
                "00000000",
                "00010000",
                "00111010",
                "00000000",
                "00010000",
                "01110110"
            };

            EngineProc engine = new EngineProc();

            RAMProgram rp = new RAMProgram(program);

            engine.Init(rp, _decoder);
            engine.Run();

            string output = engine.GetOutputReg();

            var ram = engine.GetRAMContents();

            using (StreamWriter file = new StreamWriter("LDA_PROG_1_FRAMES.txt"))
            {
                foreach (var frame in engine.FrameStack())
                {
                    file.WriteLine(frame);
                }
            }

            Assert.AreEqual(expectedResult, ram[addressChanged]);
        }

        #endregion

        #region STA 1

        // Test_MVI_MOV_1 **********************************************************
        /// <summary>
        /// This will run the following program
        ///
        /// 0x0 MVI A,0x5
        /// 0x1 STA 0x0080
        /// 0x2 HLT
        ///
        /// The expected result is OReg: 00000101
        /// </summary>
        [TestMethod]
        public void Test_STA_PROG_1()
        {
            string expectedResult = "00000101";
            int addressChanged = 0x0080;

            List<string> program = new List<string>()
            {
                "00111110",
                "00000101",
                "00110010",
                "10000000",
                "00000000",
                "01110110"
            };

            EngineProc engine = new EngineProc();

            RAMProgram rp = new RAMProgram(program);

            engine.Init(rp, _decoder);
            engine.Run();

            var ram = engine.GetRAMContents();

            using (StreamWriter file = new StreamWriter("STA_PROG_1_FRAMES.txt"))
            {
                foreach (var frame in engine.FrameStack())
                {
                    file.WriteLine(frame);
                }
            }

            Assert.AreEqual(expectedResult, ram[addressChanged]);
        }

        #endregion

        #region ADD 1-2

        // Test_ADD_1 **********************************************************
        /// <summary>
        /// This will run the following program
        ///
        /// 0x0 MVI B,0x5
        /// 0x1 ADD B
        /// 0x2 OUT 0x3
        /// 0x3 HLT
        ///
        /// The expected result is OReg: 00000101
        /// </summary>
        [TestMethod]
        public void Test_ADD_PROG_1()
        {
            string expectedResult = "00000101";

            List<string> program = new List<string>()
            {
                "00000110",
                "00000101",
                "10000000",
                "11010011",
                "00000011",
                "01110110"
            };

            EngineProc engine = new EngineProc();

            RAMProgram rp = new RAMProgram(program);

            engine.Init(rp, _decoder);
            engine.Run();

            var actualOutput = engine.GetOutputReg();

            using (StreamWriter file = new StreamWriter("ADD_PROG_1_FRAMES.txt"))
            {
                foreach (var frame in engine.FrameStack())
                {
                    file.WriteLine(frame);
                }
            }

            Assert.AreEqual(expectedResult, actualOutput);
        }

        // Test_ADD_2 **********************************************************
        /// <summary>
        /// This will run the following program
        ///
        /// 0x0 MVI B,0x5
        /// 0x1 ADD B
        /// 0x2 OUT 0x3
        /// 0x3 MVI C,0x2
        /// 0x4 ADD C
        /// 0x5 OUT 0x3
        /// 0x6 HLT
        ///
        /// The expected result is OReg: 00000101
        /// </summary>
        [TestMethod]
        public void Test_ADD_PROG_2()
        {
            string expectedResult = "00000111";

            List<string> program = new List<string>()
            {
                "00000110",
                "00000101",
                "10000000",
                "11010011",
                "00000011",
                "00001110",
                "00000010",
                "10000001",
                "11010011",
                "00000011",
                "01110110"
            };

            EngineProc engine = new EngineProc();

            RAMProgram rp = new RAMProgram(program);

            engine.Init(rp, _decoder);
            engine.Run();

            var actualOutput = engine.GetOutputReg();

            using (StreamWriter file = new StreamWriter("ADD_PROG_1_FRAMES.txt"))
            {
                foreach (var frame in engine.FrameStack())
                {
                    file.WriteLine(frame);
                }
            }

            Assert.AreEqual(expectedResult, actualOutput);
        }

        #endregion
    }
}