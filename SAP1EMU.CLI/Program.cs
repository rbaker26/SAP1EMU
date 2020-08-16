using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

using CommandLine;

using SAP1EMU.Assembler;
using SAP1EMU.Engine;
using SAP1EMU.Lib;

namespace SAP1EMU.CLI
{
    public class Program
    {
        public class Options
        {
            // File Mappings ******************************
            [Option('s', "source-file", Required = true, HelpText = "\n<FileName>.s Input file containing assembly code.\n<FileName>.b Input file containing compilied binary.")]
            public string SourceFile { get; set; }

            [Option('o', "output-file", Required = false, HelpText = "Place the output into <file>.", Default = "a.txt")]
            public string OutputFile { get; set; }
            // ********************************************


            // TODO - Figure out if I want to use these
            //// Verbosity **********************************
            //[Option('v', "verbose", Required = false, HelpText = "Set output to verbose.\n(inlcudes debug statements from the engine)")]
            //public bool Verbose { get; set; }
            //[Option('V', "very-verbose", Required = false, HelpText = "Set output to very verbose.\n(includes debug statements from the engine and the input file in output)")]
            //public bool VeryVerbose { get; set; }
            //// ********************************************


            // Frame Support ******************************
            // -f and -F are mutually exclusive. And error will appeear if the user tries to use both.
            [Option('f', "fframe", SetName = "fframe", Required = false, HelpText = "Include final frame in the output file.")]
#pragma warning disable IDE1006 // Naming Styles
            public bool fframe { get; set; }
#pragma warning restore IDE1006 // Naming Styles
            [Option('F', "Fframe", SetName = "Fframe", Required = false, HelpText = "Include all frames in the output file.")]
            public bool Fframe { get; set; }

            // TODO - Figure out why default isnt working here
            // Default should be "std"
            [Option('O', "FOfragitme", SetName = "FOframe", Required = false, HelpText = "Include Snapshots of the Output Register in the output file.\nParameters:\n  std\t\tOutputs with formatting\n  no-format\tOutputs wil no formatting")]
            public string FOframe { get; set; }
            // ********************************************

            // Debug Setting ******************************
            [Option('d', "debug", Required = false, HelpText = "Turns on Debug Mode")]
            public bool Debug { get; set; }
            // ********************************************

            // Instruction Set ****************************
            [Option('i', "instructionSet", Required = false, HelpText = "Sets the Instruction Set to use\nParameters:\n  SAP1Emu\tUses expanded SAP1EMU Instruction Set (default)\n  Malvino\tUses Malvino's Instruction Set\n  BenEater\tUses Ben Eater's Instruction Set", Default = "SAP1Emu")]
            public string InstructionSetName { get; set; }
            // ********************************************




        }
        enum FileType
        {
            S, // ASM
            B  // BIN 
        }
        public static void Main(string[] args)
        {
            _ = Parser.Default.ParseArguments<Options>(args)
                   .WithParsed(o =>
                   {
                       List<string> source_file_contents = new List<string>(); ;
                       FileType fileType = FileType.B;

                       if (!string.IsNullOrEmpty(o.SourceFile))
                       {
                           if (!File.Exists(o.SourceFile))
                           {
                               Console.Error.WriteLine($"SAP1EMU: error: {o.SourceFile}: No such file");
                               Console.Error.WriteLine($"SAP1EMU: fatal error: no input file");
                               Console.Error.WriteLine("emulation terminated");
                               CheckEnvAndExit();

                           }
                           else // Check if file is valid
                           {
                               source_file_contents = new List<string>(File.ReadAllLines(o.SourceFile));
                               int loc = source_file_contents.Count;

                               if (o.SourceFile.Length >= 3)
                               {
                                   string ftype = o.SourceFile.Substring(o.SourceFile.Length - 2, 2);
                                   if (ftype == ".s")
                                   {
                                       fileType = FileType.S;
                                   }
                                   else if (ftype == ".b")
                                   {
                                       fileType = FileType.B;

                                   }
                                   else
                                   {
                                       Console.Error.WriteLine($"SAP1EMU: error: {o.SourceFile}: Invalid file extension: Must be <FileName>.s or <FileName>.b");
                                       Console.Error.WriteLine($"SAP1EMU: fatal error: no valid input file");
                                       Console.Error.WriteLine("emulation terminated.");
                                       CheckEnvAndExit();
                                   }
                               }


                               if (loc == 0)
                               {
                                   Console.Error.WriteLine($"SAP1EMU: error: {o.SourceFile}: File is empty");
                                   Console.Error.WriteLine($"SAP1EMU: fatal error: no valid input file");
                                   Console.Error.WriteLine("emulation terminated");
                                   CheckEnvAndExit();
                               }
                               if (loc > 16)
                               {
                                   Console.Error.WriteLine($"SAP1EMU: error: {o.SourceFile}: invalid file: Contains more than 16 lines of code.");
                                   Console.Error.WriteLine($"SAP1EMU: fatal error: no valid input file");
                                   Console.Error.WriteLine("emulation terminated");
                                   CheckEnvAndExit();
                               }



                           }
                           if (!string.IsNullOrEmpty(o.FOframe))
                           {
                               if (o.FOframe.ToLower() != "no-format" && o.FOframe.ToLower() != "std")
                               {
                                   Console.Error.WriteLine($"SAP1EMU: warning: {o.SourceFile}: invalid format argument {o.FOframe}: Defaulting to \"std\".");
                                   o.FOframe = "std";
                               }
                           }

                           if (o.InstructionSetName.ToLower() != "sap1emu" && o.InstructionSetName.ToLower() != "malvino" && o.InstructionSetName.ToLower() != "beneater")
                           {
                               Console.Error.WriteLine($"SAP1EMU: warning: {o.InstructionSetName}: invalid argument:  Defaulting to \"SAP1Emu\".");
                               o.InstructionSetName = "SAP1Emu";
                           }






                           List<string> compiled_binary = null;

                           if (fileType == FileType.S)
                           {
                               try
                               {
                                   compiled_binary = Assemble.Parse(source_file_contents, o.InstructionSetName);
                               }
                               catch (ParseException pe)
                               {
                                   //Console.SetOut(new StreamWriter(Console.OpenStandardOutput()));
                                   //Console.SetError(new StreamWriter(Console.OpenStandardError()));

                                   var tempColor = Console.ForegroundColor;
                                   if (Console.BackgroundColor == ConsoleColor.Red)
                                   {
                                       Console.ForegroundColor = ConsoleColor.Cyan;
                                   }
                                   else
                                   {
                                       Console.ForegroundColor = ConsoleColor.Red;
                                   }
                                   Console.Error.WriteLine($"SAP1ASM: fatal error: " + pe.Message + " " + pe.InnerException.Message);
                                   Console.ForegroundColor = tempColor;
                                   Console.Error.WriteLine("assembly terminated");



                                   Console.Error.Flush();

                                   CheckEnvAndExit();
                               }
                           }
                           else
                           {
                               compiled_binary = source_file_contents;
                           }


                           RAMProgram rmp = new RAMProgram(compiled_binary);
                           EngineProc engine = new EngineProc();



                           //StringBuilder sb_out = new StringBuilder();
                           //TextWriter writer_out = new StringWriter(sb_out);
                           //Console.SetOut(writer_out);


                           //StringBuilder sb_error = new StringBuilder();
                           //TextWriter writer_error = new StringWriter(sb_error);
                           //Console.SetError(writer_error);


                           engine.Init(rmp, o.InstructionSetName);
                           try
                           {
                               engine.Run();

                           }
                           catch (EngineRuntimeException ere)
                           {
                               var tempColor = Console.ForegroundColor;
                               if (Console.BackgroundColor == ConsoleColor.Red)
                               {
                                   Console.ForegroundColor = ConsoleColor.Cyan;
                               }
                               else
                               {
                                   Console.ForegroundColor = ConsoleColor.Red;
                               }
                               

                               //Console.SetOut(new StreamWriter(Console.OpenStandardOutput()));
                               //Console.SetError(new StreamWriter(Console.OpenStandardError()));

                               Console.Error.WriteLine($"SAP1EMU: fatal error: " + ere.Message);
                               Console.ForegroundColor = tempColor;
                               Console.Error.WriteLine("emulation terminated");

                               Console.Error.Flush();

                               CheckEnvAndExit();
                           }



                           string engine_output = "************************************************************\n"
                                                + "Final Output Register Value: " + engine.GetOutputReg()
                                                + "\n************************************************************\n\n";

                           List<Frame> FrameStack = engine.FrameStack();

                           if (o.fframe)
                           {
                               engine_output += "\n" + engine.FinalFrame();
                           }
                           else if (o.Fframe)
                           {
                               StringBuilder sb = new StringBuilder();
                               StringWriter fw = new StringWriter(sb);

                               foreach (Frame frame in FrameStack)
                               {
                                   fw.WriteLine(frame.ToString());
                               }
                               fw.Flush();

                               engine_output += "\n" + sb.ToString();
                           }
                           else if (o.FOframe != null)
                           {
                               engine_output = null; // Clear the output

                               StringBuilder sb = new StringBuilder();
                               StringWriter fw = new StringWriter(sb);

                               foreach (Frame frame in FrameStack)
                               {
                                   if (frame.TState == 6)
                                   {
                                       if (o.FOframe.ToLower() == "std")
                                       {
                                           fw.WriteLine(frame.OutputRegister());
                                       }
                                       else if (o.FOframe.ToLower() == "no-format")
                                       {
                                           string temp = frame.OReg;
                                           if (string.IsNullOrEmpty(temp))
                                           {
                                               temp = "00000000";
                                           }
                                           fw.WriteLine(temp);
                                       }

                                   }
                               }
                               fw.Flush();

                               engine_output += sb.ToString();
                           }


                           var standardOutput = new StreamWriter(Console.OpenStandardOutput())
                           {
                               AutoFlush = true
                           };
                           Console.SetOut(standardOutput);

                           var standardError = new StreamWriter(Console.OpenStandardError())
                           {
                               AutoFlush = true
                           };
                           Console.SetError(standardError);



                           //string stdout = sb_out.ToString();
                           //string stderror = sb_error.ToString();



                           File.WriteAllText(o.OutputFile, engine_output);





                           // Start the Single Stepping Debug Session if Debug Flag is set

                           Debug_Proc(o, source_file_contents, FrameStack);
                           Console.Out.WriteLine("Debug Session Complete");

                           //foreach(Frame f in FrameStack)
                           //{
                           //    foreach(string s in f.RAM)
                           //    {
                           //        Console.Out.WriteLine(s);
                           //    }
                           //    Console.Out.WriteLine("\n");
                           //}
                       }


                   });
        }

        private static void CheckEnvAndExit()
        {
            string env = Environment.GetEnvironmentVariable("IS_TESTING_ENV");
            if(string.IsNullOrEmpty(env) == false)
            {
                if (env == "TRUE")
                {
                    throw new CLIException();
                }
                else
                {
                    Environment.Exit(1);
                }
            }
            else
            {
                Environment.Exit(1);
            }
        }

        private static void Debug_Proc(Options o, List<string> source_file_contents, List<Frame> FrameStack)
        {
            if (o.Debug)
            {
                Console.Clear();
                Console.Out.WriteLine(Banner);

                Console.Out.WriteLine("Assembly Program:\n");


                int source_file_contents_line_count = source_file_contents.Count;

                int line_mult = 1;
                int executable_line_count = 1;
                for (int i = 1; i < source_file_contents_line_count + 1; i++)
                {

                    Console.Out.WriteLine($"{((i * line_mult) != 0 ? i.ToString() + ")" : "  ")} {source_file_contents[i - 1]}");

                    if (IsAfterHLT(source_file_contents[i - 1].Substring(0, 3)))
                    {
                        line_mult = 0;
                    }
                    else
                    {
                        executable_line_count += 1 * line_mult;
                    }
                }

                Console.Out.WriteLine("\n\nDo you want to set a break point: (y/n) ");
                Console.Out.WriteLine("If no, debug will single-step though the program starting at line 1");


                Console.Out.Write(">>> ");
                string break_point_answer = Console.ReadLine();

                int break_point = 0;
                int t_break_point = 0;

                if (break_point_answer != null & break_point_answer.Length > 0)
                {
                    char answer_char = break_point_answer.ToUpper()[0];
                    if (answer_char == 'Y')
                    {
                        Console.Out.WriteLine($"Enter breakpoint number: (1-{executable_line_count})");
                        Console.Out.Write(">>> ");
                        Int32.TryParse(Console.ReadLine(), out break_point);


                        if (break_point > executable_line_count || break_point <= 0)
                        {
                            Console.Error.WriteLine($"SAP1EMU: fatal error: debug: break point must be in range (1-{executable_line_count})");
                            Console.Error.WriteLine("debug terminated.");
                            CheckEnvAndExit();
                        }

                        Console.Out.WriteLine("\n\nDo you want to set a TState break point: (y/n) ");
                        Console.Out.WriteLine("If no, debug will single-step though the program starting at T=0");
                        Console.Out.Write(">>> ");
                        break_point_answer = Console.ReadLine();

                        answer_char = break_point_answer.ToUpper()[0];
                        if (answer_char == 'Y')
                        {
                            Console.Out.WriteLine("Enter TState breakpoint number: (1-6)");
                            Console.Out.Write(">>> ");
                            Int32.TryParse(Console.ReadLine(), out t_break_point);
                            if (t_break_point > 6 || t_break_point <= 0)
                            {
                                Console.Error.WriteLine("SAP1EMU: fatal error: debug: TState break point must be in range (1-6)");
                                Console.Error.WriteLine("debug terminated.");
                                CheckEnvAndExit();
                            }
                        }
                        else if (answer_char == 'N')
                        {
                            t_break_point = 0;
                        }
                        else
                        {
                            Console.Error.WriteLine($"SAP1EMU: fatal error: debug: Invalid charecter");
                            Console.Error.WriteLine("debug terminated.");
                            CheckEnvAndExit();
                        }

                    }
                    else if (answer_char == 'N')
                    {
                        break_point = 0;
                        t_break_point = 0;
                    }
                    else
                    {
                        Console.Error.WriteLine($"SAP1EMU: fatal error: debug: Invalid charecter");
                        Console.Error.WriteLine("debug terminated.");
                        CheckEnvAndExit();
                    }



                    // After this point, break_point and t_break_point should have their correct values

                    for (int i = 0; i < FrameStack.Count; i++)
                    {
                        Console.Clear();
                        Console.Out.WriteLine("SAP1EMU: DEBUG MODE");


                        Console.Out.WriteLine("|-----------------------------|");
                        Console.Out.WriteLine("| Assembly Program:           |");
                        Console.Out.WriteLine("|-----------------------------|");

                        line_mult = 1;
                        for (int asm_print_index = 1; asm_print_index < source_file_contents_line_count + 1; asm_print_index++)
                        {

                            Console.Out.WriteLine($"| {((asm_print_index * line_mult) != 0 ? asm_print_index.ToString() + ")" : "  ")} {source_file_contents[asm_print_index - 1]}".PadRight(30, ' ') + "|");

                            if (IsAfterHLT(source_file_contents[asm_print_index - 1].Substring(0, 3)))
                            {
                                line_mult = 0;
                            }
                        }
                        Console.Out.WriteLine("|-----------------------------|\n");


                        PrintRAM(FrameStack[i].RAM);

                        Console.WriteLine(FrameStack[i]);


                        int skip_point;
                        if (t_break_point == 1)
                        {
                            skip_point = ((break_point - 1) * 6);
                        }
                        else
                        {
                            skip_point = ((break_point - 1) * 6) + t_break_point - 1;
                        }
                        if (i < skip_point)
                        {
                            Thread.Sleep(100);
                        }
                        else
                        {
                            Console.WriteLine("Press Enter to step through the program:");
                            Console.Write("Type \"quit\" to exit:\n:");
                            if (Console.ReadLine().ToUpper().Contains("QUIT"))
                            {
                                CheckEnvAndExit();
                            }

                        }

                    }

                }

            }
        }


        private static void PrintRAM(List<string> RAMContents)
        {
            Console.Out.WriteLine("|--------------------------------------------|");
            Console.Out.WriteLine("| RAM Hex Dump:                              |");
            Console.Out.WriteLine("|--------------|--------------|--------------|");
            Console.Out.WriteLine($"| Address      | Value        | Text         |");
            Console.Out.WriteLine($"|--------------|--------------|--------------|");
            for (int i = 0; i < 16; i++)
            {
                string hex_upper = String.Format("{0:X1}", Convert.ToUInt16(RAMContents[i].Substring(0, 4), 2));
                string hex_lower = String.Format("{0:X1}", Convert.ToUInt16(RAMContents[i].Substring(4, 4), 2));

                Console.Out.WriteLine($"| 0x{String.Format("{0:X1}", i)}".PadRight(15, ' ') +
                    $"| 0x{hex_upper} 0x{hex_lower}      | "
                    + RAMContents[i].PadRight(10, ' ') + "   |");
            }
            Console.Out.WriteLine($"|--------------|--------------|--------------|");



        }
        private static bool IsAfterHLT(string s)
        {
            return (s.ToUpper() == "HLT");
        }
        private const string Banner =
            "*********************************************************\n" +
            "*  ____    _    ____  _ _____                           *\n" +
            "* / ___|  / \\  |  _ \\/ | ____|_ __ ___  _   _           *\n" +
            "* \\___ \\ / _ \\ | |_) | |  _| | '_ ` _ \\| | | |          *\n" +
            "*  ___) / ___ \\|  __/| | |___| | | | | | |_| |          *\n" +
            "* |____/_/   \\_\\_|   |_|_____|_| |_| |_|\\__,_|          *\n" +
            "*                                                       *\n" +
            "*********************************************************\n" +
            "* CC Bob Baker - 2020                                   *\n" +
            "*********************************************************\n";
    }
}
