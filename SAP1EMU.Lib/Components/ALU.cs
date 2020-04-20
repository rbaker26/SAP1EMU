using System;
using System.Collections.Generic;
using System.Text;
using SAP1EMU.Lib.Registers;
using SAP1EMU.Lib.Utilities;

namespace SAP1EMU.Lib.Components
{
    public class ALU : IObserver<TicTok>
    {
        // <para> CP EP LM_ CE_ LI_ EI_ LA_ EA SU EU LB_ LO_</para>

        private readonly string controlWordMask = "000000001100"; // SU EU
        private string RegContent { get; set; }

        private AReg areg { get; set; }
        private BReg breg { get; set; }
        public ALU(ref AReg areg, ref BReg breg)
        {
            this.areg = areg;
            this.breg = breg;
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
                temp = Compute(areg.ToString(), breg.ToString(), false);

            }
            else // ADD
            {
                temp = Compute(areg.ToString(), breg.ToString(), true);

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
            int ia = BinConverter.Bin8ToInt(AReg);
            int ib = BinConverter.Bin8ToInt(BReg);

            int result;

            
            if (Add)
            {
                result = ia + ib;
            }
            else
            {
                result = ia - ib;
            }

            string val = BinConverter.IntToBin8(result);

            return val ;
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
            Console.WriteLine("The Location Tracker has completed transmitting data to {0}.", "AReg");
            this.Unsubscribe();
        }

        void IObserver<TicTok>.OnError(Exception error)
        {
            Console.WriteLine("{0}: The TicTok cannot be determined.", "AReg");
        }

        void IObserver<TicTok>.OnNext(TicTok value)
        {
            Exec(value);
        }

        public virtual void Unsubscribe()
        {
            unsubscriber.Dispose();
        }
        #endregion


        public override string ToString()
        {
            return this.RegContent;
        }

    }
}
