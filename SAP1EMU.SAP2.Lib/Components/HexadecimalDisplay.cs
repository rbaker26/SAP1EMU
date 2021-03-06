﻿using System;
using SAP1EMU.SAP2.Lib.Registers;

namespace SAP1EMU.SAP2.Lib.Components
{
    public class HexadecimalDisplay : IObserver<TicTok>
    {
        public string RegContent { get; set; } = "00";

        private readonly OReg3 outputReg;

        public HexadecimalDisplay(ref OReg3 oreg3)
        {
            outputReg = oreg3;
        }

        private void Exec(TicTok tictok)
        {
            var cw = SEQ.Instance().ControlWord;

            // Active Low, Pull on Tok
            if (string.Equals(cw["LO_"], "0", StringComparison.Ordinal) && tictok.ClockState == TicTok.State.Tok)
            {
                int value = Convert.ToInt16(outputReg.RegContent, 2);
                RegContent = value.ToString("X2");
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
