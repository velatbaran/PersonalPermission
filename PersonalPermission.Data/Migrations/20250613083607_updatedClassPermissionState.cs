using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PersonalPermission.Data.Migrations
{
    /// <inheritdoc />
    public partial class updatedClassPermissionState : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "AddedDate",
                table: "PermissionStates",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "ServiceTimeDay", "ServiceTimeMonth", "ServiceTimeYear", "UserGuid" },
                values: new object[] { new DateTime(2025, 6, 13, 11, 36, 5, 507, DateTimeKind.Local).AddTicks(5380), 20, 0, 2, new Guid("accd3a52-6127-4d9d-86b6-dc6add7198e7") });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AddedDate",
                table: "PermissionStates");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "ServiceTimeDay", "ServiceTimeMonth", "ServiceTimeYear", "UserGuid" },
                values: new object[] { new DateTime(2025, 4, 29, 10, 43, 35, 704, DateTimeKind.Local).AddTicks(8069), 5, 11, 1, new Guid("f4b0526c-99b0-485d-ad67-18651c58e482") });
        }
    }
}
