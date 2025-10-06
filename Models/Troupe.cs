using System.ComponentModel.DataAnnotations;

namespace CSE325_Team12_Project.Models
{
    public class Troupe
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(1000)]
        public string Description { get; set; } = string.Empty;

        [Required]
        public TroupeVisibility Visibility { get; set; } = TroupeVisibility.Public;

        [Required]
        public Guid CreatedById { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public virtual User CreatedBy { get; set; } = null!;
        public virtual ICollection<Membership> Memberships { get; set; } = new List<Membership>();
        public virtual ICollection<Message> Messages { get; set; } = new List<Message>();

    }

    public enum TroupeVisibility
    {
        Public,
        Private
    }
}
