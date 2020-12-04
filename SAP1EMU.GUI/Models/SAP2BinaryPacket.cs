using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SAP1EMU.GUI.Models
{
    public class SAP2BinaryPacket
    {
        public int Id { get; set; }
        public Guid EmulationID { get; set; }
        public IEnumerable<string> Code { get; set; }
    }
}
