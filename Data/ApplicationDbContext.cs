using Microsoft.EntityFrameworkCore;
using CSE325_Team12_Project.Models;

namespace CSE325_Team12_Project.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<User> Users => Set<User>();
        public DbSet<Troupe> Troupes => Set<Troupe>();
        public DbSet<Membership> Memberships => Set<Membership>();
        public DbSet<Message> Messages => Set<Message>();
        public DbSet<Conversation> Conversations => Set<Conversation>();
        public DbSet<ConversationParticipant> ConversationParticipants => Set<ConversationParticipant>();
        public DbSet<InterestTag> InterestTags => Set<InterestTag>();
        public DbSet<UserInterest> UserInterests => Set<UserInterest>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(255);
                entity.Property(e => e.Password).IsRequired();
                entity.Property(e => e.AvatarUrl).HasMaxLength(500);
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("NOW()");
                entity.Property(e => e.Role).IsRequired();

                entity.HasMany(e => e.Memberships)
                    .WithOne(m => m.User)
                    .HasForeignKey(m => m.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(e => e.CreatedTroupes)
                    .WithOne(t => t.CreatedBy)
                    .HasForeignKey(t => t.CreatedById)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(e => e.Messages)
                    .WithOne(m => m.Sender)
                    .HasForeignKey(m => m.SenderId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(e => e.Conversations)
                    .WithOne(c => c.Creator)
                    .HasForeignKey(c => c.CreatedBy)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(e => e.ConversationParticipants)
                    .WithOne(cp => cp.User)
                    .HasForeignKey(cp => cp.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(e => e.Interests)
                    .WithOne(ui => ui.User)
                    .HasForeignKey(ui => ui.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Troupe
            modelBuilder.Entity<Troupe>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Description).IsRequired().HasMaxLength(1000);
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("NOW()");
                entity.Property(e => e.Visibility).IsRequired();

                entity.HasOne(e => e.CreatedBy)
                    .WithMany(u => u.CreatedTroupes)
                    .HasForeignKey(e => e.CreatedById)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(e => e.Memberships)
                    .WithOne(m => m.Troupe)
                    .HasForeignKey(m => m.TroupeId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(e => e.Messages)
                    .WithOne(m => m.Troupe)
                    .HasForeignKey(m => m.TroupeId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Membership
            modelBuilder.Entity<Membership>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.JoinedAt).HasDefaultValueSql("NOW()");

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

            // Message
            modelBuilder.Entity<Message>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Content).IsRequired().HasMaxLength(5000);
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("NOW()");

                entity.HasOne(e => e.Sender)
                    .WithMany(u => u.Messages)
                    .HasForeignKey(e => e.SenderId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Troupe)
                    .WithMany(t => t.Messages)
                    .HasForeignKey(e => e.TroupeId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Conversation)
                    .WithMany(c => c.Messages)
                    .HasForeignKey(e => e.ConversationId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.ToTable(t => t.HasCheckConstraint("CK_Message_Target",
                    "(\"TroupeId\" IS NOT NULL AND \"ConversationId\" IS NULL) OR (\"TroupeId\" IS NULL AND \"ConversationId\" IS NOT NULL)"));

            });

            // Conversation
            modelBuilder.Entity<Conversation>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("NOW()");
                entity.Property(e => e.IsGroup).HasDefaultValue(false);

                entity.HasOne(e => e.Creator)
                    .WithMany(u => u.Conversations)
                    .HasForeignKey(e => e.CreatedBy)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(e => e.Messages)
                    .WithOne(m => m.Conversation)
                    .HasForeignKey(m => m.ConversationId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(e => e.Participants)
                    .WithOne(cp => cp.Conversation)
                    .HasForeignKey(cp => cp.ConversationId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // ConversationParticipant
            modelBuilder.Entity<ConversationParticipant>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.JoinedAt).HasDefaultValueSql("NOW()");

                entity.HasOne(e => e.User)
                    .WithMany(u => u.ConversationParticipants)
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Conversation)
                    .WithMany(c => c.Participants)
                    .HasForeignKey(e => e.ConversationId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // InterestTag
            modelBuilder.Entity<InterestTag>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);

                entity.HasMany(e => e.UserInterests)
                    .WithOne(ui => ui.InterestTag)
                    .HasForeignKey(ui => ui.InterestTagId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // UserInterest
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
            });
        }
    }
}
