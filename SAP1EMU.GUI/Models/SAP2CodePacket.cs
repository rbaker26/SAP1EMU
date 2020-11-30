using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SAP1EMU.GUI.Models
{
    public class SAP2CodePacket
    {
        public Guid EmulationID { get; set; }
        public List<string> Code { get; set; }
    }
}
