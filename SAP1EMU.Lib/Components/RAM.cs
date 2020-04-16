using System;
using System.Collections.Generic;
using System.Text;

namespace SAP1EMU.Lib.Components
{
    public static class RAM
    {
        private static List<string> RamContents = new List<string>();
        public static void LoadProgram(RAMProgram rp)
        {
            ClearRAM();
            List<string> rpc = rp.RamContents;

            foreach (string s in rpc)
            {
                RamContents.Add(s);
            }
        }


        public static string GetWordAt(string addr)
        {
            int index = (int)(Convert.ToUInt32(addr, 2));
            if(index < 0 || index > 15)
            {
                throw new ArgumentOutOfRangeException($"RAM Index Error - Addr with value {index} not inbetween 0-15");
            }
            return RamContents[index];
        }

        public static void ClearRAM()
        {
            RamContents = null;
            RamContents = new List<string>();
        }
    }
}
