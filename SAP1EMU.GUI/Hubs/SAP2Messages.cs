

namespace SAP1EMU.GUI.Hubs
{
    public static class SAP2Messages
    {
        public enum StatusCodes
        {
            ClientRegistered,
            ReadyForInput,
            EmulationFinished,
            EmulationTimeOut
        }

        public static readonly string ServerName = "ASP.NET-SignalR:EmulatorHub:" + typeof(SAP1EMU.GUI.Program).Assembly.GetName().Version;
    }

}

