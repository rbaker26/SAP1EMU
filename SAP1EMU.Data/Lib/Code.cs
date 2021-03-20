using System;
using System.Collections.Generic;
using System.Text;

namespace SAP1EMU.Data.Lib
{
    public class CodeSubmission
    {
        public int Id { get; set; }
        public Guid EmulationID { get; set; }
        public IEnumerable<string> Code { get; set; }
    }
}
