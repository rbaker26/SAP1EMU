using SAP1EMU.SAP2.Lib.Components;
using System;

namespace SAP1EMU.SAP2.Lib.Registers
{
    public class BReg : IObserver<TicTok>
    {
        private string RegContent { get; set; } = "00000000";

        private void Exec(TicTok tictok)
        {
            var cw = SEQ.Instance().ControlWord;

            // Active Hi, Push on Tic
            if (string.Equals(cw["EB"], "1", StringComparison.Ordinal) && tictok.ClockState == TicTok.State.Tic)
            {
                // Send B to the WBus
                Multiplexer.Instance().PassThroughToBus(RegContent, Convert.ToBoolean(Convert.ToInt16(cw["UB"])), Convert.ToBoolean(Convert.ToInt16(cw["CLR"])));
            }

            // Active Low, Pull on Tok
            if (string.Equals(cw["LB_"], "0", StringComparison.Ordinal) && tictok.ClockState == TicTok.State.Tok)
            {
                // Store Wbus val in B
                RegContent = Wbus.Instance().Value[8..];
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