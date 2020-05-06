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


            #region Hash Table Region
            // LDA ***********************************************
            ControlTable.Add(HashKey(1, "0000"), "01011110001111");
            ControlTable.Add(HashKey(2, "0000"), "10111110001111");
            ControlTable.Add(HashKey(3, "0000"), "00100110001111");
            ControlTable.Add(HashKey(4, "0000"), "00011010001111");
            ControlTable.Add(HashKey(5, "0000"), "00101100001111");
            ControlTable.Add(HashKey(6, "0000"), "00111110001111");
            // ***************************************************

            // ADD ***********************************************
            ControlTable.Add(HashKey(1, "0001"), "01011110001111");
            ControlTable.Add(HashKey(2, "0001"), "10111110001111");
            ControlTable.Add(HashKey(3, "0001"), "00100110001111");
            ControlTable.Add(HashKey(4, "0001"), "00011010001111");
            ControlTable.Add(HashKey(5, "0001"), "00101110000111");
            ControlTable.Add(HashKey(6, "0001"), "00111100011111");
            // ***************************************************

            // SUB ***********************************************
            ControlTable.Add(HashKey(1, "0010"), "01011110001111");
            ControlTable.Add(HashKey(2, "0010"), "10111110001111");
            ControlTable.Add(HashKey(3, "0010"), "00100110001111");
            ControlTable.Add(HashKey(4, "0010"), "00011010001111");
            ControlTable.Add(HashKey(5, "0010"), "00101110000111");
            ControlTable.Add(HashKey(6, "0010"), "00111100111111");
            // ***************************************************

            // STA ***********************************************
            ControlTable.Add(HashKey(1, "0011"), "01011110001111");
            ControlTable.Add(HashKey(2, "0011"), "10111110001111");
            ControlTable.Add(HashKey(3, "0011"), "00100110001111");
            ControlTable.Add(HashKey(4, "0011"), "00011010001111"); // IE, LM
            ControlTable.Add(HashKey(5, "0011"), "00111111001101"); // EA, LR_
            ControlTable.Add(HashKey(6, "0011"), "00111110001111");
            // ***************************************************

            // TODO - Microcode for T4-T6 for JMP, JEQ, JNQ, JLT, JGT have not been implamented

            // CP EP LM_ CE_ LI_ EI_ LA_ EA SU EU LB_ LO_ |  LR_ LP_

            //// JMP ***********************************************
            ControlTable.Add(HashKey(1, "0100"), "01011110001111");
            ControlTable.Add(HashKey(2, "0100"), "10111110001111");
            ControlTable.Add(HashKey(3, "0100"), "00100110001111");
            ControlTable.Add(HashKey(4, "0100"), "00111010001100");
            ControlTable.Add(HashKey(5, "0100"), "00111110001111");
            ControlTable.Add(HashKey(6, "0100"), "00111110001111");
            //// ***************************************************

            // JEQ ***********************************************
            ControlTable.Add(HashKey(1, "0101"), "01011110001111");
            ControlTable.Add(HashKey(2, "0101"), "10111110001111");
            ControlTable.Add(HashKey(3, "0101"), "00100110001111");
            ControlTable.Add(HashKey(4, "0101"), "00111010001100");
            ControlTable.Add(HashKey(5, "0101"), "00111110001111");
            ControlTable.Add(HashKey(6, "0101"), "00111110001111");
            // ***************************************************

            //// JNQ ***********************************************
            //ControlTable.Add(HashKey(1, "0110"), "0101111000111");
            //ControlTable.Add(HashKey(2, "0110"), "1011111000111");
            //ControlTable.Add(HashKey(3, "0110"), "0010011000111");
            //ControlTable.Add(HashKey(4, "0110"), "0011111000111");
            //ControlTable.Add(HashKey(5, "0110"), "0011111000111");
            //ControlTable.Add(HashKey(6, "0110"), "0011111000111");
            //// ***************************************************

            //// JLT ***********************************************
            //ControlTable.Add(HashKey(1, "0111"), "0101111000111");
            //ControlTable.Add(HashKey(2, "0111"), "1011111000111");
            //ControlTable.Add(HashKey(3, "0111"), "0010011000111");
            //ControlTable.Add(HashKey(4, "0111"), "0011111000111");
            //ControlTable.Add(HashKey(5, "0111"), "0011111000111");
            //ControlTable.Add(HashKey(6, "0111"), "0011111000111");
            //// ***************************************************

            //// JGT ***********************************************
            //ControlTable.Add(HashKey(1, "1000"), "0101111000111");
            //ControlTable.Add(HashKey(2, "1000"), "1011111000111");
            //ControlTable.Add(HashKey(3, "1000"), "0010011000111");
            //ControlTable.Add(HashKey(4, "1000"), "0011111000111");
            //ControlTable.Add(HashKey(5, "1000"), "0011111000111");
            //ControlTable.Add(HashKey(6, "1000"), "0011111000111");
            //// ***************************************************

            // JIC ***********************************************
            ControlTable.Add(HashKey(1, "1001"), "01011110001111");
            ControlTable.Add(HashKey(2, "1001"), "10111110001111");
            ControlTable.Add(HashKey(3, "1001"), "00100110001111");
            ControlTable.Add(HashKey(4, "1001"), "00111010001100");
            ControlTable.Add(HashKey(5, "1001"), "00111110001111");
            ControlTable.Add(HashKey(6, "1001"), "00111110001111");
            // ***************************************************

            // OUT ***********************************************
            ControlTable.Add(HashKey(1, "1110"), "01011110001111");
            ControlTable.Add(HashKey(2, "1110"), "10111110001111");
            ControlTable.Add(HashKey(3, "1110"), "00100110001111");
            ControlTable.Add(HashKey(4, "1110"), "00111111001011");
            ControlTable.Add(HashKey(5, "1110"), "00111110001111");
            ControlTable.Add(HashKey(6, "1110"), "00111110001111");
            // ***************************************************

            // HLT ***********************************************
            ControlTable.Add(HashKey(1, "1111"), "01011110001111");
            ControlTable.Add(HashKey(2, "1111"), "10111110001111");
            ControlTable.Add(HashKey(3, "1111"), "00100110001111");
            ControlTable.Add(HashKey(4, "1111"), "00111110001111");
            ControlTable.Add(HashKey(5, "1111"), "00111110001111");
            ControlTable.Add(HashKey(6, "1111"), "00111110001111");
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
                _instance.SupportedCommandsBinTable.Add("JMP", "0100");
                _instance.SupportedCommandsBinTable.Add("JEQ", "0101");
                _instance.SupportedCommandsBinTable.Add("JIC", "1001");
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
