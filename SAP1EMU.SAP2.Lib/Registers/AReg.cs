using SAP1EMU.SAP2.Lib.Components;
using System;

namespace SAP1EMU.SAP2.Lib.Registers
{
    public class AReg : IObserver<TicTok>
    {
        private string RegContent { get; set; }

        private void Exec(TicTok tictok)
        {
            var cw = SEQ.Instance().ControlWord;

            // Active Hi, Push on Tic
            if (string.Equals(cw["EA"], "1", StringComparison.Ordinal) & tictok.ClockState == TicTok.State.Tic)
            {
                // Send A to the WBus while checking if we want to output to the bus upper or lower 8 bits
                Multiplexer.Instance().PassThroughToBus(RegContent, Convert.ToBoolean(cw["UB"]));
            }

            // Active Low, Pull on Tok
            if (string.Equals(cw["LA_"], "0", StringComparison.Ordinal) && tictok.ClockState == TicTok.State.Tok)
            {
                // Store Wbus val in A
                RegContent = Wbus.Instance().Value; //Should be length 8
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