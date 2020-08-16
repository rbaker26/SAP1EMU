using System;
using System.IO;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SAP1EMU.CLI.Test
{
    [TestClass]
    public class CommandLineArgsTest
    {
        [TestMethod]
        public void LDA170()
        {
            string input_file = "LDA170.s";
            string output_file = "output_file.txt";
            string expectedResult = "10101010";


            string lineArgs = $"-s {input_file} -o {output_file}";
            SAP1EMU.CLI.Program.Main(lineArgs.Split(' '));


            string expected = $"************************************************************\n"
                            + $"Final Output Register Value: { expectedResult }"
                            + $"\n************************************************************\n\n";

            string result = File.ReadAllText(output_file);
            Assert.AreEqual(expected, result);

            File.Delete(output_file);
        }

        [TestMethod]
        public void Fib()
        {
            string input_file = "Fib.s";
            string output_file = "output_file.txt";
            string expectedResult = "11111101";


            string lineArgs = $"-s {input_file} -o {output_file}";
            SAP1EMU.CLI.Program.Main(lineArgs.Split(' '));


            string expected = $"************************************************************\n"
                            + $"Final Output Register Value: { expectedResult }"
                            + $"\n************************************************************\n\n";

            string result = File.ReadAllText(output_file);
            Assert.AreEqual(expected, result);

            File.Delete(output_file);
        }
    }
}
