using SAP1EMU.Lib.Components;
using SAP1EMU.Lib.Registers;
using SAP1EMU.Lib.Utilities;

using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SAP1EMU.Lib
{
    public class Frame
    {
        public string Instruction { get; private set; } = "???";
        public int TState { get; private set; } = 0;
        public string AReg { get; private set; } = "0000 0000";
        public string BReg { get; private set; } = "0000 0000";
        public string IReg { get; private set; } = "0000 0000";
        public string IRegShort { get; private set; } = "0000 0000";
        public string MReg { get; private set; } = "0000 0000";
        public string OReg { get; private set; } = "0000 0000";
        public string PC { get; private set; } = "0000";
        public string ALU { get; private set; } = "0000 0000";
        public string SEQ { get; private set; } = "0000 0000";
        public string WBus { get; private set; } = "0000 0000";
        public string RAM_Reg { get; private set; } = "0000 0000";
        public string Overflow_Flag { get; private set; } = "0";
        public string Underflow_Flag { get; private set; } = "0";

        public List<string> RAM { get; private set; } // The reason this is here is that the RAM might change if a STA simular command is issued.

        public Frame(string instruction, int TState, AReg areg, BReg breg, IReg ireg, MReg mreg, OReg oreg, PC pc, ALU alu, List<string> ramContents, RAM ram, SEQ seq, string wbus_string, Flags flags, IDecoder decoder, string SetName = "SAP1EMU")
        {
            this.RAM = new List<string>();

            this.TState = TState;

            this.AReg = areg.ToString_Frame_Use();
            this.BReg = breg.ToString_Frame_Use();
            this.IRegShort = ireg.ToString();
            this.IReg = ireg.ToString_Frame_Use();  // The real ToString() is in use with a substring in it.  This is needed for proper operation
            this.MReg = mreg.ToString_Frame_Use();
            this.OReg = oreg.ToString_Frame_Use();
            this.PC = pc.ToString().Substring(4, 4);
            this.ALU = alu.ToString();
            this.WBus = wbus_string;

            this.Overflow_Flag = flags.Overflow.ToString();
            this.Underflow_Flag = flags.Underflow.ToString();

            foreach (string s in ramContents)
            {
                RAM.Add(s);
            }

            this.SEQ = seq.ToString();
            this.WBus = wbus_string; // I didnt want to mess with the Singleton in the frame, so the value will just be passed as a string
            this.RAM_Reg = ram.ToString_Frame_Use();

            if (instruction.Length == 0)
            {
                this.IReg = "???";
            }

            if (TState > 3)
            {
                //Instruction = OpCodeLoader.DecodeInstruction(IReg.Substring(0, 4), SetName); // TODO this is really inifeciant.  Should prob make a service and inject it
                Instruction = decoder.Decode(IReg.Substring(0, 4), SetName);
            }
            else
            {
                Instruction = "???";
            }
        }

        // TODO - Repleace with something in the LIB OpCodeLoader
        // TODO -  is the still used? I think I replaced this somewhere else in the code
        private string InstuctionDecode(string BinInstruction, int TState)
        {
            List<string> KnownInstructions = new List<string> { "LDA", "ADD", "SUB", "STA", "JMP", "JEQ", "", "", "", "JIC", "", "", "", "", "OUT", "HLT" };
            string temp = KnownInstructions[BinConverter.Bin4ToInt(BinInstruction)];

            if (TState < 4)
            {
                return "???";
            }
            if (!string.IsNullOrEmpty(temp))
            {
                return temp;
            }
            else
            {
                return BinInstruction;
            }
        }

        public string OutputRegister()
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
            if (string.IsNullOrEmpty(OReg))
            {
                OReg = "00000000";
            }
            tw.WriteLine($"************************************************************");//60
            tw.WriteLine($"* Output: {OReg}".PadRight(59) + "*");
            tw.WriteLine("************************************************************");

            tw.Flush();
            return sb.ToString();
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
            tw.WriteLine($"* Instruction: {InstuctionDecode(IRegShort, TState)}     TState: {TState}                           *");
            tw.WriteLine("************************************************************");
            tw.WriteLine($"* PC:         {PC}              A Register:      {AReg}".PadRight(59) + "*");
            tw.WriteLine($"* MAR:        {MReg}              B Register:      {BReg}".PadRight(59) + "*");
            tw.WriteLine($"* RAM:        {RAM_Reg}          ALU:             {ALU}".PadRight(59) + "*");
            tw.WriteLine($"* I Register: {IReg}          Output Register: {OReg}".PadRight(59) + "*");
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