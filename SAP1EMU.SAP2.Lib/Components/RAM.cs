using SAP1EMU.Lib;
using SAP1EMU.Lib.Components;
using SAP1EMU.SAP2.Lib.Registers;
using System;
using System.Collections.Generic;

namespace SAP1EMU.SAP2.Lib.Components
{
    public class RAM : IObserver<TicTok>
    {
        private List<string> RamContents = new List<string>();

        private string MARContents { get; set; }
        public string RegContent { get; private set; } // For ToString()

        private readonly MDR mdrReg;

        private readonly int MIN_RAM_ADDRESS = 0x0800;
        private readonly int MAX_RAM_ADDRESS = 0xFFFF;

        public RAM(ref MDR mdrReg)
        {
            this.mdrReg = mdrReg;
        }

        private void Exec(TicTok tictok)
        {
            var cw = SEQ.Instance().ControlWord;

            // Active Low, Push on Tic (Was CE_ didnt make sense i prefer Enable RAM)
            if (string.Equals(cw["ER_"], "0", StringComparison.Ordinal) && tictok.ClockState == TicTok.State.Tic)
            {
                RegContent = GetWordAt(MARContents);
            }

            // Active Low, Pull on Tok
            if (string.Equals(cw["LR_"], "0", StringComparison.Ordinal) && tictok.ClockState == TicTok.State.Tok)
            {
                string word = mdrReg.RegContent;
                SetWordAt(MARContents, word);
                RegContent = word;
            }
        }

        public void LoadProgram(RAMProgram rp)
        {
            ClearRAM();
            List<string> rpc = rp.RamContents;

            foreach (string s in rpc)
            {
                RamContents.Add(s);
            }
        }

        public string GetWordAt(string addr)
        {
            int index = (int)(Convert.ToUInt32(addr, 2));
            if (index < MIN_RAM_ADDRESS || index > MAX_RAM_ADDRESS)
            {
                throw new ArgumentOutOfRangeException($"RAM Index Error - Addr with value {index} is not inbetween the range of {MIN_RAM_ADDRESS}-{MAX_RAM_ADDRESS}");
            }
            return RamContents[index];
        }

        public void SetWordAt(string addr, string word)
        {
            int index = (int)(Convert.ToUInt32(addr, 2));
            if (index < MIN_RAM_ADDRESS || index > MAX_RAM_ADDRESS)
            {
                throw new ArgumentOutOfRangeException($"RAM Index Error - Addr with value {index} is not inbetween the range of {MIN_RAM_ADDRESS}-{MAX_RAM_ADDRESS}");
            }
            RamContents[index] = word;
        }

        public void ClearRAM()
        {
            RamContents = null;
            RamContents = new List<string>();
        }

        public void IncomingMARData(string mar_data)
        {
            MARContents = mar_data;
            RegContent = GetWordAt(MARContents); // For tostring()
        }

        #region IObserver<TicTok> Region

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

        #endregion IObserver<TicTok> Region

        // For Frame Support
        public IList<string> RAMDump() { return RamContents; }

        public override string ToString()
        {
            return RegContent;
        }

        public string ToString_Frame_Use()
        {
            return (string.IsNullOrEmpty(RegContent) ? "00000000" : RegContent);
        }
    }
}