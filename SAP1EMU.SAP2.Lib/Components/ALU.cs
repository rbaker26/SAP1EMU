using SAP1EMU.SAP2.Lib.Utilities;
using SAP1EMU.SAP2.Lib.Registers;
using System;
using System.Diagnostics;

namespace SAP1EMU.SAP2.Lib.Components
{
    public class ALU : IObserver<TicTok>
    {
        public string RegContent { get; set; }

        public string FlagContent { get; private set; }

        private AReg Areg { get; set; }
        private TReg Treg { get; set; }

        public enum ALUOPType
        {
            ADD,
            SUB,
            AND,
            OR,
            XOR,
            CMA,
            RAL,
            RAR,
            INR,
            DEC,
            OUT
        }

        public ALU(ref AReg aReg, ref TReg tReg)
        {
            Areg = aReg;
            Treg = tReg;
        }

        //************************************************************************************************************************
        private void Exec(TicTok tictok)
        {
            var cw = SEQ.Instance().ControlWord;

            bool result = Enum.TryParse(Convert.ToInt32(cw["ALU"], 2).ToString(), out ALUOPType action);

            if(!result)
            {
                throw new Exception("ALU Operation not supported!");
            }

            //ALU is always working. Never gets a break!
            RegContent = Compute(Areg.ToString(), Treg.ToString(), action);

            // Active Hi, Push on Tic
            if (string.Equals(cw["EU"], "1", StringComparison.Ordinal) && tictok.ClockState == TicTok.State.Tic)
            {
                Multiplexer.Instance().PassThroughToBus(RegContent, Convert.ToBoolean(Convert.ToInt16(cw["UB"])), Convert.ToBoolean(Convert.ToInt16(cw["CLR"])));
            }
        }

        //************************************************************************************************************************

        //************************************************************************************************************************
        public string Compute(string AReg, string TReg, ALUOPType action = ALUOPType.ADD)
        {
            const int MAX_RESULT = 255;
            const int MIN_RESULT = 0;

            int ia = BinConverter.Bin8ToInt(AReg);
            int ib = BinConverter.Bin8ToInt(TReg);

            int result = 0;

            switch (action)
            {
                case ALUOPType.ADD:
                    result = ia + ib;
                    break;
                case ALUOPType.SUB:
                    result = ia - ib;
                    break;
                case ALUOPType.AND:
                    result = ia & ib;
                    break;
                case ALUOPType.OR:
                    result = ia | ib;
                    break;
                case ALUOPType.XOR:
                    result = ia ^ ib;
                    break;
                case ALUOPType.CMA:
                    result = ~ia;
                    break;
                case ALUOPType.RAL:
                    AReg = AReg[1..] + AReg[0];
                    result = BinConverter.Bin8ToInt(AReg);
                    break;
                case ALUOPType.RAR:
                    AReg = AReg[^1] + AReg[0..^1];
                    result = BinConverter.Bin8ToInt(AReg);
                    break;
                case ALUOPType.INR:
                    result = ib + 1;
                    break;
                case ALUOPType.DEC:
                    result = ib - 1;
                    break;
                case ALUOPType.OUT:
                    result = ib & 1; //Check if byte is 3
                    break;
            }

            //Check to see if a flag needs to be set
            if (result > MAX_RESULT || result < MIN_RESULT)
            {
                FlagContent = "10";
            }

            if (result == 0)
            {
                FlagContent = "01";
            }

            string val = BinConverter.IntToBin8(result);

            return val;
        }

        //************************************************************************************************************************

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