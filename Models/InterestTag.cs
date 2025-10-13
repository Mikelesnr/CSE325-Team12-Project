using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CSE325_Team12_Project.Models
{
    public class InterestTag
    {
        [Key]
        public Guid Id { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        // Navigation
        public virtual ICollection<UserInterest> UserInterests { get; set; } = new List<UserInterest>();
    }
}
