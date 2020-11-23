using SAP1EMU.SAP2.Lib.Components;
using System;

namespace SAP1EMU.SAP2.Lib.Registers
{
    public class MDR : IObserver<TicTok>
    {
        public string RegContent { get; private set; }

        private readonly RAM ram;

        public MDR(ref RAM ram)
        {
            this.ram = ram;
        }

        private void Exec(TicTok tictok)
        {
            var cw = SEQ.Instance().ControlWord;

            // Active Low, Push on Tok
            if (string.Equals(cw["EMDR"], "1", StringComparison.Ordinal) && tictok.ClockState == TicTok.State.Tok)
            {
                Wbus.Instance().Value = RegContent;
            }

            // Active Low, Pull on Tok
            if (string.Equals(cw["LMDR_"], "0", StringComparison.Ordinal) && tictok.ClockState == TicTok.State.Tic)
            {
                RegContent = ram.RegContent;
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
    }
}
