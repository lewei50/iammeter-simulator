using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeSimulator.Web.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ApiHistory",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    Data = table.Column<string>(type: "TEXT", nullable: true),
                    Result = table.Column<bool>(type: "INTEGER", nullable: false),
                    Message = table.Column<string>(type: "TEXT", nullable: true),
                    IPAddress = table.Column<string>(type: "TEXT", nullable: true),
                    AddTime = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApiHistory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Configs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SN = table.Column<string>(type: "TEXT", nullable: true),
                    Username = table.Column<string>(type: "TEXT", nullable: true),
                    Password = table.Column<string>(type: "TEXT", nullable: true),
                    AccessToken = table.Column<string>(type: "TEXT", nullable: true),
                    ModifyTime = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Configs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Inverters",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    RatedPower = table.Column<decimal>(type: "decimal(10,1)", nullable: true),
                    Status = table.Column<bool>(type: "INTEGER", nullable: false),
                    LastPower = table.Column<decimal>(type: "decimal(10,1)", nullable: true),
                    LastUpdateTime = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inverters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Loads",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    MinPower = table.Column<decimal>(type: "decimal(10,1)", nullable: true),
                    MaxPower = table.Column<decimal>(type: "decimal(10,1)", nullable: true),
                    RunMode = table.Column<int>(type: "INTEGER", nullable: false),
                    Status = table.Column<bool>(type: "INTEGER", nullable: false),
                    SetMode = table.Column<int>(type: "INTEGER", nullable: false),
                    LastPower = table.Column<decimal>(type: "decimal(10,1)", nullable: true),
                    LastUpdateTime = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Loads", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Meters",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    Voltage = table.Column<decimal>(type: "decimal(10,1)", nullable: true),
                    Current = table.Column<decimal>(type: "decimal(10,1)", nullable: true),
                    Power = table.Column<decimal>(type: "decimal(10,1)", nullable: true),
                    Energy = table.Column<decimal>(type: "decimal(14,6)", nullable: true),
                    ReverseEnergy = table.Column<decimal>(type: "decimal(14,6)", nullable: true),
                    LastUpdateTime = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Meters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RunTimeDetails",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    LoadId = table.Column<long>(type: "INTEGER", nullable: false),
                    Start = table.Column<string>(type: "TEXT", nullable: true),
                    End = table.Column<string>(type: "TEXT", nullable: true),
                    MinMinutes = table.Column<int>(type: "INTEGER", nullable: false),
                    MaxMinutes = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RunTimeDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RunTimeDetails_Loads_LoadId",
                        column: x => x.LoadId,
                        principalTable: "Loads",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Configs",
                columns: new[] { "Id", "AccessToken", "ModifyTime", "Password", "SN", "Username" },
                values: new object[] { 1L, "0ba28e5cd6cf467b9a7c4752a137c7f0", new DateTime(2022, 10, 17, 13, 2, 32, 725, DateTimeKind.Local).AddTicks(4500), "jGl25bVBBBW96Qi9Te4V37Fnqchz/Eu4qB9vKrRIqRg=", "NeedToBeSet", "admin" });

            migrationBuilder.InsertData(
                table: "Inverters",
                columns: new[] { "Id", "LastPower", "LastUpdateTime", "Name", "RatedPower", "Status" },
                values: new object[] { 1L, 0m, null, "Inverter", 4500m, true });

            migrationBuilder.InsertData(
                table: "Loads",
                columns: new[] { "Id", "LastPower", "LastUpdateTime", "MaxPower", "MinPower", "Name", "RunMode", "SetMode", "Status" },
                values: new object[] { 1L, null, null, 48m, 48m, "Bedroom lamp", 1, 0, true });

            migrationBuilder.InsertData(
                table: "Loads",
                columns: new[] { "Id", "LastPower", "LastUpdateTime", "MaxPower", "MinPower", "Name", "RunMode", "SetMode", "Status" },
                values: new object[] { 2L, null, null, 102m, 102m, "Living room lights", 1, 0, false });

            migrationBuilder.InsertData(
                table: "Loads",
                columns: new[] { "Id", "LastPower", "LastUpdateTime", "MaxPower", "MinPower", "Name", "RunMode", "SetMode", "Status" },
                values: new object[] { 3L, null, null, 1000m, 1000m, "Air conditioner", 1, 0, false });

            migrationBuilder.InsertData(
                table: "Loads",
                columns: new[] { "Id", "LastPower", "LastUpdateTime", "MaxPower", "MinPower", "Name", "RunMode", "SetMode", "Status" },
                values: new object[] { 4L, null, null, 1500m, 1500m, "Water heater", 0, 0, false });

            migrationBuilder.InsertData(
                table: "Loads",
                columns: new[] { "Id", "LastPower", "LastUpdateTime", "MaxPower", "MinPower", "Name", "RunMode", "SetMode", "Status" },
                values: new object[] { 5L, null, null, 300m, 300m, "TV", 1, 0, false });

            migrationBuilder.InsertData(
                table: "Loads",
                columns: new[] { "Id", "LastPower", "LastUpdateTime", "MaxPower", "MinPower", "Name", "RunMode", "SetMode", "Status" },
                values: new object[] { 6L, 2300m, null, 20000m, 0m, "Tesla", 0, 1, false });

            migrationBuilder.InsertData(
                table: "Meters",
                columns: new[] { "Id", "Current", "Energy", "LastUpdateTime", "Name", "Power", "ReverseEnergy", "Voltage" },
                values: new object[] { 1L, null, null, null, "A", null, null, null });

            migrationBuilder.InsertData(
                table: "Meters",
                columns: new[] { "Id", "Current", "Energy", "LastUpdateTime", "Name", "Power", "ReverseEnergy", "Voltage" },
                values: new object[] { 2L, null, null, null, "B", null, null, null });

            migrationBuilder.InsertData(
                table: "RunTimeDetails",
                columns: new[] { "Id", "End", "LoadId", "MaxMinutes", "MinMinutes", "Start" },
                values: new object[] { 11L, "09:00", 1L, 100, 50, "06:00" });

            migrationBuilder.InsertData(
                table: "RunTimeDetails",
                columns: new[] { "Id", "End", "LoadId", "MaxMinutes", "MinMinutes", "Start" },
                values: new object[] { 12L, "22:00", 1L, 180, 100, "16:00" });

            migrationBuilder.InsertData(
                table: "RunTimeDetails",
                columns: new[] { "Id", "End", "LoadId", "MaxMinutes", "MinMinutes", "Start" },
                values: new object[] { 21L, "10:00", 2L, 80, 20, "07:00" });

            migrationBuilder.InsertData(
                table: "RunTimeDetails",
                columns: new[] { "Id", "End", "LoadId", "MaxMinutes", "MinMinutes", "Start" },
                values: new object[] { 22L, "23:00", 2L, 280, 120, "16:00" });

            migrationBuilder.InsertData(
                table: "RunTimeDetails",
                columns: new[] { "Id", "End", "LoadId", "MaxMinutes", "MinMinutes", "Start" },
                values: new object[] { 31L, "14:00", 3L, 240, 220, "10:00" });

            migrationBuilder.InsertData(
                table: "RunTimeDetails",
                columns: new[] { "Id", "End", "LoadId", "MaxMinutes", "MinMinutes", "Start" },
                values: new object[] { 32L, "00:00", 3L, 400, 300, "16:00" });

            migrationBuilder.InsertData(
                table: "RunTimeDetails",
                columns: new[] { "Id", "End", "LoadId", "MaxMinutes", "MinMinutes", "Start" },
                values: new object[] { 51L, "23:00", 5L, 200, 150, "18:00" });

            migrationBuilder.CreateIndex(
                name: "IX_RunTimeDetails_LoadId",
                table: "RunTimeDetails",
                column: "LoadId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApiHistory");

            migrationBuilder.DropTable(
                name: "Configs");

            migrationBuilder.DropTable(
                name: "Inverters");

            migrationBuilder.DropTable(
                name: "Meters");

            migrationBuilder.DropTable(
                name: "RunTimeDetails");

            migrationBuilder.DropTable(
                name: "Loads");
        }
    }
}
