using System;
using System.Collections.Generic;
using System.Text;

namespace SAP1EMU.Lib
{
    public interface IDecoder
    {
        public string Decode(string binCode, string setName);
    }
}
