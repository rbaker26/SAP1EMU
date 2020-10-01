using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

namespace SAP1EMU.GUI.Controllers
{
    public class EightBitController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        
        public IActionResult EightBitProgramming()
        {
            return View();
        }
    }
}
