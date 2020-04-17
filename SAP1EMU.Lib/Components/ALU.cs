using System;
using System.Collections.Generic;
using System.Text;

using SAP1EMU.Lib.Utilities;

namespace SAP1EMU.Lib.Components
{
    public class ALU : IObserver<TicTok>
    {
        //************************************************************************************************************************
        public static void Exec() 
        {

            //TODO - check control word and set wbus if nessicary 
        }
        //************************************************************************************************************************



        //************************************************************************************************************************
        public static string Compute(string AReg, string BReg)
        {
            int ia = BinConverter.Bin8ToInt(AReg);
            int ib = BinConverter.Bin8ToInt(BReg);

            int result;

            // TODO ADD
            if (true)
            {
                result = ia + ib;
            }
            //TODO SUB
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
            // TODO - Check ControlWord
            // Exec();
            System.Console.WriteLine("ALU is registered!");
        }

        public virtual void Unsubscribe()
        {
            unsubscriber.Dispose();
        }
        #endregion




    }
}
