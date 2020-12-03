using SAP1EMU.SAP2.Lib;
using SAP1EMU.SAP2.Lib.Utilities;

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using System.Text;

namespace SAP1EMU.SAP2.Assembler
{
    public static class Assemble
    {
        private const string DefaultInstructionSetName = "Malvino";

        private static List<Label> labels = new List<Label>();

        public static List<string> Parse(List<string> unchecked_assembly, string InstructionSetName = DefaultInstructionSetName)
        {
            // Get Instruction Set
            InstructionSet iset = OpCodeLoader.GetSet(InstructionSetName);

            // *********************************************************************
            // Sanitize                                                            *
            // *********************************************************************

            // Remove Blank Lines
            unchecked_assembly.RemoveAll(s => s == null);
            unchecked_assembly.RemoveAll(s => Regex.IsMatch(s, "^\\s*$"));

            // Remove Newline Comments
            unchecked_assembly.RemoveAll(s => s.Trim().First() == '#');

            // Find and store info on all labels
            labels = unchecked_assembly.Where(line => line.Contains(':'))
                .Select((line, index) => new Label
                {
                    LineNumber = ++index,
                    Name = line.Trim().Substring(0, line.IndexOf(':'))
                }
            ).ToList();

            for (int i = 0; i < unchecked_assembly.Count; i++)
            {
                // Trim Outter Whitespace
                unchecked_assembly[i] = unchecked_assembly[i].Trim();

                // Trim Inner Whitspace
                unchecked_assembly[i] = Regex.Replace(unchecked_assembly[i], "\\s{2,}", " ");
            
                // Remove Inline Comments
                unchecked_assembly[i] = Regex.Replace(unchecked_assembly[i], "\\s*#.*$", "");
            }


            // *********************************************************************
            // Validate
            // *********************************************************************

            // If is not valid, will throw execptions for CLI to catch and display to user
            _ = IsValid(unchecked_assembly, iset);

            // Get rid of the labels to make it easier for conversion into binary
            // They are no longer needed anyway since we store the number they become
            // based on line numbers.
            foreach (Label label in labels)
            {
                unchecked_assembly[label.LineNumber - 1] = Regex.Replace(unchecked_assembly[label.LineNumber - 1], @"\s*\w{1,6}:", "").Trim();
            }

            // *********************************************************************
            // Assemble
            // *********************************************************************
            List<string> binary = new List<string>();

            int lines_of_asm = unchecked_assembly.Count;

            int current_line_number = 0;
            foreach (string line in unchecked_assembly)
            {
                if (line == "...")
                {
                    int nop_count = (0xFFFF - 0x0800) - lines_of_asm + 1;
                    for (int i = 0; i < nop_count; i++)
                    {
                        binary.Add("00000000");
                    }
                }
                else
                {
                    StringBuilder instructionString = new StringBuilder();
                    int index = 0;
                    bool instructionComplete = false;

                    // Instructions are character representations and if they have
                    // numbers then we are at the data portion. Should always complete
                    // as the code has been validated previously.
                    while (!instructionComplete && index < line.Length)
                    {
                        if(char.IsDigit(line[index]))
                        {
                            instructionComplete = true;
                        }
                        else
                        {
                            instructionString.Append(line[index]);
                            index++;
                        }
                    }

                    // Grab data associated, if its more than 1 byte
                    // and then convert to binary
                    Instruction instruction = iset.Instructions.Find(i => i.OpCode.Equals(instructionString.ToString().Trim().ToUpper()));

                    binary.Add(instruction.BinCode);

                    // We must convert data to binary for 2 and 3 byte instructions only
                    if(instruction.Bytes > 1)
                    {
                        string value = line[index..];

                        // If we have JXX Label
                        if (labels.Any(l => l.Name.Equals(value)))
                        {
                            Label label = labels.First(l => l.Name.Equals(value));
                            value = label.LineNumber.ToString();
                        }

                        int data = Convert.ToInt16(value);
                        string binaryRepresentation = Convert.ToString(data, 2);

                        // 16 or 24 length
                        binaryRepresentation = binaryRepresentation.PadLeft(sizeof(byte) * instruction.Bytes);

                        for(int i = 1, position = 0; i < instruction.Bytes; i++, position += 8)
                        {
                            binary.Add(binaryRepresentation.Substring(position, 8));
                        }
                    }
                }

                current_line_number++;
            }
            // *********************************************************************

            //If a program was executed, but didnt fill in every line of RAM then throw an exception. Must have 16 elements!
            //if(binary.Count != 16)
            //{
            //    throw new ParseException($"SAP1ASM: Program must have 16 lines.", new ParseException("Use \"NOP 0x0\" for a no-operation command or the \"...\" macro to fill in the rest with NOP 0x0.")); 
            //}


            return binary;
        }

        private static bool IsValid(List<string> unchecked_assembly, InstructionSet iset)
        {
            bool dot_macro_used = false;
            bool contains_hlt = false;

            int line_number = 1;
            foreach (string line in unchecked_assembly)
            {
                if (line != "...")
                {
                    // If a line contains a label we must check if its valid
                    if(labels.Count > 0 && line.Contains(':'))
                    {
                        if (!labels[line_number - 1].IsLabelValid())
                        {
                            throw new ParseException($"SAP2ASM: Label is invalid (line: {line_number}.", new ParseException("Make sure a label starts with a char and is between a length of 1 to 6 characters.");
                        }
                    }

                    string[] nibbles = line.Split(' ', 2);

                    if (nibbles.Length == 0)
                    {
                        throw new ParseException($"SAP1ASM: Line cannot be blank (line: {line_number}).", new ParseException("Use \"NOP\" for a no-operation command"));
                    }

                    string instruction = nibbles[0];
                    if(instruction.ToUpper() == "HLT")
                    {
                        contains_hlt = true;
                    }
                    if (nibbles.Length < 2)
                    {
                        throw new ParseException($"SAP1ASM: No lower nibble detected (line: {line_number}).", new ParseException($"\"{instruction}\" must be paired with a valid address in the range of 0x0 - 0xF"));
                    }
                    string addr = nibbles[1];

                    // Check Intruction
                    if (instruction.Length != 3)
                    {
                        throw new ParseException($"SAP1ASM: invalid intruction on line {line_number}.", new ParseException($"\"{instruction}\" is not a recognized instruction"));
                    }

                    if (!InstructionValidator.IsValidInstruction(instruction.ToUpper(), iset))         // Check if is valid instruction
                    {
                        if (!Regex.IsMatch(instruction, "^0[xX][0-9a-fA-F]$"))               // Make sure it isnt data
                        {
                            throw new ParseException($"SAP1ASM: invalid intruction on line {line_number}.", new ParseException($"\"{instruction}\" is not a recognized instruction or valid data"));
                        }
                    }

                    // Check Address
                    if (addr.Length != 3)                                               // should be no more than 3
                    {
                        throw new ParseException($"SAP1ASM: invalid address on line {line_number}.", new ParseException($"\"{addr}\" is not of the form \"0xX\""));
                    }
                    if (!Regex.IsMatch(addr, "^0[xX][0-9a-fA-F]$"))     // should be of the form 0xX
                    {
                        throw new ParseException($"SAP1ASM: invalid address on line {line_number}.", new ParseException($"\"{addr}\" is not of the form \"0xX\""));
                    }
                    int hex_addr = (int)(Convert.ToUInt32(addr.Substring(2, 1), 16));
                    if (hex_addr < 0 || hex_addr >= 16)                                // must tbe between 0-15
                    {
                        throw new ParseException($"SAP1ASM: address out of range on line {line_number}.", new ParseException($"\"{addr}\" must be betweeen 0x0 and 0xF"));
                    }

                    if (line.Contains("..."))
                    {
                        throw new ParseException($"SAP1ASM: invalid use of \"...\" on line {line_number}.", new ParseException($"\"{line}\" must only contain \"...\" with no extra charecters or spaces"));

                    }
                }
                else
                {
                    if (!dot_macro_used)
                    {
                        dot_macro_used = true;
                    }
                    else
                    {
                        throw new ParseException($"SAP1ASM: invalid use of \"...\" {line_number}.", new ParseException($"\"{line}\" must only contain once instance of \"...\" in the program"));
                    }
                }

                line_number++;
            }

            // If the code does not contain a HLT instruction
            if(!contains_hlt)
            {
                throw new ParseException($"SAP1ASM: program does not contain an endpoint.", new ParseException($"\"HLT\" must be present in the program at least once"));
            }

            return true;
        }
    }
}