using System;
using System.Collections.Generic;
using System.Text;

namespace SAP1EMU.Data.Lib
{
    public class ErrorLog
    {
        public int Id { get; set; }
        public Guid EmulationID { get; set; }
        public string ErrorMsg { get; set; }
    }
}
