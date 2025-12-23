using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PersonalPermission.Data.Migrations
{
    /// <inheritdoc />
    public partial class updateAllClassTables2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "UserGuid" },
                values: new object[] { new DateTime(2025, 6, 16, 13, 26, 43, 70, DateTimeKind.Local).AddTicks(8252), new Guid("10ed7389-f01d-40c5-94bb-8087d0ee0a87") });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "UserGuid" },
                values: new object[] { new DateTime(2025, 6, 16, 11, 33, 42, 550, DateTimeKind.Local).AddTicks(5929), new Guid("f9f7fb6b-e47d-4155-91b9-b7f5687d7f0a") });
        }
    }
}
