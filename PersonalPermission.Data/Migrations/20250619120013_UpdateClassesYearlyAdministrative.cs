using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PersonalPermission.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateClassesYearlyAdministrative : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UsedAdministrivePermissions_Users_UserId",
                table: "UsedAdministrivePermissions");

            migrationBuilder.DropForeignKey(
                name: "FK_UsedYearlyPermissions_Users_UserId",
                table: "UsedYearlyPermissions");

            migrationBuilder.DropIndex(
                name: "IX_UsedYearlyPermissions_UserId",
                table: "UsedYearlyPermissions");

            migrationBuilder.DropIndex(
                name: "IX_UsedAdministrivePermissions_UserId",
                table: "UsedAdministrivePermissions");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "UsedYearlyPermissions");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "UsedAdministrivePermissions");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "ServiceTimeDay", "UserGuid" },
                values: new object[] { new DateTime(2025, 6, 19, 15, 0, 12, 177, DateTimeKind.Local).AddTicks(452), 26, new Guid("13941368-5ed6-4611-9ffa-f5ae141d3c8d") });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "UsedYearlyPermissions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "UsedAdministrivePermissions",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "ServiceTimeDay", "UserGuid" },
                values: new object[] { new DateTime(2025, 6, 18, 10, 18, 41, 454, DateTimeKind.Local).AddTicks(6140), 25, new Guid("7186c646-4112-40e5-81dc-02a45be6fdbd") });

            migrationBuilder.CreateIndex(
                name: "IX_UsedYearlyPermissions_UserId",
                table: "UsedYearlyPermissions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UsedAdministrivePermissions_UserId",
                table: "UsedAdministrivePermissions",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UsedAdministrivePermissions_Users_UserId",
                table: "UsedAdministrivePermissions",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UsedYearlyPermissions_Users_UserId",
                table: "UsedYearlyPermissions",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
