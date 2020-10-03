using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using SAP1EMU.GUI.Models;

using System.Diagnostics;

namespace SAP1EMU.GUI.Controllers
{
    public class DocsController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public DocsController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Docs()
        {
            return View();
        }

        public IActionResult EightBitProgramming_1()
        {
            return View();
        }

        public IActionResult EightBitProgramming_2()
        {
            return View();
        }

        public IActionResult EightBitProgramming_3()
        {
            return View();
        }

        public IActionResult EightBitProgramming_4()
        {
            return View();
        }

        public IActionResult EightBitProgramming_5()
        {
            return View();
        }

        public IActionResult LowLevelLanguages_1()
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