using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeSimulator.Web.Migrations
{
    public partial class setinitdata : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Chargers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "IdTag", "MaxEnergy", "MaxPower" },
                values: new object[] { "9bcaece2", 50m, 10000m });

            migrationBuilder.UpdateData(
                table: "Configs",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "AccessToken", "ModifyTime" },
                values: new object[] { "852a04d4197a48129ae46576e46db783", new DateTime(2023, 8, 8, 7, 28, 38, 633, DateTimeKind.Local).AddTicks(3652) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Chargers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "IdTag", "MaxEnergy", "MaxPower" },
                values: new object[] { "ad4db7c3", 70m, 70000m });

            migrationBuilder.UpdateData(
                table: "Configs",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "AccessToken", "ModifyTime" },
                values: new object[] { "8d752795b1f94e918f0cb847ced36091", new DateTime(2023, 8, 7, 17, 36, 25, 656, DateTimeKind.Local).AddTicks(2295) });
        }
    }
}
