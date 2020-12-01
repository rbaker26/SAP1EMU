using SAP1EMU.SAP2.Lib.Components;

using System;

namespace SAP1EMU.SAP2.Lib.Registers
{
    public class IReg : IObserver<TicTok>
    {
        public string RegContent { get; private set; } = "00000000";

        private void Exec(TicTok tictok)
        {
            var cw = SEQ.Instance().ControlWord;

            // Active Low, Pull on Tok
            if (string.Equals(cw["LI_"], "0", StringComparison.Ordinal) && tictok.ClockState == TicTok.State.Tok)
            {
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
            Unsubscribe();
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

        public string ToString_Frame_Use()
        {
            return string.IsNullOrEmpty(RegContent) ? "0000 0000" : RegContent;
        }
    }
}