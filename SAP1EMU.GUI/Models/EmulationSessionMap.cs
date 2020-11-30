using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SAP1EMU.GUI.Models
{
    public class EmulationSessionMap
    {
        public int Id { get; set; }
        public Guid EmulationID { get; set; }
        public string ConnectionID {get; set;}
        public DateTime SessionStart { get; set; }
        public DateTime SessionEnd { get; set; }

    }
}
