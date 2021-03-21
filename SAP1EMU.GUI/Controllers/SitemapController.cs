using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using SimpleMvcSitemap;

namespace SAP1EMU.GUI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class SitemapController : ControllerBase
    {
        [HttpGet]
        public ActionResult Index()
        {
            List<SitemapNode> nodes = new List<SitemapNode>
        {
            new SitemapNode("https://sap1emu.net/")
            {
                Priority =1.0M
            },
            new SitemapNode(Url.Action("Index","Home"))
            {
                Priority =0.9M
            },
            new SitemapNode(Url.Action("About","Home")),
            new SitemapNode(Url.Action("Emulator","Home")),
            new SitemapNode(Url.Action("Assembler","Home")),
            new SitemapNode(Url.Action("Privacy","Home")),
            new SitemapNode(Url.Action("Contributors","Home")),
            new SitemapNode(Url.Action("Docs","Docs")),
            new SitemapNode(Url.Action("EightBitProgramming_1","Docs")),
            new SitemapNode(Url.Action("EightBitProgramming_2","Docs")),
            new SitemapNode(Url.Action("EightBitProgramming_3","Docs")),
            new SitemapNode(Url.Action("EightBitProgramming_4","Docs")),
            new SitemapNode(Url.Action("EightBitProgramming_5","Docs")),
            new SitemapNode(Url.Action("APIDocs_1","Docs")),
            new SitemapNode(Url.Action("APIDocs_2","Docs")),
            new SitemapNode(Url.Action("APIDocs_3","Docs")),
            new SitemapNode(Url.Action("APIDocs_4","Docs")),
            new SitemapNode(Url.Action("","Help")),
            new SitemapNode(Url.Action("","swagger")),



            //other nodes
        };

            return new SitemapProvider().CreateSitemap(new SitemapModel(nodes));
        }
    }
}
