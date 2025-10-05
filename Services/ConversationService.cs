using CSE325_Team12_Project.Data;
using CSE325_Team12_Project.Models;
using Microsoft.EntityFrameworkCore;

namespace CSE325_Team12_Project.Services
{
    public class ConversationService : IConversationService
    {
        private readonly ApplicationDbContext _context;

        public ConversationService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Conversation> CreateConversationAsync(List<Guid> userIds)
        {
            var conversation = new Conversation
            {
                Id = Guid.NewGuid(),
                CreatedAt = DateTime.UtcNow,
                Participants = userIds.Select(id => new ConversationParticipant
                {
                    UserId = id,
                    JoinedAt = DateTime.UtcNow
                }).ToList()
            };

            _context.Conversations.Add(conversation);
            await _context.SaveChangesAsync();
            return conversation;
        }

        public async Task<ConversationParticipant> AddParticipantAsync(Guid conversationId, Guid userId)
        {
            var participant = new ConversationParticipant
            {
                ConversationId = conversationId,
                UserId = userId,
                JoinedAt = DateTime.UtcNow
            };

            _context.ConversationParticipants.Add(participant);
            await _context.SaveChangesAsync();
            return participant;
        }
    }
}
