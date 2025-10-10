using CSE325_Team12_Project.Models;

namespace CSE325_Team12_Project.Services
{
    public interface IConversationService
    {
        Task<Conversation> CreateConversationAsync(List<Guid> userIds, Guid createdBy);
        Task<ConversationParticipant> AddParticipantAsync(Guid conversationId, Guid userId);
    }
}
