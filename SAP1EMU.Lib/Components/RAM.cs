using System;
using System.Collections.Generic;

namespace SAP1EMU.Lib.Components
{
    public class RAM : IObserver<TicTok>
    {
        private List<string> RamContents = new List<string>();

        private string MARContents { get; set; }
        private string RAM_Register_Content { get; set; } // For ToString()

        private void Exec(TicTok tictok)
        {
            string cw = SEQ.Instance().ControlWord;

            //  TODO - Find a better way of using the mask to get the value
            //          Currently is using hardcoded magic numbers

            // Active Low, Push on Tic
            if (cw[3] == '0' && tictok.ClockState == TicTok.State.Tic)
            {
                string content = GetWordAt(MARContents);
                Wbus.Instance().Value = content;
            }

            // LR_, Active Low, Pull on Tok
            if (cw[12] == '0' && tictok.ClockState == TicTok.State.Tok)
            {
                string word = Wbus.Instance().Value;
                SetWordAt(MARContents, word);
                RAM_Register_Content = word;
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
            if (index < 0 || index > 15)
            {
                throw new ArgumentOutOfRangeException($"RAM Index Error - Addr with value {index} not inbetween 0-15");
            }
            return RamContents[index];
        }

        public void SetWordAt(string addr, string word)
        {
            int index = (int)(Convert.ToUInt32(addr, 2));
            if (index < 0 || index > 15)
            {
                throw new ArgumentOutOfRangeException($"RAM Index Error - Addr with value {index} not inbetween 0-15");
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
            RAM_Register_Content = GetWordAt(MARContents); // For tostring()
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
            return RAM_Register_Content;
        }

        public string ToString_Frame_Use()
        {
            return (String.IsNullOrEmpty(this.RAM_Register_Content) ? "00000000" : this.RAM_Register_Content);
        }
    }
}