using System;

namespace CSE325_Team12_Project.Models
{
    public class Conversation
    {
        public Guid Id { get; set; }
        public Guid CreatedBy { get; set; }
        public bool IsGroup { get; set; } = false;
        public DateTime CreatedAt { get; set; }
    }
}
