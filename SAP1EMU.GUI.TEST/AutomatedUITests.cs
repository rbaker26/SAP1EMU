using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

using System;
using System.Threading;

using Xunit;

namespace SAP1EMU.GUI.Test
{
    // All of these tests will start from the Index Page
    public class AutomatedUITests : IDisposable
    {
        private static IWebDriver _driver = null;
        private static string BaseUrl;
        public AutomatedUITests()
        {
            if(_driver == null)
            {
                ChromeOptions chromeOptions = new ChromeOptions();

                if (Environment.GetEnvironmentVariable("IsDeploymentSlotTest") == "true")
                {
                    BaseUrl = "http://test.sap1emu.net/";
                    chromeOptions.AddArgument("--headless");                // No GUI 
                    chromeOptions.AddArgument("--disable-dev-shm-usage");   // overcome limited resource problems
                    chromeOptions.AddArgument("--no-sandbox");              // Bypass OS security model
                }
                else
                {
                    //BaseUrl = "https://localhost:5001/";
                    BaseUrl = "http://test.sap1emu.net/";

                }

                _driver = new ChromeDriver(chromeOptions);
            }
        }


        [Fact]
        public void ChromeTests()
        {
            Load_Home_Page();
            Load_About_Page();
            Load_GitHub_Profile();
            Load_Swagger_Index();
        }

        private void Load_Home_Page()
        {
            string TEST_NAME = "Load_Home_Page";
            try
            {
                Console.Write(TEST_NAME);
                _driver.Navigate().GoToUrl(BaseUrl);

                var pageText = _driver.FindElement(By.CssSelector(".display-4")).Text;
                Assert.Equal("Welcome to the SAP1Emu Project", pageText);
            }
            catch(Exception)
            {
                Console.Error.Write(":\t Failed\n");
                throw;
            }
            Console.Write(":\t Passed\n");
        }

        private void Load_About_Page()
        {
            string TEST_NAME = "Load_About_Page";
            try
            {
                Console.Write(TEST_NAME);

                _driver.Navigate().GoToUrl(BaseUrl);
                _driver.FindElement(By.LinkText("About")).Click();

                var pageText = _driver.FindElement(By.TagName("h2")).Text;
                Assert.Contains("About this Project", pageText);
            }
            catch (Exception)
            {
                Console.Error.Write(":\t Failed\n");
                throw;
            }
            Console.Write(":\t Passed\n");
        }

        private void Load_GitHub_Profile()
        {
            string TEST_NAME = "Load_GitHub_Profile";
            try
            {
                Console.Write(TEST_NAME);

                _driver.Navigate().GoToUrl(BaseUrl);
                _driver.Navigate().GoToUrl(BaseUrl);
                _driver.FindElement(By.LinkText("About")).Click();

                _driver.FindElement(By.CssSelector(".card")).FindElement(By.LinkText("Follow")).Click();
                _driver.SwitchTo().Window(_driver.WindowHandles[1]);

                Assert.Contains("rbaker26", _driver.Title);

                // Cleanup extra tab
                _driver.SwitchTo().Window(_driver.WindowHandles[1]);
                _driver.Close();
                _driver.SwitchTo().Window(_driver.WindowHandles[0]);
            }
            catch (Exception)
            {
                Console.Error.Write(":\t Failed\n");
                throw;
            }
            Console.Write(":\t Passed\n");
        }

        private void Load_Swagger_Index()
        {
            string TEST_NAME = "Load_Swagger_Index";
            try
            {
                Console.Write(TEST_NAME);

                _driver.Navigate().GoToUrl(BaseUrl + "/swagger/");
                Thread.Sleep(1500);
                var title = _driver.FindElement(By.ClassName("title")).Text;
                

                Assert.Contains("SAP1Emu API", title);
            }
            catch (Exception)
            {
                Console.Error.Write(":\t Failed\n");
                throw;
            }
            Console.Write(":\t Passed\n");
        }


        public void Dispose()
        {
            _driver.Quit();
            _driver.Dispose();
        }
    }
}
