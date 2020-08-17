using System;

using BenchmarkDotNet.Running;

namespace SAP1EMU.Benchmarks
{
    class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<EngineBenchmark>();
        }
    }
}
