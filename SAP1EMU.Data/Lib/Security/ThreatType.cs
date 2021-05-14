using System;
using System.Collections.Generic;
using System.Text;

namespace SAP1EMU.Data.Lib.Security
{
    public enum ThreatType
    {
        Xss = 1,
        SqlInjection = 2,
        CSRF = 3,
        Ddos = 4,
        Unknown = 5
    }
}
