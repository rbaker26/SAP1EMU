using System;
using System.Collections.Generic;
using System.Text;



namespace SAP1EMU.Lib
{
    class Instruction
    {
        string OpCode { get; set; }
        string BinCode { get; set; }
        List<string> MicroCode { get; set; }
    }
}
