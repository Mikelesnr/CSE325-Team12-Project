using System.ComponentModel.DataAnnotations;
using System;

namespace CSE325_Team12_Project.Models
{
    public class ConversationParticipant
    {
        public Guid Id { get; set; }
        public Guid ConversationId { get; set; }
        public Guid UserId { get; set; }
        public DateTime JoinedAt { get; set; }

        // Navigation
        public virtual Conversation? Conversation { get; set; }
        public virtual User? User { get; set; }
    }
}
