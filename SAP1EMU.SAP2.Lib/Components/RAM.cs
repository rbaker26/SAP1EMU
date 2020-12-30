using SAP1EMU.SAP2.Lib.Registers;
using System;
using System.Collections.Generic;

namespace SAP1EMU.SAP2.Lib.Components
{
    public class RAM : IObserver<TicTok>
    {
        private List<string> RamContents = new List<string>();

        private string MARContents { get; set; }
        public string RegContent { get; private set; } = "00000000";

        private MDR mdrReg;

        private const int MIN_RAM_ADDRESS = 0x0800;
        private const int MAX_RAM_ADDRESS = 0xFFFF;

        public RAM() { }

        public void SetRefToMDR(ref MDR mdrReg)
        {
            this.mdrReg = mdrReg;
        }

        private void Exec(TicTok tictok)
        {
            var cw = SEQ.Instance().ControlWord;

            // Active Low, Push on Tic (Was CE_ didnt make sense i prefer Enable RAM)
            if (string.Equals(cw["EM_"], "0", StringComparison.Ordinal) && tictok.ClockState == TicTok.State.Tic)
            {
                RegContent = GetWordAt(MARContents);
            }

            // Active Low, Pull on Tok
            if (string.Equals(cw["LM_"], "0", StringComparison.Ordinal) && tictok.ClockState == TicTok.State.Tok)
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
            int index = Convert.ToInt32(addr, 2); // Needs to be from 0x0800 to 0xFFFF
            //if (index < MIN_RAM_ADDRESS || index > MAX_RAM_ADDRESS)
            //{
            //    throw new ArgumentOutOfRangeException($"RAM Index Error - Addr with value {index} is not inbetween the range of {MIN_RAM_ADDRESS}-{MAX_RAM_ADDRESS}");
            //}
            return RamContents[index];
        }

        public void SetWordAt(string addr, string word)
        {
            int index = Convert.ToInt32(addr, 2);
            //if (index < MIN_RAM_ADDRESS || index > MAX_RAM_ADDRESS)
            //{
            //    throw new ArgumentOutOfRangeException($"RAM Index Error - Addr with value {index} is not inbetween the range of {MIN_RAM_ADDRESS}-{MAX_RAM_ADDRESS}");
            //}
            RamContents[index] = word;
        }

        public void SetWordAtMARAddress(string word)
        {
            SetWordAt(MARContents, word);
        }

        public void ClearRAM()
        {
            RamContents = new List<string>();
        }

        public void IncomingMARData(string marData)
        {
            MARContents = marData;
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
        public List<string> RAMDump() { return RamContents; }

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