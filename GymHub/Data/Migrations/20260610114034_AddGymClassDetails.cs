using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymHub.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddGymClassDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DifficultyLevel",
                table: "GymClasses",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Instructor",
                table: "GymClasses",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "FitnessContents",
                keyColumn: "FitnessContentId",
                keyValue: 1,
                column: "ImagePath",
                value: null);

            migrationBuilder.UpdateData(
                table: "GymClasses",
                keyColumn: "GymClassId",
                keyValue: 1,
                columns: new[] { "ClassDate", "DifficultyLevel", "EndTime", "Instructor", "StartTime" },
                values: new object[] { new DateTime(2026, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Beginner", "10:00", "Sarah Ahmed", "09:00" });

            migrationBuilder.UpdateData(
                table: "GymClasses",
                keyColumn: "GymClassId",
                keyValue: 2,
                columns: new[] { "ClassDate", "DifficultyLevel", "EndTime", "Instructor", "StartTime" },
                values: new object[] { new DateTime(2026, 1, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), "Advanced", "18:00", "James Wilson", "17:00" });

            migrationBuilder.UpdateData(
                table: "GymClasses",
                keyColumn: "GymClassId",
                keyValue: 3,
                columns: new[] { "ClassDate", "DifficultyLevel", "EndTime", "Instructor", "StartTime" },
                values: new object[] { new DateTime(2026, 1, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), "Intermediate", "19:00", "Michael Brown", "18:00" });

            migrationBuilder.UpdateData(
                table: "Memberships",
                keyColumn: "MembershipId",
                keyValue: 1,
                column: "StartDate",
                value: new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Memberships",
                keyColumn: "MembershipId",
                keyValue: 2,
                column: "StartDate",
                value: new DateTime(2026, 1, 5, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Memberships",
                keyColumn: "MembershipId",
                keyValue: 3,
                column: "StartDate",
                value: new DateTime(2026, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DifficultyLevel",
                table: "GymClasses");

            migrationBuilder.DropColumn(
                name: "Instructor",
                table: "GymClasses");

            migrationBuilder.UpdateData(
                table: "FitnessContents",
                keyColumn: "FitnessContentId",
                keyValue: 1,
                column: "ImagePath",
                value: "/images/default-workout.jpg");

            migrationBuilder.UpdateData(
                table: "GymClasses",
                keyColumn: "GymClassId",
                keyValue: 1,
                columns: new[] { "ClassDate", "EndTime", "StartTime" },
                values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", "" });

            migrationBuilder.UpdateData(
                table: "GymClasses",
                keyColumn: "GymClassId",
                keyValue: 2,
                columns: new[] { "ClassDate", "EndTime", "StartTime" },
                values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", "" });

            migrationBuilder.UpdateData(
                table: "GymClasses",
                keyColumn: "GymClassId",
                keyValue: 3,
                columns: new[] { "ClassDate", "EndTime", "StartTime" },
                values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", "" });

            migrationBuilder.UpdateData(
                table: "Memberships",
                keyColumn: "MembershipId",
                keyValue: 1,
                column: "StartDate",
                value: new DateTime(2026, 6, 10, 12, 11, 47, 950, DateTimeKind.Local).AddTicks(501));

            migrationBuilder.UpdateData(
                table: "Memberships",
                keyColumn: "MembershipId",
                keyValue: 2,
                column: "StartDate",
                value: new DateTime(2026, 6, 10, 12, 11, 47, 950, DateTimeKind.Local).AddTicks(568));

            migrationBuilder.UpdateData(
                table: "Memberships",
                keyColumn: "MembershipId",
                keyValue: 3,
                column: "StartDate",
                value: new DateTime(2026, 6, 10, 12, 11, 47, 950, DateTimeKind.Local).AddTicks(573));
        }
    }
}
