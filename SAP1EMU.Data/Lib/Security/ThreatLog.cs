using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace SAP1EMU.Data.Lib.Security
{
    public class ThreatLog
    {
        public int Id { get; set; }
        public Guid ThreatIdentifier { get; set; }
        public string ClientIpAddress { get; set; }
        public string Url { get; set; }
        public string QueryString { get; set; }
        public string ThreatContent { get; set; }
        public int ThreatTypeId { get; set; }
    }
}
