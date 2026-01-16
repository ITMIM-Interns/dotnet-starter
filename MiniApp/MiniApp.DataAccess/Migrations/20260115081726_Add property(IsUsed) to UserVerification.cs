using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MiniApp.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddpropertyIsUsedtoUserVerification : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsUsed",
                table: "UserVerifications",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsUsed",
                table: "UserVerifications");
        }
    }
}
