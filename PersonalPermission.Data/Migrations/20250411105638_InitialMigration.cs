using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PersonalPermission.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Surname = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    RegistryNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Title = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Department = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    StartingWorkDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ServiceTimeYear = table.Column<int>(type: "int", nullable: false),
                    ServiceTimeMonth = table.Column<int>(type: "int", nullable: false),
                    ServiceTimeDay = table.Column<int>(type: "int", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsAdmin = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    UserGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PermissionStates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    GainedAdministrativePermission = table.Column<int>(type: "int", nullable: false),
                    UsedAdministrativePermission = table.Column<int>(type: "int", nullable: false),
                    RemainAdministrativePermission = table.Column<int>(type: "int", nullable: false),
                    GainedYearlyPermission = table.Column<int>(type: "int", nullable: false),
                    UsedYearlyPermission = table.Column<int>(type: "int", nullable: false),
                    RemainYearlyPermission = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermissionStates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PermissionStates_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UsedAdministrivePermissions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    StartingTime = table.Column<int>(type: "int", nullable: false),
                    ExpirationTime = table.Column<int>(type: "int", nullable: false),
                    TotalTime = table.Column<int>(type: "int", nullable: false),
                    YearofBelong = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsedAdministrivePermissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UsedAdministrivePermissions_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UsedYearlyPermissions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    StartingDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpirationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NumberOfDay = table.Column<int>(type: "int", nullable: false),
                    YearofBelong = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsedYearlyPermissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UsedYearlyPermissions_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedDate", "Department", "IsActive", "IsAdmin", "Name", "Password", "RegistryNo", "ServiceTimeDay", "ServiceTimeMonth", "ServiceTimeYear", "StartingWorkDate", "Surname", "Title", "UserGuid" },
                values: new object[] { 1, new DateTime(2025, 4, 11, 13, 56, 37, 365, DateTimeKind.Local).AddTicks(477), "Bilgi Teknolojileri Şube Müdürlüğü", true, true, "Velat", "212121", "SP-1578", 18, 10, 1, new DateTime(2023, 5, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), "BARAN", "Mühendis", new Guid("cae4ac9e-a87f-40d1-9b53-c8ca57f2711b") });

            migrationBuilder.CreateIndex(
                name: "IX_PermissionStates_UserId",
                table: "PermissionStates",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UsedAdministrivePermissions_UserId",
                table: "UsedAdministrivePermissions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UsedYearlyPermissions_UserId",
                table: "UsedYearlyPermissions",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PermissionStates");

            migrationBuilder.DropTable(
                name: "UsedAdministrivePermissions");

            migrationBuilder.DropTable(
                name: "UsedYearlyPermissions");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
