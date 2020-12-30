using SAP1EMU.Lib;

using System;

namespace SAP1EMU.Assembler
{
    public class InstructionValidator
    {
        public static bool IsValidInstruction(string instruction, InstructionSet iset)
        {
            if (instruction.ToLower() == "nop")
            {
                return true;
            }
            try
            {
                return !string.IsNullOrEmpty(iset.Instructions.Find(x => x.OpCode.ToLower().Equals(instruction.ToLower())).OpCode);
            }
            catch (NullReferenceException)
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
            return iset.Instructions.Find(x => x.OpCode.ToLower().Equals(instruction.ToLower())).BinCode;
        }
    }
}