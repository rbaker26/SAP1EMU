using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SAP1EMU.Data.Lib
{
    public class SAP2CodePacket
    {
        public int Id { get; set; }
        [Required]
        public Guid EmulationID { get; set; }
        [Required]
        public IEnumerable<string> Code { get; set; }
        [Required]
        public string SetName { get; set; }
    }
}
