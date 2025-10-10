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

        public async Task<Conversation> CreateConversationAsync(List<Guid> userIds, Guid createdBy)
        {
            if (userIds.Count == 2)
            {
                var existing = await _context.Conversations
                    .Include(c => c.Participants)
                    .Where(c => !c.IsGroup &&
                                c.Participants.Count == 2 &&
                                userIds.All(uid => c.Participants.Any(p => p.UserId == uid)))
                    .FirstOrDefaultAsync();

                if (existing != null)
                {
                    return existing;
                }
            }

            var conversation = new Conversation
            {
                Id = Guid.NewGuid(),
                CreatedBy = createdBy,
                CreatedAt = DateTime.UtcNow,
                IsGroup = userIds.Count > 2,
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
