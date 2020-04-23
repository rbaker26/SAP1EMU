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
        string OutPutRegContents = ""; // todo add frame support
        private List<Frame> _FrameStack = new List<Frame>();

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


        private RAMProgram program { get; set; }
        public void Init(RAMProgram program)
        {
            if (program == null)
            {
                this.program = new RAMProgram(new List<string>());
            }
            this.program = program;
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
            PC pc = new PC();
            RAM ram = new RAM();

            ALU alu = new ALU(ref areg, ref breg);
            MReg mreg = new MReg(ref ram);
            SEQ seq = SEQ.Instance();

            Wbus.Instance().Value = "00000000";

            areg.Subscribe(clock);
            breg.Subscribe(clock);
            ireg.Subscribe(clock);
            mreg.Subscribe(clock);
            oreg.Subscribe(clock);
            pc.Subscribe(clock);
            alu.Subscribe(clock); // ALU must come after A and B
            ram.Subscribe(clock);




            // Load the program into the RAM
            ram.LoadProgram(program);

            


            Frame tempFrame;

            #region Program_Exec
            // Since T1-T3 for all of the Intruction is the same,
            // LDA or "0000" will be used as the intruction for all T1-T3's
            clock.IsEnabled = true;
            while (clock.IsEnabled)
            {
                System.Console.Error.WriteLine("T1:");
                // T1 ************************************
                seq.UpdateControlWordReg(1, "0000");
                clock.SendTicTok(tictok);


                //CODE 
                tictok.ToggleClockState();
                clock.SendTicTok(tictok);
                tictok.ToggleClockState();
                System.Console.Error.WriteLine("\n");

                tempFrame = new Frame(ireg.ToString(), 1, areg, breg, ireg, mreg, oreg, pc, alu, ram, seq, Wbus.Instance().ToString());
                _FrameStack.Add(tempFrame);
                // ***************************************

                // T2 ************************************
                System.Console.Error.WriteLine("T2:");

                seq.UpdateControlWordReg(2, "0000");
                clock.SendTicTok(tictok);


                //CODE 
                tictok.ToggleClockState();
                clock.SendTicTok(tictok);
                tictok.ToggleClockState();
                System.Console.Error.WriteLine("\n");

                tempFrame = new Frame(ireg.ToString(), 2, areg, breg, ireg, mreg, oreg, pc, alu, ram, seq, Wbus.Instance().ToString());
                _FrameStack.Add(tempFrame);
                // ***************************************

                // T3 ************************************
                System.Console.Error.WriteLine("T3:");

                seq.UpdateControlWordReg(3, "0000");
                clock.SendTicTok(tictok);


                //CODE 
                tictok.ToggleClockState();
                clock.SendTicTok(tictok);
                tictok.ToggleClockState();
                System.Console.Error.WriteLine("\n");

                tempFrame = new Frame(ireg.ToString(), 3, areg, breg, ireg, mreg, oreg, pc, alu, ram, seq, Wbus.Instance().ToString());
                _FrameStack.Add(tempFrame);
                // ***************************************

                // T4 ************************************
                System.Console.Error.WriteLine("T4:");
                seq.UpdateControlWordReg(4, ireg.ToString());



                clock.SendTicTok(tictok);
                //CODE
                tictok.ToggleClockState();
                clock.SendTicTok(tictok);
                //CODE
                tictok.ToggleClockState();
                System.Console.Error.WriteLine("\n");

                tempFrame = new Frame(ireg.ToString(), 4, areg, breg, ireg, mreg, oreg, pc, alu, ram, seq, Wbus.Instance().ToString());
                _FrameStack.Add(tempFrame);
                // ***************************************

                // T5 ************************************
                System.Console.Error.WriteLine("T5:");

                seq.UpdateControlWordReg(5, ireg.ToString());


                clock.SendTicTok(tictok);
                //CODE
                tictok.ToggleClockState();
                clock.SendTicTok(tictok);
                //CODE
                tictok.ToggleClockState();
                System.Console.Error.WriteLine("\n");

                tempFrame = new Frame(ireg.ToString(), 5, areg, breg, ireg, mreg, oreg, pc, alu, ram, seq, Wbus.Instance().ToString());
                _FrameStack.Add(tempFrame);
                // ***************************************

                // T6 ************************************
                System.Console.Error.WriteLine("T6:");

                seq.UpdateControlWordReg(6, ireg.ToString());

                clock.SendTicTok(tictok);
                //CODE
                tictok.ToggleClockState();
                clock.SendTicTok(tictok);
                //CODE
                tictok.ToggleClockState();
                System.Console.Error.WriteLine("\n");

                tempFrame = new Frame(ireg.ToString(), 6, areg, breg, ireg, mreg, oreg, pc, alu, ram, seq, Wbus.Instance().ToString());
                _FrameStack.Add(tempFrame);
                // ***************************************


                if (ireg.ToString() == "1111")
                {
                    clock.IsEnabled = false;
                }
            }
            OutPutRegContents = oreg.ToString();
            #endregion


            System.Console.WriteLine("A      Register: " + areg.ToString());
            System.Console.WriteLine("B      Register: " + breg.ToString());
            System.Console.WriteLine("Output Register: " + oreg.ToString());


            #region Test Code
            //****************************************
            //Wbus.Instance().Value = "10101010";
            //
            //System.Console.WriteLine(areg.ToString());
            //
            //seq.UpdateControlWordReg(1, "0000");
            //clock.SendTicTok(tictok);
            //System.Console.WriteLine(areg.ToString());
            //
            //seq.UpdateControlWordReg(2, "0000");
            //clock.SendTicTok(tictok);
            //System.Console.WriteLine(areg.ToString());
            //
            //seq.UpdateControlWordReg(3, "0000");
            //clock.SendTicTok(tictok);
            //System.Console.WriteLine(areg.ToString());
            //
            //seq.UpdateControlWordReg(4, "0000");
            //clock.SendTicTok(tictok);
            //System.Console.WriteLine(areg.ToString());
            //
            //seq.UpdateControlWordReg(5, "0000");
            //clock.SendTicTok(tictok);
            //System.Console.WriteLine(areg.ToString());
            //
            //seq.UpdateControlWordReg(6, "0000");
            //clock.SendTicTok(tictok);
            //System.Console.WriteLine(areg.ToString());
            //****************************************
            #endregion
        }


    }



}
