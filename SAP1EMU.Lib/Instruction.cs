using System;
using System.Collections.Generic;
using System.Text;



namespace SAP1EMU.Lib
{
    public class Instruction
    {
        public string OpCode { get; set; }
        public string BinCode { get; set; }
        public List<string> MicroCode { get; set; }
    }
}
