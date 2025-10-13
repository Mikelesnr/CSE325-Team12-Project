using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CSE325_Team12_Project.Models
{
    public class UserInterest
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [Required]
        public Guid InterestTagId { get; set; }

        // Navigation
        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; } = null!;

        [ForeignKey(nameof(InterestTagId))]
        public virtual InterestTag InterestTag { get; set; } = null!;
    }
}
