﻿using SAP1EMU.SAP2.Lib;
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

        private static readonly List<Label> labels = new List<Label>();
        private static Dictionary<string, int> labelDictionary = new Dictionary<string, int>();

        public static List<string> Parse(List<string> unchecked_assembly, string InstructionSetName = DefaultInstructionSetName)
        {
            labels.Clear();
            labelDictionary.Clear();

            // Get Instruction Set
            InstructionSet iset = OpCodeLoader.GetSet(InstructionSetName);

            // *********************************************************************
            // Sanitize                                                            *
            // *********************************************************************

            // Remove Blank Lines
            unchecked_assembly.RemoveAll(s => s == null);
            unchecked_assembly.RemoveAll(s => Regex.IsMatch(s, "^\\s*$"));

            // Remove Newline Comments
            unchecked_assembly.RemoveAll(s => s.Trim().First().Equals('#'));

            for (int i = 0; i < unchecked_assembly.Count; i++)
            {
                // Trim Outter Whitespace
                unchecked_assembly[i] = unchecked_assembly[i].Trim();

                // Trim Inner Whitspace
                unchecked_assembly[i] = Regex.Replace(unchecked_assembly[i], "\\s{2,}", " ");
            
                // Remove Inline Comments
                unchecked_assembly[i] = Regex.Replace(unchecked_assembly[i], "\\s*#.*$", "");

                // Update Label addresses
                if(unchecked_assembly[i].Contains(':'))
                {
                    labels.Add(new Label
                    {
                        Name = unchecked_assembly[i].Trim().Substring(0, unchecked_assembly[i].IndexOf(':')),
                        LineNumber = i
                    });
                }
            }


            // *********************************************************************
            // Validate
            // *********************************************************************

            // If is not valid, will throw execptions for CLI to catch and display to user
            _ = IsValid(unchecked_assembly, iset);

            // Get rid of the labels to make it easier for conversion into binary
            // They are no longer needed anyway since we store the number they become
            // based on line numbers.
            //foreach (Label label in labels)
            //{
            //    unchecked_assembly[label.Address - 1] = Regex.Replace(unchecked_assembly[label.Address - 1], "\\s*\\w{1,6}:", "").Trim();
            //}

            // *********************************************************************
            // Assemble
            // *********************************************************************
            List<string> binary = new List<string>();

            int lines_of_asm = unchecked_assembly.Count;

            int current_line_number = 0;
            int index;

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
                    index = 0;
                    bool instructionComplete = false;

                    if (line.Contains(':'))
                    {
                        labelDictionary.Add(line[0..line.IndexOf(':')], binary.Count);
                        index = line.IndexOf(':') + 1;
                    }

                    // Instructions are character representations and if they have
                    // numbers then we are at the data portion. Should always complete
                    // as the code has been validated previously.
                    while (!instructionComplete && index < line.Length)
                    {
                        if(iset.Instructions.Any(i => i.OpCode.Equals(instructionString.ToString().Trim().ToUpper())))
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
                        string value = line[index..].Trim();

                        // If we have JXX Label then sub the label for its respective line number in the code
                        if (labels.Any(l => l.Name.Equals(value)))
                        {
                            Label label = labels.First(l => l.Name.Equals(value));
                            //value = label.Address.ToString();
                        }

                        bool isInt = int.TryParse(value[2..], out int data);

                        if(!isInt)
                        {
                            binary.Add(value);
                            binary.Add(value);
                        }
                        else
                        {
                            //int data = Convert.ToInt16(value, 16);
                            string binaryRepresentation = Convert.ToString(data, 2);

                            // start at 0 or 8 length
                            binaryRepresentation = binaryRepresentation.PadLeft(8 * (instruction.Bytes - 1), '0');
                            int position = binaryRepresentation.Length == 8 ? 0 : 8;

                            // Add from right to left
                            for (int i = 1; i < instruction.Bytes; i++, position -= 8)
                            {
                                binary.Add(string.Join("", binaryRepresentation.Skip(position).Take(8)));
                            }
                        }
                    }
                }

                current_line_number++;
            }
            // *********************************************************************

            for(int i = 0; i < binary.Count; i++)
            {
                if (labelDictionary.ContainsKey(binary[i]))
                {
                    int value = labelDictionary[binary[i]];

                    string bits = BinConverter.IntToBin16(value);

                    int firstByte = BinConverter.Bin8ToInt(bits[8..]);
                    int secondByte = BinConverter.Bin8ToInt(bits[0..8]);

                    binary[i] = BinConverter.IntToBinary(firstByte, 8);
                    binary[++i] = BinConverter.IntToBinary(secondByte, 8);
                }
            }

            // If a program contains way too many instructions
            if (binary.Count > (0xFFFF - 0x0800))
            {
                throw new ParseException($"SAP2ASM: Program contains too many lines of code.", new ParseException("The SAP2 can only contain up to 65,535 lines of code."));
            }


            return binary;
        }

        private static bool IsValid(List<string> unchecked_assembly, InstructionSet iset)
        {
            bool dot_macro_used = false;
            bool contains_hlt = false;

            // HLT is a special case that cannot be included in this but it is a non data instruction
            string[] nonDataInstructions = { "CMA", "NOP", "RAL", "RAR", "RET", "MOV A,B", "MOV A,C", "MOV B,A", "MOV B,C", "MOV C,A", "MOV C,B"};

            int line_number = 1;
            foreach (string line in unchecked_assembly)
            {
                if (line != "...")
                {
                    // If a line contains a label we must check if its valid
                    if(labels.Count > 0 && line.Contains(':'))
                    {
                        Label label = labels.Where(l => l.LineNumber == line_number - 1).FirstOrDefault();
                        if (label != null && !label.IsLabelValid())
                        {
                            throw new ParseException($"SAP2ASM: Label is invalid (line: {line_number}.", new ParseException("Make sure a label starts with a char and is between a length of 1 to 6 characters."));
                        }
                    }

                    StringBuilder instructionString = new StringBuilder();
                    int index = 0;
                    bool instructionComplete = false;

                    if(line.Contains(':'))
                    {
                        index = line.IndexOf(':') + 1;
                    }
                    
                    // Instructions are character representations and if they have
                    // numbers then we are at the data portion. Should always complete
                    // as the code has been validated previously.
                    while (!instructionComplete && index < line.Length)
                    {
                        if (iset.Instructions.Any(i => i.OpCode.Equals(instructionString.ToString().Trim().ToUpper())))
                        {
                            instructionComplete = true;
                        }
                        else
                        {
                            instructionString.Append(line[index]);
                            index++;
                        }
                    }

                    Instruction instruction = iset.Instructions.FirstOrDefault(i => i.OpCode.Equals(instructionString.ToString().Trim().ToUpper()));

                    if (string.IsNullOrEmpty(line))
                    {
                        throw new ParseException($"SAP2ASM: Line cannot be blank (line: {line_number}).", new ParseException("Use \"NOP\" for a no-operation command"));
                    }

                    if (instruction == null)
                    {
                        throw new ParseException($"SAP2ASM: invalid intruction on line {line_number}.", new ParseException($"\"{instruction}\" is not a recognized instruction"));
                    }

                    // Check against instructions that have no data following them
                    if (nonDataInstructions.Contains(instruction.OpCode))
                    {
                        continue;
                    }

                    // Special case 
                    if (instruction.OpCode == "HLT")
                    {
                        contains_hlt = true;
                        continue;
                    }

                    // Check the validity of the instructions and their data
                    string value = line[index..].Trim();

                    // Check Jump addresses and check if labels exist to jump too
                    int data = 0;
                    try
                    {
                        data = Convert.ToInt16(value, 16);
                    }
                    catch(FormatException) //data is a label possibly
                    {
                        // There are no records of this label used
                        if (!labels.Any(l => l.Name.Equals(value)))
                        {
                            throw new ParseException($"SAP2ASM: There are no labels with that name.", new ParseException($"In order to use this label for jumping you must declare \"{value}:\" in the front of the line where you would like to jump"));
                        }
                    }

                    if (!labels.Any(l => l.Name.Equals(value)) && !Regex.IsMatch(value, "^0[xX][0-9a-fA-F]$"))
                    {
                        throw new ParseException($"SAP2ASM: invalid address on line {line_number}.", new ParseException($"\"{value}\" is not of the form \"0xX\""));
                    }


                    // Check immediate value instructions(uses byte or addresses) to be in the range of their respective  bytes
                    if (instruction.Bytes == 3 && data < 0 || data > 65536)
                    {
                        throw new ParseException($"SAP2ASM: address out of range on line {line_number}.", new ParseException($"\"{data}\" must be betweeen 0x0 and 0xFFFF"));
                    }
                    else if (instruction.Bytes == 2)
                    {
                        // IN and OUT are special occasionals as they only have 2 options IN has 1 or 2 and OUT has 3 or 4
                        if (instruction.OpCode.Equals("IN") && (data < 1 || data > 2))
                        {
                            throw new ParseException($"SAP2ASM: port number for input is out of range on line {line_number}.", new ParseException($"\"{data}\" must be either 0x1 or 0x2"));
                        }
                        else if (instruction.OpCode.Equals("OUT") && (data < 3 || data > 4))
                        {
                            throw new ParseException($"SAP2ASM: port number for output is out of range on line {line_number}.", new ParseException($"\"{data}\" must be either 0x3 or 0x4"));
                        }
                        else if(data < 0 || data > 256)
                        {
                            throw new ParseException($"SAP2ASM: byte value out of range on line {line_number}.", new ParseException($"\"{data}\" must be betweeen 0x0 and 0xFF"));
                        }
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
                        throw new ParseException($"SAP2ASM: invalid use of \"...\" {line_number}.", new ParseException($"\"{line}\" must only contain once instance of \"...\" in the program"));
                    }
                }

                line_number++;
            }

            // If the code does not contain a HLT instruction
            if(!contains_hlt)
            {
                throw new ParseException($"SAP2ASM: program does not contain an endpoint.", new ParseException($"\"HLT\" must be present in the program at least once"));
            }

            return true;
        }
    }
}