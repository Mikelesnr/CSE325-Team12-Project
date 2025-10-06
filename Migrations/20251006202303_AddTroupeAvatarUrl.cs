using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CSE325_Team12_Project.Migrations
{
    /// <inheritdoc />
    public partial class AddTroupeAvatarUrl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AvatarUrl",
                table: "Troupes",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AvatarUrl",
                table: "Troupes");
        }
    }
}
