﻿using System.Collections.Generic;

namespace SAP1EMU.Lib
{
    public class InstructionSet
    {
        public string SetName { get; set; }
        public string SetDescription { get; set; }
        public List<Instruction> instructions { get; set; }
    }
}