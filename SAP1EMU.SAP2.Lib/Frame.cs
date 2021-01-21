using SAP1EMU.SAP2.Lib.Components;
using SAP1EMU.SAP2.Lib.Registers;
using SAP1EMU.SAP2.Lib.Utilities;

using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SAP1EMU.SAP2.Lib
{
    public class Frame
    {
        public string Instruction { get; private set; } = "???";
        private Instruction InstructionData { get; set; }
        public int TState { get; private set; } = 0;

        // left side of computer
        public string Input_Port_1 { get; private set; } = "0000 0000";
        public string Input_Port_2 { get; private set; } = "0000 0000";

        public string PC { get; private set; } = "0000 0000 0000 0000";

        public string MAR { get; private set; } = "0000 0000 0000 0000";
        public string RAM_Reg { get; private set; } = "0000 0000 0000 0000";
        public List<string> RAM { get; private set; } // The reason this is here is that the RAM might change if a STA simular command is issued.
        public string MDR { get; private set; } = "0000 0000";

        public string IReg { get; private set; } = "0000 0000";

        public string SEQ { get; private set; } = "0000 0000";

        // center
        public string WBus { get; private set; } = "0000 0000 0000 0000";

        // right side of computer
        public string AReg { get; private set; } = "0000 0000";

        public string ALU { get; private set; } = "0000 0000";
        public string Flags { get; private set; } = "0000 0000";
        public string Overflow_Flag { get; private set; } = "0";
        public string Underflow_Flag { get; private set; } = "0";
        public string Zero_Flag { get; private set; } = "0";

        public string TReg { get; private set; } = "0000 0000";
        public string BReg { get; private set; } = "0000 0000";
        public string CReg { get; private set; } = "0000 0000";
        
        public string OReg3 { get; private set; } = "0000 0000";
        public string HexadecimalDisplay { get; private set; } = "00";

        public string OReg4 { get; private set; } = "0000 0000";
        

        public Frame(Instruction instruction, int TState, IPort1 ip1, IPort2 ip2, PC pc, MAR mar, RAM ram,
                     List<string> ramContents, MDR mdr, IReg ireg, SEQ seq, string wbus_string,
                     AReg areg, ALU alu, Flag flagReg, TReg treg, BReg breg, CReg creg,
                     OReg3 oreg3, OReg4 oreg4, HexadecimalDisplay hexadecimalDisplay)
        {
            InstructionData = instruction;

            this.TState = TState;

            this.AReg = areg.ToString_Frame_Use();
            this.BReg = breg.ToString_Frame_Use();
            this.CReg = creg.ToString_Frame_Use();
            this.TReg = treg.ToString_Frame_Use();
            this.IReg = ireg.ToString_Frame_Use();  // The real ToString() is in use with a substring in it.  This is needed for proper operation
            this.MAR = mar.ToString_Frame_Use();
            this.MDR = mdr.RegContent;
            
            this.PC = pc.RegContent;
            this.ALU = alu.ToString();
            this.WBus = wbus_string;

            this.OReg3 = oreg3.ToString_Frame_Use();
            this.OReg4 = oreg4.ToString_Frame_Use();
            this.HexadecimalDisplay = hexadecimalDisplay.RegContent;

            this.RAM = ramContents;

            this.SEQ = seq.ToString();
            this.WBus = wbus_string; // I didnt want to mess with the Singleton in the frame, so the value will just be passed as a string
            this.RAM_Reg = ram.ToString_Frame_Use();
            this.Flags = flagReg.RegContent;

            if (instruction == null)
            {
                this.IReg = "???";
            }

            if (TState > 3)
            {
                Instruction = InstructionData.OpCode;
            }
            else
            {
                Instruction = "???";
            }
        }

        public string OutputRegister()
        {
            StringBuilder sb = new StringBuilder();
            StringWriter tw = new StringWriter(sb);

            int unsigned_ouput = BinConverter.Bin8ToInt(OReg3);

            int signed_output = 0;
            if (OReg3 != null)
            {
                if (OReg3.StartsWith('1'))
                {
                    signed_output = -1 * (255 - unsigned_ouput + 1);
                }
                else
                {
                    signed_output = unsigned_ouput;
                }
            }
            if (string.IsNullOrEmpty(OReg3))
            {
                OReg3 = "00000000";
            }
            tw.WriteLine($"************************************************************");//60
            tw.WriteLine($"* Output: {OReg3}".PadRight(47) + "*");
            tw.WriteLine("************************************************************");

            tw.Flush();
            return sb.ToString();
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            StringWriter tw = new StringWriter(sb);

            int unsigned_ouput = BinConverter.Bin8ToInt(OReg3);

            int signed_output = 0;
            if (OReg3 != null)
            {
                if (OReg3.StartsWith('1'))
                {
                    signed_output = -1 * (255 - unsigned_ouput + 1);
                }
                else
                {
                    signed_output = unsigned_ouput;
                }
            }

            tw.WriteLine($"***********************************************************************************");//82
            tw.WriteLine($"* Instruction: {InstructionData.OpCode}     TState: {TState}".PadRight(82) + "*");
            tw.WriteLine($"***********************************************************************************");
            tw.WriteLine($"* Input 1:    {Input_Port_1}".PadRight(35) + $"A Register:    {AReg}".PadRight(47) + "*");
            tw.WriteLine($"* Input 2:    {Input_Port_2}".PadRight(35) + $"ALU:           {ALU}     Flags:   {Flags}".PadRight(47) + "*");
            tw.WriteLine($"* PC:         {PC}".PadRight(35)           + $"Temp Register: {TReg}".PadRight(47) + "*");
            tw.WriteLine($"* MAR:        {MAR}".PadRight(35)          + $"B Register:    {BReg}".PadRight(47) + "*");
            tw.WriteLine($"* RAM:        {RAM_Reg}".PadRight(35)      + $"C Register:    {CReg}".PadRight(47) + "*");
            tw.WriteLine($"* MDR:        {MDR}".PadRight(35)          + $"Output 3:      {OReg3}     Display: 0x{HexadecimalDisplay}".PadRight(47) + "*");
            tw.WriteLine($"* I Register: {IReg}".PadRight(35)         + $"Output 4:      {OReg4}".PadRight(47) + "*");
            tw.WriteLine($"* Sequencer:  {SEQ}       ".PadRight(82) + "*");
            tw.WriteLine($"* BUS:        {WBus}      ".PadRight(82) + "*");
            tw.WriteLine($"***********************************************************************************");
            tw.WriteLine($"* Output Unsigned: {unsigned_ouput}".PadRight(82) + "*");
            tw.WriteLine($"* Output Signed:   {signed_output}".PadRight(82) + "*");
            tw.WriteLine($"***********************************************************************************");

            tw.Flush();
            return sb.ToString();
        }
    }
}