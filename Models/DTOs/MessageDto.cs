using System;

namespace CSE325_Team12_Project.Models.DTOs
{
    public class MessageDto
    {
        public Guid Id { get; set; }
        public Guid SenderId { get; set; }
        public string SenderName { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }

        // ✅ Optional linkage to troupe or conversation
        public Guid? TroupeId { get; set; }
        public Guid? ConversationId { get; set; }


        // ✅ For alert filtering
        public TroupeDto? Troupe { get; set; }
        public ConversationDto? Conversation { get; set; }
    }
}
