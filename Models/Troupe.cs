using System;

namespace CSE325_Team12_Project.Models
{
    public class Troupe
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public Visibility Visibility { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public enum Visibility
    {
        Public,
        Private
    }
}
