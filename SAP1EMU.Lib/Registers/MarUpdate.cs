using System;
using System.Collections.Generic;
using System.Text;

namespace SAP1EMU.Lib.Registers
{
    // TODO for Observer Pattern between RAM and MAR
    public struct MarUpdate
    {
        public string Value { get; set; }




        public override bool Equals(object obj)
        {
            return this.Value == ((MarUpdate)obj).Value;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public static bool operator ==(MarUpdate left, MarUpdate right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(MarUpdate left, MarUpdate right)
        {
            return !(left == right);
        }
    }
}
