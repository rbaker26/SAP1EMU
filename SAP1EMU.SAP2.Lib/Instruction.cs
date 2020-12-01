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
        public string OpCode { get; set; } = "";
        public string BinCode { get; set; } = "";
        public int TStates { get; set; } = 3;
        public bool AffectsFlags { get; set; } = false;
        public AddressingMode AddressingMode { get; set; } = AddressingMode.None;
        public int Bytes { get; set; } = 1;
        public List<string> MicroCode { get; set; } = new List<string>();
        public List<string>? UpdatedFetchCycleStates { get; set; }
    }
}