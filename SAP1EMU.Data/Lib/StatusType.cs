using System;
using System.Collections.Generic;
using System.Text;

namespace SAP1EMU.Data.Lib
{
    public enum StatusType
    {
        Ok = 1,
        ParsingError = 2,
        EmulationError = 3,
        SystemError = 4,
        Pending = 5
    }
}
