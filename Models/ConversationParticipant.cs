using System.ComponentModel.DataAnnotations;

namespace CSE325_Team12_Project.Models
{
    public class ConversationParticipant
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        
        [Required]
        public Guid ConversationId { get; set; }
        
        [Required]
        public Guid UserId { get; set; }
        
        public DateTime JoinedAt { get; set; } = DateTime.UtcNow;
        
        // Navigation properties
        public virtual Conversation Conversation { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }
}
