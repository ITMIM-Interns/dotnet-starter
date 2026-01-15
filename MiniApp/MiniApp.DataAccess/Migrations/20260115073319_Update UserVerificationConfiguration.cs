using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MiniApp.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserVerificationConfiguration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserVerifications_UserId_Type",
                table: "UserVerifications");

            migrationBuilder.CreateIndex(
                name: "IX_UserVerifications_UserId",
                table: "UserVerifications",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserVerifications_UserId",
                table: "UserVerifications");

            migrationBuilder.CreateIndex(
                name: "IX_UserVerifications_UserId_Type",
                table: "UserVerifications",
                columns: new[] { "UserId", "Type" },
                unique: true);
        }
    }
}
