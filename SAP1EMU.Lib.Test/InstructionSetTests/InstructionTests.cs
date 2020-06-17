using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using NuGet.Frameworks;

namespace SAP1EMU.Lib.Test.InstructionSetTests
{
    [TestClass]
    public class InstructionTests
    {

        [TestMethod]
        public void TestInstruction()
        {
            try
            {
                Instruction i = new Instruction();
            }
            catch (Exception e)
            {
                Assert.Fail(e.ToString());
            }
        }

        [TestMethod]
        public void TestInstructionSet()
        {
            try
            {
                InstructionSet i = new InstructionSet();
            }
            catch (Exception e)
            {
                Assert.Fail(e.ToString());
            }
        }
        [TestMethod]
        public void TestopCodeLoader()
        {
            try
            {
                OpCodeLoader.GetSet("SAP1Emu");
                OpCodeLoader.GetSet("Malvino");
                OpCodeLoader.GetSet("BenEater");
            }
            catch(Exception e)
            {
                Assert.Fail(e.ToString());
            }
        }
    }
}