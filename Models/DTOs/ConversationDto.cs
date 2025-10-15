namespace CSE325_Team12_Project.Models.DTOs
{
    public class ConversationDto
    {
        public Guid Id { get; set; }
        public Guid CreatedBy { get; set; }
        public bool IsGroup { get; set; }
        public DateTime CreatedAt { get; set; }

        public List<ConversationParticipantDto> Participants { get; set; } = new();
        public List<MessageDto> Messages { get; set; } = new();
        public ConversationParticipantDto? OtherParticipant { get; set; }
    }
}
