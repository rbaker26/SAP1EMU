using System;

namespace SAP1EMU.Lib.Components
{
    public class ClockException : Exception
    {
        internal ClockException()
        { }

        public ClockException(string message) : base(message)
        {
        }

        public ClockException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}