using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeSimulator.Web.Migrations
{
    public partial class allownull : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "OCPPServer",
                table: "Chargers",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "IdTag",
                table: "Chargers",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "ChargePointId",
                table: "Chargers",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.UpdateData(
                table: "Chargers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ChargePointId", "IdTag" },
                values: new object[] { "", "ad4db7c3" });

            migrationBuilder.UpdateData(
                table: "Configs",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "AccessToken", "ModifyTime" },
                values: new object[] { "8d752795b1f94e918f0cb847ced36091", new DateTime(2023, 8, 7, 17, 36, 25, 656, DateTimeKind.Local).AddTicks(2295) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "OCPPServer",
                table: "Chargers",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "IdTag",
                table: "Chargers",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ChargePointId",
                table: "Chargers",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Chargers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ChargePointId", "IdTag" },
                values: new object[] { "bc74c304657547b2b3b948b0449e23c8", "4e37400b" });

            migrationBuilder.UpdateData(
                table: "Configs",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "AccessToken", "ModifyTime" },
                values: new object[] { "b9be574521064c5488b1d392aa786546", new DateTime(2023, 8, 2, 10, 5, 36, 885, DateTimeKind.Local).AddTicks(6840) });
        }
    }
}
