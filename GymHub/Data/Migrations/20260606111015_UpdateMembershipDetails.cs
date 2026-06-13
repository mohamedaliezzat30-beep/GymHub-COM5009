using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymHub.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateMembershipDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PlanName",
                table: "Memberships",
                newName: "MembershipType");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Memberships",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "MemberName",
                table: "Memberships",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "Memberships",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MemberName",
                table: "Memberships");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "Memberships");

            migrationBuilder.RenameColumn(
                name: "MembershipType",
                table: "Memberships",
                newName: "PlanName");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Memberships",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);
        }
    }
}
