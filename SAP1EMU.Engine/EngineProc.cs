using System;
using System.Collections.Generic;
using System.Text;
using SAP1EMU.Lib;

using SAP1EMU.Lib.Registers;
using SAP1EMU.Lib.Components;
using System.IO;

namespace SAP1EMU.Engine
{
    public class EngineProc
    {
        string OutPutRegContents = "";
        private readonly List<Frame> _FrameStack = new List<Frame>();

        public List<Frame> FrameStack()
        {
            return _FrameStack;
        }


        public Frame FinalFrame()
        {
            if(_FrameStack.Count != 0)
            {
                return _FrameStack[_FrameStack.Count - 1];
            }
            else
            {
                return null;
            }
        }

        public string GetOutput()
        {
            return OutPutRegContents;
        }


        private RAMProgram Program { get; set; }
        public void Init(RAMProgram program)
        {
            if (program == null)
            {
                this.Program = new RAMProgram(new List<string>());
            }
            this.Program = program;
        }


        public void Run()
        {

            Clock clock = new Clock();
            TicTok tictok = new TicTok();


            tictok.Init(); ;

            AReg areg = new AReg();
            BReg breg = new BReg();
            IReg ireg = new IReg();
            OReg oreg = new OReg();
            RAM ram = new RAM();

            PC pc = new PC(ref ireg, ref areg);
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




            // Load the program into the RAM
            ram.LoadProgram(Program);

            


            Frame tempFrame;

            #region Program_Exec
            // Since T1-T3 for all of the Intruction is the same,
            // LDA or "0000" will be used as the intruction for all T1-T3's
            clock.IsEnabled = true;

            // These vars will make sure that the engine will not hang if there is an infinite loop
            int max_loop_count = 5000;
            int loop_counter = 0;

            int TState = 1;
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

                clock.SendTicTok(tictok);
                tictok.ToggleClockState();
                clock.SendTicTok(tictok);
                tictok.ToggleClockState();
                tempFrame = new Frame(ireg.ToString(), TState, areg, breg, ireg, mreg, oreg, pc, alu, ram.RAMDump(), ram, seq, Wbus.Instance().ToString());
                _FrameStack.Add(tempFrame);

                if (ireg.ToString() == "1111" && TState == 6)
                {
                    clock.IsEnabled = false;
                }


                // Infinite Loop Check
                if (loop_counter >= max_loop_count)
                {
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

            OutPutRegContents = oreg.ToString();
            #endregion
        }


    }



}
