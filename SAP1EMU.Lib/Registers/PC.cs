using System;
using System.Collections.Generic;
using System.Text;
using SAP1EMU.Lib.Components;
using SAP1EMU.Lib.Utilities;

namespace SAP1EMU.Lib.Registers
{
    public class PC : IObserver<TicTok>
    {
        readonly IReg ireg;
        readonly AReg areg;
        private string RegContent { get; set; }

        public PC(ref AReg areg)
        {
            RegContent = "00000000";
            this.ireg = ireg;
            this.areg = areg;
        }
        public void Exec(TicTok tictok)
        {
            string cw = SEQ.Instance().ControlWord;

            // Active Hi, Count on Tic
            if (cw[0] == '1' && tictok.ClockState == TicTok.State.Tic)
            {
                int count = BinConverter.Bin8ToInt(RegContent);
                count++;
                RegContent = BinConverter.IntToBin8(count);
            }
            // Active Hi, Push on Tic
            if (cw[1] == '1' & tictok.ClockState == TicTok.State.Tic)
            {
                // Send A to the WBus
                Wbus.Instance().Value = RegContent;
            }

            // Active Low - Broadside Load, Pull
            if (cw[13] == '0' & tictok.ClockState == TicTok.State.Tok)
            {
                string count = Wbus.Instance().Value;
                if (count.Length >= 8)
                {
                    count = "0000" + count.Substring(4, 4);
                }


                // The cw[14-16] is a 3-bit jump code that tells the PC which jump code to preform.
                // JMP == 000
                // JEQ == 001
                // JNQ == 010
                // JLT == 011
                // JGT == 100
                // JIC == 101

                
                string jump_code = cw.Substring(14, 3);

                // JMP
                if (jump_code == "000")
                {
                    this.RegContent = count;
                }
                // JEQ
                else if (jump_code == "001")
                {
                    if (areg.ToString() == "00000000")
                    {
                        this.RegContent = count;
                    }
                }
                // JNQ
                else if(jump_code == "010")
                {
                    if (areg.ToString() != "00000000")
                    {
                        this.RegContent = count;
                    }
                }
                // JLT  
                else if (jump_code == "011")
                {
                    if (areg.ToString()[0] == '1')
                    {
                        this.RegContent = count;
                    }
                }
                // JGT 
                else if (jump_code == "100")
                {
                    if (areg.ToString() != "00000000" && areg.ToString()[0] == '0')
                    {
                        this.RegContent = count;
                    }
                }
                // JIC
                else if(jump_code == "101")
                {
                    if (Flags.Instance().Overflow == 1)
                    {
                        this.RegContent = count;
                    }
                }


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
        #endregion


        public override string ToString()
        {
            return RegContent;
        }

    }
}
