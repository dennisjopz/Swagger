using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Swagger.Migrations
{
    /// <inheritdoc />
    public partial class seeddatabasefix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "UserId",
                keyValue: "UID001",
                column: "AccountCreated",
                value: new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "UserId",
                keyValue: "UID001",
                column: "AccountCreated",
                value: new DateTime(2026, 1, 2, 12, 9, 32, 423, DateTimeKind.Local).AddTicks(1849));
        }
    }
}
