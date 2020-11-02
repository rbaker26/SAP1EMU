using SAP1EMU.SAP2.Lib.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace SAP1EMU.SAP2.Lib.Registers
{
    public class Flag : IObserver<TicTok>
    {
        private readonly ALU aluReg;
        private string RegContent { get; set; }

        public Flag(ref ALU aluReg)
        {
            this.aluReg = aluReg;
        }

        public bool Signed
        {
            get => Convert.ToBoolean(RegContent[0]);
        }

        public bool Zero
        {
            get => Convert.ToBoolean(RegContent[1]);
        }

        private void Exec(TicTok tictok)
        {
            var cw = SEQ.Instance().ControlWord;

            if(string.Equals(cw["LF"], "1", StringComparison.Ordinal) && tictok.ClockState == TicTok.State.Tic)
            {
                RegContent = aluReg.FlagContent;
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
