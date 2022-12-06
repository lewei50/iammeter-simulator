using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeSimulator.Web.Migrations
{
    public partial class addtometerforload : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ToMeter",
                table: "Loads",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Configs",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "AccessToken", "ModifyTime" },
                values: new object[] { "4fec0ff0e0054a8b9dc29cbe65966a73", new DateTime(2022, 12, 6, 9, 22, 25, 776, DateTimeKind.Local).AddTicks(5950) });

            migrationBuilder.UpdateData(
                table: "Loads",
                keyColumn: "Id",
                keyValue: 6L,
                column: "ToMeter",
                value: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ToMeter",
                table: "Loads");

            migrationBuilder.UpdateData(
                table: "Configs",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "AccessToken", "ModifyTime" },
                values: new object[] { "40566105289246c1a7abc18072dc17ad", new DateTime(2022, 10, 20, 11, 50, 26, 538, DateTimeKind.Local).AddTicks(6790) });
        }
    }
}
