using System.ComponentModel.DataAnnotations;

namespace CSE325_Team12_Project.Models
{
    public class Membership
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public Guid UserId { get; set; }

        [Required]
        public Guid TroupeId { get; set; }

        public DateTime JoinedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public virtual User User { get; set; } = null!;
        public virtual Troupe Troupe { get; set; } = null!;
    }
}
