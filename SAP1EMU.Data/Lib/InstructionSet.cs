using System;
using System.Collections.Generic;
using System.Text;

namespace SAP1EMU.Data.Lib
{
    public class InstructionSet
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int EmulatorId { get; set; }
        public Emulator Emulator { get; set; }
    }
}
