using System;
using System.Collections.Generic;
using System.Text;
using SAP1EMU.Lib.Components;
using SAP1EMU.Lib.Registers;

namespace SAP1EMU.Lib
{
    public class Frame
    {

        public AReg areg  {get; private set;}
        public BReg breg  {get; private set;}
        public IReg ireg  {get; private set;}
        public MReg mreg  {get; private set;}
        public OReg oreg  {get; private set;}
        public PC   PCReg {get; private set;}
        public ALU alu { get; private set; }
        public RAM ram { get; private set; }
        public SEQ seq { get; private set; }

        public Frame(AReg areg, BReg breg, IReg ireg, MReg mreg, OReg oreg, PC pCReg, ALU alu, RAM ram, SEQ seq)
        {
            areg = new AReg();
            breg = new BReg();
            ireg = new IReg();
            mreg = new MReg();
            oreg = new OReg();
            PCReg = new PC();
            alu = new ALU();
            ram = new RAM();
            seq = new SEQ();
            this.areg = areg;
            this.breg = breg;
            this.ireg = ireg;
            this.mreg = mreg;
            this.oreg = oreg;
            PCReg = pCReg;
            this.alu = alu;
            this.ram = ram;
            this.seq = seq;
        }

    }
}
