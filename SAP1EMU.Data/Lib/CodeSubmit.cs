using System;

namespace SAP1EMU.Data.Lib
{
    public class CodeSubmit
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public int Status { get; set; }
        public Guid EmulationId { get; set; }
        public DateTime SubmittedAt { get; set; }
    }
}