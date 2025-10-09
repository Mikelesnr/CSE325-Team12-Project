using System;

namespace CSE325_Team12_Project.Models.DTOs
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? AvatarUrl { get; set; }
    }
}

