using Microsoft.EntityFrameworkCore;
using CSE325_Team12_Project.Models;

namespace CSE325_Team12_Project.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Troupe> Troupes { get; set; }
        public DbSet<Membership> Memberships { get; set; }
        public DbSet<Conversation> Conversations { get; set; }
        public DbSet<ConversationParticipant> ConversationParticipants { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<InterestTag> InterestTags { get; set; }
        public DbSet<UserInterest> UserInterests { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User configuration
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(255);
                entity.HasIndex(e => e.Email).IsUnique();
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Password).IsRequired();
                entity.Property(e => e.AvatarUrl).HasMaxLength(500);
                entity.Property(e => e.Role).HasConversion<string>();
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("datetime('now')");
            });

            // Troupe configuration
            modelBuilder.Entity<Troupe>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Description).IsRequired().HasMaxLength(1000);
                entity.Property(e => e.Visibility).HasConversion<string>();
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("datetime('now')");

                entity.HasOne(e => e.CreatedBy)
                    .WithMany(u => u.CreatedTroupes)
                    .HasForeignKey(e => e.CreatedById)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(e => e.Messages)
                    .WithOne(m => m.Troupe)
                    .HasForeignKey(m => m.TroupeId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Membership configuration
            modelBuilder.Entity<Membership>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.JoinedAt).HasDefaultValueSql("datetime('now')");

                entity.HasOne(e => e.User)
                    .WithMany(u => u.Memberships)
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Troupe)
                    .WithMany(t => t.Memberships)
                    .HasForeignKey(e => e.TroupeId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(e => new { e.UserId, e.TroupeId }).IsUnique();
            });

            // Conversation configuration
            modelBuilder.Entity<Conversation>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.IsGroup).HasDefaultValue(false);
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("datetime('now')");

                entity.HasOne(e => e.Creator)
                    .WithMany(u => u.Conversations)
                    .HasForeignKey(e => e.CreatedBy)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // ConversationParticipant configuration
            modelBuilder.Entity<ConversationParticipant>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.JoinedAt).HasDefaultValueSql("datetime('now')");

                entity.HasOne(e => e.Conversation)
                    .WithMany(c => c.Participants)
                    .HasForeignKey(e => e.ConversationId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.User)
                    .WithMany(u => u.ConversationParticipants)
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Message configuration
            modelBuilder.Entity<Message>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Content).IsRequired();
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("datetime('now')");

                entity.HasOne(e => e.Sender)
                    .WithMany(u => u.Messages)
                    .HasForeignKey(e => e.SenderId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Troupe)
                    .WithMany(t => t.Messages)
                    .HasForeignKey(e => e.TroupeId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Conversation)
                    .WithMany(c => c.Messages)
                    .HasForeignKey(e => e.ConversationId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // InterestTag configuration
            modelBuilder.Entity<InterestTag>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);

                entity.HasMany(e => e.UserInterests)
                    .WithOne(ui => ui.InterestTag)
                    .HasForeignKey(ui => ui.InterestTagId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // UserInterest configuration
            modelBuilder.Entity<UserInterest>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.HasOne(e => e.User)
                    .WithMany(u => u.Interests)
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.InterestTag)
                    .WithMany(t => t.UserInterests)
                    .HasForeignKey(e => e.InterestTagId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(e => new { e.UserId, e.InterestTagId }).IsUnique();
            });
        }
    }
}
