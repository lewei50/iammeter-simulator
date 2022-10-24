namespace HomeSimulator.Services;
using HomeSimulator.Web.Models;
using HomeSimulator.Web.Models.DTO;
using HomeSimulator.Web.Models.Entities;
using HomeSimulator.Web.Helpers;
using Newtonsoft.Json;
public class InverterService
{
    private readonly ILogger<InverterService> _logger;
    private readonly MyContext _myContext;
    public InverterService(ILogger<InverterService> logger, MyContext myContext)
    {
        _myContext = myContext;
        _logger = logger;

        inverter = _myContext.Inverters.First();
        meter = _myContext.Meters.First(o => o.Name == "A");
    }
    Meter meter;
    Inverter inverter;
    public Meter? SetInverterMeterData(DateTime? time = null)
    {
        if (time == null) time = DateTime.Now;
        Meter? data = null;
        if (inverter.Status == true)
        {
            var sourceType = (SourceTypeEnums)inverter.SourceType;
            if (sourceType == SourceTypeEnums.Simulation)
            {
                data = SetInverterSimulationData(time);
            }
            else if (sourceType == SourceTypeEnums.IAMMETERCloud)
            {
                data = SetInverterCloudData();
            }
            else
            {
                // local
                data = SetInverterLocalData();
            }

            // save
            if (data != null && data.Power != null)
            {
                meter.Voltage = data.Voltage;
                meter.Current = data.Current;
                meter.Power = data.Power;
                meter.Energy = data.Energy;
                meter.ReverseEnergy = data.ReverseEnergy;
                meter.LastUpdateTime = time;
                _myContext.SaveChanges();
            }
        }

        return data;
    }

    public Meter? GetFromDecimalArray(decimal?[] data)
    {
        if (data.Length >= 5)
        {
            return new Meter
            {
                Voltage = data[0],
                Current = data[1],
                Power = data[2],
                Energy = data[3],
                ReverseEnergy = data[4],
            };
        }
        else return null;
    }

    public Meter? SetInverterCloudData()
    {
        var minSeconds = 55;
        // retrieve every minite
        if (meter.LastUpdateTime == null || ((DateTime.Now - meter.LastUpdateTime.Value).TotalSeconds) > minSeconds)
        {
            var config = JsonConvert.DeserializeObject<InverterIAMMETERCloudDTO>(inverter.SourceConfigJson ?? "");
            var rt = IammeterCloudHelper.GetCurrentData(config.SN).Result;
            if (rt.Successful && rt.Data != null)
            {
                decimal?[] data = new decimal?[] { };
                if (rt.Data.Data != null)
                    data = rt.Data.Data;
                if (rt.Data.Datas != null && rt.Data.Datas.Length > config.DataIndex)
                    data = rt.Data.Datas[config.DataIndex];

                return GetFromDecimalArray(data);

            }
            else
                _logger.LogWarning($"SetInverterCloudData error:{rt.Message}");
        }
        return null;
    }

    public Meter? SetInverterLocalData()
    {
        var config = JsonConvert.DeserializeObject<InverterIAMMETERLocalDTO>(inverter.SourceConfigJson ?? "");
        var rt = IammeterLocalHelper.GetCurrentData(config.IPAddress).Result;
        if (rt.Successful && rt.Data != null)
        {
            decimal?[] data = new decimal?[] { };
            if (rt.Data.Data != null)
                data = rt.Data.Data;
            if (rt.Data.Datas != null && rt.Data.Datas.Length > config.DataIndex)
                data = rt.Data.Datas[config.DataIndex];

            return GetFromDecimalArray(data);

        }
        else
            _logger.LogWarning($"SetInverterLocalData error:{rt.Message}");
        return null;
    }

    public Meter SetInverterSimulationData(DateTime? time = null)
    {
        if (time == null) time = DateTime.Now;
        var config = JsonConvert.DeserializeObject<InverterSimulatorDTO>(inverter.SourceConfigJson ?? "");
        var power = config.RatedPower * GetPowerForATime(time.Value) * CommonHelper.GetRandomNumber(0.95m, 1m);
        var data = new Meter()
        {
            Voltage = 230,
            Current = power / 230,
            Power = power,
            ReverseEnergy = 0
        };
        if (meter.LastUpdateTime == null)
        {
            data.Energy = 0;
        }
        else
        {
            var energy = 0.5m * (power + meter.Power) * (decimal)(time.Value - meter.LastUpdateTime.Value).TotalSeconds / 3600000;
            if (energy < 0) energy = 0;
            data.Energy = (meter.Energy ?? 0) + energy;
        }


        return data;
    }

    // public decimal GetMeterPower(DateTime time)
    // {
    //     var inverter = _myContext.Inverters.First();
    //     var power = (inverter.RatedPower ?? 0) * GetPowerForATime(time);
    //     return power;
    // }

    public decimal GetPowerForATime(DateTime time)
    {
        var config = JsonConvert.DeserializeObject<InverterSimulatorDTO>(inverter.SourceConfigJson ?? "");

        // hour:9-15,max date: 06-21
        var year = time.Year;

        // default north
        var maxDate = DateTime.Parse($"{year}-06-21");
        if (config.LocationType == LocationTypeEnums.Southern)
        {
            maxDate = DateTime.Parse($"{year}-12-21");
        }
        var days = Math.Abs((maxDate - time).TotalDays);
        var sunSeconds = 3600 * (config.MaxHour - (config.MaxHour - config.MinHour) * days / 183);

        var min = (3600 * 24 - sunSeconds) / 2;
        var max = 3600 * 24 - min;

        var date = time.Date;
        var seconds = (time - date).TotalSeconds;
        if (seconds < min || seconds > max)
            return 0;
        var x = seconds - min;
        var b = sunSeconds / 2;

        // solar efficiency 0.7-0.9
        decimal efficiency = (decimal)(0.9 - 0.2 * days / 183);

        // c 4500 - 7000
        var c = (7000 - 2500 * days / 183);
        return efficiency * FX(x, b, c);
    }

    public decimal FX(double x, double b, double c)
    {
        return (decimal)(Math.Pow(Math.E, -1 * Math.Pow(x - b, 2) / (2 * c * c)));
    }
}