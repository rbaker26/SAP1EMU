using System;
using System.Collections.Generic;
using System.Text;

namespace SAP1EMU.Data.Lib
{
    public class Emulator
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<InstructionSet> InstructionSets { get; set; }
    }
}
