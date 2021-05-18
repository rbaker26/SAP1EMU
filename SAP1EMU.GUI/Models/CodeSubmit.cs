using System;

namespace SAP1EMU.GUI.Models
{
    public class CodeSubmit
    {
        public int id { get; set; }
        public Guid EmulationId { get; set; }
        public string code { get; set; }
        public DateTime submitted_at { get; set; }
        public EmulationStatus Status {get; set;}
    }

    public enum EmulationStatus
    {
        Pending,
        Success,
        ParseError,
        EngineError,
        UnknownError
    };
}