using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SAP1EMU.GUI.Models
{
    public class PageNavHelperModel
    {
        public string ControllerName { get; set; }
        public string PreviousView { get; set; }
        public string PreviousViewDisplay { get; set; }
        public string NextView { get; set; }
        public string NextViewDisplay { get; set; }
    }
}
