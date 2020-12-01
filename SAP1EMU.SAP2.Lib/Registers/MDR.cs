using SAP1EMU.SAP2.Lib.Components;
using System;

namespace SAP1EMU.SAP2.Lib.Registers
{
    public class MDR : IObserver<TicTok>
    {
        public string RegContent { get; private set; }

        private RAM ram;

        public MDR() { }

        public void SetRefToRAM(ref RAM ram)
        {
            this.ram = ram;
        }

        private void Exec(TicTok tictok)
        {
            var cw = SEQ.Instance().ControlWord;

            // Both tied to the same signal from ram so when ram is emitting then MDR is changed as well
            if (string.Equals(cw["EM_"], "0", StringComparison.Ordinal) && tictok.ClockState == TicTok.State.Tic)
            {
                RegContent = ram.RegContent;
            }

            // LM_ is tied to MAR -> RAM so i did LR_ for MDR -> RAM
            if (string.Equals(cw["LR_"], "0", StringComparison.Ordinal) && tictok.ClockState == TicTok.State.Tic)
            {
                ram.SetWordAtMARAddress(RegContent);
            }

            // Active High, Push on Tic
            if (string.Equals(cw["EMDR"], "1", StringComparison.Ordinal) && tictok.ClockState == TicTok.State.Tic)
            {
                //Wbus.Instance().Value = RegContent;
                Multiplexer.Instance().PassThroughToBus(RegContent, Convert.ToBoolean(cw["UB"]));
            }

            // Active Low, Pull on Tok
            if (string.Equals(cw["LMDR_"], "0", StringComparison.Ordinal) && tictok.ClockState == TicTok.State.Tok)
            {
                //RegContent = Wbus.Instance().Value;
                RegContent = Multiplexer.Instance().PassThroughToRegister(Convert.ToBoolean(cw["UB"]));
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
