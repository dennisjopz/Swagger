using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Swagger.Migrations
{
    /// <inheritdoc />
    public partial class seeddatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Accounts",
                columns: new[] { "UserId", "AccountCreated", "Email", "Id", "IsAdmin", "Password", "Status", "Username" },
                values: new object[] { "UID001", new DateTime(2026, 1, 2, 12, 9, 32, 423, DateTimeKind.Local).AddTicks(1849), "email@email.com", 1, true, "password123", true, "admin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Accounts",
                keyColumn: "UserId",
                keyValue: "UID001");
        }
    }
}
