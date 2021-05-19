using System;
using System.Collections.Generic;
using System.Text;

namespace SAP1EMU.Data.Lib
{
    public enum StatusType
    {
        Ok = 1,
        SQLError = 2,
        ParsingError = 3,
        EmulationError = 4,
        SystemError = 5,
        Pending = 6,
        InProgress = 7
    }
}
