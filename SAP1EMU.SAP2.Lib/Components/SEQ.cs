using SAP1EMU.Lib;
using System;
using System.Collections.Generic;

namespace SAP1EMU.SAP2.Lib.Components
{
    public class SEQ
    {
        //************************************************************************************************************************
        private static SEQ _instance; // Singleton Pattern

        /// <summary>
        /// Hold all the control words
        /// </summary>
        private readonly Dictionary<int, string> ControlTable = new Dictionary<int, string>();

        /// <summary>
        /// The control word storage location for all registers and components
        /// <para>
        /// CP EP LM_ CE_ LI_ EI_ LA_ EA SU EU LB_ LO_ |  LR_ LP_ | 0bXXX (Jump Code)
        /// </para>
        /// </summary>
        private string _controlWord 
        {
            get => _controlWord;
            set
            {
                ControlWord["CP"] = value[0..1];
            }
        }
            
        public readonly Dictionary<string, string> ControlWord;

        public readonly Dictionary<string, string> SupportedCommandsBinTable = new Dictionary<string, string>(StringComparer.Ordinal);

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
            _controlWord = ControlTable[hash];
            return _controlWord;
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
            _instance._controlWord = ControlTable[HashKey(6, "1111")]; // sets the default to a NOP
        }

        // Singleton Pattern
        private SEQ()
        {
            ControlWord = new Dictionary<string, string>(StringComparer.Ordinal)
            {
                { "CP", "" },    //Increment PC
                { "EP", "" },    //Enable PC
                { "LP_", "" },   //Load PC

                { "EP1", "" },   //Enable Input Port 1
                { "EP2", "" },   //Enable Input Port 2

                { "LA_", "" },   //Load Accumulator
                { "EA", "" },    //Enable Accumulator
                { "LT_", "" },   //Load Temp
                { "ET", "" },    //Enable Temp
                { "LB_", "" },   //Load B
                { "EB", ""},     //Enable B
                { "LC_", "" },   //Load C
                { "EC", "" },    //Enable C
                { "LF", ""},     //Load flag
                { "EU", "" },    //Enable ALU
                { "ALU", "" },   //ALU Control flags
                { "JC", "" },    //Jump Control flags

                { "LM_", "" },   //Load MAR
                { "EM_", "" },   //Enable MAR
                { "LR_", "" },   //Load RAM
                { "ER_", "" },   //Enable RAM
                { "LMDR_", "" }, //Load Memory Data Register
                { "EMDR", "" },  //Enable Memory Data Register

                { "LI_", ""},    //Load IR
                { "EI_", "" },   //Enable IR

                { "LO3_", "" },  //Load Output port 3
                { "LO4_", "" },  //Load Output port 4
            };
        }

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
            return _controlWord;
        }
    }
}