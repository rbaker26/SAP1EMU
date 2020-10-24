using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SAP1EMU.GUI.Models
{
    public class CodePacket
    {
        public List<string> CodeList { get; set; }
        public string SetName { get; set; }
    }
}
