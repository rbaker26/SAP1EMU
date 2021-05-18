using SAP1EMU.SAP2.Lib.Components;
using SAP1EMU.SAP2.Lib.Utilities;
using System;

namespace SAP1EMU.SAP2.Lib.Registers
{
    public enum FlagResult
    {
        None = 0,
        Carry = 1,
        Parity = 4,
        AuxiliaryCarry = 16,
        Zero = 64,
        Sign = 128
    }

    public class Flag : IObserver<TicTok>
    {
        private readonly ALU aluReg;

        //SZ-AC-P-C Flags ('-' is unused bits)
        public string RegContent { get; private set; } = "00000000";
        public FlagResult flags;

        public Flag(ref ALU aluReg)
        {
            this.aluReg = aluReg;
            flags = FlagResult.None;
        }

        public bool Signed
        {
            get => flags.HasFlag(FlagResult.Sign);
        }

        public bool Zero
        {
            get => flags.HasFlag(FlagResult.Zero);
        }

        public bool Parity
        {
            get => flags.HasFlag(FlagResult.Parity);
        }

        public bool Carry
        {
            get => flags.HasFlag(FlagResult.Carry);
        }

        public bool AuxiliaryCarry
        {
            get => flags.HasFlag(FlagResult.AuxiliaryCarry);
        }

        private void Exec(TicTok tictok)
        {
            var cw = SEQ.Instance().ControlWord;

            if(cw["LF"].IsActiveHigh() && tictok.ClockState == TicTok.State.Tok)
            {
                RegContent = BinConverter.IntToBinary(aluReg.FlagContent, 8);
                flags = (FlagResult)Convert.ToInt16(RegContent, 2);
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
