using System;
using System.Collections.Generic;

namespace CSE325_Team12_Project.Models
{
    public class InterestTag
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;

        // Navigation
        public virtual ICollection<UserInterest> UserInterests { get; set; } = new List<UserInterest>(); // ✅ Add this
    }
}
