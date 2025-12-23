using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PersonalPermission.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUsedAdministrativee : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Titles_TitleId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "YearofBelong",
                table: "UsedAdministrivePermissions");

            migrationBuilder.AlterColumn<int>(
                name: "TitleId",
                table: "Users",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "UserGuid" },
                values: new object[] { new DateTime(2025, 12, 17, 11, 11, 29, 648, DateTimeKind.Local).AddTicks(2955), new Guid("02c8adde-296a-49f0-98ca-d47dcf191d81") });

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Titles_TitleId",
                table: "Users",
                column: "TitleId",
                principalTable: "Titles",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Titles_TitleId",
                table: "Users");

            migrationBuilder.AlterColumn<int>(
                name: "TitleId",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "YearofBelong",
                table: "UsedAdministrivePermissions",
                type: "datetime2",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "UserGuid" },
                values: new object[] { new DateTime(2025, 12, 17, 10, 19, 29, 757, DateTimeKind.Local).AddTicks(6623), new Guid("3cc8c0b4-76ef-45fc-81ed-7b6495e79c8f") });

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Titles_TitleId",
                table: "Users",
                column: "TitleId",
                principalTable: "Titles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
