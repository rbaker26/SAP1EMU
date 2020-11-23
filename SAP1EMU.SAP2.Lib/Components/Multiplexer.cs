using System;
using System.Collections.Generic;
using System.Text;

namespace SAP1EMU.SAP2.Lib.Components
{
    class Multiplexer
    {
        private static Multiplexer _instance; // Singleton Pattern

        public void PassThroughToBus(string bits, bool isUpperByte = false)
        {
            if (bits.Length == 16)
            {
                Wbus.Instance().Value = bits;
            }
            else
            {
                if (isUpperByte)
                {
                    Wbus.Instance().Value = bits.PadRight(16, '0');
                }
                else
                {
                    Wbus.Instance().Value = bits.PadLeft(16, '0');

                }
            }
        }

        public static Multiplexer Instance()
        {
            if (_instance == null)
            {
                _instance = new Multiplexer();
            }
            return _instance;
        }
    }
}
