using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SAP1EMU.SAP2.Lib
{
    public class OpCodeLoader
    {
        private const string jsonFile = "InstructionSets.json";

        public static InstructionSet GetSet(string SetName)
        {
            string json;
            try
            {
                json = File.ReadAllText(jsonFile);
            }
            catch (Exception e)
            {
                throw new Exception($"SAP2EMU: Error reading Instruction Set File: \"{jsonFile}\" ", e);
            }

            List<InstructionSet> sets;
            try
            {
                JsonSerializerOptions options = new JsonSerializerOptions
                {
                    Converters =
                    {
                        new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)
                    },

                };

                sets = JsonSerializer.Deserialize<List<InstructionSet>>(json, options);
            }
            catch (Exception e)
            {
                throw new Exception($"SAP2EMU: Error reading Instruction Set File: \"{SetName}\", Invalid JSON", e);
            }

            InstructionSet? setChoice = sets.Find(x => x.SetName.ToLower().Equals(SetName.ToLower(), StringComparison.Ordinal));

            if (setChoice == null || string.IsNullOrEmpty(setChoice.SetName))
            {
                throw new Exception($"SAP2EMU: Instruction Set \"{SetName}\" does not exist");
            }

            return setChoice;
        }

        public static List<string> GetISetNames()
        {
            string json;
            try
            {
                json = File.ReadAllText(jsonFile);
            }
            catch (Exception e)
            {
                throw new Exception($"SAP2EMU: Error reading Instruction Set File: \"{jsonFile}\" ", e);
            }

            List<InstructionSet> sets;
            try
            {
                JsonSerializerOptions options = new JsonSerializerOptions
                {
                    Converters =
                    {
                        new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)
                    },

                };

                sets = JsonSerializer.Deserialize<List<InstructionSet>>(json, options);
            }
            catch (Exception e)
            {
                throw new Exception($"SAP2EMU: Error reading Instruction Set File: Invalid JSON", e);
            }
            List<string> names = new List<string>();

            foreach (InstructionSet instructionSet in sets)
            {
                names.Add(instructionSet.SetName);
            }

            return names;
        }
    }
}