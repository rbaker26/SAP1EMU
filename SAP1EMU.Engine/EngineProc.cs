using System;
using System.Collections.Generic;
using System.Text;
using SAP1EMU.Lib;

using SAP1EMU.Lib.Registers;
using SAP1EMU.Lib.Components;

namespace SAP1EMU.Engine
{
    public class EngineProc
    {
        private RAMProgram program { get; set; }
        void Init(RAMProgram program) 
        {
            if(program == null)
            {
                this.program = new RAMProgram(new List<string>());
            }
            this.program = program;
        }


        public void Run()
        {
            Clock clock = new Clock();
            TicTok tictok = new TicTok();

            AReg areg = new AReg();
            BReg breg = new BReg();
            IReg ireg = new IReg();
            MReg mreg = new MReg();
            OReg oreg = new OReg();
            PC pc = new PC();
            ALU alu = new ALU();
            RAM ram = new RAM();

            SEQ seq = SEQ.Instance();


            areg.Subscribe(clock);
            breg.Subscribe(clock);
            ireg.Subscribe(clock);
            mreg.Subscribe(clock);
            oreg.Subscribe(clock);
            pc.Subscribe(clock);
            alu.Subscribe(clock);
            ram.Subscribe(clock);




            // Load the program into the RAM
            ram.LoadProgram(program);
            
            // Since T1-T3 for all of the Intruction is the same,
            // LDA or "0000" will be used as the intruction for all T1-T3's
            while (clock.IsEnabled)
            {
                // T1 ************************************
                seq.UpdateControlWordReg(1, "0000");
                clock.SendTicTok(tictok);


                //CODE 
                tictok.ToggleClockState();
                clock.SendTicTok(tictok);
                tictok.ToggleClockState();
                // ***************************************

                // T2 ************************************
                seq.UpdateControlWordReg(2, "0000");
                clock.SendTicTok(tictok);


                //CODE 
                tictok.ToggleClockState();
                clock.SendTicTok(tictok);
                tictok.ToggleClockState();
                // ***************************************

                // T3 ************************************
                seq.UpdateControlWordReg(3, "0000");
                clock.SendTicTok(tictok);


                //CODE 
                tictok.ToggleClockState();
                clock.SendTicTok(tictok);
                tictok.ToggleClockState();
                // ***************************************

                // T4 ************************************
                clock.SendTicTok(tictok);
                //CODE
                tictok.ToggleClockState();
                clock.SendTicTok(tictok);
                //CODE
                tictok.ToggleClockState();
                // ***************************************

                // T5 ************************************
                clock.SendTicTok(tictok);
                //CODE
                tictok.ToggleClockState();
                clock.SendTicTok(tictok);
                //CODE
                tictok.ToggleClockState();
                // ***************************************

                // T6 ************************************
                clock.SendTicTok(tictok);
                //CODE
                tictok.ToggleClockState();
                clock.SendTicTok(tictok);
                //CODE
                tictok.ToggleClockState();
                // ***************************************

            }

            clock.SendTicTok(tictok);

        }


    }



}
