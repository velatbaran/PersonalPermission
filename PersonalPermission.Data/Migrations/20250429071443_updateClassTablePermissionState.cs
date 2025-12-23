using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PersonalPermission.Data.Migrations
{
    /// <inheritdoc />
    public partial class updateClassTablePermissionState : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "WhichYears",
                table: "PermissionStates",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "ServiceTimeDay", "UserGuid" },
                values: new object[] { new DateTime(2025, 4, 29, 10, 14, 41, 568, DateTimeKind.Local).AddTicks(8246), 5, new Guid("0463127b-a243-44f8-80ca-11a99e89a6b5") });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WhichYears",
                table: "PermissionStates");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "ServiceTimeDay", "UserGuid" },
                values: new object[] { new DateTime(2025, 4, 24, 13, 14, 2, 67, DateTimeKind.Local).AddTicks(8049), 0, new Guid("ff790f5e-b765-4517-aac1-d57ddff270b5") });
        }
    }
}
