﻿using System;
using System.Collections.Generic;
using System.Linq;

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

        private List<Instruction> instructionsThatModifyNextInstruction = new List<Instruction>();

        private readonly List<string> executedInstructions = new List<string>();
        private string lastInstructionBinary = string.Empty;

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

                ControlWord["LMDR_"] = value[9..10];
                ControlWord["EMDR"] = value[10..11];

                //Registers/Flag/ALU Output
                ControlWord["LA_"] = value[12..13];
                ControlWord["EA"] = value[13..14];

                ControlWord["LT_"] = value[14..15];
                ControlWord["ET"] = value[15..16];

                ControlWord["LB_"] = value[16..17];
                ControlWord["EB"] = value[17..18];

                ControlWord["LC_"] = value[18..19];
                ControlWord["EC"] = value[19..29];

                ControlWord["LF"] = value[20..21];

                ControlWord["EU"] = value[21..22];

                //Output
                ControlWord["L03_"] = value[22..23];
                ControlWord["L04_"] = value[23..24];

                //ALU
                ControlWord["ALU"] = value[24..29];

                //Jump
                ControlWord["JC"] = value[29..32];

                //Output to upper byte
                ControlWord["UB"] = value[32..33];
                ControlWord["CLR"] = value[33..34];

                // Hardcode PC address locations
                ControlWord["RTNA"] = value[34..35];
            }
        }
            
        public readonly Dictionary<string, string> ControlWord;

        //************************************************************************************************************************

        //************************************************************************************************************************
        /// <summary>
        /// Gets the Control Word for the specified TState and Instruction
        /// </summary>
        /// <param name="TState"></param>
        /// <param name="Instruction"></param>
        /// <returns></returns>
        public void UpdateControlWordReg(int TState, string instructionBinaryCode)
        {
            int hash = HashKey(TState, instructionBinaryCode);
            _controlWord = ControlTable[hash];

            //Beginning of a new instruction
            if(TState == 1)
            {
                executedInstructions.Add(instructionBinaryCode);
            }

            //If we have more than 1 we need to keep track of the previous one to see if itll influence this instructions fetch cycle control word
            if(executedInstructions.Count > 1 && TState <= 3)
            {
                lastInstructionBinary = executedInstructions[^2]; //similar to count - 2

                Instruction? instruction = instructionsThatModifyNextInstruction.Find(i => string.Equals(i.BinCode, lastInstructionBinary, StringComparison.Ordinal));

                if (instruction != null && instruction.UpdatedFetchCycleStates != null)
                {
                    List<string> updatedMicroCode = instruction.UpdatedFetchCycleStates;

                    //If the code is empty then do nothing to the microcode otherwise modify the control word.
                    if (!string.IsNullOrEmpty(updatedMicroCode[TState]))
                    {
                        _controlWord = updatedMicroCode[TState];
                    }
                }
            }
        }

        //************************************************************************************************************************

        //************************************************************************************************************************
        private static int HashKey(int TState, string Instruction)
        {
            return HashCode.Combine(TState, Instruction);
        }

        //************************************************************************************************************************

        public void Load(InstructionSet iset)
        {
            ControlTable.Clear();

            instructionsThatModifyNextInstruction = iset.Instructions.Where(i => i.UpdatedFetchCycleStates != null).ToList();

            foreach (Instruction instruction in iset.Instructions)
            {
                for (int i = 0; i < instruction.MicroCode.Count; i++)
                {
                    ControlTable.Add(HashKey(i + 1, instruction.BinCode), instruction.MicroCode[i]);
                }
            }

            _instance._controlWord = ControlTable[HashKey(4, "00000000")]; // sets the default to a NOP
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

                { "EIP1", "" },  //Enable Input Port 1
                { "EIP2", "" },  //Enable Input Port 2

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

                { "LM_", "" },   //Load RAM (MAR)
                { "EM_", "" },   //Enable RAM
                { "LR_", "" },   //Load RAM from MDR
                { "LMDR_", "" }, //Load Memory Data Register
                { "EMDR", "" },  //Enable Memory Data Register

                { "LO3_", "" },  //Load Output port 3
                { "LO4_", "" },  //Load Output port 4

                { "UB", "" },    //take bus upper byte if on or output to bus upper byte
                { "CLR", "" },   //Clear bus value when outputting to bus
                { "RTNA", "" }   //Return Address => Marks whether to make MAR point to 0xFFFE or 0xFFFF for pc contents in memory
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