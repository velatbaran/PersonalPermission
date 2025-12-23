using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PersonalPermission.Data.Migrations
{
    /// <inheritdoc />
    public partial class updateClassTablePermissionState4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PermissionStates_Users_UserId",
                table: "PermissionStates");

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

            migrationBuilder.DropIndex(
                name: "IX_PermissionStates_UserId",
                table: "PermissionStates");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "UsedYearlyPermissions");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "UsedAdministrivePermissions");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "PermissionStates");

            migrationBuilder.AddColumn<Guid>(
                name: "UserGuid",
                table: "UsedYearlyPermissions",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UserGuid",
                table: "UsedAdministrivePermissions",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UserGuid",
                table: "PermissionStates",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "UserGuid" },
                values: new object[] { new DateTime(2025, 4, 29, 10, 43, 35, 704, DateTimeKind.Local).AddTicks(8069), new Guid("f4b0526c-99b0-485d-ad67-18651c58e482") });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserGuid",
                table: "UsedYearlyPermissions");

            migrationBuilder.DropColumn(
                name: "UserGuid",
                table: "UsedAdministrivePermissions");

            migrationBuilder.DropColumn(
                name: "UserGuid",
                table: "PermissionStates");

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

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "PermissionStates",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "UserGuid" },
                values: new object[] { new DateTime(2025, 4, 29, 10, 28, 5, 773, DateTimeKind.Local).AddTicks(6460), new Guid("ef7bc9b6-9322-4ad5-81ad-5956bb3684fb") });

            migrationBuilder.CreateIndex(
                name: "IX_UsedYearlyPermissions_UserId",
                table: "UsedYearlyPermissions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UsedAdministrivePermissions_UserId",
                table: "UsedAdministrivePermissions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_PermissionStates_UserId",
                table: "PermissionStates",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_PermissionStates_Users_UserId",
                table: "PermissionStates",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");

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
