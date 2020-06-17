using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using SAP1EMU.Lib;

namespace SAP1EMU.Lib
{
    public class OpCodeLoader
    {
        const string jsonFile = "InstructionSets.json";

        public static InstructionSet GetSet(string SetName)
        {
            string json;
            try
            {
                json = File.ReadAllText(jsonFile);
            }
            catch(Exception e)
            {
                throw new Exception($"SAP1EMU: Error reading Instruction Set File: \"{jsonFile}\" ", e);
            }


            List<InstructionSet> sets;
            try
            {
                sets = JsonSerializer.Deserialize<List<InstructionSet>>(json);
            }
            catch(Exception e)
            {
                throw new Exception($"SAP1EMU: Error reading Instruction Set File: \"{SetName}\", Invalid JSON", e);
            }

            InstructionSet setChoice = sets.Find( x => x.SetName.ToLower().Equals(SetName.ToLower()) );

            if(setChoice == null || string.IsNullOrEmpty(setChoice.SetName))
            {
                throw new Exception($"SAP1EMU: Instruction Set \"{SetName}\" does not exist");
            }

            return setChoice;
        }

    }
}
