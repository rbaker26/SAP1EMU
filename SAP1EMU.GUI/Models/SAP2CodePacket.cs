using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SAP1EMU.GUI.Models
{
    public class SAP2CodePacket
    {
        public int Id { get; set; }
        public Guid EmulationID { get; set; }
        public IEnumerable<string> Code { get; set; }
        public string SetName { get; set; }
    }
}
