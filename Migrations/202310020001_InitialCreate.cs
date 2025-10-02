using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CSE325_Team12_Project.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable("Users", table => new
            {
                Id = table.Column<Guid>(nullable: false),
                Name = table.Column<string>(nullable: false),
                Email = table.Column<string>(nullable: false),
                Password = table.Column<string>(nullable: false),
                AvatarUrl = table.Column<string>(nullable: true),
                Role = table.Column<string>(nullable: false),
                CreatedAt = table.Column<DateTime>(nullable: false)
            }, constraints: table => { table.PrimaryKey("PK_Users", x => x.Id); });

            migrationBuilder.CreateTable("Troupes", table => new
            {
                Id = table.Column<Guid>(nullable: false),
                Name = table.Column<string>(nullable: false),
                Description = table.Column<string>(nullable: true),
                Visibility = table.Column<string>(nullable: false),
                CreatedBy = table.Column<Guid>(nullable: false),
                CreatedAt = table.Column<DateTime>(nullable: false)
            }, constraints: table =>
            {
                table.PrimaryKey("PK_Troupes", x => x.Id);
                table.ForeignKey("FK_Troupes_Users_CreatedBy", x => x.CreatedBy, "Users", "Id", onDelete: ReferentialAction.Cascade);
            });

            migrationBuilder.CreateTable("Conversations", table => new
            {
                Id = table.Column<Guid>(nullable: false),
                CreatedBy = table.Column<Guid>(nullable: false),
                IsGroup = table.Column<bool>(nullable: false, defaultValue: false),
                CreatedAt = table.Column<DateTime>(nullable: false)
            }, constraints: table =>
            {
                table.PrimaryKey("PK_Conversations", x => x.Id);
                table.ForeignKey("FK_Conversations_Users_CreatedBy", x => x.CreatedBy, "Users", "Id", onDelete: ReferentialAction.Cascade);
            });

            migrationBuilder.CreateTable("InterestTags", table => new
            {
                Id = table.Column<Guid>(nullable: false),
                Name = table.Column<string>(nullable: false)
            }, constraints: table => { table.PrimaryKey("PK_InterestTags", x => x.Id); });

            migrationBuilder.CreateTable("ConversationParticipants", table => new
            {
                ConversationId = table.Column<Guid>(nullable: false),
                UserId = table.Column<Guid>(nullable: false),
                JoinedAt = table.Column<DateTime>(nullable: false)
            }, constraints: table =>
            {
                table.PrimaryKey("PK_ConversationParticipants", x => new { x.ConversationId, x.UserId });
                table.ForeignKey("FK_ConversationParticipants_Conversations_ConversationId", x => x.ConversationId, "Conversations", "Id", onDelete: ReferentialAction.Cascade);
                table.ForeignKey("FK_ConversationParticipants_Users_UserId", x => x.UserId, "Users", "Id", onDelete: ReferentialAction.Cascade);
            });

            migrationBuilder.CreateTable("Memberships", table => new
            {
                Id = table.Column<Guid>(nullable: false),
                UserId = table.Column<Guid>(nullable: false),
                TroupeId = table.Column<Guid>(nullable: false),
                JoinedAt = table.Column<DateTime>(nullable: false)
            }, constraints: table =>
            {
                table.PrimaryKey("PK_Memberships", x => x.Id);
                table.ForeignKey("FK_Memberships_Users_UserId", x => x.UserId, "Users", "Id", onDelete: ReferentialAction.Cascade);
                table.ForeignKey("FK_Memberships_Troupes_TroupeId", x => x.TroupeId, "Troupes", "Id", onDelete: ReferentialAction.Cascade);
            });

            migrationBuilder.CreateTable("UserInterests", table => new
            {
                UserId = table.Column<Guid>(nullable: false),
                InterestTagId = table.Column<Guid>(nullable: false)
            }, constraints: table =>
            {
                table.PrimaryKey("PK_UserInterests", x => new { x.UserId, x.InterestTagId });
                table.ForeignKey("FK_UserInterests_Users_UserId", x => x.UserId, "Users", "Id", onDelete: ReferentialAction.Cascade);
                table.ForeignKey("FK_UserInterests_InterestTags_InterestTagId", x => x.InterestTagId, "InterestTags", "Id", onDelete: ReferentialAction.Cascade);
            });

            migrationBuilder.CreateTable("Messages", table => new
            {
                Id = table.Column<Guid>(nullable: false),
                SenderId = table.Column<Guid>(nullable: false),
                Content = table.Column<string>(nullable: false),
                TroupeId = table.Column<Guid>(nullable: true),
                ConversationId = table.Column<Guid>(nullable: true),
                CreatedAt = table.Column<DateTime>(nullable: false)
            }, constraints: table =>
            {
                table.PrimaryKey("PK_Messages", x => x.Id);
                table.ForeignKey("FK_Messages_Users_SenderId", x => x.SenderId, "Users", "Id", onDelete: ReferentialAction.Cascade);
                table.ForeignKey("FK_Messages_Troupes_TroupeId", x => x.TroupeId, "Troupes", "Id");
                table.ForeignKey("FK_Messages_Conversations_ConversationId", x => x.ConversationId, "Conversations", "Id");
            });

            migrationBuilder.CreateIndex("IX_Troupes_CreatedBy", "Troupes", "CreatedBy");
            migrationBuilder.CreateIndex("IX_Conversations_CreatedBy", "Conversations", "CreatedBy");
            migrationBuilder.CreateIndex("IX_ConversationParticipants_UserId", "ConversationParticipants", "UserId");
            migrationBuilder.CreateIndex("IX_Memberships_UserId", "Memberships", "UserId");
            migrationBuilder.CreateIndex("IX_Memberships_TroupeId", "Memberships", "TroupeId");
            migrationBuilder.CreateIndex("IX_UserInterests_InterestTagId", "UserInterests", "InterestTagId");
            migrationBuilder.CreateIndex("IX_Messages_SenderId", "Messages", "SenderId");
            migrationBuilder.CreateIndex("IX_Messages_TroupeId", "Messages", "TroupeId");
            migrationBuilder.CreateIndex("IX_Messages_ConversationId", "Messages", "ConversationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable("Messages");
            migrationBuilder.DropTable("UserInterests");
            migrationBuilder.DropTable("Memberships");
            migrationBuilder.DropTable("ConversationParticipants");
            migrationBuilder.DropTable("InterestTags");
            migrationBuilder.DropTable("Troupes");
            migrationBuilder.DropTable("Conversations");
            migrationBuilder.DropTable("Users");
        }
    }
}
