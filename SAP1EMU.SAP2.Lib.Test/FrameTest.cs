﻿using Microsoft.VisualStudio.TestTools.UnitTesting;

using SAP1EMU.SAP2.Lib.Components;
using SAP1EMU.SAP2.Lib.Registers;

namespace SAP1EMU.SAP2.Lib.Test
{
    //[TestClass]
    //public class FrameTest
    //{
    //    private IDecoder _decoder = new InstructionDecoder();

    //    [TestMethod]
    //    public void TestToString()
    //    {
            //Clock clock = new Clock();
            //TicTok tictok = new TicTok();

            //tictok.Init(); ;
            //int TState = 0;
            //AReg areg = new AReg();
            //BReg breg = new BReg();
            //IReg ireg = new IReg();
            //OReg oreg = new OReg();
            //RAM ram = new RAM();

            //PC pc = new PC(ref areg);
            //ALU alu = new ALU(ref areg, ref breg);
            //MReg mreg = new MReg(ref ram);
            //SEQ seq = SEQ.Instance();

            //Wbus.Instance().Value = "00000000";
            //Flags.Instance().Clear();

            //Frame frame = new Frame(ireg.ToString(), TState, areg, breg, ireg, mreg, oreg, pc, alu, ram.RAMDump(), ram, seq, Wbus.Instance().ToString(), Flags.Instance(), _decoder);
            //_ = frame.ToString();
            //_ = frame.OutputRegister();
    //    }
    //}
}