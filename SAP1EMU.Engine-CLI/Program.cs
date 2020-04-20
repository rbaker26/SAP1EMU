using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using CommandLine;
using SAP1EMU.Assembler;
using SAP1EMU.Engine;
using SAP1EMU.Lib;

namespace SAP1EMU.Engine_CLI
{
    class Program
    {
        public class Options
        {
            [Option('s', "source-file", Required = true, HelpText = "\n<FileName>.s Input file containing assembly code.\n<FileName>.b Input file containing compilied binary.")]
            public string SourceFile { get; set; }

            [Option('v', "verbose", Required = false, HelpText = "Set output to verbose.\n(inlcudes debug statements from the engine)")]
            public bool Verbose { get; set; }
            [Option('V', "very-verbose", Required = false, HelpText = "Set output to very verbose.\n(includes debug statements from the engine and the input file in output)")]
            public bool VeryVerbose { get; set; }

            [Option('o', "output-file", Required = false, HelpText = "Place the output into <file>.", Default = "a.out")]
            public string OutputFile { get; set; }






            // -f and -F are mutually exclusive. And error will appeear if the user tries to use both.
            [Option('f', "fframe", SetName = "fframe", Required = false, HelpText = "Include final frame in the output file.")]
            public bool fframe { get; set; }
            [Option('F', "Fframe", SetName = "Fframe", Required = false, HelpText = "Include all frames in the output file.")]
            public bool Fframe { get; set; }




        }
        enum FileType
        {
            S, // ASM
            B  // BIN 
        }
        static void Main(string[] args)
        {
            _ = Parser.Default.ParseArguments<Options>(args)
                   .WithParsed<Options>(o =>
                   {
                       List<string> source_file_contents = new List<string>(); ;
                       FileType fileType = FileType.B;


                       if (o.SourceFile != null || o.SourceFile != "")
                       {
                           if (!File.Exists(o.SourceFile))
                           {
                               Console.Error.WriteLine($"SAP1EMU: error: {o.SourceFile}: No such file");
                               Console.Error.WriteLine($"SAP1EMU: fatal error: no input file");
                               Console.Error.WriteLine("emulation terminated.");
                               System.Environment.Exit(1);

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
                                       System.Environment.Exit(1);
                                   }
                               }


                               if (loc == 0)
                               {
                                   Console.Error.WriteLine($"SAP1EMU: error: {o.SourceFile}: File is empty");
                                   Console.Error.WriteLine($"SAP1EMU: fatal error: no valid input file");
                                   Console.Error.WriteLine("emulation terminated.");
                                   System.Environment.Exit(1);
                               }
                               if (loc > 16)
                               {
                                   Console.Error.WriteLine($"SAP1EMU: error: {o.SourceFile}: invalid file: Contains more than 16 lines of code.");
                                   Console.Error.WriteLine($"SAP1EMU: fatal error: no valid input file");
                                   Console.Error.WriteLine("emulation terminated.");
                                   System.Environment.Exit(1);
                               }



                           }



                           List<string> compiled_binary;

                           if (fileType == FileType.S)
                           {
                               compiled_binary = Assemble.ParseFileContents(source_file_contents);
;
                           }
                           else
                           {
                               compiled_binary = source_file_contents;
                           }


                           RAMProgram rmp = new RAMProgram(compiled_binary);
                           EngineProc engine = new EngineProc();



                           StringBuilder sb_out = new StringBuilder();
                           TextWriter writer_out = new StringWriter(sb_out);
                           Console.SetOut(writer_out);


                           StringBuilder sb_error = new StringBuilder();
                           TextWriter writer_error = new StringWriter(sb_error);
                           Console.SetError(writer_error);


                           engine.Init(rmp);
                           engine.Run();



                           string engine_output = "************************************************************\n" 
                                                + "Final Output Register Value: " + engine.GetOutput() 
                                                + "\n************************************************************\n\n";
                           if (o.fframe)
                           {
                               engine_output += "\n" + engine.FinalFrame();
                           }
                           if (o.Fframe)
                           {
                               StringBuilder sb = new StringBuilder();
                               StringWriter fw = new StringWriter(sb);

                               List<Frame> FrameStack = engine.FrameStack();
                               foreach (Frame frame in FrameStack)
                               {
                                   fw.WriteLine(frame.ToString());
                               }
                               fw.Flush();

                               engine_output += "\n" + sb.ToString();
                           }

                           var standardOutput = new StreamWriter(Console.OpenStandardOutput());
                           standardOutput.AutoFlush = true;
                           Console.SetOut(standardOutput);

                           var standardError = new StreamWriter(Console.OpenStandardError());
                           standardError.AutoFlush = true;
                           Console.SetError(standardError);



                           string stdout = sb_out.ToString();
                           string stderror = sb_error.ToString();

                           
                           
                           File.WriteAllText(o.OutputFile, engine_output);

                          
                       }


                   });
        }
    }
}
