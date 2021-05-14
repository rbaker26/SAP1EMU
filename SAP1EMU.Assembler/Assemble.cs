using SAP1EMU.Lib;
using SAP1EMU.Lib.Utilities;

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SAP1EMU.Assembler
{
    public static class Assemble
    {
        private const string DefaultInstructionSetName = "SAP1Emu";

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
            unchecked_assembly.RemoveAll(s => s[0] == '#');

            for (int i = 0; i < unchecked_assembly.Count; i++)
            {
                // *******************************
                // Trim Whitespace               *
                // *******************************

                // Outter Whitespace
                unchecked_assembly[i] = unchecked_assembly[i].Trim();

                // Inner Whitspace
                unchecked_assembly[i] = Regex.Replace(unchecked_assembly[i], "\\s{2,}", " ");
                // *******************************

                // *******************************
                // Remove Inline Comments
                // *******************************
                unchecked_assembly[i] = Regex.Replace(unchecked_assembly[i], "\\s*#.*$", "");
                // *******************************
            }
            // *********************************************************************

            // *********************************************************************
            // Validate
            // *********************************************************************

            //if is not valid, will throw execptions for CLI to catch and display to user
            _ = IsValid(unchecked_assembly, iset);
            // *********************************************************************

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
                    int nop_count = 16 - lines_of_asm + 1;
                    for (int i = 0; i < nop_count; i++)
                    {
                        binary.Add("00000000");
                    }
                }
                else
                {
                    string upper_nibble_asm = line.Split(" ", 2)[0];
                    string lower_nibble_asm = line.Split(" ", 2)[1];
                    string upper_nibble_bin = "";
                    string lower_nibble_bin = "";

                    // Convert Upper Nibble
                    if (InstructionValidator.IsValidInstruction(upper_nibble_asm, iset))     // Is instruction
                    {
                        upper_nibble_bin = InstructionValidator.GetUpperNibble(line.Substring(0, 3), iset);
                    }
                    else if (Regex.IsMatch(upper_nibble_asm, "^0[xX][0-9a-fA-F]$"))                    // Is Data
                    {
                        int value_upper = (int)(Convert.ToUInt32(upper_nibble_asm, 16));
                        upper_nibble_bin = BinConverter.IntToBin4(value_upper);
                    }

                    // Convert Lower Nibble
                    int value_lower = (int)(Convert.ToUInt32(lower_nibble_asm, 16));
                    lower_nibble_bin = BinConverter.IntToBin4(value_lower);

                    binary.Add(upper_nibble_bin + lower_nibble_bin);
                }

                current_line_number++;
            }
            // *********************************************************************

            //If a program was executed, but didnt fill in every line of RAM then throw an exception. Must have 16 elements!
            if (binary.Count != 16)
            {
                throw new ParseException($"SAP1ASM: Program must have 16 lines.", new ParseException("Use \"NOP 0x0\" for a no-operation command or the \"...\" macro to fill in the rest with NOP 0x0."));
            }

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
                    string[] nibbles = line.Split(' ', 2);

                    if (nibbles.Length == 0)
                    {
                        throw new ParseException($"SAP1ASM: Line cannot be blank (line: {line_number}).", new ParseException("Use \"NOP 0x0\" for a no-operation command"));
                    }

                    string instruction = nibbles[0];
                    if (instruction.ToUpper() == "HLT")
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
            if (!contains_hlt)
            {
                throw new ParseException($"SAP1ASM: program does not contain an endpoint.", new ParseException($"\"HLT\" must be present in the program at least once"));
            }

            return true;
        }
    }
}