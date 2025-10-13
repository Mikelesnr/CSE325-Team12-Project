using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CSE325_Team12_Project.Models
{
    public class Message
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid SenderId { get; set; }

        [Required, MaxLength(5000)]
        public string Content { get; set; } = string.Empty;

        public Guid? TroupeId { get; set; }
        public Guid? ConversationId { get; set; }

        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        [ForeignKey(nameof(SenderId))]
        public virtual User Sender { get; set; } = null!;

        [ForeignKey(nameof(TroupeId))]
        public virtual Troupe? Troupe { get; set; }

        [ForeignKey(nameof(ConversationId))]
        public virtual Conversation? Conversation { get; set; }
    }
}
