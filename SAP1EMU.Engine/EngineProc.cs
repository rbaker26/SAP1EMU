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
        void Init(RAMProgram program) { }
        public void Run() 
        {
            Clock clock = new Clock();
            TicTok tictok = new TicTok();

            AReg areg = new AReg();
            BReg breg = new BReg();
            IReg ireg = new IReg();
            MReg mreg = new MReg();
            OReg oreg = new OReg();


            areg.Subscribe(clock);
            breg.Subscribe(clock);
            ireg.Subscribe(clock);
            mreg.Subscribe(clock);
            oreg.Subscribe(clock);



            clock.SendTicTok(tictok);
            
        }


    }

    
    
}
