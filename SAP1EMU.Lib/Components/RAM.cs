﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SAP1EMU.Lib.Components
{
    public class RAM : IObserver<TicTok>
    {
        private List<string> RamContents = new List<string>();
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
            if(index < 0 || index > 15)
            {
                throw new ArgumentOutOfRangeException($"RAM Index Error - Addr with value {index} not inbetween 0-15");
            }
            return RamContents[index];
        }

        public void ClearRAM()
        {
            RamContents = null;
            RamContents = new List<string>();
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
            // TODO - Check ControlWord
            // Exec();
            System.Console.WriteLine("RAM is registered!");
        }

        public virtual void Unsubscribe()
        {
            unsubscriber.Dispose();
        }
        #endregion

    }
}
