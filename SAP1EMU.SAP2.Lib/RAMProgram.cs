using System;
using System.Collections.Generic;
using System.Linq;

namespace SAP1EMU.SAP2.Lib
{
    /// <summary>
    /// This object is used for transfering the program into the Engine.
    /// The Engine will then use it to fill the RAM.
    /// It will not be used after using it to fill the RAM.
    /// </summary>
    public class RAMProgram
    {
        public List<string> RamContents { get; }

        private const int MIN_RAM_ADDRESS = 0x0800;
        private readonly int MAX_RAM_SIZE = 0xFFFF - MIN_RAM_ADDRESS;

        public RAMProgram(List<string> RamContents)
        {
            // Make sure no more than  intructions are entered.
            int count = RamContents.Count;

            if (count > MAX_RAM_SIZE)
            {
                throw new ArgumentOutOfRangeException($"RAM Overflow - More than {MAX_RAM_SIZE} lines of code.");
            }

            this.RamContents = RamContents;

            for (int i = count; i < MAX_RAM_SIZE; i++)
            {
                this.RamContents.Add(string.Concat(Enumerable.Repeat("0", 8)));
            }
        }
    }
}