using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GymHub.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "FitnessContents",
                columns: new[] { "FitnessContentId", "Description", "ImagePath", "Title" },
                values: new object[] { 1, "A workout plan designed for new gym members.", "/images/default-workout.jpg", "Beginner Workout Plan" });

            migrationBuilder.InsertData(
                table: "GymClasses",
                columns: new[] { "GymClassId", "Capacity", "ClassDate", "ClassName", "EndTime", "StartTime" },
                values: new object[,]
                {
                    { 1, 20, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Yoga", "", "" },
                    { 2, 15, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "HIIT", "", "" },
                    { 3, 25, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Strength Training", "", "" }
                });

            migrationBuilder.InsertData(
                table: "Memberships",
                columns: new[] { "MembershipId", "Duration", "MemberName", "MembershipType", "Price", "StartDate", "Status" },
                values: new object[,]
                {
                    { 1, 30, "John Smith", "Basic", 19.99m, new DateTime(2026, 6, 10, 12, 11, 47, 950, DateTimeKind.Local).AddTicks(501), "Active" },
                    { 2, 30, "Sarah Johnson", "Premium", 34.99m, new DateTime(2026, 6, 10, 12, 11, 47, 950, DateTimeKind.Local).AddTicks(568), "Active" },
                    { 3, 30, "Michael Brown", "VIP", 49.99m, new DateTime(2026, 6, 10, 12, 11, 47, 950, DateTimeKind.Local).AddTicks(573), "Active" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "FitnessContents",
                keyColumn: "FitnessContentId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "GymClasses",
                keyColumn: "GymClassId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "GymClasses",
                keyColumn: "GymClassId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "GymClasses",
                keyColumn: "GymClassId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Memberships",
                keyColumn: "MembershipId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Memberships",
                keyColumn: "MembershipId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Memberships",
                keyColumn: "MembershipId",
                keyValue: 3);
        }
    }
}
