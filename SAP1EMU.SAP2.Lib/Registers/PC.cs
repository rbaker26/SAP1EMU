using SAP1EMU.SAP2.Lib.Components;
using SAP1EMU.SAP2.Lib.Utilities;
using System;
using System.Linq;

namespace SAP1EMU.SAP2.Lib.Registers
{
    public class PC : IObserver<TicTok>
    {
        private readonly Flag flagReg;
        public string RegContent { get; private set; }

        public bool WontJump { get; set; }

        public PC(ref Flag flagReg)
        {
            RegContent = string.Concat(Enumerable.Repeat('0', 16));
            this.flagReg = flagReg;
        }

        public void Exec(TicTok tictok)
        {
            var cw = SEQ.Instance().ControlWord;

            // Active Hi, Count on Tic
            if (string.Equals(cw["CP"], "1", StringComparison.Ordinal) && tictok.ClockState == TicTok.State.Tic)
            {
                int count = BinConverter.Bin8ToInt(RegContent);
                count++;
                RegContent = BinConverter.IntToBin16(count);
            }

            // Active Hi, Push on Tic
            if (string.Equals(cw["EP"], "1", StringComparison.Ordinal) && tictok.ClockState == TicTok.State.Tic)
            {
                // Send PC to the WBus
                Wbus.Instance().Value = RegContent;
            }

            // Active Low - Broadside Load, Pull
            if (string.Equals(cw["LP_"], "0", StringComparison.Ordinal) && tictok.ClockState == TicTok.State.Tok)
            {
                RegContent = Wbus.Instance().Value;

                string jumpType = cw["JC"];

                WontJump = jumpType switch
                {
                    "001" => !flagReg.Signed,
                    "010" => true,
                    "011" => true,
                    "100" => true,
                    "101" => true,
                    "110" => true,
                    "111" => true,
                    _ => false
                };
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
    }
}