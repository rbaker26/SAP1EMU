using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SAP1EMU.SAP2.Lib
{
    public class InstructionDecoder : IDecoder
    {
        private readonly Dictionary<int, string> _instructions = new Dictionary<int, string>();
        private readonly string _filename = "InstructionSets.json";

        public InstructionDecoder()
        {
            string json = File.ReadAllText(_filename);

            JsonSerializerOptions options = new JsonSerializerOptions
            {
                Converters =
                    {
                        new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)
                    },

            };

            List<InstructionSet> sets = JsonSerializer.Deserialize<List<InstructionSet>>(json, options);

            foreach (InstructionSet iset in sets)
            {
                foreach (Instruction i in iset.Instructions)
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
            catch (Exception)
            {
                return "???";
            }
        }
    }
}