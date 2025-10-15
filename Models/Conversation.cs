using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CSE325_Team12_Project.Models
{
    public class Conversation
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid CreatedBy { get; set; }

        public bool IsGroup { get; set; } = false;

        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        [ForeignKey(nameof(CreatedBy))]
        public virtual User? Creator { get; set; }

        public virtual ICollection<Message> Messages { get; set; } = new List<Message>();
        public virtual ICollection<ConversationParticipant> Participants { get; set; } = new List<ConversationParticipant>();
    }
}
