using SAP1EMU.SAP2.Lib;

using System;

namespace SAP1EMU.SAP2.Assembler
{
    public class InstructionValidator
    {
        public static bool IsValidInstruction(string instruction, InstructionSet iset)
        {
            try
            {
                return !string.IsNullOrEmpty(iset.Instructions.Find(x => x.OpCode.Equals(instruction.ToUpper())).OpCode);
            }
            catch (NullReferenceException)
            {
                return false;
            }
        }
    }
}