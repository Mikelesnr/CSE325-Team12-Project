using System.ComponentModel.DataAnnotations;

namespace CSE325_Team12_Project.Models
{
    public class Conversation
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        
        [Required]
        public Guid CreatedById { get; set; }
        
        public bool IsGroup { get; set; } = false;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        // Navigation properties
        public virtual User CreatedBy { get; set; } = null!;
        public virtual ICollection<ConversationParticipant> Participants { get; set; } = new List<ConversationParticipant>();
        public virtual ICollection<Message> Messages { get; set; } = new List<Message>();
    }
}
