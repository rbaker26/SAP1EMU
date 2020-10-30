﻿using SAP1EMU.Lib.Components;
using SAP1EMU.SAP2.Lib.Components;

using System;

namespace SAP1EMU.SAP2.Lib.Registers
{
    public class MReg : IObserver<TicTok>
    {
        private string RegContent { get; set; }
        private readonly RAM ram;

        public MReg(ref RAM ram)
        {
            this.ram = ram;
        }

        private void Exec(TicTok tictok)
        {
            var cw = SEQ.Instance().ControlWord;

            // Active Low, Pull on Tok
            if (string.Equals(cw["LM_"], "0", StringComparison.Ordinal) && tictok.ClockState == TicTok.State.Tok)
            {
                // Store Wbus val in A
                RegContent = Wbus.Instance().Value;

                // Send the MAR data to the RAM
                ram.IncomingMARData(RegContent);
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
            return this.RegContent;
        }

        public string ToString_Frame_Use()
        {
            return (String.IsNullOrEmpty(this.RegContent) ? "0000 0000" : this.RegContent);
        }
    }
}