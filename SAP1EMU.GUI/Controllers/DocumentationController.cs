using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using SAP1EMU.GUI.Models;
using SAP1EMU.GUI.Services;

namespace SAP1EMU.GUI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentationController : Controller
    {
        IDocumentationService _docService;

        public DocumentationController(IDocumentationService docService)
        {
            _docService = docService;
        }

        [HttpGet]
        public ActionResult<DocumentationModel> Get()
        {
            return Ok(_docService.GetDocs());
        }
    }
}
