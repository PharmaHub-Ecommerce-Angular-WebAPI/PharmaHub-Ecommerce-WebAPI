using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PharmaHub.DAL.Migrations
{
    /// <inheritdoc />
    public partial class SeedingRolesAdminAccount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "18421918-9b74-44f5-938b-e41fcce6bdf2");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "51dee237-1a18-4f00-9191-45e43690f329");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "7d3cf71c-fe12-4f71-807e-72b2070d9fb7", "2e00a63c-391e-4937-849a-54afdb5985c3" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7d3cf71c-fe12-4f71-807e-72b2070d9fb7");

            migrationBuilder.DeleteData(
                table: "users",
                keyColumn: "Id",
                keyValue: "2e00a63c-391e-4937-849a-54afdb5985c3");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
                    { "18421918-9b74-44f5-938b-e41fcce6bdf2", null, "Customer", "CUSTOMER" },
                    { "51dee237-1a18-4f00-9191-45e43690f329", null, "Pharmacy", "PHARMACY" },
                    { "7d3cf71c-fe12-4f71-807e-72b2070d9fb7", null, "Admin", "ADMIN" }
                });

            migrationBuilder.InsertData(
                table: "users",
                columns: new[] { "Id", "AccessFailedCount", "AccountStat", "Address", "ConcurrencyStamp", "Country", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName", "city" },
                values: new object[] { "2e00a63c-391e-4937-849a-54afdb5985c3", 0, "0", "Admin HQ", "f5990828-2329-463d-9976-2ca9569f3dca", "Egypt", "admin@example.com", true, false, null, "ADMIN@EXAMPLE.COM", "ADMIN@EXAMPLE.COM", "AQAAAAIAAYagAAAAEFnupFy23xRw9iVXAg5GjG/XYytPSD0EnHT4OLSctd+IC/rDBqUPFPbBLyEztH7fwg==", "+200000000000", false, "2c76dbbf-a0d4-4b06-b3e2-849fb69baf36", false, "admin@example.com", "Alexandria" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "7d3cf71c-fe12-4f71-807e-72b2070d9fb7", "2e00a63c-391e-4937-849a-54afdb5985c3" });
        }
    }
}
