using System.Collections.Generic;

namespace SAP1EMU.SAP2.Lib
{
    public enum AddressingMode
    {
        Register,
        Immediate,
        Direct,
        None,
    }

    public class Instruction
    {
        public string OpCode { get; set; }
        public string BinCode { get; set; }
        public int TStates { get; set; }
        public bool AffectsFlags { get; set; }
        public AddressingMode AddressingMode { get; set; }
        public int Bytes { get; set; }
        public List<string> MicroCode { get; set; }
        public List<string>? UpdatedFetchCycleStates { get; set; }
    }
}