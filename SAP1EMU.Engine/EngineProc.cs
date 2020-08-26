using System;
using System.Collections.Generic;
using System.Text;
using SAP1EMU.Lib;


using SAP1EMU.Lib.Registers;
using SAP1EMU.Lib.Components;
using System.IO;
using System.Diagnostics;

namespace SAP1EMU.Engine
{
    public class EngineProc
    {
        
        string OutPutRegContents = "";
        private readonly List<Frame> _FrameStack = new List<Frame>();
        private RAMProgram Program { get; set; }
        private InstructionSet InstructionSet { get; set; }
        private const string DefaultInstructionSetName = "SAP1Emu";

        // *************************************************************************
        // Init Engine
        // *************************************************************************
        public void Init(RAMProgram program, string InstructionSetName = DefaultInstructionSetName)
        {
            //string log_name = "runtime_log.txt";
            //// Clear Old Log 
            //if (File.Exists(log_name))
            //{
            //    File.Delete(log_name);
            //}
            //File.Create(log_name).Close();  // must close the file or the handle will stay open and be locked


            //// Init Logger
            //Log.Logger = new LoggerConfiguration()
            //    .WriteTo.File(log_name)
            //   // .WriteTo.Console()
            //    .CreateLogger();

            //Serilog.Debugging.SelfLog.Enable(msg => Debug.WriteLine(msg));
            //Serilog.Debugging.SelfLog.Enable(Console.Error);

            //Log.Information("SAP1Emu: Begin Engine Initialization");


            // Get Instruction Set
            //Log.Information($"SAP1Emu: Using Instruction Set: \"{InstructionSetName}\"");
            InstructionSet = OpCodeLoader.GetSet(InstructionSetName);

            // Init RAM
            if (program == null)
            {
                this.Program = new RAMProgram(new List<string>());
            }
            this.Program = program;
    
            //Log.Information("SAP1Emu: Finished Engine Initialization");

        }
        // *************************************************************************


        // *************************************************************************
        // Engine Runtime 
        // *************************************************************************
        public void Run()
        {
            //Log.Information("SAP1Emu: Begin Engine Run");

            //Log.Verbose("SAP1Emu: Initializing Registers");
            Clock clock = new Clock();
            TicTok tictok = new TicTok();


            tictok.Init();

            AReg areg = new AReg();
            BReg breg = new BReg();
            IReg ireg = new IReg();
            OReg oreg = new OReg();
            RAM ram = new RAM();

            PC pc = new PC(ref areg);
            ALU alu = new ALU(ref areg, ref breg);
            MReg mreg = new MReg(ref ram);
            SEQ seq = SEQ.Instance();

            Wbus.Instance().Value = "00000000";
            Flags.Instance().Clear();

            areg.Subscribe(clock);
            breg.Subscribe(clock);
            ireg.Subscribe(clock);
            mreg.Subscribe(clock);
            oreg.Subscribe(clock);
            pc.Subscribe(clock);
            alu.Subscribe(clock); // ALU must come after A and B
            ram.Subscribe(clock);


          //  Log.Verbose("SAP1Emu: Initialized Registers");



         //   Log.Information("SAP1Emu: Loading Ram");
            // Load the program into the RAM
            ram.LoadProgram(Program);
        //    Log.Information("SAP1Emu: RAM:\n{RAM}", Program.RamContents);

            // Load the intsructionSet into the SEQ
            seq.Load(InstructionSet);
        //    Log.Information($"SAP1Emu: Loaded Instruction Set: \"{InstructionSet.SetName}\"");



            Frame tempFrame;

            #region Program_Exec
            // Since T1-T3 for all of the Intruction is the same,
            // LDA or "0000" will be used as the intruction for all T1-T3's
            clock.IsEnabled = true;

            // These vars will make sure that the engine will not hang if there is an infinite loop
            int warning1_loop_counter = 3000;
            int warning2_loop_counter = 5000;
            int warning3_loop_counter = 7000;
            int warning4_loop_counter = 9000;
            int max_loop_count = 10000;
            int loop_counter = 0;

            int TState = 1;
          //  Log.Information("SAP1Emu: Start Program Execution");
            while(clock.IsEnabled)
            {
                if(TState <= 3)
                {
                    seq.UpdateControlWordReg(TState, "0000");
                }
                else
                {
                    seq.UpdateControlWordReg(TState, ireg.ToString());
                }


                // Log the Instruction 
                if(TState == 4)
                {
                    string iname = InstructionSet.instructions.Find(x => x.BinCode.Equals(ireg.ToString())).OpCode;
                    int operandVal = Convert.ToInt32(ireg.ToString_Frame_Use().Substring(4, 4), 2);
                    string hexOperand = "0x" + operandVal.ToString("X");
                //    Log.Information($"SAP1Emu: Instruction: {iname}, Operand: {hexOperand}");
                }

                clock.SendTicTok(tictok);
                tictok.ToggleClockState();
                clock.SendTicTok(tictok);
                tictok.ToggleClockState();
                tempFrame = new Frame(ireg.ToString(), TState, areg, breg, ireg, mreg, oreg, pc, alu, ram.RAMDump(), ram, seq, Wbus.Instance().ToString());
                _FrameStack.Add(tempFrame);

                if (ireg.ToString() == "1111" && TState == 6)
                {
                   // Log.Information("SAP1Emu: HLT Detected");
                    clock.IsEnabled = false;
                }


                // Infinite Loop Checks
                //if(loop_counter == warning1_loop_counter)
                //{
                //    Log.Warning($"SAP1Emu: Infinite Loop Warning: interations count: {warning1_loop_counter} ");
                //}
                //else if (loop_counter == warning2_loop_counter)
                //{
                //    Log.Warning($"SAP1Emu: Infinite Loop Warning: interations count: {warning2_loop_counter} ");
                //}
                //else if (loop_counter == warning3_loop_counter)
                //{
                //    Log.Warning($"SAP1Emu: Infinite Loop Warning: interations count: {warning3_loop_counter} ");
                //}
                //else if (loop_counter == warning4_loop_counter)
                //{
                //    Log.Warning($"SAP1Emu: Infinite Loop Warning: interations count: {warning4_loop_counter} ");
                //}



                if (loop_counter >= max_loop_count)
                {
                   // Log.Fatal($"SAP1Emu: Infinite Loop Fatal Error: Infinite Loop Detected");
                   // Log.CloseAndFlush();
                    throw new EngineRuntimeException("Engine Error: Infinite Loop Detected");
                }
                else
                {
                    loop_counter++;
                }

                if(TState < 6)
                {
                    TState++;
                }
                else
                {
                    TState = 1;
                }
            }
           // Log.Information("SAP1Emu: End Program Execution");


            OutPutRegContents = oreg.ToString();
          //  Log.Information("SAP1Emu: End Engine Run");

          //  Log.CloseAndFlush();

            #endregion
        }
        // *************************************************************************






        // *************************************************************************
        // Output Functions
        // *************************************************************************
        public List<Frame> FrameStack()
        {
            return _FrameStack;
        }


        public Frame FinalFrame()
        {
            if (_FrameStack.Count != 0)
            {
                return _FrameStack[_FrameStack.Count - 1];
            }
            else
            {
                return null;
            }
        }

        public string GetOutputReg()
        {
            return OutPutRegContents;
        }
        // *************************************************************************




    }



}
