using SAP1EMU.Lib.Components;
using SAP1EMU.SAP2.Lib.Components;

using System;

namespace SAP1EMU.SAP2.Lib.Registers
{
    public class IReg : IObserver<TicTok>
    {
        private string RegContent { get; set; } = "00000000";

        private void Exec(TicTok tictok)
        {
            var cw = SEQ.Instance().ControlWord;

            // Active Low, Push on Tic
            //if (string.Equals(cw["EI_"], "0", StringComparison.Ordinal) & tictok.ClockState == TicTok.State.Tic)
            //{
            //    // Send A to the WBus
            //    Wbus.Instance().Value = "0000" + RegContent.Substring(4, 4); //Instruction register only outputs the least significant bits to the WBus
            //}

            // Active Low, Pull on Tok
            if (string.Equals(cw["LI_"], "0", StringComparison.Ordinal) && tictok.ClockState == TicTok.State.Tok)
            {
                RegContent = Wbus.Instance().Value;
            }
        }

        /// <summary>
        /// For the real ToString, use the ToString_Frame_use() method
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            //  I dont know this this is the best place to put this substring command, but it is needed
            return RegContent.Substring(0, 4);
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
            Unsubscribe();
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

        public string ToString_Frame_Use()
        {
            return string.IsNullOrEmpty(RegContent) ? "0000 0000" : RegContent;
        }
    }
}