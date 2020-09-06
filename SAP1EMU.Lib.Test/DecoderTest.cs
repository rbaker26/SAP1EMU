using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace SAP1EMU.Lib.Test
{
    [TestClass]

    public class DecoderTest
    {

        IDecoder _decoder = new InstructionDecoder();
        [TestMethod]
        [DataRow("LDA", "0000", "SAP1Emu")]
        [DataRow("ADD", "0001", "SAP1Emu")]
        [DataRow("SUB", "0010", "SAP1Emu")]
        [DataRow("STA", "0011", "SAP1Emu")]
        [DataRow("JMP", "0100", "SAP1Emu")]
        [DataRow("JEQ", "0101", "SAP1Emu")]

        [DataRow("LDA", "0000", "Malvino")]
        [DataRow("ADD", "0001", "Malvino")]
        [DataRow("SUB", "0010", "Malvino")]
        [DataRow("OUT", "1110", "Malvino")]
        [DataRow("HLT", "1111", "Malvino")]
        [DataRow("???", "0101", "Malvino")] // Should not exist in Malvio


        [DataRow("NOP", "0000", "BenEater")]
        [DataRow("LDA", "0001", "BenEater")]
        [DataRow("ADD", "0010", "BenEater")]
        [DataRow("SUB", "0011", "BenEater")]
        [DataRow("STA", "0100", "BenEater")]
        [DataRow("LDI", "0101", "BenEater")]
        [DataRow("OUT", "1110", "BenEater")]
        [DataRow("HLT", "1111", "BenEater")]
        
        public void TestDecode(string OpCode, string BinCode, string SetName)
        {
            Assert.AreEqual(OpCode, _decoder.Decode(BinCode, SetName));
        }
    }
}
