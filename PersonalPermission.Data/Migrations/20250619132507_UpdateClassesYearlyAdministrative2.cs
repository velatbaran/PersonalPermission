using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PersonalPermission.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateClassesYearlyAdministrative2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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
                columns: new[] { "CreatedDate", "UserGuid" },
                values: new object[] { new DateTime(2025, 6, 19, 16, 25, 6, 299, DateTimeKind.Local).AddTicks(3450), new Guid("553bd042-7e09-46d0-92a9-c8a446d85b81") });

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
                columns: new[] { "CreatedDate", "UserGuid" },
                values: new object[] { new DateTime(2025, 6, 19, 15, 11, 54, 215, DateTimeKind.Local).AddTicks(8123), new Guid("6add1531-a56b-422b-8b5a-2af028e30c4b") });
        }
    }
}
