using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SAP1EMU.Lib.Components;
using SAP1EMU.Lib.Registers;

namespace SAP1EMU.Lib.Test.ComponentTests
{
    [TestClass]
    public class PubSub_ClockTest
    {

        [TestMethod]
        public void TestSubscribe()
        {
            try
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
            }
            catch (Exception e)
            {
                Assert.Fail(e.ToString());
            }
        }


        [TestMethod]
        public void TestUpdate()
        {
            try
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


                for (int i = 0; i < 500; i++)
                {
                    clock.SendTicTok(tictok);
                    tictok.ToggleClockState();
                    clock.SendTicTok(tictok);
                    tictok.ToggleClockState();
                }
            }
            catch (Exception e)
            {
                Assert.Fail(e.ToString());
            }
        }

        [TestMethod]
        public void TestUnsubscribe()
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


            for (int i = 0; i < 500; i++)
            {
                clock.SendTicTok(tictok);
                tictok.ToggleClockState();
                clock.SendTicTok(tictok);
                tictok.ToggleClockState();
            }

            // UnSub all
            clock.EndTransmission();


            for (int i = 0; i < 500; i++)
            {
                clock.SendTicTok(tictok);
                tictok.ToggleClockState();
                clock.SendTicTok(tictok);
                tictok.ToggleClockState();
            }

        }

    }
}
