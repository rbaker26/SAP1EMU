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
        /// CP EP LM_ CE_ LI_ EI_ LA_ EA SU EU LB_ LO_ |  LR_                 
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


            #region Hash Table Region
            // LDA ***********************************************
            ControlTable.Add(HashKey(1, "0000"), "0101111000111");
            ControlTable.Add(HashKey(2, "0000"), "1011111000111");
            ControlTable.Add(HashKey(3, "0000"), "0010011000111");
            ControlTable.Add(HashKey(4, "0000"), "0001101000111");
            ControlTable.Add(HashKey(5, "0000"), "0010110000111");
            ControlTable.Add(HashKey(6, "0000"), "0011111000111");
            // ***************************************************

            // ADD ***********************************************
            ControlTable.Add(HashKey(1, "0001"), "0101111000111");
            ControlTable.Add(HashKey(2, "0001"), "1011111000111");
            ControlTable.Add(HashKey(3, "0001"), "0010011000111");
            ControlTable.Add(HashKey(4, "0001"), "0001101000111");
            ControlTable.Add(HashKey(5, "0001"), "0010111000011");
            ControlTable.Add(HashKey(6, "0001"), "0011110001111");
            // ***************************************************

            // SUB ***********************************************
            ControlTable.Add(HashKey(1, "0010"), "0101111000111");
            ControlTable.Add(HashKey(2, "0010"), "1011111000111");
            ControlTable.Add(HashKey(3, "0010"), "0010011000111");
            ControlTable.Add(HashKey(4, "0010"), "0001101000111");
            ControlTable.Add(HashKey(5, "0010"), "0010111000011");
            ControlTable.Add(HashKey(6, "0010"), "0011110011111");
            // ***************************************************

            // TODO - Microcode for T4-T6 for STA, STB, JMP, JEQ, JNQ, JLT, JGT have not been implamented
            // CP EP LM_ CE_ LI_ EI_ LA_ EA SU EU LB_ LO_ |  LR_
            // STA ***********************************************
            ControlTable.Add(HashKey(1, "0011"), "0101111000111");
            ControlTable.Add(HashKey(2, "0011"), "1011111000111");
            ControlTable.Add(HashKey(3, "0011"), "0010011000111");
            ControlTable.Add(HashKey(4, "0011"), "0001101000111"); // IE, LM
            ControlTable.Add(HashKey(5, "0011"), "0011111100110"); // EA, LR_
            ControlTable.Add(HashKey(6, "0011"), "0011111000111");
            // ***************************************************

            // STB ***********************************************
            ControlTable.Add(HashKey(1, "0100"), "0101111000111");
            ControlTable.Add(HashKey(2, "0100"), "1011111000111");
            ControlTable.Add(HashKey(3, "0100"), "0010011000111");
            ControlTable.Add(HashKey(4, "0100"), "001111100011");
            ControlTable.Add(HashKey(5, "0100"), "001111100011");
            ControlTable.Add(HashKey(6, "0100"), "001111100011");
            // ***************************************************

            //// JMP ***********************************************
            //ControlTable.Add(HashKey(1, "0101"), "010111100011");
            //ControlTable.Add(HashKey(2, "0101"), "101111100011");
            //ControlTable.Add(HashKey(3, "0101"), "001001100011");
            //ControlTable.Add(HashKey(4, "0101"), "001111100011");
            //ControlTable.Add(HashKey(5, "0101"), "001111100011");
            //ControlTable.Add(HashKey(6, "0101"), "001111100011");
            //// ***************************************************

            //// JEQ ***********************************************
            //ControlTable.Add(HashKey(1, "0110"), "010111100011");
            //ControlTable.Add(HashKey(2, "0110"), "101111100011");
            //ControlTable.Add(HashKey(3, "0110"), "001001100011");
            //ControlTable.Add(HashKey(4, "0110"), "001111100011");
            //ControlTable.Add(HashKey(5, "0110"), "001111100011");
            //ControlTable.Add(HashKey(6, "0110"), "001111100011");
            //// ***************************************************

            //// JNQ ***********************************************
            //ControlTable.Add(HashKey(1, "0111"), "010111100011");
            //ControlTable.Add(HashKey(2, "0111"), "101111100011");
            //ControlTable.Add(HashKey(3, "0111"), "001001100011");
            //ControlTable.Add(HashKey(4, "0111"), "001111100011");
            //ControlTable.Add(HashKey(5, "0111"), "001111100011");
            //ControlTable.Add(HashKey(6, "0111"), "001111100011");
            //// ***************************************************

            //// JLT ***********************************************
            //ControlTable.Add(HashKey(1, "1000"), "010111100011");
            //ControlTable.Add(HashKey(2, "1000"), "101111100011");
            //ControlTable.Add(HashKey(3, "1000"), "001001100011");
            //ControlTable.Add(HashKey(4, "1000"), "001111100011");
            //ControlTable.Add(HashKey(5, "1000"), "001111100011");
            //ControlTable.Add(HashKey(6, "1000"), "001111100011");
            //// ***************************************************

            //// JGT ***********************************************
            //ControlTable.Add(HashKey(1, "1001"), "010111100011");
            //ControlTable.Add(HashKey(2, "1001"), "101111100011");
            //ControlTable.Add(HashKey(3, "1001"), "001001100011");
            //ControlTable.Add(HashKey(4, "1001"), "001111100011");
            //ControlTable.Add(HashKey(5, "1001"), "001111100011");
            //ControlTable.Add(HashKey(6, "1001"), "001111100011");
            //// ***************************************************

            // OUT ***********************************************
            ControlTable.Add(HashKey(1, "1110"), "0101111000111");
            ControlTable.Add(HashKey(2, "1110"), "1011111000111");
            ControlTable.Add(HashKey(3, "1110"), "0010011000111");
            ControlTable.Add(HashKey(4, "1110"), "0011111100101");
            ControlTable.Add(HashKey(5, "1110"), "0011111000111");
            ControlTable.Add(HashKey(6, "1110"), "0011111000111");
            // ***************************************************

            // HLT ***********************************************
            ControlTable.Add(HashKey(1, "1111"), "0101111000111");
            ControlTable.Add(HashKey(2, "1111"), "1011111000111");
            ControlTable.Add(HashKey(3, "1111"), "0010011000111");
            ControlTable.Add(HashKey(4, "1111"), "0011111000111");
            ControlTable.Add(HashKey(5, "1111"), "0011111000111");
            ControlTable.Add(HashKey(6, "1111"), "0011111000111");
            // ***************************************************

            #endregion
        }
        //************************************************************************************************************************



        //************************************************************************************************************************
        private static int HashKey(int TState, string Instruction)
        {
            return HashCode.Combine<int, string>(TState, Instruction);
        }
        //************************************************************************************************************************


        // Singleton Pattern
        private SEQ() { }
        public static SEQ Instance()
        {
            // not thread safe
            if (_instance == null)
            {
                _instance = new SEQ();
            }
            if (_instance.SupportedCommandsBinTable.Count == 0)
            {
                _instance.SupportedCommandsBinTable.Add("LDA", "0000");
                _instance.SupportedCommandsBinTable.Add("ADD", "0001");
                _instance.SupportedCommandsBinTable.Add("SUB", "0010");
                _instance.SupportedCommandsBinTable.Add("STA", "0011");
                _instance.SupportedCommandsBinTable.Add("OUT", "1110");
                _instance.SupportedCommandsBinTable.Add("HLT", "1111");
                _instance.SupportedCommandsBinTable.Add("NOP", "0000");
            }
            return _instance;
        }


        public override string ToString()
        {
            return ControlWord;
        }

    }
}
