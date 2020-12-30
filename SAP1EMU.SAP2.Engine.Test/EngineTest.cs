using Microsoft.VisualStudio.TestTools.UnitTesting;

using SAP1EMU.SAP2.Engine;
using SAP1EMU.SAP2.Lib;

using System.Collections.Generic;

namespace SAP1EMU.Engine.Test
{
    [TestClass]
    public class EngineTest
    {
        private IDecoder _decoder = new InstructionDecoder();
        // LDA Tests 1-3 ************************************************************

        #region MVI MOV 1-3

        // Test_LDA_PROG_1 **********************************************************
        /// <summary>
        /// This will run the following program
        ///
        /// 0x0 MVI A,0x5
        /// 0x1 MOV B,A
        /// 0x2 OUT 0x3
        /// 0x3 HLT
        ///
        /// The expected result is OReg: 00000000
        /// Testing to see if B == A
        /// </summary>
        [TestMethod]
        public void Test_MVI_MOV_PROG_1()
        {
            string expectedResult = "00000001";
            List<string> program = new List<string>()
            {
                "00111110",
                "00000101",
                "01000111",
                "01110110"
            };

            EngineProc engine = new EngineProc();

            RAMProgram rp = new RAMProgram(program);
             
            engine.Init(rp, _decoder);
            engine.Run();

            string output = engine.GetOutputReg();

            Assert.AreEqual(expectedResult, output);
        }
    }

    #endregion
}