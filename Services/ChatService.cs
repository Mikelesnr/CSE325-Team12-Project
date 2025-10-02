using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;

namespace CSE325_Team12_Project.Services
{
    // Client-side SignalR service for Blazor components
    // Handles connection setup, message sending, and receiving
    public class ChatService
    {
        private HubConnection _connection;

        // Event triggered when a new message is received
        public event Action<string, string>? OnMessageReceived;

        // Starts the SignalR connection and sets up the listener
        public async Task StartAsync()
        {
            _connection = new HubConnectionBuilder()
                .WithUrl("/chathub") // Update this if your dev server uses a different port
                .WithAutomaticReconnect()
                .Build();

            // Listen for incoming messages from the hub
            _connection.On<string, string>("ReceiveMessage", (sender, message) =>
            {
                OnMessageReceived?.Invoke(sender, message);
            });

            await _connection.StartAsync();
        }

        // Sends a message to the hub
        public async Task SendMessageAsync(string sender, string message)
        {
            if (_connection.State == HubConnectionState.Connected)
            {
                await _connection.SendAsync("SendMessage", sender, message);
            }
        }
    }
}
