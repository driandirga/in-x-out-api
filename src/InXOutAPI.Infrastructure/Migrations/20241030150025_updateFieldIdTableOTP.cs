using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InXOutAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateFieldIdTableOTP : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "otps",
                newName: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "id",
                table: "otps",
                newName: "Id");
        }
    }
}
