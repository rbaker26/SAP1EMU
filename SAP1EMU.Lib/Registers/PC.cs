using System;
using System.Collections.Generic;
using System.Text;
using SAP1EMU.Lib.Components;
using SAP1EMU.Lib.Utilities;

namespace SAP1EMU.Lib.Registers
{
    public class PC : IObserver<TicTok>
    {
        // CP EP LM_ CE_ LI_ EI_ LA_ EA SU EU LB_ LO_

        private string RegContent { get; set; }
        private readonly string controlWordMask = "110000000000"; // CP EP
        public PC()
        {
            RegContent = "00000000";
        }
        public void Exec(TicTok tictok)
        {
            string cw = SEQ.Instance().ControlWord;

            // Active Hi, Count on Tic
            if (cw[0] == '1' && tictok.ClockState == TicTok.State.Tic)
            {
                int count = BinConverter.Bin8ToInt(RegContent);
                count++;
                RegContent = BinConverter.IntToBin8(count);
            }


            // Active Hi, Push on Tic
            if (cw[1] == '1' & tictok.ClockState == TicTok.State.Tic)
            {
                // Send A to the WBus
                Wbus.Instance().Value = RegContent;
                System.Console.Error.WriteLine($"PCOut: {RegContent}");

            }

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

    }
}
