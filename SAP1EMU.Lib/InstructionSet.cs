using System;
using System.Collections.Generic;
using System.Text;

namespace SAP1EMU.Lib
{
    public class InstructionSet
    {
        public string SetName { get; set; }
        public string SetDescription { get; set; }
        public List<Instruction> instructions { get; set; }
    }
}
