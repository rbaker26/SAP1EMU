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

        private void Exec(TicTok value)
        {
            string cw = SEQ.Instance().ControlWord;

            // May have to convert the controlWordMark to a list of int as indexs

            throw new NotImplementedException();
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
    }
}
