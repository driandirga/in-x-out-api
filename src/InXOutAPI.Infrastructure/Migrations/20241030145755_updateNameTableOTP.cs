using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InXOutAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateNameTableOTP : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OTPs_users_user_id",
                table: "OTPs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OTPs",
                table: "OTPs");

            migrationBuilder.RenameTable(
                name: "OTPs",
                newName: "otps");

            migrationBuilder.RenameIndex(
                name: "IX_OTPs_user_id",
                table: "otps",
                newName: "IX_otps_user_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_otps",
                table: "otps",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_otps_users_user_id",
                table: "otps",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_otps_users_user_id",
                table: "otps");

            migrationBuilder.DropPrimaryKey(
                name: "PK_otps",
                table: "otps");

            migrationBuilder.RenameTable(
                name: "otps",
                newName: "OTPs");

            migrationBuilder.RenameIndex(
                name: "IX_otps_user_id",
                table: "OTPs",
                newName: "IX_OTPs_user_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OTPs",
                table: "OTPs",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OTPs_users_user_id",
                table: "OTPs",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id");
        }
    }
}
