using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeSimulator.Web.Migrations
{
    public partial class addcharger : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Chargers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MaxPower = table.Column<decimal>(type: "decimal(13,3)", nullable: true),
                    MaxEnergy = table.Column<decimal>(type: "decimal(13,3)", nullable: true),
                    OCPPServer = table.Column<string>(type: "TEXT", nullable: false),
                    ChargePointId = table.Column<string>(type: "TEXT", nullable: false),
                    IdTag = table.Column<string>(type: "TEXT", nullable: false),
                    LimitPower = table.Column<decimal>(type: "decimal(13,3)", nullable: true),
                    LimitPowerExpiredTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    LastUpdateTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    Voltage = table.Column<decimal>(type: "decimal(13,3)", nullable: true),
                    Current = table.Column<decimal>(type: "decimal(13,3)", nullable: true),
                    Power = table.Column<decimal>(type: "decimal(13,3)", nullable: true),
                    Energy = table.Column<decimal>(type: "decimal(13,3)", nullable: true),
                    SOC = table.Column<decimal>(type: "decimal(13,3)", nullable: true),
                    TransactionId = table.Column<int>(type: "INTEGER", nullable: false),
                    MeterStart = table.Column<int>(type: "INTEGER", nullable: true),
                    MeterStartTime = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    MeterStop = table.Column<int>(type: "INTEGER", nullable: true),
                    MeterStopTime = table.Column<DateTimeOffset>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chargers", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Chargers",
                columns: new[] { "Id", "ChargePointId", "Current", "Energy", "IdTag", "LastUpdateTime", "LimitPower", "LimitPowerExpiredTime", "MaxEnergy", "MaxPower", "MeterStart", "MeterStartTime", "MeterStop", "MeterStopTime", "OCPPServer", "Power", "SOC", "TransactionId", "Voltage" },
                values: new object[] { 1, "bc74c304657547b2b3b948b0449e23c8", null, null, "4e37400b", null, null, null, 70m, 70000m, null, null, null, null, "ws://ocpp.iammeter.com/ocpp", null, null, 0, null });

            migrationBuilder.UpdateData(
                table: "Configs",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "AccessToken", "ModifyTime" },
                values: new object[] { "b9be574521064c5488b1d392aa786546", new DateTime(2023, 8, 2, 10, 5, 36, 885, DateTimeKind.Local).AddTicks(6840) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Chargers");

            migrationBuilder.UpdateData(
                table: "Configs",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "AccessToken", "ModifyTime" },
                values: new object[] { "65553614ffe446c6a48754f78a2eddd2", new DateTime(2022, 12, 6, 9, 40, 27, 950, DateTimeKind.Local).AddTicks(5330) });
        }
    }
}
