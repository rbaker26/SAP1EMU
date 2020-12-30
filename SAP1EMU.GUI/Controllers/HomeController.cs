using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using SAP1EMU.GUI.Models;
using System.Diagnostics;

namespace SAP1EMU.GUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();

        }

        public IActionResult About()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Emulator()
        {
            return View("EmulatorPicker");
        }

        [Route("/Emulator/SAP1")]
        public IActionResult SAP1()
        {
            return View();
        }

        [Route("/Emulator/SAP2")]
        public IActionResult SAP2()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Emulator([FromForm] CodePacket codePacket)
        {
            return View(codePacket);
        }

        public IActionResult EmulatorPicker()
        {
            return View();
        }

        public IActionResult Assembler()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Contributors()
        {
            return View();
        }

        public IActionResult TestSignalR()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}