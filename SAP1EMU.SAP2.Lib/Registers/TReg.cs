using SAP1EMU.SAP2.Lib.Components;
using System;

namespace SAP1EMU.SAP2.Lib.Registers
{
    public class TReg : IObserver<TicTok>
    {
        private string RegContent { get; set; }

        private void Exec(TicTok tictok)
        {
            var cw = SEQ.Instance().ControlWord;

            // Active Hi, Push on Tic
            if (string.Equals(cw["ET"], "1", StringComparison.Ordinal) && tictok.ClockState == TicTok.State.Tic)
            {
                // Send Temp to the WBus
                Multiplexer.Instance().PassThroughToBus(RegContent, Convert.ToBoolean(cw["UB"]), Convert.ToBoolean(cw["CLR"]));
            }

            // Active Low, Pull on Tok
            if (string.Equals(cw["LT_"], "0", StringComparison.Ordinal) && tictok.ClockState == TicTok.State.Tok)
            {
                // Store Wbus val in Temp
                RegContent = Wbus.Instance().Value[0..8];
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
            return RegContent;
        }

        public string ToString_Frame_Use()
        {
            return (string.IsNullOrEmpty(RegContent) ? "00000000" : RegContent);
        }
    }
}