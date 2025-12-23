using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PersonalPermission.Data.Migrations
{
    /// <inheritdoc />
    public partial class updateTableClassUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "ServiceTimeDay", "ServiceTimeMonth", "UserGuid" },
                values: new object[] { new DateTime(2025, 4, 24, 13, 14, 2, 67, DateTimeKind.Local).AddTicks(8049), 0, 11, new Guid("ff790f5e-b765-4517-aac1-d57ddff270b5") });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "ServiceTimeDay", "ServiceTimeMonth", "UserGuid" },
                values: new object[] { new DateTime(2025, 4, 11, 13, 56, 37, 365, DateTimeKind.Local).AddTicks(477), 18, 10, new Guid("cae4ac9e-a87f-40d1-9b53-c8ca57f2711b") });
        }
    }
}
