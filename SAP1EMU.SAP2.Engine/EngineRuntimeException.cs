﻿using System;

namespace SAP1EMU.SAP2.Engine
{
    public class EngineRuntimeException : Exception
    {
        public EngineRuntimeException(string message) : base(message)
        {
        }

        public EngineRuntimeException(string message, Exception innerException) : base(message, innerException)
        {
        }

        private EngineRuntimeException()
        {
        }
    }
}