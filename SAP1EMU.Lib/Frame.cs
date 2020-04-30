using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using SAP1EMU.Lib.Components;
using SAP1EMU.Lib.Registers;
using SAP1EMU.Lib.Utilities;

namespace SAP1EMU.Lib
{
    public class Frame
    {
        public string Instruction { get; private set; } = "????";
        public int TState { get; private set; } = 0;
        public string AReg { get; private set; } = "????";
        public string BReg { get; private set; } = "????";
        public string IReg { get; private set; } = "????";
        public string MReg { get; private set; } = "????";
        public string OReg { get; private set; } = "????";
        public string PC { get; private set; } = "????";
        public string ALU { get; private set; } = "????";
        public string SEQ { get; private set; } = "????";
        public string WBus { get; private set; } = "????";
        public string RAM_Reg { get; private set; } = "?????";

        public List<string> RAM { get; private set; } // The reason this is here is that the RAM might change if a STA simular command is issued.
        public Frame(string instruction, int TState, AReg areg, BReg breg, IReg ireg, MReg mreg, OReg oreg, PC pc, ALU alu, List<string> ramContents, RAM ram, SEQ seq, string wbus_string)
        {
            this.RAM = new List<string>();

            this.TState = TState;

            this.AReg = areg.ToString();
            this.BReg = breg.ToString(); 
            this.IReg = ireg.ToString();  // The realy ToString() is in use with a substring in it.  This is needed for proper operation
            this.MReg = mreg.ToString(); 
            this.OReg = oreg.ToString();
            this.PC = pc.ToString().Substring(4, 4);
            this.ALU = alu.ToString();

            foreach(string s in ramContents)
            {
                RAM.Add(s);
            }


            this.SEQ = seq.ToString();
            this.WBus = wbus_string; // I didnt want to mess with the Singleton in the frame, so the value will just be passed as a string
            this.RAM_Reg = ram.ToString();

            if (instruction.Length == 0)
            {
                this.IReg = "????";
            }

        }

        private string InstuctionDecode(string BinInstruction, int TState)
        {
            List<string> KnownInstructions = new List<string> { "LDA", "ADD", "SUB", "STA", "JMP", "", "", "", "", "", "", "", "", "", "OUT", "HLT" };
            string temp = KnownInstructions[BinConverter.Bin4ToInt(BinInstruction)];

            if(TState < 4)
            {
                return "???";
            }
            if (temp != "")
            {
                return temp;
            }
            else
            {
                return BinInstruction;
            }
        }


        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            StringWriter tw = new StringWriter(sb);


            int unsigned_ouput = BinConverter.Bin8ToInt(OReg);

            int signed_output = 0;
            if (OReg != null)
            {
                if (OReg[0] == '1')
                {
                    signed_output = -1 * (255 - unsigned_ouput + 1);
                }
                else
                {
                    signed_output = unsigned_ouput;
                }
            }

            tw.WriteLine($"************************************************************");//60
            tw.WriteLine($"* Instruction: {InstuctionDecode(IReg,TState)}     TState: {TState}                           *");
            tw.WriteLine( "************************************************************");
            tw.WriteLine($"* PC:         {PC}              A Register:      {AReg}".PadRight(59) + "*");
            tw.WriteLine($"* MAR:        {MReg}              B Register:      {BReg}".PadRight(59) + "*");
            tw.WriteLine($"* RAM:        {RAM_Reg}          ALU:             {ALU}".PadRight(59) + "*");
            tw.WriteLine($"* I Register: {IReg}              Output Register: {OReg}".PadRight(59) + "*");
            tw.WriteLine($"* Sequencer:  {SEQ}      ".PadRight(59) + "*");
            tw.WriteLine($"************************************************************");
            tw.WriteLine($"* Output Unsigned: {unsigned_ouput}".PadRight(59) + "*");
            tw.WriteLine($"* Output Signed:   {signed_output}".PadRight(59) + "*");
            tw.WriteLine($"************************************************************");




            tw.Flush();
            return sb.ToString();
        }
    }
}
