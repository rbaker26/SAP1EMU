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
        /// CP EP LM_ CE_ LI_ EI_ LA_ EA SU EU LB_ LO_ |  LR_ LP_
        /// </para>
        /// </summary>
        public string ControlWord { get; private set; }


        public readonly Dictionary<string, string> SupportedCommandsBinTable = new Dictionary<string, string>();

        //************************************************************************************************************************



        //************************************************************************************************************************
        /// <summary>
        /// Insures the ControlTable is only filled once and 
        /// not access before it is filled.
        /// </summary>
        private bool Initialized = false;
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
            // Check to make sure the hash table has been filled
            if(!Initialized)
            {
                Init();
                Initialized = true;
            }
            int hash = HashKey(TState, Instruction);
            ControlWord = ControlTable[hash];
            return ControlWord;
            
        }
        //************************************************************************************************************************



        //************************************************************************************************************************
        /// <summary>
        /// Initializes the ControlWord Table
        /// <para>
        ///     It emulates the EEPROM that would store the words on the SAP1.
        ///     I did not follow the same mmemory optimization as Ben Eater because that is stupid when running on a x86 system.
        ///     Instead, the table is "addressed" by a hash of the TState and the Instruction
        /// </para>
        /// </summary>
        private void Init()
        {
            // Set ControlWord to NO OPP
            ControlWord = "00‬1111100011";

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
            
        }



        // Singleton Pattern
        private SEQ() { }
        public static SEQ Instance()
        {
            // not thread safe
            if (_instance == null)
            {
                _instance = new SEQ();
            }
            
            return _instance;
        }


        public override string ToString()
        {
            return ControlWord;
        }

    }
}
