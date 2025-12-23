using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PersonalPermission.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUsedAdministrativeee : Migration
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

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "UserGuid" },
                values: new object[] { new DateTime(2025, 12, 17, 13, 9, 17, 840, DateTimeKind.Local).AddTicks(5352), new Guid("cede59fc-de1e-44d9-934b-dd180cef6221") });

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

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "UserGuid" },
                values: new object[] { new DateTime(2025, 12, 17, 11, 11, 29, 648, DateTimeKind.Local).AddTicks(2955), new Guid("02c8adde-296a-49f0-98ca-d47dcf191d81") });

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
