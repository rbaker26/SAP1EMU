using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SAP1EMU.Lib.Components;

namespace SAP1EMU.Lib.Test.ComponentTests
{
    [TestClass]
    public class ControllerTest
    {
        [TestMethod]
        public void TestControlWordTableInit()
        {
            const int INT_COUNT = 6;
            const int T_STATE_COUNT = 6;
            string[] INSTRUCTIONS = new string[6]{ "0000", "0001", "0010", "0011", "1110", "1111" };

            for(int instruction =0; instruction < INT_COUNT; instruction++)
            {
                for(int tstate =0; tstate < T_STATE_COUNT; tstate++)
                {
                    string temp = INSTRUCTIONS[instruction];
                    // T States start at 1, not 0
                    Assert.AreNotEqual<string>(SEQ.Instance().UpdateControlWordReg(tstate+1, temp), "", "Control Table not properly initialized");
                }

            }
        }
    }
}
