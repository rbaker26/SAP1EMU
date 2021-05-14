using System;
using System.Collections.Generic;
using System.Text;

namespace SAP1EMU.Data.Lib
{
    public static class StatusFactory
    {
        public static Status GetStatus(StatusType statusType)
        {
            switch (statusType)
            {
                case StatusType.Pending:
                    return new Status()
                    {
                        Id = (int)StatusType.Pending,
                        Description = "Pending"
                    };
                case StatusType.Ok:
                    return new Status()
                    {
                        Id = (int)StatusType.Ok,
                        Description = "Ok"
                    };
                case StatusType.ParsingError:
                    return new Status()
                    {
                        Id = (int)StatusType.ParsingError,
                        Description = "Parsing Error"
                    };
                case StatusType.EmulationError:
                    return new Status()
                    {
                        Id = (int)StatusType.EmulationError,
                        Description = "Emulation Error"
                    };
                case StatusType.InProgress:
                    return new Status()
                    {
                        Id = (int)StatusType.InProgress,
                        Description = "In Progress"
                    };
                default:
                    return new Status()
                    {
                        Id = (int)StatusType.SystemError,
                        Description = "System Error"
                    };
            }
        }
    }
}
