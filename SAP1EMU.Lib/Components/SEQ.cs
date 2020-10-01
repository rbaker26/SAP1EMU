using System;
using System.Collections.Generic;
using System.Text;

namespace SAP1EMU.Lib.Components
{
    public class SEQ
    {
        //************************************************************************************************************************
        private static SEQ _instance; // Singleton Pattern
            
        /// <summary>
        /// Hold all the control words
        /// </summary>
        private readonly Dictionary<int,string> ControlTable = new Dictionary<int, string>();



        /// <summary>
        /// The control word storage location for all registers and components
        /// <para> 
        /// CP EP LM_ CE_ LI_ EI_ LA_ EA SU EU LB_ LO_ |  LR_ LP_ | 0bXXX (Jump Code)
        /// </para>
        /// </summary>
        public string ControlWord { get; private set; }


        public readonly Dictionary<string, string> SupportedCommandsBinTable = new Dictionary<string, string>();

        //************************************************************************************************************************



        //************************************************************************************************************************
        /// <summary>
        /// Gets the Control Word for the specified TState and Instruction
        /// </summary>
        /// <param name="TState"></param>
        /// <param name="Instruction"></param>
        /// <returns></returns>
        public string UpdateControlWordReg(int TState, string Instruction)
        {
            int hash = HashKey(TState, Instruction);
            ControlWord = ControlTable[hash];
            return ControlWord;
            
        }
        //************************************************************************************************************************



        //************************************************************************************************************************
        private static int HashKey(int TState, string Instruction)
        {
            return HashCode.Combine<int, string>(TState, Instruction);
        }
        //************************************************************************************************************************


        public void Load(InstructionSet iset)
        {
            //SupportedCommandsBinTable.Clear();
            ControlTable.Clear();

            foreach (Instruction instruction in iset.instructions)
            {
              //  SupportedCommandsBinTable.Add(instruction.OpCode, instruction.BinCode);

                for (int i = 0; i < 6; i++)
                {
                    ControlTable.Add(HashKey(i + 1, instruction.BinCode), instruction.MicroCode[i]);
                }
            }
            _instance.ControlWord = ControlTable[HashKey(6,"1111")]; // sets the default to a NOP
            
        }



        // Singleton Pattern
        private SEQ() { }
        public static SEQ Instance()
        {
            // not thread safe
            if (_instance == null)
            {
                _instance = new SEQ();
                //_instance.ControlWord = "00‬1111100011"; 
                // TODO - this wasnt enough chars bc the words got longer, I fixed by addding _instance.ControlWord = ControlTable[0]; abouve
                // not a greate fix, but it works
            }

            return _instance;
        }


        public override string ToString()
        {
            return ControlWord;
        }

    }
}
