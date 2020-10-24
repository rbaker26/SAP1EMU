using SAP1EMU.Lib.Components;

using System;

namespace SAP1EMU.Lib.Registers
{
    public class BReg : IObserver<TicTok>
    {
        private string RegContent { get; set; }

        private void Exec(TicTok tictok)
        {
            string cw = SEQ.Instance().ControlWord;

            //  TODO - Find a better way of using the mask to get the value
            //          Currently is using hardcoded magic numbers

            // Active Low, Pull on Tok
            if (cw[10] == '0' && tictok.ClockState == TicTok.State.Tok)
            {
                // Store Wbus val in B
                RegContent = Wbus.Instance().Value;
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
            this.Unsubscribe();
        }

        void IObserver<TicTok>.OnError(Exception error)
        {
            throw error;
        }

        void IObserver<TicTok>.OnNext(TicTok value)
        {
            Exec(value);
        }

        public virtual void Unsubscribe()
        {
            unsubscriber.Dispose();
        }

        #endregion IObserver Region

        public override string ToString()
        {
            return this.RegContent;
        }

        public string ToString_Frame_Use()
        {
            return (String.IsNullOrEmpty(this.RegContent) ? "00000000" : this.RegContent);
        }
    }
}