using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeSimulator.Web.Migrations
{
    public partial class Addinvertersource : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RatedPower",
                table: "Inverters");

            migrationBuilder.AddColumn<string>(
                name: "SourceConfigJson",
                table: "Inverters",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SourceType",
                table: "Inverters",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Configs",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "AccessToken", "ModifyTime" },
                values: new object[] { "40566105289246c1a7abc18072dc17ad", new DateTime(2022, 10, 20, 11, 50, 26, 538, DateTimeKind.Local).AddTicks(6790) });

            migrationBuilder.UpdateData(
                table: "Inverters",
                keyColumn: "Id",
                keyValue: 1L,
                column: "SourceConfigJson",
                value: "{\"RatedPower\":4500.0,\"LocationType\":0,\"MinHour\":10.0,\"MaxHour\":16.0}");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SourceConfigJson",
                table: "Inverters");

            migrationBuilder.DropColumn(
                name: "SourceType",
                table: "Inverters");

            migrationBuilder.AddColumn<decimal>(
                name: "RatedPower",
                table: "Inverters",
                type: "decimal(10,1)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Configs",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "AccessToken", "ModifyTime" },
                values: new object[] { "0ba28e5cd6cf467b9a7c4752a137c7f0", new DateTime(2022, 10, 17, 13, 2, 32, 725, DateTimeKind.Local).AddTicks(4500) });

            migrationBuilder.UpdateData(
                table: "Inverters",
                keyColumn: "Id",
                keyValue: 1L,
                column: "RatedPower",
                value: 4500m);
        }
    }
}
