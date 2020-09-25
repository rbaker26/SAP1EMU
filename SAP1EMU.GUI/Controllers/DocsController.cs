﻿using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SAP1EMU.GUI.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
