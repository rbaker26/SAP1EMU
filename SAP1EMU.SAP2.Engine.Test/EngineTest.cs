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
        /// The expected result is OReg: 00000111
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

            using (StreamWriter file = new StreamWriter("ADD_PROG_2_FRAMES.txt"))
            {
                foreach (var frame in engine.FrameStack())
                {
                    file.WriteLine(frame);
                }
            }

            Assert.AreEqual(expectedResult, actualOutput);
        }

        #endregion

        #region SUB 1-2

        // Test_SUB_1 **********************************************************
        /// <summary>
        /// This will run the following program
        ///
        /// 0x0 MVI B,0xF
        /// 0x1 SUB B
        /// 0x2 OUT 0x3
        /// 0x3 HLT
        ///
        /// The expected result is OReg: 11110001
        /// </summary>
        [TestMethod]
        public void Test_SUB_PROG_1()
        {
            string expectedResult = "11110001";

            List<string> program = new List<string>()
            {
                "00000110",
                "00001111",
                "10010000",
                "11010011",
                "00000011",
                "01110110"
            };

            EngineProc engine = new EngineProc();

            RAMProgram rp = new RAMProgram(program);

            engine.Init(rp, _decoder);
            engine.Run();

            var actualOutput = engine.GetOutputReg();

            using (StreamWriter file = new StreamWriter("SUB_PROG_1_FRAMES.txt"))
            {
                foreach (var frame in engine.FrameStack())
                {
                    file.WriteLine(frame);
                }
            }

            Assert.AreEqual(expectedResult, actualOutput);
        }

        // Test_SUB_2 **********************************************************
        /// <summary>
        /// This will run the following program
        ///
        /// 0x0 MVI B,0x5
        /// 0x1 SUB B
        /// 0x2 OUT 0x3
        /// 0x3 MVI C,0x2
        /// 0x4 SUB C
        /// 0x5 OUT 0x3
        /// 0x6 HLT
        ///
        /// The expected result is OReg: 11111001
        /// </summary>
        [TestMethod]
        public void Test_SUB_PROG_2()
        {
            string expectedResult = "11111001";

            List<string> program = new List<string>()
            {
                "00000110",
                "00000101",
                "10010000",
                "11010011",
                "00000011",
                "00001110",
                "00000010",
                "10010001",
                "11010011",
                "00000011",
                "01110110"
            };

            EngineProc engine = new EngineProc();

            RAMProgram rp = new RAMProgram(program);

            engine.Init(rp, _decoder);
            engine.Run();

            var actualOutput = engine.GetOutputReg();

            using (StreamWriter file = new StreamWriter("SUB_PROG_2_FRAMES.txt"))
            {
                foreach (var frame in engine.FrameStack())
                {
                    file.WriteLine(frame);
                }
            }

            Assert.AreEqual(expectedResult, actualOutput);
        }

        #endregion

        #region AND 1-3

        // Test_AND_1 **********************************************************
        /// <summary>
        /// This will run the following program
        ///
        /// 0x0 MVI A,0xA
        /// 0x1 MVI B,0x3
        /// 0x2 ANA B
        /// 0x3 OUT 0x3
        /// 0x4 HLT
        ///
        /// The expected result is OReg: 00000010
        /// </summary>
        [TestMethod]
        public void Test_AND_PROG_1()
        {
            string expectedResult = "00000010";

            List<string> program = new List<string>()
            {
                "00111110",
                "00001010",
                "00000110",
                "00000011",
                "10100000",
                "11010011",
                "00000011",
                "01110110"
            };

            EngineProc engine = new EngineProc();

            RAMProgram rp = new RAMProgram(program);

            engine.Init(rp, _decoder);
            engine.Run();

            var actualOutput = engine.GetOutputReg();

            using (StreamWriter file = new StreamWriter("AND_PROG_1_FRAMES.txt"))
            {
                foreach (var frame in engine.FrameStack())
                {
                    file.WriteLine(frame);
                }
            }

            Assert.AreEqual(expectedResult, actualOutput);
        }

        // Test_AND_1 **********************************************************
        /// <summary>
        /// This will run the following program
        ///
        /// 0x0 MVI A,0xF
        /// 0x1 MVI B,0xE
        /// 0x2 MVI C,0x4
        /// 0x3 ANA B
        /// 0x4 ANA C
        /// 0x5 OUT 0x3
        /// 0x6 HLT
        ///
        /// The expected result is OReg: 00000100
        /// </summary>
        [TestMethod]
        public void Test_AND_PROG_2()
        {
            string expectedResult = "00000100";

            List<string> program = new List<string>()
            {
                "00111110",
                "00001111",
                "00000110",
                "00001110",
                "00001110",
                "00000100", 
                "10100000",
                "10100001",
                "11010011",
                "00000011",
                "01110110"
            };

            EngineProc engine = new EngineProc();

            RAMProgram rp = new RAMProgram(program);

            engine.Init(rp, _decoder);
            engine.Run();

            var actualOutput = engine.GetOutputReg();

            using (StreamWriter file = new StreamWriter("AND_PROG_2_FRAMES.txt"))
            {
                foreach (var frame in engine.FrameStack())
                {
                    file.WriteLine(frame);
                }
            }

            Assert.AreEqual(expectedResult, actualOutput);
        }

        // Test_AND_3 **********************************************************
        /// <summary>
        /// This will run the following program
        ///
        /// 0x0 MVI A,0xA
        /// 0x1 ANI 0x3
        /// 0x2 OUT 0x3
        /// 0x3 HLT
        ///
        /// The expected result is OReg: 00000010
        /// </summary>
        [TestMethod]
        public void Test_ANI_PROG_1()
        {
            string expectedResult = "00000010";

            List<string> program = new List<string>()
            {
                "00111110",
                "00001010",
                "11100110",
                "00000011",
                "11010011",
                "00000011",
                "01110110"
            };

            EngineProc engine = new EngineProc();

            RAMProgram rp = new RAMProgram(program);

            engine.Init(rp, _decoder);
            engine.Run();

            var actualOutput = engine.GetOutputReg();

            using (StreamWriter file = new StreamWriter("ANI_PROG_1_FRAMES.txt"))
            {
                foreach (var frame in engine.FrameStack())
                {
                    file.WriteLine(frame);
                }
            }

            Assert.AreEqual(expectedResult, actualOutput);
        }

        #endregion

        #region OR 1-3

        // Test_OR_1 **********************************************************
        /// <summary>
        /// This will run the following program
        ///
        /// 0x0 MVI B,0xAF
        /// 0x1 ORA B
        /// 0x2 OUT 0x3
        /// 0x3 HLT
        ///
        /// The expected result is OReg: 10101111
        /// </summary>
        [TestMethod]
        public void Test_OR_PROG_1()
        {
            string expectedResult = "10101111";

            List<string> program = new List<string>()
            {
                "00000110",
                "10101111",
                "10110000",
                "11010011",
                "00000011",
                "01110110"
            };

            EngineProc engine = new EngineProc();

            RAMProgram rp = new RAMProgram(program);

            engine.Init(rp, _decoder);
            engine.Run();

            var actualOutput = engine.GetOutputReg();

            using (StreamWriter file = new StreamWriter("OR_PROG_1_FRAMES.txt"))
            {
                foreach (var frame in engine.FrameStack())
                {
                    file.WriteLine(frame);
                }
            }

            Assert.AreEqual(expectedResult, actualOutput);
        }

        // Test_OR_2 **********************************************************
        /// <summary>
        /// This will run the following program
        ///
        /// 0x0 MVI A,0xAF
        /// 0x1 MVI B,0xE2
        /// 0x2 MVI C,0x4
        /// 0x3 ORA B
        /// 0x4 ORA C
        /// 0x5 OUT 0x3
        /// 0x6 HLT
        ///
        /// The expected result is OReg: 11101111
        /// </summary>
        [TestMethod]
        public void Test_OR_PROG_2()
        {
            string expectedResult = "11101111";

            List<string> program = new List<string>()
            {
                "00111110",
                "10101111",
                "00000110",
                "11100010",
                "00001110",
                "00000100",
                "10110000",
                "10110001",
                "11010011",
                "00000011",
                "01110110"
            };

            EngineProc engine = new EngineProc();

            RAMProgram rp = new RAMProgram(program);

            engine.Init(rp, _decoder);
            engine.Run();

            var actualOutput = engine.GetOutputReg();

            using (StreamWriter file = new StreamWriter("OR_PROG_2_FRAMES.txt"))
            {
                foreach (var frame in engine.FrameStack())
                {
                    file.WriteLine(frame);
                }
            }

            Assert.AreEqual(expectedResult, actualOutput);
        }

        // Test_OR_3 **********************************************************
        /// <summary>
        /// This will run the following program
        ///
        /// 0x0 MVI A,0xA
        /// 0x1 ORI 0x3
        /// 0x2 OUT 0x3
        /// 0x3 HLT
        ///
        /// The expected result is OReg: 00001011
        /// </summary>
        [TestMethod]
        public void Test_ORI_PROG_1()
        {
            string expectedResult = "00001011";

            List<string> program = new List<string>()
            {
                "00111110",
                "00001010",
                "11110110",
                "00000011",
                "11010011",
                "00000011",
                "01110110"
            };

            EngineProc engine = new EngineProc();

            RAMProgram rp = new RAMProgram(program);

            engine.Init(rp, _decoder);
            engine.Run();

            var actualOutput = engine.GetOutputReg();

            using (StreamWriter file = new StreamWriter("ORI_PROG_1_FRAMES.txt"))
            {
                foreach (var frame in engine.FrameStack())
                {
                    file.WriteLine(frame);
                }
            }

            Assert.AreEqual(expectedResult, actualOutput);
        }

        #endregion

        #region XOR 1-3

        // Test_XOR_1 **********************************************************
        /// <summary>
        /// This will run the following program
        ///
        /// 0x0 MVI B,0xAA
        /// 0x1 MVI A,0x55
        /// 0x2 XRA B
        /// 0x3 OUT 0x3
        /// 0x4 HLT
        ///
        /// The expected result is OReg: 11111111
        /// </summary>
        [TestMethod]
        public void Test_XOR_PROG_1()
        {
            string expectedResult = "11111111";

            List<string> program = new List<string>()
            {
                "00000110",
                "10101010",
                "00111110",
                "01010101",
                "10101000",
                "11010011",
                "00000011",
                "01110110"
            };

            EngineProc engine = new EngineProc();

            RAMProgram rp = new RAMProgram(program);

            engine.Init(rp, _decoder);
            engine.Run();

            var actualOutput = engine.GetOutputReg();

            using (StreamWriter file = new StreamWriter("XOR_PROG_1_FRAMES.txt"))
            {
                foreach (var frame in engine.FrameStack())
                {
                    file.WriteLine(frame);
                }
            }

            Assert.AreEqual(expectedResult, actualOutput);
        }

        // Test_XOR_2 **********************************************************
        /// <summary>
        /// This will run the following program
        ///
        /// 0x0 MVI A,0xAF
        /// 0x1 MVI B,0xE2
        /// 0x2 MVI C,0x4
        /// 0x3 XRA B
        /// 0x4 XRA C
        /// 0x5 OUT 0x3
        /// 0x6 HLT
        ///
        /// The expected result is OReg: 01001001
        /// </summary>
        [TestMethod]
        public void Test_XOR_PROG_2()
        {
            string expectedResult = "01001001";

            List<string> program = new List<string>()
            {
                "00111110",
                "10101111",
                "00000110",
                "11100010",
                "00001110",
                "00000100",
                "10101000",
                "10101001",
                "11010011",
                "00000011",
                "01110110"
            };

            EngineProc engine = new EngineProc();

            RAMProgram rp = new RAMProgram(program);

            engine.Init(rp, _decoder);
            engine.Run();

            var actualOutput = engine.GetOutputReg();

            using (StreamWriter file = new StreamWriter("XOR_PROG_2_FRAMES.txt"))
            {
                foreach (var frame in engine.FrameStack())
                {
                    file.WriteLine(frame);
                }
            }

            Assert.AreEqual(expectedResult, actualOutput);
        }

        // Test_XOR_3 **********************************************************
        /// <summary>
        /// This will run the following program
        ///
        /// 0x0 MVI A,0xA
        /// 0x1 XRI 0x3
        /// 0x2 OUT 0x3
        /// 0x3 HLT
        ///
        /// The expected result is OReg: 00001001
        /// </summary>
        [TestMethod]
        public void Test_XRI_PROG_1()
        {
            string expectedResult = "00001001";

            List<string> program = new List<string>()
            {
                "00111110",
                "00001010",
                "11101110",
                "00000011",
                "11010011",
                "00000011",
                "01110110"
            };

            EngineProc engine = new EngineProc();

            RAMProgram rp = new RAMProgram(program);

            engine.Init(rp, _decoder);
            engine.Run();

            var actualOutput = engine.GetOutputReg();

            using (StreamWriter file = new StreamWriter("XRI_PROG_1_FRAMES.txt"))
            {
                foreach (var frame in engine.FrameStack())
                {
                    file.WriteLine(frame);
                }
            }

            Assert.AreEqual(expectedResult, actualOutput);
        }

        #endregion

        #region CMA

        // Test_CMA **********************************************************
        /// <summary>
        /// This will run the following program
        ///
        /// 0x0 MVI A,0xAF
        /// 0x1 CMA
        /// 0x2 OUT 0x3
        /// 0x3 HLT
        ///
        /// The expected result is OReg: 01010000
        /// </summary>
        [TestMethod]
        public void Test_CMA_PROG_1()
        {
            string expectedResult = "01010000";

            List<string> program = new List<string>()
            {
                "00111110",
                "10101111",
                "00101111",
                "11010011",
                "00000011",
                "01110110"
            };

            EngineProc engine = new EngineProc();

            RAMProgram rp = new RAMProgram(program);

            engine.Init(rp, _decoder);
            engine.Run();

            var actualOutput = engine.GetOutputReg();

            using (StreamWriter file = new StreamWriter("XOR_PROG_2_FRAMES.txt"))
            {
                foreach (var frame in engine.FrameStack())
                {
                    file.WriteLine(frame);
                }
            }

            Assert.AreEqual(expectedResult, actualOutput);
        }

        #endregion

        #region RAL

        // Test_RAL **********************************************************
        /// <summary>
        /// This will run the following program
        ///
        /// 0x0 MVI A,0xAF
        /// 0x1 RAL
        /// 0x2 OUT 0x3
        /// 0x3 HLT
        ///
        /// The expected result is OReg: 0101111
        /// </summary>
        [TestMethod]
        public void Test_RAL_PROG_1()
        {
            string expectedResult = "01011111";

            List<string> program = new List<string>()
            {
                "00111110",
                "10101111",
                "00010111",
                "11010011",
                "00000011",
                "01110110"
            };

            EngineProc engine = new EngineProc();

            RAMProgram rp = new RAMProgram(program);

            engine.Init(rp, _decoder);
            engine.Run();

            var actualOutput = engine.GetOutputReg();

            using (StreamWriter file = new StreamWriter("RAL_PROG_1_FRAMES.txt"))
            {
                foreach (var frame in engine.FrameStack())
                {
                    file.WriteLine(frame);
                }
            }

            Assert.AreEqual(expectedResult, actualOutput);
        }

        #endregion

        #region RAR

        // Test_RAR **********************************************************
        /// <summary>
        /// This will run the following program
        ///
        /// 0x0 MVI A,0xAF
        /// 0x1 RAR
        /// 0x2 OUT 0x3
        /// 0x3 HLT
        ///
        /// The expected result is OReg: 11010111
        /// </summary>
        [TestMethod]
        public void Test_RAR_PROG_1()
        {
            string expectedResult = "11010111";

            List<string> program = new List<string>()
            {
                "00111110",
                "10101111",
                "00011111",
                "11010011",
                "00000011",
                "01110110"
            };

            EngineProc engine = new EngineProc();

            RAMProgram rp = new RAMProgram(program);

            engine.Init(rp, _decoder);
            engine.Run();

            var actualOutput = engine.GetOutputReg();

            using (StreamWriter file = new StreamWriter("RAR_PROG_1_FRAMES.txt"))
            {
                foreach (var frame in engine.FrameStack())
                {
                    file.WriteLine(frame);
                }
            }

            Assert.AreEqual(expectedResult, actualOutput);
        }

        #endregion

        #region INR 1-3

        // Test_INR_1 **********************************************************
        /// <summary>
        /// This will run the following program
        ///
        /// 0x0 MVI A,0xAF
        /// 0x1 INR A
        /// 0x2 OUT 0x3
        /// 0x3 HLT
        ///
        /// The expected result is OReg: 10110000
        /// </summary>
        [TestMethod]
        public void Test_INR_PROG_1()
        {
            string expectedResult = "10110000";

            List<string> program = new List<string>()
            {
                "00111110",
                "10101111",
                "00111100",
                "11010011",
                "00000011",
                "01110110"
            };

            EngineProc engine = new EngineProc();

            RAMProgram rp = new RAMProgram(program);

            engine.Init(rp, _decoder);
            engine.Run();

            var actualOutput = engine.GetOutputReg();

            using (StreamWriter file = new StreamWriter("INR_PROG_1_FRAMES.txt"))
            {
                foreach (var frame in engine.FrameStack())
                {
                    file.WriteLine(frame);
                }
            }

            Assert.AreEqual(expectedResult, actualOutput);
        }

        // Test_INR_2 **********************************************************
        /// <summary>
        /// This will run the following program
        ///
        /// 0x0 MVI B,0xAF
        /// 0x1 INR B
        /// 0x2 MOV A,B
        /// 0x3 OUT 0x3
        /// 0x4 HLT
        ///
        /// The expected result is OReg: 10110000
        /// </summary>
        [TestMethod]
        public void Test_INR_PROG_2()
        {
            string expectedResult = "10110000";

            List<string> program = new List<string>()
            {
                "00000110",
                "10101111",
                "00000100",
                "01111000",
                "11010011",
                "00000011",
                "01110110"
            };

            EngineProc engine = new EngineProc();

            RAMProgram rp = new RAMProgram(program);

            engine.Init(rp, _decoder);
            engine.Run();

            var actualOutput = engine.GetOutputReg();

            using (StreamWriter file = new StreamWriter("INR_PROG_2_FRAMES.txt"))
            {
                foreach (var frame in engine.FrameStack())
                {
                    file.WriteLine(frame);
                }
            }

            Assert.AreEqual(expectedResult, actualOutput);
        }

        // Test_INR_3 **********************************************************
        /// <summary>
        /// This will run the following program
        ///
        /// 0x0 MVI C,0xAF
        /// 0x1 INR C
        /// 0x2 MOV A,C
        /// 0x2 OUT 0x3
        /// 0x3 HLT
        ///
        /// The expected result is OReg: 10110000
        /// </summary>
        [TestMethod]
        public void Test_INR_PROG_3()
        {
            string expectedResult = "10110000";

            List<string> program = new List<string>()
            {
                "00001110",
                "10101111",
                "00001100",
                "01111001",
                "11010011",
                "00000011",
                "01110110"
            };

            EngineProc engine = new EngineProc();

            RAMProgram rp = new RAMProgram(program);

            engine.Init(rp, _decoder);
            engine.Run();

            var actualOutput = engine.GetOutputReg();

            using (StreamWriter file = new StreamWriter("INR_PROG_3_FRAMES.txt"))
            {
                foreach (var frame in engine.FrameStack())
                {
                    file.WriteLine(frame);
                }
            }

            Assert.AreEqual(expectedResult, actualOutput);
        }

        #endregion

        #region DCR 1-3

        // Test_DCR_1 **********************************************************
        /// <summary>
        /// This will run the following program
        ///
        /// 0x0 MVI A,0xAF
        /// 0x1 DCR A
        /// 0x2 OUT 0x3
        /// 0x3 HLT
        ///
        /// The expected result is OReg: 10101110
        /// </summary>
        [TestMethod]
        public void Test_DCR_PROG_1()
        {
            string expectedResult = "10101110";

            List<string> program = new List<string>()
            {
                "00111110",
                "10101111",
                "00111101",
                "11010011",
                "00000011",
                "01110110"
            };

            EngineProc engine = new EngineProc();

            RAMProgram rp = new RAMProgram(program);

            engine.Init(rp, _decoder);
            engine.Run();

            var actualOutput = engine.GetOutputReg();

            using (StreamWriter file = new StreamWriter("DCR_PROG_1_FRAMES.txt"))
            {
                foreach (var frame in engine.FrameStack())
                {
                    file.WriteLine(frame);
                }
            }

            Assert.AreEqual(expectedResult, actualOutput);
        }

        // Test_DCR_2 **********************************************************
        /// <summary>
        /// This will run the following program
        ///
        /// 0x0 MVI B,0xAF
        /// 0x1 DCR B
        /// 0x2 MOV A,B
        /// 0x3 OUT 0x3
        /// 0x4 HLT
        ///
        /// The expected result is OReg: 10101110
        /// </summary>
        [TestMethod]
        public void Test_DCR_PROG_2()
        {
            string expectedResult = "10101110";

            List<string> program = new List<string>()
            {
                "00000110",
                "10101111",
                "00000101",
                "01111000",
                "11010011",
                "00000011",
                "01110110"
            };

            EngineProc engine = new EngineProc();

            RAMProgram rp = new RAMProgram(program);

            engine.Init(rp, _decoder);
            engine.Run();

            var actualOutput = engine.GetOutputReg();

            using (StreamWriter file = new StreamWriter("DCR_PROG_2_FRAMES.txt"))
            {
                foreach (var frame in engine.FrameStack())
                {
                    file.WriteLine(frame);
                }
            }

            Assert.AreEqual(expectedResult, actualOutput);
        }

        // Test_DCR_3 **********************************************************
        /// <summary>
        /// This will run the following program
        ///
        /// 0x0 MVI C,0xAF
        /// 0x1 DCR C
        /// 0x2 MOV A,C
        /// 0x2 OUT 0x3
        /// 0x3 HLT
        ///
        /// The expected result is OReg: 10101110
        /// </summary>
        [TestMethod]
        public void Test_DCR_PROG_3()
        {
            string expectedResult = "10101110";

            List<string> program = new List<string>()
            {
                "00001110",
                "10101111",
                "00001101",
                "01111001",
                "11010011",
                "00000011",
                "01110110"
            };

            EngineProc engine = new EngineProc();

            RAMProgram rp = new RAMProgram(program);

            engine.Init(rp, _decoder);
            engine.Run();

            var actualOutput = engine.GetOutputReg();

            using (StreamWriter file = new StreamWriter("DCR_PROG_3_FRAMES.txt"))
            {
                foreach (var frame in engine.FrameStack())
                {
                    file.WriteLine(frame);
                }
            }

            Assert.AreEqual(expectedResult, actualOutput);
        }

        #endregion

        #region Jumps 1-X

        // Test_JMP **********************************************************
        /// <summary>
        /// This will run the following program
        ///
        /// 0x0 MVI A,0xAF
        /// 0x1 MVI B,0xE2
        /// 0x2 MVI C,0x4
        /// 0x3 JMP 0xB
        /// 0x4 XRA B
        /// 0x5 XRA C
        /// 0x6 OUT 0x3
        /// 0x7 HLT
        ///
        /// The expected result is OReg: 10101111
        /// </summary>
        [TestMethod]
        public void Test_JMP_PROG_1()
        {
            string expectedResult = "10101111";

            List<string> program = new List<string>()
            {
                "00111110",
                "10101111",
                "00000110",
                "11100010",
                "00001110",
                "00000100",
                "11000011",
                "00001011",
                "00000000",
                "10101000",
                "10101001",
                "11010011",
                "00000011",
                "01110110"
            };

            EngineProc engine = new EngineProc();

            RAMProgram rp = new RAMProgram(program);

            engine.Init(rp, _decoder);
            engine.Run();

            var actualOutput = engine.GetOutputReg();

            using (StreamWriter file = new StreamWriter("JMP_PROG_1_FRAMES.txt"))
            {
                foreach (var frame in engine.FrameStack())
                {
                    file.WriteLine(frame);
                }
            }

            Assert.AreEqual(expectedResult, actualOutput);
        }

        // Test_JM **********************************************************
        /// <summary>
        /// This will run the following program
        ///
        /// 0x0 MVI A,0xAF
        /// 0x1 MVI B,0xFF
        /// 0x2 MVI C,0x4
        /// 0x3 SUB B
        /// 0x4 JM 0xC
        /// 0x5 XRA B
        /// 0x6 XRA C
        /// 0x7 OUT 0x3
        /// 0x8 HLT
        ///
        /// The expected result is OReg: 10110000
        /// </summary>
        [TestMethod]
        public void Test_JM_PROG_1()
        {
            string expectedResult = "10110000";

            List<string> program = new List<string>()
            {
                "00111110",
                "10101111",
                "00000110",
                "11111111",
                "00001110",
                "00000100",
                "10010000",
                "11111010",
                "00001100",
                "00000000",
                "10101000",
                "10101001",
                "11010011",
                "00000011",
                "01110110"
            };

            EngineProc engine = new EngineProc();

            RAMProgram rp = new RAMProgram(program);

            engine.Init(rp, _decoder);
            engine.Run();

            var actualOutput = engine.GetOutputReg();

            using (StreamWriter file = new StreamWriter("JM_PROG_1_FRAMES.txt"))
            {
                foreach (var frame in engine.FrameStack())
                {
                    file.WriteLine(frame);
                }
            }

            Assert.AreEqual(expectedResult, actualOutput);
        }

        // Test_JM **********************************************************
        /// <summary>
        /// This will run the following program
        ///
        /// 0x0 MVI A,0xAF
        /// 0x1 MVI B,0xE2
        /// 0x2 MVI C,0x4
        /// 0x3 ADD C
        /// 0x4 JM 0xC
        /// 0x5 XRA B
        /// 0x6 XRA C
        /// 0x7 OUT 0x3
        /// 0x8 HLT
        ///
        /// The expected result is OReg: 01001001
        /// </summary>
        [TestMethod]
        public void Test_JM_PROG_2()
        {
            string expectedResult = "01001001";

            List<string> program = new List<string>()
            {
                "00111110",
                "10101111",
                "00000110",
                "11100010",
                "00001110",
                "00000100",
                "10000001",
                "11111010",
                "00001100",
                "00000000",
                "10101000",
                "10101001",
                "11010011",
                "00000011",
                "01110110"
            };

            EngineProc engine = new EngineProc();

            RAMProgram rp = new RAMProgram(program);

            engine.Init(rp, _decoder);
            engine.Run();

            var actualOutput = engine.GetOutputReg();

            using (StreamWriter file = new StreamWriter("JM_PROG_2_FRAMES.txt"))
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