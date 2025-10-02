using System;
using System.Collections.Generic;

namespace CSE325_Team12_Project.Models
{
    public class Conversation
    {
        public Guid Id { get; set; }
        public Guid CreatedBy { get; set; }
        public bool IsGroup { get; set; } = false;
        public DateTime CreatedAt { get; set; }

        // Navigation
        public virtual User? Creator { get; set; }
        public virtual ICollection<Message> Messages { get; set; } = new List<Message>();
        public virtual ICollection<ConversationParticipant> Participants { get; set; } = new List<ConversationParticipant>();
    }
}
