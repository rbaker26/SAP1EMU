using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SAP1EMU.Data.Lib
{
    public class EmulationSessionMap
    {
        public int Id { get; set; }
        public Guid EmulationID { get; set; }
        public string ConnectionID {get; set;}
        public DateTime SessionStart { get; set; }
        public DateTime SessionEnd { get; set; }
        public int EmulatorId { get; set; }
        public int InstructionSetId { get; set; }
        public int StatusId { get; set; }

    }
}
