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
        // TODO: Update this to a dictionary model to it is not using 0xFFFF strings 
        public Dictionary<int, string> RAMContents { get; }

        private const int MIN_RAM_ADDRESS = 0x0800;
        private readonly int MAX_RAM_SIZE = 0xFFFF - MIN_RAM_ADDRESS;

        public RAMProgram(List<string> ramContents)
        {
            // Make sure no more than  intructions are entered.
            int count = ramContents.Count;

            if (count > MAX_RAM_SIZE)
            {
                throw new ArgumentOutOfRangeException($"RAM Overflow - More than {MAX_RAM_SIZE} lines of code.");
            }

            var ramToDictionary = new Dictionary<int, string>();
            
            for(int i = 0; i < ramContents.Count; i++)
            {
                ramToDictionary.Add(i + 0x0800, ramContents[i]);
            }

            this.RAMContents = ramToDictionary;

            // for (int i = count; i < MAX_RAM_SIZE; i++)
            // {
            //     this.RamContents[count] = string.Concat(Enumerable.Repeat("0", 8));
            // }
        }
    }
}