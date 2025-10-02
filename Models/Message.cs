using System;

namespace CSE325_Team12_Project.Models
{
    public class Message
    {
        public Guid Id { get; set; }
        public Guid SenderId { get; set; }
        public string Content { get; set; } = string.Empty;
        public Guid? TroupeId { get; set; }
        public Guid? ConversationId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
