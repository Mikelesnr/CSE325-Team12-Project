using System;

namespace CSE325_Team12_Project.Models.DTOs
{
    public class MemberDto
    {
        public Guid UserId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime JoinedAt { get; set; }
    }
}
