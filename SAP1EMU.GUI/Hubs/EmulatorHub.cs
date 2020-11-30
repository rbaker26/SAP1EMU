using Microsoft.AspNetCore.SignalR;

using SAP1EMU.GUI.Contexts;
using SAP1EMU.GUI.Models;

using System;
using System.Threading.Tasks;

namespace SAP1EMU.GUI.Hubs
{
    public class EmulatorHub : Hub
    {
        private Sap1EmuContext _sap1EmuContext { get; set; }
        public EmulatorHub(Sap1EmuContext sap1EmuContext)
        {
            _sap1EmuContext = sap1EmuContext;
        }


        // TODO - Remove this after the test
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }


        public Task NotifyClient(string user, string message)
        {
            return Clients.User(user).SendAsync("ReceiveMessage", message);
        }


        // When the Clients First Connect to SignalR, associate the ConnectionID with their AccountID
        // This way the we can avoid sending access IDs to the Client over SignalR
        public Task RegisterClient(Guid EmulationID)
        {
            _sap1EmuContext.Add<EmulationSessionMap>(new EmulationSessionMap
            {
                EmulationID = EmulationID,
                ConnectionID = Context.ConnectionId,
                SessionStart = DateTime.UtcNow,
            });
            _sap1EmuContext.SaveChanges();
            return Clients.Caller.SendAsync("ReceiveMessage", SAP2Messages.ServerName, SAP2Messages.StatusCodes.ClientRegistered.ToString());
        }

    }
}
