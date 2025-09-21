using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VisitorApp.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class FixUserRefreshTokenEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "UserRefreshTokens",
                newName: "CreatedAt");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "UserRefreshTokens",
                newName: "created_at");
        }
    }
}
