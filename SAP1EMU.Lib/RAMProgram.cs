using System;
using System.Collections.Generic;

namespace SAP1EMU.Lib
{
    /// <summary>
    /// This object is used for transfering the program into the Engine.
    /// The Engine will then use it to fill the RAM.
    /// It will not be used after using it to fill the RAM.
    /// </summary>
    public class RAMProgram
    {
        public List<string> RamContents { get; }

        //  Program() { }
        public RAMProgram(List<string> RamContents)
        {
            this.RamContents = new List<string>();
            // Make sure no more than 16 intructions are entered.
            int count = RamContents.Count;
            if (count > 16)
            {
                throw new ArgumentOutOfRangeException("RAM Overflow - More than 16 lines of code.");
            }

            this.RamContents = RamContents;

            for (int i = count; i < 15; i++)
            {
                this.RamContents.Add("00000000");
            }
        }
    }
}