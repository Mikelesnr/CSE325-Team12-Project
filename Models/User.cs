using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CSE325_Team12_Project.Models
{
    public class User
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(255)]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty; // Will be hashed

        [StringLength(500)]
        public string? AvatarUrl { get; set; }

        [Required]
        public UserRole Role { get; set; } = UserRole.Trouper;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Existing navigation
        public virtual ICollection<Membership> Memberships { get; set; } = new List<Membership>();
        public virtual ICollection<Troupe> CreatedTroupes { get; set; } = new List<Troupe>();

        // Added navigation (safe for auth)
        public virtual ICollection<Message> Messages { get; set; } = new List<Message>();
        public virtual ICollection<Conversation> Conversations { get; set; } = new List<Conversation>();
        public virtual ICollection<ConversationParticipant> ConversationParticipants { get; set; } = new List<ConversationParticipant>();
        public virtual ICollection<UserInterest> Interests { get; set; } = new List<UserInterest>();
    }

    public enum UserRole
    {
        Trouper,
        Admin
    }
}
