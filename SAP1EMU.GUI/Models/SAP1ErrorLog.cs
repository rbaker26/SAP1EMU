using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SAP1EMU.GUI.Models
{
    public class SAP1ErrorLog
    {
        public int id { get; set; }
        public Guid EmulationID { get; set; }
        public string Error { get; set; }
    }
}
