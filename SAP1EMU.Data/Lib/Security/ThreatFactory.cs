using System;
using System.Collections.Generic;
using System.Text;

namespace SAP1EMU.Data.Lib.Security
{
    public static class ThreatFactory
    {
        public static Threat GetThreat(ThreatType threatType)
        {
            switch (threatType)
            {
                case ThreatType.Xss:
                    return new Threat()
                    {
                        Id = (int)ThreatType.Xss,
                        Description = "Xss"
                    };
                case ThreatType.SqlInjection:
                    return new Threat()
                    {
                        Id = (int)ThreatType.SqlInjection,
                        Description = "Sql Injection"
                    };
                case ThreatType.CSRF:
                    return new Threat()
                    {
                        Id = (int)ThreatType.CSRF,
                        Description = "Cross-Site Request Forgery"
                    };
                case ThreatType.Ddos:
                    return new Threat()
                    {
                        Id = (int)ThreatType.Ddos,
                        Description = "Ddos"
                    };
                default:
                    return new Threat()
                    {
                        Id = (int)ThreatType.Unknown,
                        Description = "Unknown"
                    };
            }
        }
    }
}
