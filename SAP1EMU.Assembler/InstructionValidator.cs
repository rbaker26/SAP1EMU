using System;
using System.Collections.Generic;
using System.Text;

using SAP1EMU.Lib;

namespace SAP1EMU.Assembler
{
    public class InstructionValidator
    {
        public static bool IsValidInstruction(string instruction, InstructionSet iset)
        {
            if(instruction.ToLower() == "nop")
            {
                return true;
            }
            try
            {
                return !string.IsNullOrEmpty(iset.instructions.Find(x => x.OpCode.ToLower().Equals(instruction.ToLower())).OpCode);
            }
            catch(NullReferenceException)
            {
                return false;
            }
        }
        public static string GetUpperNibble(string instruction, InstructionSet iset)
        {
            if (instruction.ToLower() == "nop")
            {
                return "0000";
            }
            return iset.instructions.Find(x => x.OpCode.ToLower().Equals(instruction.ToLower())).BinCode;
        }
    }
}
