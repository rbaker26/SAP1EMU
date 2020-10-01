using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace SAP1EMU.Lib
{
    public class InstructionDecoder : IDecoder
    {
        Dictionary<int, string> _instructions = new Dictionary<int, string>();
        string _filename = "InstructionSets.json";
        public InstructionDecoder()
        {
            string json = File.ReadAllText(_filename);
            List<InstructionSet> sets = JsonSerializer.Deserialize<List<InstructionSet>>(json); 

            foreach(InstructionSet iset in sets)
            {
                foreach(Instruction i in iset.instructions)
                {
                    _instructions.Add((i.BinCode, iset.SetName).GetHashCode(), i.OpCode);
                }
            }
        }

        public string Decode(string binCode, string setName)
        {
            try
            {
                return _instructions[(binCode, setName).GetHashCode()];
            }
            catch(Exception)
            {
                return "???";
            }
        }
    }
}
