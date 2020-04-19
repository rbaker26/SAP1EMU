using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using SAP1EMU.Lib.Components;
using SAP1EMU.Lib.Utilities;

namespace SAP1EMU.Assembler
{
    public class Assemble
    {

        public static List<string> ParseFileContents(List<string> unchecked_assembly)
        {
            _ = IsValid(unchecked_assembly);  //if is not valid, will throw execptions for CLI to catch and display to user



            List<string> binary = new List<string>();

            int lines_of_asm = unchecked_assembly.Count;


            int current_line_number = 0;
            foreach (string line in unchecked_assembly)
            {

                if (line == "...")
                {
                    int nop_count = 16 - lines_of_asm+1;
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
                    if (SEQ.Instance().SupportedCommandsBinTable.ContainsKey(upper_nibble_asm))     // Is instruction
                    {
                        upper_nibble_bin = SEQ.Instance().SupportedCommandsBinTable[line.Substring(0, 3)];
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


            return binary;
        }


        public static List<string> ParseAssembly(string filename)
        {
            // Loads the file into the list
            List<string> unchecked_assembly = new List<string>(System.IO.File.ReadAllLines(filename));

            return ParseFileContents(unchecked_assembly);

        }


        private static bool IsValid(List<string> unchecked_assembly)
        {
            int line_number = 1;
            foreach (string line in unchecked_assembly)
            {
                if (line != "...")
                {
                    string[] nibbles = line.Split(' ', 2);
                    
                    if(nibbles.Length == 0)
                    {
                        throw new ParseException($"SAP1ASM: Line cannot be blank {line_number}", new ParseException("Use \"NOP 0x0\" for a no-operation command"));

                    }

                    string instruction = nibbles[0];
                    if (nibbles.Length < 2)
                    {
                        throw new ParseException($"SAP1ASM: No lower nibble detected {line_number}", new ParseException($"{instruction} must be paired with a valid address in the range of 0x0 - 0xF"));
                    }
                    string addr = nibbles[1];

                    

                    // Check Intruction
                    if (instruction.Length != 3)
                    {
                        throw new ParseException($"SAP1ASM: invalid intruction on line {line_number}", new ParseException($"{instruction} is not a recognized instruction"));
                    }
                    if (!SEQ.Instance().SupportedCommandsBinTable.ContainsKey(instruction.ToUpper()))         // Check if is valid instruction
                    {
                        if (!Regex.IsMatch(instruction, "^0[xX][0-9a-fA-F]$"))               // Make sure it isnt data
                        {
                            throw new ParseException($"SAP1ASM: invalid intruction on line {line_number}", new ParseException($"{instruction} is not a recognized instruction or valid data"));
                        }
                    }



                    // Check Address
                    if (addr.Length != 3)                                               // should be no more than 3
                    {
                        throw new ParseException($"SAP1ASM: invalid address on line {line_number}", new ParseException($"{addr} is not of the form \"0xX\""));
                    }
                    if (!Regex.IsMatch(addr, "^0[xX][0-9a-fA-F]$"))     // should be of the form 0xX
                    {
                        throw new ParseException($"SAP1ASM: invalid address on line {line_number}", new ParseException($"{addr} is not of the form \"0xX\""));
                    }
                    int hex_addr = (int)(Convert.ToUInt32(addr.Substring(2, 1), 16));
                    if (hex_addr < 0 || hex_addr >= 16)                                // must tbe between 0-15
                    {
                        throw new ParseException($"SAP1ASM: address out of range on line {line_number}", new ParseException($"{addr} must be betweeen 0x0 and 0xF"));
                    }

                    if(line.Contains("..."))
                    {
                        throw new ParseException($"SAP1ASM: invalid use of \"...\" {line_number}", new ParseException($"{line} must only contain \"...\" with no extra charecters or spaces"));

                    }
                }



                line_number++;
            }

            return true;
        }
    }


}
