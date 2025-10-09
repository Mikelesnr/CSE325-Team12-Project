using System.ComponentModel.DataAnnotations;

namespace CSE325_Team12_Project.Models
{
    public class Message
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        
        [Required]
        public Guid SenderId { get; set; }
        
        [Required]
        [StringLength(5000)]
        public string Content { get; set; } = string.Empty;
        
        // Either TroupeId or ConversationId should be set, but not both
        public Guid? TroupeId { get; set; }
        public Guid? ConversationId { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        // Navigation properties
        public virtual User Sender { get; set; } = null!;
        public virtual Troupe? Troupe { get; set; }
        public virtual Conversation? Conversation { get; set; }
    }
}
