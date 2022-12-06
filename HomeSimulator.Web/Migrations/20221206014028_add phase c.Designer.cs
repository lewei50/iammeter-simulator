﻿// <auto-generated />
using System;
using HomeSimulator.Web.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace HomeSimulator.Web.Migrations
{
    [DbContext(typeof(MyContext))]
    [Migration("20221206014028_add phase c")]
    partial class addphasec
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.10");

            modelBuilder.Entity("HomeSimulator.Web.Models.Entities.ApiHistory", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("AddTime")
                        .HasColumnType("datetime");

                    b.Property<string>("Data")
                        .HasColumnType("TEXT");

                    b.Property<string>("IPAddress")
                        .HasColumnType("TEXT");

                    b.Property<string>("Message")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<bool>("Result")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("ApiHistory");
                });

            modelBuilder.Entity("HomeSimulator.Web.Models.Entities.Config", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("AccessToken")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("ModifyTime")
                        .HasColumnType("datetime");

                    b.Property<string>("Password")
                        .HasColumnType("TEXT");

                    b.Property<string>("SN")
                        .HasColumnType("TEXT");

                    b.Property<string>("Username")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Configs");

                    b.HasData(
                        new
                        {
                            Id = 1L,
                            AccessToken = "65553614ffe446c6a48754f78a2eddd2",
                            ModifyTime = new DateTime(2022, 12, 6, 9, 40, 27, 950, DateTimeKind.Local).AddTicks(5330),
                            Password = "jGl25bVBBBW96Qi9Te4V37Fnqchz/Eu4qB9vKrRIqRg=",
                            SN = "NeedToBeSet",
                            Username = "admin"
                        });
                });

            modelBuilder.Entity("HomeSimulator.Web.Models.Entities.Inverter", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<decimal?>("LastPower")
                        .HasColumnType("decimal(10,1)");

                    b.Property<DateTime?>("LastUpdateTime")
                        .HasColumnType("datetime");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<string>("SourceConfigJson")
                        .HasColumnType("TEXT");

                    b.Property<int>("SourceType")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("Status")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Inverters");

                    b.HasData(
                        new
                        {
                            Id = 1L,
                            LastPower = 0m,
                            Name = "Inverter",
                            SourceConfigJson = "{\"RatedPower\":4500.0,\"LocationType\":0,\"MinHour\":10.0,\"MaxHour\":16.0}",
                            SourceType = 0,
                            Status = true
                        });
                });

            modelBuilder.Entity("HomeSimulator.Web.Models.Entities.Load", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<decimal?>("LastPower")
                        .HasColumnType("decimal(10,1)");

                    b.Property<DateTime?>("LastUpdateTime")
                        .HasColumnType("datetime");

                    b.Property<decimal?>("MaxPower")
                        .HasColumnType("decimal(10,1)");

                    b.Property<decimal?>("MinPower")
                        .HasColumnType("decimal(10,1)");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<int>("RunMode")
                        .HasColumnType("INTEGER");

                    b.Property<int>("SetMode")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("Status")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("ToMeter")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Loads");

                    b.HasData(
                        new
                        {
                            Id = 1L,
                            MaxPower = 48m,
                            MinPower = 48m,
                            Name = "Bedroom lamp",
                            RunMode = 1,
                            SetMode = 0,
                            Status = true,
                            ToMeter = false
                        },
                        new
                        {
                            Id = 2L,
                            MaxPower = 102m,
                            MinPower = 102m,
                            Name = "Living room lights",
                            RunMode = 1,
                            SetMode = 0,
                            Status = false,
                            ToMeter = false
                        },
                        new
                        {
                            Id = 3L,
                            MaxPower = 1000m,
                            MinPower = 1000m,
                            Name = "Air conditioner",
                            RunMode = 1,
                            SetMode = 0,
                            Status = false,
                            ToMeter = false
                        },
                        new
                        {
                            Id = 4L,
                            MaxPower = 1500m,
                            MinPower = 1500m,
                            Name = "Water heater",
                            RunMode = 0,
                            SetMode = 0,
                            Status = false,
                            ToMeter = false
                        },
                        new
                        {
                            Id = 5L,
                            MaxPower = 300m,
                            MinPower = 300m,
                            Name = "TV",
                            RunMode = 1,
                            SetMode = 0,
                            Status = false,
                            ToMeter = false
                        },
                        new
                        {
                            Id = 6L,
                            LastPower = 2300m,
                            MaxPower = 20000m,
                            MinPower = 0m,
                            Name = "Tesla",
                            RunMode = 0,
                            SetMode = 1,
                            Status = false,
                            ToMeter = true
                        });
                });

            modelBuilder.Entity("HomeSimulator.Web.Models.Entities.Meter", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<decimal?>("Current")
                        .HasColumnType("decimal(10,1)");

                    b.Property<decimal?>("Energy")
                        .HasColumnType("decimal(14,6)");

                    b.Property<DateTime?>("LastUpdateTime")
                        .HasColumnType("datetime");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<decimal?>("Power")
                        .HasColumnType("decimal(10,1)");

                    b.Property<decimal?>("ReverseEnergy")
                        .HasColumnType("decimal(14,6)");

                    b.Property<decimal?>("Voltage")
                        .HasColumnType("decimal(10,1)");

                    b.HasKey("Id");

                    b.ToTable("Meters");

                    b.HasData(
                        new
                        {
                            Id = 1L,
                            Name = "A"
                        },
                        new
                        {
                            Id = 2L,
                            Name = "B"
                        },
                        new
                        {
                            Id = 3L,
                            Name = "C"
                        });
                });

            modelBuilder.Entity("HomeSimulator.Web.Models.Entities.RunTimeDetail", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("End")
                        .HasColumnType("TEXT");

                    b.Property<long>("LoadId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("MaxMinutes")
                        .HasColumnType("INTEGER");

                    b.Property<int>("MinMinutes")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Start")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("LoadId");

                    b.ToTable("RunTimeDetails");

                    b.HasData(
                        new
                        {
                            Id = 11L,
                            End = "09:00",
                            LoadId = 1L,
                            MaxMinutes = 100,
                            MinMinutes = 50,
                            Start = "06:00"
                        },
                        new
                        {
                            Id = 12L,
                            End = "22:00",
                            LoadId = 1L,
                            MaxMinutes = 180,
                            MinMinutes = 100,
                            Start = "16:00"
                        },
                        new
                        {
                            Id = 21L,
                            End = "10:00",
                            LoadId = 2L,
                            MaxMinutes = 80,
                            MinMinutes = 20,
                            Start = "07:00"
                        },
                        new
                        {
                            Id = 22L,
                            End = "23:00",
                            LoadId = 2L,
                            MaxMinutes = 280,
                            MinMinutes = 120,
                            Start = "16:00"
                        },
                        new
                        {
                            Id = 31L,
                            End = "14:00",
                            LoadId = 3L,
                            MaxMinutes = 240,
                            MinMinutes = 220,
                            Start = "10:00"
                        },
                        new
                        {
                            Id = 32L,
                            End = "00:00",
                            LoadId = 3L,
                            MaxMinutes = 400,
                            MinMinutes = 300,
                            Start = "16:00"
                        },
                        new
                        {
                            Id = 51L,
                            End = "23:00",
                            LoadId = 5L,
                            MaxMinutes = 200,
                            MinMinutes = 150,
                            Start = "18:00"
                        });
                });

            modelBuilder.Entity("HomeSimulator.Web.Models.Entities.RunTimeDetail", b =>
                {
                    b.HasOne("HomeSimulator.Web.Models.Entities.Load", "Load")
                        .WithMany("RunTimeDetails")
                        .HasForeignKey("LoadId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Load");
                });

            modelBuilder.Entity("HomeSimulator.Web.Models.Entities.Load", b =>
                {
                    b.Navigation("RunTimeDetails");
                });
#pragma warning restore 612, 618
        }
    }
}
