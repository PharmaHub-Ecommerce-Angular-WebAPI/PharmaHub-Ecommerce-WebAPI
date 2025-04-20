using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PharmaHub.DAL.Migrations
{
    /// <inheritdoc />
    public partial class FixEnumProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0ec70b0d-f09c-450e-8720-a3e03e39367f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4c00d278-b8d1-47b1-8fb6-96e55a7199d9");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "270450c4-18d8-4a7d-970f-b86716662270", "a0176f0e-b51e-4b4a-840c-75e1be96362e" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "270450c4-18d8-4a7d-970f-b86716662270");

            migrationBuilder.DeleteData(
                table: "users",
                keyColumn: "Id",
                keyValue: "a0176f0e-b51e-4b4a-840c-75e1be96362e");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "302856ec-f998-463e-8738-d3f87f53bc83", null, "Admin", "ADMIN" },
                    { "6ea757a5-01b7-43bb-bf37-4d7571b4e36b", null, "Customer", "CUSTOMER" },
                    { "ed9491ed-15d9-4978-a76c-db04cfe39c0d", null, "Pharmacy", "PHARMACY" }
                });

            migrationBuilder.InsertData(
                table: "users",
                columns: new[] { "Id", "AccessFailedCount", "AccountStat", "Address", "ConcurrencyStamp", "Country", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName", "city" },
                values: new object[] { "b0eafe56-23df-4e3d-884d-c563a15b9c2e", 0, "0", "Admin HQ", "5fd75426-badc-4154-9768-22f037e8b719", "Egypt", "admin@example.com", true, false, null, "ADMIN@EXAMPLE.COM", "ADMIN@EXAMPLE.COM", "AQAAAAIAAYagAAAAEFnupFy23xRw9iVXAg5GjG/XYytPSD0EnHT4OLSctd+IC/rDBqUPFPbBLyEztH7fwg==", "+200000000000", false, "f5c845b8-f758-4f10-b89f-92bef6f37f67", false, "admin@example.com", "Alexandria" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "302856ec-f998-463e-8738-d3f87f53bc83", "b0eafe56-23df-4e3d-884d-c563a15b9c2e" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6ea757a5-01b7-43bb-bf37-4d7571b4e36b");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ed9491ed-15d9-4978-a76c-db04cfe39c0d");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "302856ec-f998-463e-8738-d3f87f53bc83", "b0eafe56-23df-4e3d-884d-c563a15b9c2e" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "302856ec-f998-463e-8738-d3f87f53bc83");

            migrationBuilder.DeleteData(
                table: "users",
                keyColumn: "Id",
                keyValue: "b0eafe56-23df-4e3d-884d-c563a15b9c2e");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "0ec70b0d-f09c-450e-8720-a3e03e39367f", null, "Customer", "CUSTOMER" },
                    { "270450c4-18d8-4a7d-970f-b86716662270", null, "Admin", "ADMIN" },
                    { "4c00d278-b8d1-47b1-8fb6-96e55a7199d9", null, "Pharmacy", "PHARMACY" }
                });

            migrationBuilder.InsertData(
                table: "users",
                columns: new[] { "Id", "AccessFailedCount", "AccountStat", "Address", "ConcurrencyStamp", "Country", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName", "city" },
                values: new object[] { "a0176f0e-b51e-4b4a-840c-75e1be96362e", 0, "0", "Admin HQ", "11115964-bbbb-478e-a86d-5f098d099046", "Egypt", "admin@example.com", true, false, null, "ADMIN@EXAMPLE.COM", "ADMIN@EXAMPLE.COM", "AQAAAAIAAYagAAAAEFnupFy23xRw9iVXAg5GjG/XYytPSD0EnHT4OLSctd+IC/rDBqUPFPbBLyEztH7fwg==", "+200000000000", false, "b69d2d43-cdcc-4ab8-8631-3ca6fb2e5adb", false, "admin@example.com", "Alexandria" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "270450c4-18d8-4a7d-970f-b86716662270", "a0176f0e-b51e-4b4a-840c-75e1be96362e" });
        }
    }
}
