namespace HomeSimulator.Web.Models;
using HomeSimulator.Web.Models.Entities;
using Microsoft.EntityFrameworkCore;
using HomeSimulator.Web.Helpers;
using HomeSimulator.Web.Extensions;

public class MyContext : DbContext
{

    public MyContext(DbContextOptions options) : base(options)
    {

    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseLazyLoadingProxies();

        base.OnConfiguring(optionsBuilder);
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // relations
        modelBuilder.Entity<RunTimeDetail>().HasOne(o => o.Load).WithMany(x => x.RunTimeDetails).HasForeignKey(x => x.LoadId);


        // datas
        modelBuilder.Entity<Config>().HasData(new Config
        {
            Id = 1,
            SN = "NeedToBeSet",
            Username = "admin",
            Password = "admin".ToSHA256String(),
            AccessToken = CommonHelper.GetNewGuidString(),
            ModifyTime = DateTime.Now
        });

        modelBuilder.Entity<Inverter>().HasData(new Inverter
        {
            Id = 1,
            Name = "Inverter",
            SourceType = (int)SourceTypeEnums.Simulation,
            SourceConfigJson = Newtonsoft.Json.JsonConvert.SerializeObject(new DTO.InverterSimulatorDTO()),
            Status = true,
            LastPower = 0m,
            LastUpdateTime = null
        });

        modelBuilder.Entity<Meter>().HasData(new Meter { Id = 1, Name = "A" });
        modelBuilder.Entity<Meter>().HasData(new Meter { Id = 2, Name = "B" });
        modelBuilder.Entity<Meter>().HasData(new Meter { Id = 3, Name = "C" });

        modelBuilder.Entity<Load>().HasData(new Load
        {
            Id = 1,
            Name = "Bedroom lamp",
            MinPower = 48m,
            MaxPower = 48m,
            RunMode = (int)RunModeEnums.Timing,
            Status = true,
            SetMode = (int)SetModeEnums.Cannot,
            LastPower = null,
            LastUpdateTime = null,
        });

        modelBuilder.Entity<RunTimeDetail>().HasData(new RunTimeDetail { Id = 11, LoadId = 1, Start = "06:00", End = "09:00", MinMinutes = 50, MaxMinutes = 100 });
        modelBuilder.Entity<RunTimeDetail>().HasData(new RunTimeDetail { Id = 12, LoadId = 1, Start = "16:00", End = "22:00", MinMinutes = 100, MaxMinutes = 180 });

        modelBuilder.Entity<Load>().HasData(new Load
        {
            Id = 2,
            Name = "Living room lights",
            MinPower = 102m,
            MaxPower = 102m,
            RunMode = (int)RunModeEnums.Timing,
            Status = false,
            SetMode = (int)SetModeEnums.Cannot,
            LastPower = null,
            LastUpdateTime = null
        });
        modelBuilder.Entity<RunTimeDetail>().HasData(new RunTimeDetail { Id = 21, LoadId = 2, Start = "07:00", End = "10:00", MinMinutes = 20, MaxMinutes = 80 });
        modelBuilder.Entity<RunTimeDetail>().HasData(new RunTimeDetail { Id = 22, LoadId = 2, Start = "16:00", End = "23:00", MinMinutes = 120, MaxMinutes = 280 });

        modelBuilder.Entity<Load>().HasData(new Load
        {
            Id = 3,
            Name = "Air conditioner",
            MinPower = 1000m,
            MaxPower = 1000m,
            RunMode = (int)RunModeEnums.Timing,
            Status = false,
            SetMode = (int)SetModeEnums.Cannot,
            LastPower = null,
            LastUpdateTime = null
        });
        modelBuilder.Entity<RunTimeDetail>().HasData(new RunTimeDetail { Id = 31, LoadId = 3, Start = "10:00", End = "14:00", MinMinutes = 220, MaxMinutes = 240 });
        modelBuilder.Entity<RunTimeDetail>().HasData(new RunTimeDetail { Id = 32, LoadId = 3, Start = "16:00", End = "00:00", MinMinutes = 300, MaxMinutes = 400 });


        modelBuilder.Entity<Load>().HasData(new Load
        {
            Id = 4,
            Name = "Water heater",
            MinPower = 1500m,
            MaxPower = 1500m,
            RunMode = (int)RunModeEnums.Manual,
            Status = false,
            SetMode = (int)SetModeEnums.Cannot,
            LastPower = null,
            LastUpdateTime = null
        });

        modelBuilder.Entity<Load>().HasData(new Load
        {
            Id = 5,
            Name = "TV",
            MinPower = 300m,
            MaxPower = 300m,
            RunMode = (int)RunModeEnums.Timing,
            Status = false,
            SetMode = (int)SetModeEnums.Cannot,
            LastPower = null,
            LastUpdateTime = null
        });
        modelBuilder.Entity<RunTimeDetail>().HasData(new RunTimeDetail { Id = 51, LoadId = 5, Start = "18:00", End = "23:00", MinMinutes = 150, MaxMinutes = 200 });

        modelBuilder.Entity<Load>().HasData(new Load
        {
            Id = 6,
            Name = "Tesla",
            MinPower = 0m,
            MaxPower = 20000m,
            RunMode = (int)RunModeEnums.Manual,
            Status = false,
            SetMode = (int)SetModeEnums.Configurable,
            LastPower = 2300,
            ToMeter = true,
            LastUpdateTime = null
        });
    }

    public DbSet<Config> Configs { get; set; }
    public DbSet<ApiHistory> ApiHistory { get; set; }
    public DbSet<Inverter> Inverters { get; set; }
    public DbSet<Load> Loads { get; set; }
    public DbSet<RunTimeDetail> RunTimeDetails { get; set; }
    public DbSet<Meter> Meters { get; set; }

}
