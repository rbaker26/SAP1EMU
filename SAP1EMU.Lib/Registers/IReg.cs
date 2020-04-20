using System;
using System.Collections.Generic;
using System.Text;
using SAP1EMU.Lib.Components;

namespace SAP1EMU.Lib.Registers
{
    public class IReg : IObserver<TicTok>
    {

        private string RegContent { get; set; } = "00000000";

        private readonly string controlWordMask = "000011000000"; // LI_ EI_

        private void Exec(TicTok tictok)
        {
            string cw = SEQ.Instance().ControlWord;

            //  TODO - Find a better way of using the mask to get the value
            //          Currently is using hardcoded magic numbers

            // Active Low, Push on Tic
            if (cw[5] == '0' & tictok.ClockState == TicTok.State.Tic)
            {
                // Send A to the WBus
                Wbus.Instance().Value = RegContent;
                System.Console.Error.WriteLine($"I Out: {RegContent}");


            }

            // Active Low, Pull on Tok
            if (cw[4] == '0' && tictok.ClockState == TicTok.State.Tok)
            {
                // Store Wbus val in A
                // Only upper 4 bits
                //string temp = Wbus.Instance().Value.Substring(0, 4);
                //RegContent = temp + "0000"; // Zero out the lowwer 4 bits
                RegContent = Wbus.Instance().Value;

                System.Console.Error.WriteLine($"I In : {RegContent}");

            }
        }

        /// <summary>
        /// For the real ToString, use the ToString_Fram_use() method
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            //  I dont know this this is the best place to put this substring command, but it is needed
            // Currently, 
            return RegContent.Substring(0,4);
        }
        #region IObserver Region
        private IDisposable unsubscriber;
        public virtual void Subscribe(IObservable<TicTok> clock)
        {
            if (clock != null)
                unsubscriber = clock.Subscribe(this);
        }


        void IObserver<TicTok>.OnCompleted()
        {
            Console.WriteLine("The Location Tracker has completed transmitting data to {0}.", "AReg");
            this.Unsubscribe();
        }

        void IObserver<TicTok>.OnError(Exception error)
        {
            Console.WriteLine("{0}: The TicTok cannot be determined.", "AReg");
        }

        void IObserver<TicTok>.OnNext(TicTok value)
        {
            Exec(value);
        }

        public virtual void Unsubscribe()
        {
            unsubscriber.Dispose();
        }
        #endregion


        public  string ToString_Frame_Use()
        {
            return this.RegContent;
        }
    }





}
