using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PersonalPermission.Data.Migrations
{
    /// <inheritdoc />
    public partial class updateClasses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "WhichYears",
                table: "UsedYearlyPermissions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WhichYears",
                table: "UsedAdministrivePermissions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "ServiceTimeDay", "UserGuid" },
                values: new object[] { new DateTime(2025, 6, 18, 10, 18, 41, 454, DateTimeKind.Local).AddTicks(6140), 25, new Guid("7186c646-4112-40e5-81dc-02a45be6fdbd") });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WhichYears",
                table: "UsedYearlyPermissions");

            migrationBuilder.DropColumn(
                name: "WhichYears",
                table: "UsedAdministrivePermissions");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "ServiceTimeDay", "UserGuid" },
                values: new object[] { new DateTime(2025, 6, 16, 14, 33, 7, 277, DateTimeKind.Local).AddTicks(9219), 23, new Guid("bcda2bf3-2912-4ec3-ad5c-cb29ae78a4a1") });
        }
    }
}
