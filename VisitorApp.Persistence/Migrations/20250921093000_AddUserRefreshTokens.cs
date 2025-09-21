using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VisitorApp.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddUserRefreshTokens : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserRefreshTokens",
                columns: table => new
                {
                    user_refresh_token_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    token = table.Column<string>(type: "nvarchar(500)", nullable: false),
                    expires_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    is_revoked = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    revoked_reason = table.Column<string>(type: "nvarchar(200)", nullable: true),
                    revoked_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    replaced_by_token = table.Column<string>(type: "nvarchar(500)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RemovedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Version = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRefreshTokens", x => x.user_refresh_token_id);
                    table.ForeignKey(
                        name: "FK_UserRefreshTokens_AspNetUsers_user_id",
                        column: x => x.user_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserRefreshTokens_expires_at",
                table: "UserRefreshTokens",
                column: "expires_at");

            migrationBuilder.CreateIndex(
                name: "IX_UserRefreshTokens_token",
                table: "UserRefreshTokens",
                column: "token",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserRefreshTokens_user_id",
                table: "UserRefreshTokens",
                column: "user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserRefreshTokens");
        }
    }
}
