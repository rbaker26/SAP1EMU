using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

using System;
using System.Collections.Generic;
using System.Text;

using Xunit;

namespace SAP1EMU.GUI.Test
{
    // All of these tests will start from the Index Page
    public class AutomatedUITests : IDisposable
    {
        private readonly IWebDriver _driver;
        private string BaseUrl;
        public AutomatedUITests()
        {
            ChromeOptions chromeOptions = new ChromeOptions();
            _driver = new ChromeDriver(chromeOptions);
            throw new Exception("FDJHKDSHJFKJSDKJFSDK");

            if (Environment.GetEnvironmentVariable("IsDeploymentSlotTest") == "true")
            {
                BaseUrl = "http://test.sap1emu.net/";
                chromeOptions.AddArgument("--headless");                // No GUI 
                chromeOptions.AddArgument("--disable-dev-shm-usage");   // overcome limited resource problems
                chromeOptions.AddArgument("--no-sandbox");              // Bypass OS security model
                chromeOptions.AddArgument("--remote-debugging-port=9222");
            }
            else
            {
                //BaseUrl = "https://localhost:5001/";
                BaseUrl = "http://test.sap1emu.net/";
            }
            _driver = new ChromeDriver(chromeOptions);
        }


        [Fact]
        public void Load_Home_Page()
        {
            _driver.Navigate()
                .GoToUrl(BaseUrl);
            var pageText = _driver.FindElement(By.CssSelector(".display-4")).Text;
            Assert.Equal("Welcome to the SAP1Emu Project", pageText);
        }

        [Fact]
        public void Navigate_To_About_Page()
        {
            _driver.Navigate()
                .GoToUrl(BaseUrl);
            _driver.FindElement(By.LinkText("About")).Click();
            var pageText = _driver.FindElement(By.TagName("h2")).Text;
            Assert.Contains("About this Project", pageText);
        }

        [Fact]
        public void Navigate_To_Author_GitHub()
        {
            _driver.Navigate()
                .GoToUrl(BaseUrl);
            _driver.FindElement(By.LinkText("About")).Click();

            // Nav to GitHub Profile
            _driver.FindElement(By.CssSelector(".card")).FindElement(By.LinkText("Follow")).Click();
            _driver.SwitchTo().Window(_driver.WindowHandles[1]);
            Assert.Contains("rbaker26", _driver.Title);
        }

        public void Dispose()
        {
            _driver.Quit();
            _driver.Dispose();
        }
    }
}
