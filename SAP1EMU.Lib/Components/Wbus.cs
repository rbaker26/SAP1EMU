using System;
using System.Collections.Generic;
using System.Text;

namespace SAP1EMU.Lib.Components
{
    public class Wbus
    {
        private static Wbus _instance; // Singleton Pattern


        // Value on the bus
        public string Value { get; set; }


        private Wbus() { Value = "00000000"; }

        public static Wbus Instance()
        {
            if(_instance == null)
            {
                _instance = new Wbus();
                
            }
            return _instance;
        }

        public override string ToString()
        {
            return Value;
        }
    }
}
