using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PersonalPermission.Data.Migrations
{
    /// <inheritdoc />
    public partial class updateAllClassTables3 : Migration
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

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "UsedYearlyPermissions",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "UsedAdministrivePermissions",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "PermissionStates",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "UserGuid" },
                values: new object[] { new DateTime(2025, 6, 16, 14, 33, 7, 277, DateTimeKind.Local).AddTicks(9219), new Guid("bcda2bf3-2912-4ec3-ad5c-cb29ae78a4a1") });

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

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "UsedYearlyPermissions",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "UsedAdministrivePermissions",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "PermissionStates",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "UserGuid" },
                values: new object[] { new DateTime(2025, 6, 16, 13, 26, 43, 70, DateTimeKind.Local).AddTicks(8252), new Guid("10ed7389-f01d-40c5-94bb-8087d0ee0a87") });

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
    }
}
