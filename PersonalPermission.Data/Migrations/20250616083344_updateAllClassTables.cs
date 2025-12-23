using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PersonalPermission.Data.Migrations
{
    /// <inheritdoc />
    public partial class updateAllClassTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "UsedYearlyPermissions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "UsedAdministrivePermissions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "PermissionStates",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "ServiceTimeDay", "UserGuid" },
                values: new object[] { new DateTime(2025, 6, 16, 11, 33, 42, 550, DateTimeKind.Local).AddTicks(5929), 23, new Guid("f9f7fb6b-e47d-4155-91b9-b7f5687d7f0a") });

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
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UsedAdministrivePermissions_Users_UserId",
                table: "UsedAdministrivePermissions",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UsedYearlyPermissions_Users_UserId",
                table: "UsedYearlyPermissions",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "ServiceTimeDay", "UserGuid" },
                values: new object[] { new DateTime(2025, 6, 13, 11, 36, 5, 507, DateTimeKind.Local).AddTicks(5380), 20, new Guid("accd3a52-6127-4d9d-86b6-dc6add7198e7") });
        }
    }
}
