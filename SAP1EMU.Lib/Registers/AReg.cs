using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAP1EMU.Lib.Components;


namespace SAP1EMU.Lib.Registers
{
    public class AReg : IObserver<TicTok>
    {
        private string RegContent { get; set; }

        // LA_ EA
        private readonly string controlWordMask = "000000110000";
        
        private void Exec(TicTok tictok)
        {
            string cw = SEQ.Instance().ControlWord;

            //  TODO - Find a better way of using the mask to get the value
            //          Currently is using hardcoded magic numbers

            // Active Hi, Push on Tic
            if (cw[7] == '1' & tictok.ClockState == TicTok.State.Tic)
            {
                // Send A to the WBus
                Wbus.Instance().Value = RegContent;

                System.Console.Error.WriteLine($"A Out: {RegContent}");

            }

            // Active Low, Pull on Tok
            if (cw[6] == '0' && tictok.ClockState == TicTok.State.Tok)
            {
                // Store Wbus val in A
                RegContent = Wbus.Instance().Value;
                System.Console.Error.WriteLine($"A In : {RegContent}");

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
            //System.Console.WriteLine("AReg is registered!");
        }

        public virtual void Unsubscribe()
        {
            unsubscriber.Dispose();
        }
        #endregion




        public override string ToString()
        {
            return this.RegContent;
        }
    }
}
