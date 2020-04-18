using System;
using System.Collections.Generic;
using System.Text;
using SAP1EMU.Lib.Components;

namespace SAP1EMU.Lib.Registers
{
    public class MReg : IObserver<TicTok>
    {
        private string RegContent { get; set; }
        private readonly string controlWordMask = "001000000000"; // LM_
        private readonly RAM ram;
        public MReg(ref RAM ram)
        {
            this.ram = ram;
        }

        private void Exec(TicTok tictok)
        {
            string cw = SEQ.Instance().ControlWord;

            //  TODO - Find a better way of using the mask to get the value
            //          Currently is using hardcoded magic numbers

            // Active Low, Pull on Tok
            if (cw[2] == '0' && tictok.ClockState == TicTok.State.Tok)
            {
                Wbus bus = Wbus.Instance();
                // Store Wbus val in A
                RegContent = Wbus.Instance().Value.Substring(4,4);

                // Send the MAR data to the RAM
                ram.IncomingMARData(RegContent);
                // TODO - likely bug here with this ram pointer bs.
                // I didnt want to do this, but setting up the observer pattern twice in one object was not working well.

            
                System.Console.Error.WriteLine($"M In : {RegContent}");

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
