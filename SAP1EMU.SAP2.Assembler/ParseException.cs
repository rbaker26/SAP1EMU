using System;

namespace SAP1EMU.SAP2.Assembler
{
    public class ParseException : Exception
    {
        public ParseException(string message) : base(message)
        {
        }

        public ParseException(string message, Exception innerException) : base(message, innerException)
        {
        }

        private ParseException()
        {
        }
    }
}