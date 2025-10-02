using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using CSE325_Team12_Project.Data;

namespace CSE325_Team12_Project.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "9.0.0");

            modelBuilder.Entity("CSE325_Team12_Project.Models.User", b =>
            {
                b.Property<Guid>("Id").ValueGeneratedOnAdd();
                b.Property<string>("Name").IsRequired();
                b.Property<string>("Email").IsRequired();
                b.Property<string>("Password").IsRequired();
                b.Property<string>("AvatarUrl");
                b.Property<string>("Role").IsRequired();
                b.Property<DateTime>("CreatedAt");
                b.HasKey("Id");
                b.ToTable("Users");
            });

            modelBuilder.Entity("CSE325_Team12_Project.Models.Troupe", b =>
            {
                b.Property<Guid>("Id").ValueGeneratedOnAdd();
                b.Property<string>("Name").IsRequired();
                b.Property<string>("Description");
                b.Property<string>("Visibility").IsRequired();
                b.Property<Guid>("CreatedBy");
                b.Property<DateTime>("CreatedAt");
                b.HasKey("Id");
                b.HasIndex("CreatedBy");
                b.ToTable("Troupes");
            });

            modelBuilder.Entity("CSE325_Team12_Project.Models.Conversation", b =>
            {
                b.Property<Guid>("Id").ValueGeneratedOnAdd();
                b.Property<Guid>("CreatedBy");
                b.Property<bool>("IsGroup").HasDefaultValue(false);
                b.Property<DateTime>("CreatedAt");
                b.HasKey("Id");
                b.HasIndex("CreatedBy");
                b.ToTable("Conversations");
            });

            modelBuilder.Entity("CSE325_Team12_Project.Models.ConversationParticipant", b =>
            {
                b.Property<Guid>("ConversationId");
                b.Property<Guid>("UserId");
                b.Property<DateTime>("JoinedAt");
                b.HasKey("ConversationId", "UserId");
                b.HasIndex("UserId");
                b.ToTable("ConversationParticipants");
            });

            modelBuilder.Entity("CSE325_Team12_Project.Models.Membership", b =>
            {
                b.Property<Guid>("Id").ValueGeneratedOnAdd();
                b.Property<Guid>("UserId");
                b.Property<Guid>("TroupeId");
                b.Property<DateTime>("JoinedAt");
                b.HasKey("Id");
                b.HasIndex("UserId");
                b.HasIndex("TroupeId");
                b.ToTable("Memberships");
            });

            modelBuilder.Entity("CSE325_Team12_Project.Models.InterestTag", b =>
            {
                b.Property<Guid>("Id").ValueGeneratedOnAdd();
                b.Property<string>("Name").IsRequired();
                b.HasKey("Id");
                b.ToTable("InterestTags");
            });

            modelBuilder.Entity("CSE325_Team12_Project.Models.UserInterest", b =>
            {
                b.Property<Guid>("UserId");
                b.Property<Guid>("InterestTagId");
                b.HasKey("UserId", "InterestTagId");
                b.HasIndex("InterestTagId");
                b.ToTable("UserInterests");
            });

            modelBuilder.Entity("CSE325_Team12_Project.Models.Message", b =>
            {
                b.Property<Guid>("Id").ValueGeneratedOnAdd();
                b.Property<Guid>("SenderId");
                b.Property<string>("Content").IsRequired();
                b.Property<Guid?>("TroupeId");
                b.Property<Guid?>("ConversationId");
                b.Property<DateTime>("CreatedAt");
                b.HasKey("Id");
                b.HasIndex("SenderId");
                b.HasIndex("TroupeId");
                b.HasIndex("ConversationId");
                b.ToTable("Messages");
            });
        }
    }
}
