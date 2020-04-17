using System;
using System.Collections.Generic;
using System.Text;
using SAP1EMU.Lib.Components;

namespace SAP1EMU.Lib.Registers
{
    public class PC : IObserver<TicTok>
    {
        private string RegContent { get; set; }

        public void Exec()
        {
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
            // TODO - Check ControlWord
            // Exec();
            System.Console.WriteLine("PC is registered!");
        }

        public virtual void Unsubscribe()
        {
            unsubscriber.Dispose();
        }
        #endregion

    }
}
