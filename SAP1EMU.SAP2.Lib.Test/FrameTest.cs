using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using SAP1EMU.SAP2.Lib.Components;
using SAP1EMU.SAP2.Lib.Registers;

namespace SAP1EMU.SAP2.Lib.Test
{
    [TestClass]
    public class FrameTest
    {
        [TestMethod]
        public void TestToString()
        {
            Clock clock = new Clock();
            TicTok tictok = new TicTok();

            tictok.Init();

            int TState = 0;
            AReg areg = new AReg();
            TReg treg = new TReg();
            BReg breg = new BReg();
            CReg creg = new CReg();
            IReg ireg = new IReg();

            IPort1 port1 = new IPort1();
            IPort2 port2 = new IPort2();
            
            MDR mdr = new MDR();
            RAM ram = new RAM();

            mdr.SetRefToRAM(ref ram);

            ALU alu = new ALU(ref areg, ref treg);

            OReg3 oreg3 = new OReg3(ref alu);
            OReg4 oreg4 = new OReg4(ref alu);

            HexadecimalDisplay hexadecimalDisplay = new HexadecimalDisplay(ref oreg3);


            Flag flagReg = new Flag(ref alu);
            PC pc = new PC(ref flagReg);
            MAR mar = new MAR(ref ram);
            SEQ seq = SEQ.Instance();

            Wbus.Instance().Value = string.Concat(Enumerable.Repeat('0', 16));

            Instruction currentInstruction = new Instruction
            {
                OpCode = "ADD B",
                BinCode = "10000000",
                TStates = 4,
                AffectsFlags = true,
                AddressingMode = AddressingMode.Register,
                Bytes = 1
            };

            Frame frame = new Frame(currentInstruction, TState, port1, port2, pc, mar, ram,
                                    ram.RAMDump(), mdr, ireg, SEQ.Instance(),
                                    Wbus.Instance().Value, areg, alu, flagReg,
                                    treg, breg, creg, oreg3, oreg4, hexadecimalDisplay);

            _ = frame.ToString();
            _ = frame.OutputRegister();
        }
    }
}