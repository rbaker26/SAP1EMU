using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ElectronNET.API;
using ElectronNET.API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SAP1EMU.WebApp.Models;

namespace SAP1EMU.WebApp.Controllers
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

            if (HybridSupport.IsElectronActive)
            {
                // Must remvoe EL's before adding new ones or else duplicate EL's will be created
                Electron.IpcMain.RemoveAllListeners("open-wiki");
                
                Electron.IpcMain.On("open-wiki", async (args) =>
                {
                    await Electron.Shell.OpenExternalAsync("https://github.com/rbaker26/SAP1EMU/wiki");
                });
            }

            return View();
        }
        public IActionResult About()
        {
            if (HybridSupport.IsElectronActive)
            {
                // Must remvoe EL's before adding new ones or else duplicate EL's will be created
                Electron.IpcMain.RemoveAllListeners("open-github-profile");
                Electron.IpcMain.RemoveAllListeners("open-ben-eater");

                Electron.IpcMain.On("open-github-profile", async (args) =>
                {
                    await Electron.Shell.OpenExternalAsync("https://github.com/rbaker26/");
                });
                Electron.IpcMain.On("open-ben-eater", async (args) =>
                {
                    await Electron.Shell.OpenExternalAsync("https://eater.net/");
                });
                
            }
            return View();
        }
        public IActionResult Emulator()
        {
            return View();
        }
        public IActionResult Assembler()
        {
            if (HybridSupport.IsElectronActive)
            {
                Electron.IpcMain.RemoveAllListeners("open-from-file-asm");

                Electron.IpcMain.On("open-from-file-asm", async (args) =>
                {
                    var mainWindow = Electron.WindowManager.BrowserWindows.First();
                    var options = new OpenDialogOptions {
                        Title = "Open SAP1Emu Assembly Code",
                        Filters = new FileFilter[] {
                           new FileFilter
                           {
                               Name = "SAP1Emu Assembly",
                               Extensions = new string[] {"s"}
                           }
                        },
                        Properties = new OpenDialogProperty[] {
                            OpenDialogProperty.openFile,
                        }
                    };
                    string[] files = await Electron.Dialog.ShowOpenDialogAsync(mainWindow, options);

                    string code = "";
                    if (files.Length != 0)
                    {
                        if(!string.IsNullOrEmpty(files[0]))
                        {
                            code = System.IO.File.ReadAllText(files[0]);
                        }
                    }

                    Electron.IpcMain.Send(mainWindow, "code-from-file-asm", code);

                });


                Electron.IpcMain.On("save-to-file-asm", async (args) =>
                {
                    var mainWindow = Electron.WindowManager.BrowserWindows.First();
                    var options = new SaveDialogOptions {
                        Title = "Save SAP1Emu Binary Code",
                        Filters = new FileFilter[] {
                            new FileFilter { 
                                Name = "SAP1Emu Binary", 
                                Extensions = new string[] {"b"} 
                            }
                        }
                    };

                    var path = await Electron.Dialog.ShowSaveDialogAsync(mainWindow, options);

                    if(!string.IsNullOrEmpty(path))
                    {
                        string contents = (string)args;
                        System.IO.File.WriteAllText(path, contents);
                    }
                   
                });
            }
            return View();
        }

        public IActionResult Privacy()
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
