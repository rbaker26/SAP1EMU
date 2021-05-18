using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SAP1EMU.SAP2.Lib.Test
{
    [TestClass]
    public class DecoderTest
    {
        private IDecoder _decoder = new InstructionDecoder();

        [TestMethod]
        [DataRow("ADD B",   "10000000", "Malvino")]
        [DataRow("ADD C",   "10000001", "Malvino")]
        [DataRow("ANA B",   "10100000", "Malvino")]
        [DataRow("ANA C",   "10100001", "Malvino")]
        [DataRow("ANI",     "11100110", "Malvino")]
        [DataRow("CALL",    "11001101", "Malvino")]
        [DataRow("CMA",     "00101111", "Malvino")]
        [DataRow("DCR A",   "00111101", "Malvino")]
        [DataRow("DCR B",   "00000101", "Malvino")]
        [DataRow("DCR C",   "00001101", "Malvino")]
        [DataRow("HLT",     "01110110", "Malvino")]
        [DataRow("IN",      "11011011", "Malvino")]
        [DataRow("INR A",   "00111100", "Malvino")]
        [DataRow("INR B",   "00000100", "Malvino")]
        [DataRow("INR C",   "00001100", "Malvino")]
        [DataRow("JM",      "11111010", "Malvino")]
        [DataRow("JMP",     "11000011", "Malvino")]
        [DataRow("JNZ",     "11000010", "Malvino")]
        [DataRow("JZ",      "11001010", "Malvino")]
        [DataRow("LDA",     "00111010", "Malvino")]
        [DataRow("MOV A,B", "01111000", "Malvino")]
        [DataRow("MOV A,C", "01111001", "Malvino")]
        [DataRow("MOV B,A", "01000111", "Malvino")]
        [DataRow("MOV B,C", "01000001", "Malvino")]
        [DataRow("MOV C,A", "01001111", "Malvino")]
        [DataRow("MOV C,B", "01001000", "Malvino")]
        [DataRow("MVI A,",  "00111110", "Malvino")]
        [DataRow("MVI B,",  "00000110", "Malvino")]
        [DataRow("MVI C,",  "00001110", "Malvino")]
        [DataRow("NOP",     "00000000", "Malvino")]
        [DataRow("ORA B",   "10110000", "Malvino")]
        [DataRow("ORA C",   "10110001", "Malvino")]
        [DataRow("ORI",     "11110110", "Malvino")]
        [DataRow("OUT",     "11010011", "Malvino")]
        [DataRow("RAL",     "00010111", "Malvino")]
        [DataRow("RAR",     "00011111", "Malvino")]
        [DataRow("RET",     "11001001", "Malvino")]
        [DataRow("STA",     "00110010", "Malvino")]
        [DataRow("SUB B",   "10010000", "Malvino")]
        [DataRow("SUB C",   "10010001", "Malvino")]
        [DataRow("XRA B",   "10101000", "Malvino")]
        [DataRow("XRA C",   "10101001", "Malvino")]
        [DataRow("XRI",     "11101110", "Malvino")]
        public void TestDecode(string OpCode, string BinCode, string SetName)
        {
            Assert.AreEqual(OpCode, _decoder.Decode(BinCode, SetName));
        }
    }
}