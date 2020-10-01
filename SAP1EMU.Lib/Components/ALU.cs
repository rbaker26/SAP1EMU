using SAP1EMU.Lib.Registers;
using SAP1EMU.Lib.Utilities;

using System;

namespace SAP1EMU.Lib.Components
{
    public class ALU : IObserver<TicTok>
    {
        private string RegContent { get; set; }

        private AReg Areg { get; set; }
        private BReg Breg { get; set; }

        public ALU(ref AReg areg, ref BReg breg)
        {
            this.Areg = areg;
            this.Breg = breg;
        }

        //************************************************************************************************************************
        private void Exec(TicTok tictok)
        {
            string cw = SEQ.Instance().ControlWord;

            //  TODO - Find a better way of using the mask to get the value
            //          Currently is using hardcoded magic numbers

            string temp;

            // Active Hi, SUB on Tic
            if (cw[8] == '1' && tictok.ClockState == TicTok.State.Tic)
            {
                temp = Compute(Areg.ToString(), Breg.ToString(), false);
            }
            else // ADD
            {
                temp = Compute(Areg.ToString(), Breg.ToString(), true);
            }

            // For Frame ToString support
            RegContent = temp;

            // Active Hi, Push on Tic
            if (cw[9] == '1' & tictok.ClockState == TicTok.State.Tic)
            {
                Wbus.Instance().Value = temp;
            }
        }

        //************************************************************************************************************************

        //************************************************************************************************************************
        public static string Compute(string AReg, string BReg, bool Add = true)
        {
            const int MAX_RESEULT = 255;
            const int MIN_RESULT = 0;

            int ia = BinConverter.Bin8ToInt(AReg);
            int ib = BinConverter.Bin8ToInt(BReg);

            int result;

            if (Add)
            {
                result = ia + ib;

                // Set Flags
                if (result > MAX_RESEULT)
                {
                    Flags.Instance().Overflow = 1;
                }
            }
            else // SUB
            {
                Flags.Instance().Clear();

                result = ia - ib;

                // Set Flags
                if (result < MIN_RESULT)
                {
                    Flags.Instance().Underflow = 1;
                }
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
            return this.RegContent;
        }
    }
}