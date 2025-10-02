using System;

namespace CSE325_Team12_Project.Models
{
    public class Membership
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid TroupeId { get; set; }
        public DateTime JoinedAt { get; set; }
    }
}
