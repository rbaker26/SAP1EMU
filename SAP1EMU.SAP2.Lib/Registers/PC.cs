using SAP1EMU.SAP2.Lib.Components;
using SAP1EMU.SAP2.Lib.Utilities;
using System;
using System.Linq;

namespace SAP1EMU.SAP2.Lib.Registers
{
    public class PC : IObserver<TicTok>
    {
        private readonly Flag flagReg;
        private string RegContent { get; set; }

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
            if (string.Equals(cw["EP"], "1", StringComparison.Ordinal) & tictok.ClockState == TicTok.State.Tic)
            {
                // Send A to the WBus
                Wbus.Instance().Value = RegContent;
            }

            // Active Low - Broadside Load, Pull
            if (string.Equals(cw["LP_"], "0", StringComparison.Ordinal) & tictok.ClockState == TicTok.State.Tok)
            {
                //string jumpAddress = Wbus.Instance().Value;
                //string jumpCode = cw["JC"];
                
                //if(jumpAddress.Length < 16)
                //{
                //    jumpAddress = jumpAddress.PadLeft(16, '0');
                //}

                //// JMP
                //if (jumpCode == "000")
                //{
                //    RegContent = jumpAddress;
                //}
                //// JEQ
                //else if (jumpCode == "001")
                //{
                //    if (flagReg.Zero)
                //    {
                //        RegContent = jumpAddress;
                //    }
                //}
                //// JNQ
                //else if (jumpCode == "010")
                //{
                //    if (!flagReg.Zero)
                //    {
                //        RegContent = jumpAddress;
                //    }
                //}
                //// JLT
                //else if (jumpCode == "011")
                //{
                //    if (areg.ToString()[0] == '1')
                //    {
                //        RegContent = jumpAddress;
                //    }
                //}
                //// JGT
                //else if (jumpCode == "100")
                //{
                //    if (areg.ToString() != "00000000" && areg.ToString()[0] == '0')
                //    {
                //        RegContent = jumpAddress;
                //    }
                //}
                //// JIC
                //else if (jumpCode == "101")
                //{
                //    if (flagReg.Signed)
                //    {
                //        RegContent = jumpAddress;
                //    }
                //}

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