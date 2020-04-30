using System;
using System.Collections.Generic;
using System.Text;
using SAP1EMU.Lib.Components;
using SAP1EMU.Lib.Utilities;

namespace SAP1EMU.Lib.Registers
{
    public class PC : IObserver<TicTok>
    {
        // CP EP LM_ CE_ LI_ EI_ LA_ EA SU EU LB_ LO_

        private string RegContent { get; set; }
        private readonly string controlWordMask = "110000000000"; // CP EP
        public PC()
        {
            RegContent = "00000000";
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
                System.Console.Error.WriteLine($"PCOut: {RegContent}");

            }


            // TODO - Still need the other jumps 
            // Active Low - Broadside Load, Pull
            if (cw[13] == '0' & tictok.ClockState == TicTok.State.Tok)
            {
                string count = Wbus.Instance().Value;
                if(count.Length >= 8)
                {
                    count = "0000" + count.Substring(4, 4);
                }

                this.RegContent = count;
                // Check Flags
                // Check intruction
                
                

            // I might change LP_ to CJ for check_jump
            // than i could pass an Ireg* to the PC to it can tell what the instruction is
            // it would be like a mini-jmp register in the PC



                //// Send A to the WBus
                //Wbus.Instance().Value = RegContent;
                //System.Console.Error.WriteLine($"PCOut: {RegContent}");

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
            return RegContent;
        }

    }
}
