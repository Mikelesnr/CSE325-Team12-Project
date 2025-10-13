namespace CSE325_Team12_Project.Models.DTOs
{
    public class SendMessageRequest
    {
        public Guid SenderId { get; set; }
        public string Content { get; set; } = string.Empty;
        public Guid? TroupeId { get; set; }
        public Guid? ConversationId { get; set; }
    }
}
