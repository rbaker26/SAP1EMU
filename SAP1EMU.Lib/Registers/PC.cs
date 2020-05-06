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

        public PC(ref IReg ireg, ref AReg areg)
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


            // TODO - Still need the other jumps 
            // Active Low - Broadside Load, Pull
            if (cw[13] == '0' & tictok.ClockState == TicTok.State.Tok)
            {
                string count = Wbus.Instance().Value;
                if (count.Length >= 8)
                {
                    count = "0000" + count.Substring(4, 4);
                }


                // JMP
                if (ireg.ToString().Substring(0,4) == "0100")
                {
                    this.RegContent = count;

                }
                // JEQ
                else if(ireg.ToString().Substring(0, 4) == "0101")
                {
                    if(areg.ToString()=="00000000")
                    {
                        this.RegContent = count;
                    }
                }
                // JIC
                else if (ireg.ToString().Substring(0, 4) == "1001")
                {
                    if(Flags.Instance().Overflow == 1)
                    {
                        this.RegContent = count;
                    }
                }

                // I might change LP_ to CJ for check_jump
                // than i could pass an Ireg* to the PC to it can tell what the instruction is
                // it would be like a mini-jmp register in the PC

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
