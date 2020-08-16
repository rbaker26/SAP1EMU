using System;
using System.IO;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SAP1EMU.CLI.Test
{
    [TestClass]
    public class CommandLineArgsTest
    {
        // This will run once for all functions in this class
        [ClassInitialize]
        public static void TestEnvSetup(TestContext context)
        {
            Environment.SetEnvironmentVariable("IS_TESTING_ENV", "TRUE");
        }

        // This will run once after all functions in this class have run
        [ClassCleanup]
        public static void TestEnvCleanup()
        {
            Environment.SetEnvironmentVariable("IS_TESTING_ENV", "FALSE");
        }


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

            try
            {
                string result = File.ReadAllText(output_file);
                Assert.AreEqual(expected, result);

                File.Delete(output_file);
            }
            catch (Exception e)
            {
                Assert.Fail(e.ToString());
            }
        }



        [TestMethod]
        public void NoSourceFile()
        {
            string input_file = "DNE.s";
            string output_file = "output_file.txt";

            string lineArgs = $"-s {input_file} -o {output_file}";

            try
            {
                // Redirect output to string
                string consoleError = "";
                var originalConsoleError = Console.Error; // preserve the original stream
                using (var writer = new StringWriter())
                {
                    Console.SetError(writer);

                    try
                    {
                        SAP1EMU.CLI.Program.Main(lineArgs.Split(' '));
                    }
                    catch(CLIException)
                    {
                        // This should happen.
                        // All good
                    }
                    catch(Exception e)
                    {
                        // this should not happen
                        Assert.Fail(e.ToString());
                    }

                    writer.Flush(); // when you're done, make sure everything is written out

                    consoleError = writer.GetStringBuilder().ToString();
                }
                // Reset output
                Console.SetOut(originalConsoleError); // restore Console.Out




                string expected = $"SAP1EMU: error: {input_file}: No such file" + Environment.NewLine
                                + $"SAP1EMU: fatal error: no input file" + Environment.NewLine
                                + $"emulation terminated" + Environment.NewLine;

                Assert.AreEqual(expected, consoleError);

            }
            catch (Exception e)
            {
                Assert.Fail(e.ToString());
            }

        }
    }
}
