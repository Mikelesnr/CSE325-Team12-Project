using System;
using System.Collections.Generic;
using CSE325_Team12_Project.Models; // For TroupeVisibility
using CSE325_Team12_Project.Models.DTOs; // For MemberDto and MessageDto

namespace CSE325_Team12_Project.Models.DTOs
{
    public class TroupeDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public TroupeVisibility Visibility { get; set; }
        public Guid CreatedById { get; set; }
        public DateTime CreatedAt { get; set; }

        public string? AvatarUrl { get; set; }

        public List<MemberDto> Members { get; set; } = new();
        public List<MessageDto> Messages { get; set; } = new();
    }
}
