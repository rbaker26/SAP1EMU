using BenchmarkDotNet.Attributes;

using SAP1EMU.Engine;
using SAP1EMU.Lib;

using System.Collections.Generic;

namespace SAP1EMU.Benchmarks
{
    public class EngineBenchmark
    {
        private RAMProgram RP_HLT { get; set; }
        private RAMProgram RP_LDA170 { get; set; }
        private RAMProgram RP_FIB { get; set; }
        private IDecoder _decoder = new InstructionDecoder();

        [GlobalSetup]
        public void GlobalSetup()
        {
            RP_HLT = new RAMProgram(new List<string>() {
                "11110000",
                "00000000",
                "00000000",
                "00000000",
                "00000000",
                "00000000",
                "00000000",
                "00000000",
                "00000000",
                "00000000",
                "00000000",
                "00000000",
                "00000000",
                "00000000",
                "00000000",
                "00000000"
            });

            RP_LDA170 = new RAMProgram(new List<string>() {
                "00001111",
                "11100000",
                "11110000",
                "00000000",
                "00000000",
                "00000000",
                "00000000",
                "00000000",
                "00000000",
                "00000000",
                "00000000",
                "00000000",
                "00000000",
                "00000000",
                "00000000",
                "10101010",
            });

            RP_FIB = new RAMProgram(new List<string>() {
                "00001110",
                "00011111",
                "00111111",
                "00001111",
                "00111110",
                "00001101",
                "00011100",
                "00111101",
                "10011010",
                "01000000",
                "11100000",
                "11110000",
                "00000001",
                "11111001",
                "00000001",
                "00000001",
            });
        }

        [Benchmark(Baseline = true)]
        public void EngineRun_HLT()
        {
            EngineProc engine = new EngineProc();

            engine.Init(RP_HLT, _decoder);
            engine.Run();
        }

        [Benchmark]
        public void EngineRun_LDA170()
        {
            EngineProc engine = new EngineProc();

            engine.Init(RP_LDA170, _decoder);
            engine.Run();
        }

        [Benchmark]
        public void EngineRun_FIB5()
        {
            EngineProc engine = new EngineProc();

            engine.Init(RP_FIB, _decoder);
            engine.Run();
        }
    }
}