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
        /// <para> CP EP LM_ CE_ LI_ EI_ LA_ EA SU EU LB_ LO_</para>
        /// </summary>
        public string ControlWord { get; private set; }
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


            #region Hash Table Regieon
            // LDA ***********************************************
            ControlTable.Add(HashKey(1, "0000"), "010111100011");
            ControlTable.Add(HashKey(2, "0000"), "101111100011‬");
            ControlTable.Add(HashKey(3, "0000"), "001001100011");
            ControlTable.Add(HashKey(4, "0000"), "000110100011‬");
            ControlTable.Add(HashKey(5, "0000"), "001011000011");
            ControlTable.Add(HashKey(6, "0000"), "00‬1111100011");
            // ***************************************************

            // ADD ***********************************************
            ControlTable.Add(HashKey(1, "0001"), "010111100011");
            ControlTable.Add(HashKey(2, "0001"), "101111100011‬");
            ControlTable.Add(HashKey(3, "0001"), "001001100011");
            ControlTable.Add(HashKey(4, "0001"), "000110100011");
            ControlTable.Add(HashKey(5, "0001"), "001011100001");
            ControlTable.Add(HashKey(6, "0001"), "001111000111‬");
            // ***************************************************

            // SUB ***********************************************
            ControlTable.Add(HashKey(1, "0010"), "010111100011");
            ControlTable.Add(HashKey(2, "0010"), "101111100011‬");
            ControlTable.Add(HashKey(3, "0010"), "001001100011");
            ControlTable.Add(HashKey(4, "0010"), "000110100011");
            ControlTable.Add(HashKey(5, "0010"), "001011100001");
            ControlTable.Add(HashKey(6, "0010"), "001111001111");
            // ***************************************************

            // JMP ***********************************************
            ControlTable.Add(HashKey(1, "0011"), "010111100011");
            ControlTable.Add(HashKey(2, "0011"), "101111100011‬");
            ControlTable.Add(HashKey(3, "0011"), "001001100011");
            ControlTable.Add(HashKey(4, "0011"), "000000000000");
            ControlTable.Add(HashKey(5, "0011"), "000000000000");
            ControlTable.Add(HashKey(6, "0011"), "000000000000");
            // ***************************************************

            // OUT ***********************************************
            ControlTable.Add(HashKey(1, "1110"), "010111100011");
            ControlTable.Add(HashKey(2, "1110"), "101111100011‬");
            ControlTable.Add(HashKey(3, "1110"), "001001100011");
            ControlTable.Add(HashKey(4, "1110"), "001011110010");
            ControlTable.Add(HashKey(5, "1110"), "00‬1111100011");
            ControlTable.Add(HashKey(6, "1110"), "00‬1111100011");
            // ***************************************************

            // HLT ***********************************************
            ControlTable.Add(HashKey(1, "1111"), "010111100011");
            ControlTable.Add(HashKey(2, "1111"), "101111100011‬");
            ControlTable.Add(HashKey(3, "1111"), "001001100011");
            ControlTable.Add(HashKey(4, "1111"), "00‬1111100011");
            ControlTable.Add(HashKey(5, "1111"), "00‬1111100011");
            ControlTable.Add(HashKey(6, "1111"), "00‬1111100011");
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
            return _instance;
        }



        //#region IObserver Region
        //private IDisposable unsubscriber;
        //public virtual void Subscribe(IObservable<TicTok> clock)
        //{
        //    if (clock != null)
        //        unsubscriber = clock.Subscribe(this);
        //}


        //void IObserver<TicTok>.OnCompleted()
        //{
        //    Console.WriteLine("The Location Tracker has completed transmitting data to {0}.", "AReg");
        //    this.Unsubscribe();
        //}

        //void IObserver<TicTok>.OnError(Exception error)
        //{
        //    Console.WriteLine("{0}: The TicTok cannot be determined.", "AReg");
        //}

        //void IObserver<TicTok>.OnNext(TicTok value)
        //{
        //    // TODO - Check ControlWord
        //    // Exec();
        //    System.Console.WriteLine("SEQ is registered!");
        //}

        //public virtual void Unsubscribe()
        //{
        //    unsubscriber.Dispose();
        //}
        //#endregion


    }
}
