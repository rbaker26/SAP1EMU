using System;
using System.Collections.Generic;
using System.Text;

namespace SAP1EMU.Data.Lib
{
    public enum StatusType
    {
        Ok = 0,
        SQLError = 1,
        ParsingError = 2,
        EmulationError = 3,
        SystemError = 4,
        Pending = 5,
        InProgress = 6
    }
}
