using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.AspNetCore.Components;

namespace CSE325_Team12_Project.Services
{
    public class SignalRClientService
    {
        public HubConnection? Connection { get; private set; }

        public async Task InitializeAsync(NavigationManager nav)
        {
            Connection = new HubConnectionBuilder()
                .WithUrl(nav.ToAbsoluteUri("/chathub"))
                .WithAutomaticReconnect()
                .Build();

            await Connection.StartAsync();
        }

        public async Task JoinTroupe(string troupeId)
        {
            if (Connection != null)
                await Connection.SendAsync("JoinTroupe", troupeId);
        }

        public async Task SendTroupeMessage(string troupeId, string userId, string userName, string message)
        {
            if (Connection != null)
                await Connection.SendAsync("SendTroupeMessage", troupeId, userId, userName, message);
        }

    }
}
