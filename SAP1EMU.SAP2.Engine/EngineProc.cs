using SAP1EMU.SAP2.Lib;
using SAP1EMU.SAP2.Lib.Components;
using SAP1EMU.SAP2.Lib.Registers;

using System;
using System.Collections.Generic;
using System.Linq;

namespace SAP1EMU.SAP2.Engine
{
    public class EngineProc
    {
        private string OutputReg = "";
        private readonly List<Frame> _FrameStack = new List<Frame>();
        private RAMProgram Program { get; set; } = new RAMProgram(new List<string>());
        private InstructionSet InstructionSet { get; set; } = new InstructionSet();
        private const string DefaultInstructionSetName = "Malvino";
        private IDecoder _decoder { get; set; }

        // *************************************************************************
        // Init Engine
        // *************************************************************************
        public void Init(RAMProgram program, IDecoder decoder, string InstructionSetName = DefaultInstructionSetName)
        {
            // Get Instruction Set
            InstructionSet = OpCodeLoader.GetSet(InstructionSetName);

            _decoder = decoder;

            // Init RAM
            if (program == null)
            {
                Program = new RAMProgram(new List<string>());
            }

            Program = program;
        }

        // *************************************************************************

        // *************************************************************************
        // Engine Runtime
        // *************************************************************************
        public void Run()
        {
            Clock clock = new Clock();
            TicTok tictok = new TicTok();

            tictok.Init();

            AReg areg = new AReg();
            TReg treg = new TReg();
            BReg breg = new BReg();
            CReg creg = new CReg();
            IReg ireg = new IReg();

            IPort1 port1 = new IPort1();
            IPort2 port2 = new IPort2();
            OReg3 oreg3 = new OReg3();
            OReg4 oreg4 = new OReg4();

            MDR mdr = new MDR();
            RAM ram = new RAM();

            mdr.SetRefToRAM(ref ram);
            ram.SetRefToMDR(ref mdr);

            ALU alu = new ALU(ref areg, ref treg);
            Flag flagReg = new Flag(ref alu);
            PC pc = new PC(ref flagReg);
            MReg mreg = new MReg(ref ram);
            SEQ seq = SEQ.Instance();

            Wbus.Instance().Value = string.Concat(Enumerable.Repeat('0', 16));

            areg.Subscribe(clock);
            treg.Subscribe(clock);
            breg.Subscribe(clock);
            creg.Subscribe(clock);
            ireg.Subscribe(clock);
            mreg.Subscribe(clock);
            oreg3.Subscribe(clock);
            oreg4.Subscribe(clock);
            pc.Subscribe(clock);
            alu.Subscribe(clock); // ALU must come after A and B
            ram.Subscribe(clock);
            mdr.Subscribe(clock);

            // Load the program into the RAM
            ram.LoadProgram(Program);

            // Load the intsructionSet into the SEQ
            seq.Load(InstructionSet);

            Frame tempFrame;

            #region Program_Exec

            // Since T1-T3 for all of the Intruction is the same,
            // LDA or "0000" will be used as the intruction for all T1-T3's
            clock.IsEnabled = true;

            int max_loop_count = 10_000;
            int loop_counter = 0;

            int TState = 1;

            // A basic empty instruction state with 3 TStates since on the 4th the instruction
            // will be known and set.
            Instruction currentInstruction = new Instruction()
            {
                TStates = 3
            };

            while (clock.IsEnabled)
            {
                if (TState <= 3)
                {
                    seq.UpdateControlWordReg(TState, "00000000");
                }
                else
                {
                    seq.UpdateControlWordReg(TState, ireg.RegContent);
                }

                // Log the Instruction
                if (TState == 4)
                {
                    currentInstruction = InstructionSet.Instructions.FirstOrDefault(x => x.BinCode.Equals(ireg.ToString()));
                    string iname = currentInstruction.OpCode;
                    int operandVal = Convert.ToInt32(ireg.RegContent, 2);
                    string hexOperand = "0x" + operandVal.ToString("X");
                }

                clock.SendTicTok(tictok);
                tictok.ToggleClockState();
                clock.SendTicTok(tictok);
                tictok.ToggleClockState();

                tempFrame = new Frame(ireg.RegContent, TState, areg, breg, ireg, mreg, oreg3, pc, alu, ram.RAMDump(), ram, seq, Wbus.Instance().ToString(), flagReg, _decoder, InstructionSet.SetName);
                _FrameStack.Add(tempFrame);

                // HLT 
                if (ireg.ToString() == "01110110" && TState == 5)
                {
                    clock.IsEnabled = false;
                }

                if (loop_counter >= max_loop_count)
                {
                    throw new EngineRuntimeException("Engine Error: Infinite Loop Detected");
                }
                else
                {
                    loop_counter++;
                }

                // TODO -> figure out what to do when jumps take 7
                if (TState < currentInstruction.TStates)
                {
                    TState++;
                }
                else
                {
                    TState = 1;
                }
            }

            OutputReg = oreg3.ToString();

            #endregion Program_Exec
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
            return OutputReg;
        }

        // *************************************************************************
    }
}