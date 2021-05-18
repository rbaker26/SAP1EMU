using SAP1EMU.SAP2.Lib;
using SAP1EMU.SAP2.Lib.Components;
using SAP1EMU.SAP2.Lib.Registers;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace SAP1EMU.SAP2.Engine
{
    public class EngineProc
    {
        private string OutputReg = "";
        private readonly List<Frame> _FrameStack = new List<Frame>();
        private List<string> _RAMDump = new List<string>();
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

            IPort1 iport1 = new IPort1();
            IPort2 iport2 = new IPort2();
            
            MDR mdr = new MDR();
            RAM ram = new RAM();

            mdr.SetRefToRAM(ref ram);

            ALU alu = new ALU(ref areg, ref treg);

            areg.SetALUReference(ref alu);
            breg.SetALUReference(ref alu);
            creg.SetALUReference(ref alu);

            OReg3 oreg3 = new OReg3(ref alu);
            OReg4 oreg4 = new OReg4(ref alu);
            HexadecimalDisplay hexadecimalDisplay = new HexadecimalDisplay(ref oreg3);

            Flag flagReg = new Flag(ref alu);
            PC pc = new PC(ref flagReg);
            MAR mar = new MAR(ref ram);
            SEQ seq = SEQ.Instance();

            Wbus.Instance().Value = string.Concat(Enumerable.Repeat('0', 16));

            areg.Subscribe(clock);
            treg.Subscribe(clock);
            breg.Subscribe(clock);
            creg.Subscribe(clock);
            ireg.Subscribe(clock);
            mar.Subscribe(clock);
            
            pc.Subscribe(clock);
            alu.Subscribe(clock); // ALU must come after A and T
            flagReg.Subscribe(clock);
            ram.Subscribe(clock);
            mdr.Subscribe(clock);
            
            oreg3.Subscribe(clock);
            hexadecimalDisplay.Subscribe(clock);
            oreg4.Subscribe(clock);

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
            // will be known and set to a new object reference.
            Instruction currentInstruction = new Instruction()
            {
                OpCode = "???",
                TStates = 4 // Since by 4 TStates it should know what instruction it is on
            };

            List<string> controlWords = new List<string>();
            bool? didntJump = null;

            while (clock.IsEnabled)
            {
                // Log the Instruction
                if (TState == 4)
                {
                    currentInstruction = InstructionSet.Instructions.FirstOrDefault(i => i.BinCode.Equals(ireg.RegContent));
                    seq.LoadBackupControlWords(currentInstruction.MicroCode);

                    string iname = currentInstruction.OpCode;
                    int operandVal = Convert.ToInt32(ireg.RegContent, 2);
                    string hexOperand = "0x" + operandVal.ToString("X");
                }

                if (TState <= 3)
                {
                    seq.UpdateControlWordReg(TState, "00000000", didntJump);

                    if(didntJump ?? false)
                    {
                        pc.SkipByte();
                        didntJump = null;
                    }
                }
                else
                {
                    seq.UpdateControlWordReg(TState, ireg.RegContent);
                }

                clock.SendTicTok(tictok);
                tictok.ToggleClockState();
                clock.SendTicTok(tictok);
                tictok.ToggleClockState();

                tempFrame = new Frame(currentInstruction, TState, iport1, iport2, pc, mar, ram,
                                      ram.RAMDump(), mdr, ireg, SEQ.Instance(),
                                      Wbus.Instance().Value, areg, alu, flagReg,
                                      treg, breg, creg, oreg3, oreg4, hexadecimalDisplay);

                _FrameStack.Add(tempFrame);

                // HLT 
                if (ireg.RegContent.Equals("01110110", StringComparison.Ordinal) && TState == 5)
                {
                    clock.IsEnabled = false;

                    _RAMDump = ram.RAMDump();
                }

                if (loop_counter >= max_loop_count)
                {
                    throw new EngineRuntimeException("Engine Error: Infinite Loop Detected");
                }
                else
                {
                    loop_counter++;
                }

                if(TState == 7 && currentInstruction.OpCode.StartsWith('J'))
                {
                    pc.CheckForJumpCondition();

                    // PC is going to jump so do not let it fetch the next byte and immediately endx
                    if(!pc.WontJump)
                    {
                        currentInstruction.TStates = 7;
                        didntJump = true;
                    }
                    
                }

                if (TState < currentInstruction.TStates)
                {
                    TState++;
                }
                else
                {
                    TState = 1;
                    currentInstruction = new Instruction()
                    {
                        OpCode = "???",
                        TStates = 4 // Since by 4 TStates it should know what instruction it is on
                    };
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

        public Frame? FinalFrame()
        {
            if (_FrameStack.Count != 0)
            {
                return _FrameStack[^1];
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

        public List<string> GetRAMContents()
        {
            return _RAMDump;
        }

        // *************************************************************************
    }
}