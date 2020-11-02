using System.Collections.Generic;

namespace SAP1EMU.SAP2.Lib
{
    public class InstructionSet
    {
        public string SetName { get; set; }
        public string SetDescription { get; set; }
        public List<Instruction> Instructions { get; set; }
    }
}