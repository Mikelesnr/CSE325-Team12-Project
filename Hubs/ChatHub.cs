using Microsoft.AspNetCore.SignalR;
using CSE325_Team12_Project.Models;
using CSE325_Team12_Project.Data;
using Microsoft.EntityFrameworkCore;

namespace CSE325_Team12_Project.Hubs
{
    public class ChatHub : Hub
    {
        private readonly ApplicationDbContext _context;

        public ChatHub(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Send a message to a troupe (group chat)
        /// </summary>
        public async Task SendTroupeMessage(string troupeId, string userId, string userName, string message)
        {
            // Save message to database
            var newMessage = new Message
            {
                SenderId = Guid.Parse(userId),
                Content = message,
                TroupeId = Guid.Parse(troupeId),
                CreatedAt = DateTime.UtcNow
            };

            _context.Messages.Add(newMessage);
            await _context.SaveChangesAsync();

            // Broadcast to all clients in the troupe group
            await Clients.Group($"troupe_{troupeId}").SendAsync("ReceiveTroupeMessage", new
            {
                id = newMessage.Id.ToString(),
                senderId = userId,
                senderName = userName,
                content = message,
                troupeId = troupeId,
                createdAt = newMessage.CreatedAt
            });
        }

        /// <summary>
        /// Send a direct message to a conversation
        /// </summary>
        public async Task SendDirectMessage(string conversationId, string userId, string userName, string message)
        {
            // Save message to database
            var newMessage = new Message
            {
                SenderId = Guid.Parse(userId),
                Content = message,
                ConversationId = Guid.Parse(conversationId),
                CreatedAt = DateTime.UtcNow
            };

            _context.Messages.Add(newMessage);
            await _context.SaveChangesAsync();

            // Broadcast to all clients in the conversation group
            await Clients.Group($"conversation_{conversationId}").SendAsync("ReceiveDirectMessage", new
            {
                id = newMessage.Id.ToString(),
                senderId = userId,
                senderName = userName,
                content = message,
                conversationId = conversationId,
                createdAt = newMessage.CreatedAt
            });
        }

        /// <summary>
        /// Join a troupe group to receive real-time updates
        /// </summary>
        public async Task JoinTroupe(string troupeId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"troupe_{troupeId}");
            
            // Notify others that someone joined
            await Clients.OthersInGroup($"troupe_{troupeId}").SendAsync("UserJoinedTroupe", new
            {
                connectionId = Context.ConnectionId,
                troupeId = troupeId,
                timestamp = DateTime.UtcNow
            });
        }

        /// <summary>
        /// Leave a troupe group
        /// </summary>
        public async Task LeaveTroupe(string troupeId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"troupe_{troupeId}");
            
            // Notify others that someone left
            await Clients.OthersInGroup($"troupe_{troupeId}").SendAsync("UserLeftTroupe", new
            {
                connectionId = Context.ConnectionId,
                troupeId = troupeId,
                timestamp = DateTime.UtcNow
            });
        }

        /// <summary>
        /// Join a conversation group to receive real-time updates
        /// </summary>
        public async Task JoinConversation(string conversationId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"conversation_{conversationId}");
            
            // Notify others that someone joined
            await Clients.OthersInGroup($"conversation_{conversationId}").SendAsync("UserJoinedConversation", new
            {
                connectionId = Context.ConnectionId,
                conversationId = conversationId,
                timestamp = DateTime.UtcNow
            });
        }

        /// <summary>
        /// Leave a conversation group
        /// </summary>
        public async Task LeaveConversation(string conversationId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"conversation_{conversationId}");
            
            // Notify others that someone left
            await Clients.OthersInGroup($"conversation_{conversationId}").SendAsync("UserLeftConversation", new
            {
                connectionId = Context.ConnectionId,
                conversationId = conversationId,
                timestamp = DateTime.UtcNow
            });
        }

        /// <summary>
        /// Broadcast typing indicator to troupe
        /// </summary>
        public async Task SendTypingIndicator(string troupeId, string userName)
        {
            await Clients.OthersInGroup($"troupe_{troupeId}").SendAsync("UserTyping", new
            {
                userName = userName,
                troupeId = troupeId,
                timestamp = DateTime.UtcNow
            });
        }

        /// <summary>
        /// Broadcast typing indicator to conversation
        /// </summary>
        public async Task SendTypingIndicatorToConversation(string conversationId, string userName)
        {
            await Clients.OthersInGroup($"conversation_{conversationId}").SendAsync("UserTypingInConversation", new
            {
                userName = userName,
                conversationId = conversationId,
                timestamp = DateTime.UtcNow
            });
        }

        /// <summary>
        /// Handle client connection
        /// </summary>
        public override async Task OnConnectedAsync()
        {
            await Clients.Caller.SendAsync("Connected", Context.ConnectionId);
            await base.OnConnectedAsync();
        }

        /// <summary>
        /// Handle client disconnection
        /// </summary>
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await base.OnDisconnectedAsync(exception);
        }
    }
}
