using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeSimulator.Web.Migrations
{
    public partial class CloudUrlRoot : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CloudUrlRoot",
                table: "Configs",
                type: "TEXT",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Chargers",
                keyColumn: "Id",
                keyValue: 1,
                column: "IdTag",
                value: "64e42b33");

            migrationBuilder.UpdateData(
                table: "Configs",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "AccessToken", "CloudUrlRoot", "ModifyTime" },
                values: new object[] { "f2850e1b5c03444bb2403f511595e40d", "https://www.iammeter.com", new DateTime(2024, 8, 13, 8, 42, 51, 992, DateTimeKind.Local).AddTicks(4213) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CloudUrlRoot",
                table: "Configs");

            migrationBuilder.UpdateData(
                table: "Chargers",
                keyColumn: "Id",
                keyValue: 1,
                column: "IdTag",
                value: "9bcaece2");

            migrationBuilder.UpdateData(
                table: "Configs",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "AccessToken", "ModifyTime" },
                values: new object[] { "852a04d4197a48129ae46576e46db783", new DateTime(2023, 8, 8, 7, 28, 38, 633, DateTimeKind.Local).AddTicks(3652) });
        }
    }
}
