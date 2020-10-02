using BenchmarkDotNet.Running;

namespace SAP1EMU.Benchmarks
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            BenchmarkRunner.Run<EngineBenchmark>();
        }
    }
}