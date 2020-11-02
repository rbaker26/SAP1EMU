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
                //PC
                ControlWord["CP"] = value[0..1];
                ControlWord["EP"] = value[1..2];
                ControlWord["LP_"] = value[2..3];

                //IR
                ControlWord["LI_"] = value[3..4];

                //Input
                ControlWord["EIP1"] = value[4..5];
                ControlWord["EIP2"] = value[5..6];

                //Memory
                ControlWord["LM_"] = value[6..7];
                ControlWord["EM_"] = value[7..8];

                ControlWord["LR_"] = value[8..9];
                ControlWord["ER_"] = value[9..10];

                ControlWord["LMDR_"] = value[10..11];
                ControlWord["EMDR"] = value[11..12];

                //Registers/Flag/ALU Output
                ControlWord["LA_"] = value[13..14];
                ControlWord["EA"] = value[14..15];

                ControlWord["LT_"] = value[15..16];
                ControlWord["ET"] = value[16..17];

                ControlWord["LB_"] = value[17..18];
                ControlWord["EB"] = value[18..19];

                ControlWord["LC_"] = value[19..20];
                ControlWord["EC"] = value[20..21];

                ControlWord["LF"] = value[21..22];

                ControlWord["EU"] = value[22..23];

                //Output
                ControlWord["L03_"] = value[23..24];
                ControlWord["L04_"] = value[24..25];

                //ALU
                ControlWord["ALU"] = value[25..29];

                //Jump
                ControlWord["JC"] = value[29..]; 
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

            foreach (Instruction instruction in iset.Instructions)
            {
                //  SupportedCommandsBinTable.Add(instruction.OpCode, instruction.BinCode);

                //TODO::Figure out if this is the TState number and what to do with it
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

                { "LI_", ""},    //Load IR

                { "EIP1", "" },   //Enable Input Port 1
                { "EIP2", "" },   //Enable Input Port 2

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