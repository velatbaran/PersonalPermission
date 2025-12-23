using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PersonalPermission.Data.Migrations
{
    /// <inheritdoc />
    public partial class updateclassUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "Users",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "ServiceTimeDay", "ServiceTimeMonth", "UserGuid", "Username" },
                values: new object[] { new DateTime(2025, 6, 24, 16, 23, 8, 593, DateTimeKind.Local).AddTicks(4110), 0, 1, new Guid("a878c55e-c478-45fb-bb3d-99208295d5d9"), null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Username",
                table: "Users");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "ServiceTimeDay", "ServiceTimeMonth", "UserGuid" },
                values: new object[] { new DateTime(2025, 6, 23, 10, 11, 51, 216, DateTimeKind.Local).AddTicks(1957), 30, 0, new Guid("07699017-8037-4e9c-9559-37b8d6f3cdf8") });
        }
    }
}
