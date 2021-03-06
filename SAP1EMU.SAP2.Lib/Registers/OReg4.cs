﻿using SAP1EMU.SAP2.Lib.Components;
using SAP1EMU.SAP2.Lib.Utilities;
using System;

namespace SAP1EMU.SAP2.Lib.Registers
{
    public class OReg4 : IObserver<TicTok>
    {
        private string RegContent { get; set; } = "00000000";

        private ALU alu { get; set; }

        public OReg4(ref ALU alu)
        {
            this.alu = alu;
        }

        private void Exec(TicTok tictok)
        {
            var cw = SEQ.Instance().ControlWord;

            string aluResult = alu.RegContent;

            // Active Low, Pull on Tok
            if (aluResult[^1].Equals('0') && string.Equals(cw["LO_"], "0", StringComparison.Ordinal) && tictok.ClockState == TicTok.State.Tok)
            {
                // Store Wbus val in Output port 4
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