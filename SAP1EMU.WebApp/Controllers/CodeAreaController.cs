using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SAP1EMU.WebApp.Models;
using SAP1EMU.Assembler;

namespace SAP1EMU.WebApp.Controllers
{
    public class CodeAreaController : Controller
    {
        List<string> Code = new List<string>();

        [HttpPost]
        [ActionName("AssembleCode")]
        public ActionResult<string> PostAssembleCode(CodeAreaModel codeAreaModel)
        {
            try
            {
                List<string> binary = Assemble.ParseFileContents(new List<string>(codeAreaModel.CodeList.Split("\n")));
                return new ActionResult<string>(binary.ToString());
            }
            catch (Exception e)
            {
                return BadRequest(e.ToString());
            }
        }
        



        //// GET: CodeArea
        //public ActionResult Index()
        //{
        //    return View();
        //}

        //// GET: CodeArea/Details/5
        //public ActionResult Details(int id)
        //{
        //    return View();
        //}

        //// GET: CodeArea/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        //// POST: CodeArea/Create
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create(IFormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add insert logic here

        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //// GET: CodeArea/Edit/5
        //public ActionResult Edit(int id)
        //{
        //    return View();
        //}

        //// POST: CodeArea/Edit/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add update logic here

        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //// GET: CodeArea/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        //// POST: CodeArea/Delete/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Delete(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add delete logic here

        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        

    }
}