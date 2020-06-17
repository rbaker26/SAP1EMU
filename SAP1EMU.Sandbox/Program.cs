using System;
using System.Collections.Generic;
using SAP1EMU.Engine;
using SAP1EMU.Lib;

namespace SAP1EMU.CLI
{
    class Program
    {
        static void Main(string[] args)
        {

            //EngineProc e = new EngineProc();
            //RAMProgram rmp = new RAMProgram(new List<string>());
            //e.Init(rmp);
            //e.Run();

            OpCodeLoader.GetSet("SAP1Emu");
            OpCodeLoader.GetSet("MalVIno");
            OpCodeLoader.GetSet("BenEater");
        }
    }
}
