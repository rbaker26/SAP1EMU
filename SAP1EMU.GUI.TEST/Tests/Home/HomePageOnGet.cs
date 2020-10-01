using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Hosting;
using System;
using System.Net.Http;
using Xunit;
using SAP1EMU.GUI.Test;
using System.Threading.Tasks;

namespace SAP1EMU.GUI.Test.Tests.Home
{


    [Collection("Sequential")]
    public class HomePageOnGet : IClassFixture<WebTestFixture>
    {
        public HttpClient Client { get; }

        public HomePageOnGet(WebTestFixture factory)
        {
            Client = factory.CreateClient();
        }




        [Fact]
        public async Task ReturnsHomePage()
        {
            // Arrange & Act
            var response = await Client.GetAsync("/");
            response.EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.Contains("Welcome to the SAP1Emu Project", stringResponse);
        }
    }
}
