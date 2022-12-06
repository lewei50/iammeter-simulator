using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeSimulator.Web.Migrations
{
    public partial class addphasec : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Configs",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "AccessToken", "ModifyTime" },
                values: new object[] { "65553614ffe446c6a48754f78a2eddd2", new DateTime(2022, 12, 6, 9, 40, 27, 950, DateTimeKind.Local).AddTicks(5330) });

            migrationBuilder.InsertData(
                table: "Meters",
                columns: new[] { "Id", "Current", "Energy", "LastUpdateTime", "Name", "Power", "ReverseEnergy", "Voltage" },
                values: new object[] { 3L, null, null, null, "C", null, null, null });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Meters",
                keyColumn: "Id",
                keyValue: 3L);

            migrationBuilder.UpdateData(
                table: "Configs",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "AccessToken", "ModifyTime" },
                values: new object[] { "4fec0ff0e0054a8b9dc29cbe65966a73", new DateTime(2022, 12, 6, 9, 22, 25, 776, DateTimeKind.Local).AddTicks(5950) });
        }
    }
}
