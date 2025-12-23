using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PersonalPermission.Data.Migrations
{
    /// <inheritdoc />
    public partial class updateAdministrativeClass : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "StartingDate",
                table: "UsedAdministrivePermissions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "ServiceTimeDay", "UserGuid" },
                values: new object[] { new DateTime(2025, 6, 23, 10, 11, 51, 216, DateTimeKind.Local).AddTicks(1957), 30, new Guid("07699017-8037-4e9c-9559-37b8d6f3cdf8") });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StartingDate",
                table: "UsedAdministrivePermissions");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "ServiceTimeDay", "UserGuid" },
                values: new object[] { new DateTime(2025, 6, 19, 16, 25, 6, 299, DateTimeKind.Local).AddTicks(3450), 26, new Guid("553bd042-7e09-46d0-92a9-c8a446d85b81") });
        }
    }
}
