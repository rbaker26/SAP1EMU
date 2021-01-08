using SAP1EMU.SAP2.Lib.Components;
using System;

namespace SAP1EMU.SAP2.Lib.Registers
{
    public class AReg : IObserver<TicTok>
    {
        private string RegContent { get; set; } = "00000000";

        private ALU alu;

        public void SetALUReference(ref ALU alu)
        {
            this.alu = alu;
        }

        private void Exec(TicTok tictok)
        {
            var cw = SEQ.Instance().ControlWord;

            // Active Hi, Push on Tic
            if (string.Equals(cw["EA"], "1", StringComparison.Ordinal) & tictok.ClockState == TicTok.State.Tic)
            {
                // Send A to the WBus while checking if we want to output to the bus upper or lower 8 bits
                Multiplexer.Instance().PassThroughToBus(RegContent, Convert.ToBoolean(Convert.ToInt16(cw["UB"])), Convert.ToBoolean(Convert.ToInt16(cw["CLR"])));
            }

            // Active Low, Pull on Tok
            if (string.Equals(cw["LA_"], "0", StringComparison.Ordinal) && tictok.ClockState == TicTok.State.Tok)
            {
                // Special logic so we dont have immediate feedback when ALU -> A happens on the updated fetch cycle
                // Only applies to A Register
                if(string.Equals(cw["EU"], "1", StringComparison.Ordinal))
                {
                    return;
                }

                // Store Wbus val in A
                RegContent = Wbus.Instance().Value[8..];
            }

            // Special logic so we dont have immediate feedback when ALU -> A happens on the updated fetch cycle
            // Only applies to A Register
            if (string.Equals(cw["EU"], "1", StringComparison.Ordinal) && tictok.ClockState == TicTok.State.Tic)
            {
                // Store ALU in A
                RegContent = alu.RegContent;
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