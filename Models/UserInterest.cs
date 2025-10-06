using System;

namespace CSE325_Team12_Project.Models
{
    public class UserInterest
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid InterestTagId { get; set; }

        // Navigation
        public virtual User? User { get; set; }
        public virtual InterestTag? InterestTag { get; set; }
    }
}
