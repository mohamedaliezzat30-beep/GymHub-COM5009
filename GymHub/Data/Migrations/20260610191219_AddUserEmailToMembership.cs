using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymHub.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddUserEmailToMembership : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserEmail",
                table: "Memberships",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Memberships",
                keyColumn: "MembershipId",
                keyValue: 1,
                column: "UserEmail",
                value: null);

            migrationBuilder.UpdateData(
                table: "Memberships",
                keyColumn: "MembershipId",
                keyValue: 2,
                column: "UserEmail",
                value: null);

            migrationBuilder.UpdateData(
                table: "Memberships",
                keyColumn: "MembershipId",
                keyValue: 3,
                column: "UserEmail",
                value: null);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserEmail",
                table: "Memberships");
        }
    }
}
