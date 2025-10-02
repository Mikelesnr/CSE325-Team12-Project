using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace CSE325_Team12_Project.Hubs
{
    // SignalR hub for real-time chat communication
    // Clients send messages via SendMessage, and all connected clients receive them via ReceiveMessage
    public class ChatHub : Hub
    {
        // Called by clients to broadcast a message to all users
        public async Task SendMessage(string sender, string message)
        {
            // Broadcast to all connected clients
            await Clients.All.SendAsync("ReceiveMessage", sender, message);
        }
    }
}
