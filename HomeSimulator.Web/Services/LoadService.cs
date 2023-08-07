namespace HomeSimulator.Services;
using HomeSimulator.Web.Models;
using HomeSimulator.Web.Models.DTO;
using HomeSimulator.Web.Models.Entities;
using HomeSimulator.Web.Helpers;
using HomeSimulator.Web.Extensions;
using Microsoft.EntityFrameworkCore;
public class LoadService
{
    private readonly MyContext _myContext;
    private readonly OCPPSocketBackgroundService _oCPPSocketBackgroundService;

    public LoadService(MyContext myContext, OCPPSocketBackgroundService oCPPSocketBackgroundService)
    {
        _myContext = myContext;
        _oCPPSocketBackgroundService = oCPPSocketBackgroundService;
    }

    public Meter SetLoadMeterData(DateTime? time = null)
    {
        if (time == null) time = DateTime.Now;
        var loadList = _myContext.Loads.Include(o => o.RunTimeDetails).ToList();
        foreach (var load in loadList)
        {
            SetLoad(load, time.Value);
        }
        var gridMeter = _myContext.Meters.OrderBy(o => o.Id).First(o => o.Name == "B");
        var inverterMeter = _myContext.Meters.OrderBy(o => o.Id).First(o => o.Name == "A");

        var loadMeter = _myContext.Meters.OrderBy(o => o.Id).First(o => o.Name == "C");
        var specialLoadPower = loadList.Where(o => o.Status == true && o.ToMeter == true).Sum(o => o.LastPower);
        SetSpecialLoadData(loadMeter, specialLoadPower, time);

        var loadPower = loadList.Where(o => o.Status == true).Sum(o => o.LastPower);
        if(_oCPPSocketBackgroundService.Charger.IsCharging)
            loadPower+=_oCPPSocketBackgroundService.Charger.Power;
        var inverterPower = inverterMeter.Power ?? 0;
        var power = (loadPower ?? 0) - inverterPower;
        var data = new Meter
        {
            Voltage = 230m,
            Current = power / 230m,
            Power = power
        };
        if (gridMeter.LastUpdateTime == null)
        {
            data.Energy = 0;
            data.ReverseEnergy = 0;
        }
        else
        {
            var lastPower = gridMeter.Power ?? 0;
            var timeValue = Math.Abs((decimal)(time.Value - gridMeter.LastUpdateTime.Value).TotalSeconds / 3600000);
            if (lastPower >= 0)
            {
                if (power >= 0)
                {
                    // add energy
                    var energy = 0.5m * (power + lastPower) * timeValue;
                    data.Energy = gridMeter.Energy + energy;
                    data.ReverseEnergy = gridMeter.ReverseEnergy;
                }
                else
                {
                    // both energy -- reverseenergy
                    var x = Math.Abs(lastPower);
                    var y = Math.Abs(power);
                    var energy = 0.5m * x * timeValue * x / (x + y);
                    var renergy = 0.5m * y * timeValue * y / (x + y);
                    data.Energy = gridMeter.Energy + energy;
                    data.ReverseEnergy = gridMeter.ReverseEnergy + renergy;
                }
            }
            else
            {
                if (power >= 0)
                {
                    // both reverse energy -- energy
                    var x = Math.Abs(lastPower);
                    var y = Math.Abs(power);
                    var renergy = 0.5m * x * timeValue * x / (x + y);
                    var energy = 0.5m * y * timeValue * y / (x + y);
                    data.Energy = gridMeter.Energy + energy;
                    data.ReverseEnergy = gridMeter.ReverseEnergy + renergy;
                }
                else
                {
                    // add reverse energy
                    var energy = Math.Abs(0.5m * (power + lastPower) * timeValue);
                    data.ReverseEnergy = gridMeter.ReverseEnergy + energy;
                    data.Energy = gridMeter.Energy;
                }
            }
        }
        gridMeter.Voltage = data.Voltage;
        gridMeter.Current = data.Current;
        gridMeter.Power = data.Power;
        gridMeter.Energy = data.Energy;
        gridMeter.ReverseEnergy = data.ReverseEnergy;
        gridMeter.LastUpdateTime = time;
        _myContext.SaveChanges();
        return data;
    }


    // to meter
    public void SetSpecialLoadData(Meter meter, decimal? power, DateTime? time)
    {
        if (time == null) time = DateTime.Now;

        if (meter.LastUpdateTime == null)
        {
            meter.Energy = 0;
        }
        else
        {
            var energy = 0.5m * (power + meter.Power) * (decimal)(time.Value - meter.LastUpdateTime.Value).TotalSeconds / 3600000;
            if (energy < 0) energy = 0;
            meter.Energy = (meter.Energy ?? 0) + energy;
        }

        meter.Voltage = 230;
        meter.Power = power;
        meter.ReverseEnergy = 0;
        meter.Current = power / 230;
        meter.LastUpdateTime = time;
    }

    public void SetLoad(Load load, DateTime time)
    {
        if (load.RunMode == (int)RunModeEnums.Manual)
        {
            load.SetStatus(load.Status, time);
            // if (load.Status == true)
            // {
            //     load.LastPower = CommonHelper.GetRandomNumber(load.MinPower ?? 0, load.MaxPower ?? 0);
            //     load.LastUpdateTime = time;
            // }
        }
        else
        {
            var timeSecond1 = time.TodaySeconds();
            var timeSecond2 = timeSecond1 + 3600 * 24;
            var details = load.GetValidDetails().Data;
            if (details != null)
            {
                var detail = details.FirstOrDefault(o => (timeSecond1 >= o.Start * 60 && timeSecond1 < o.End * 60));
                if (detail != null)
                {
                    GetPowerFromDetail(load, detail, timeSecond1, time);
                }
                else
                {
                    details.FirstOrDefault(o => (timeSecond2 >= o.Start * 60 && timeSecond2 < o.End * 60));
                    if (detail != null)
                    {
                        GetPowerFromDetail(load, detail, timeSecond2, time);
                    }
                    else
                    {
                        // not run
                        load.SetStatus(false, time);
                        //load.Status = false;
                    }
                }
            }
        }
    }

    public void GetPowerFromDetail(Load load, RunTimeDetailDTO detail, int timeSecond, DateTime time)
    {
        // random time area for date
        // eg. minite 100 - 200 ,2022/10/22 , year*day+year+month+day  % (200-100) +100
        var random1 = (time.Year * time.Day + time.Year + time.Month + time.Day);
        var random2 = (time.Month * time.Day + time.Year + time.Month + time.Day);
        var areaMinutes = detail.MinMinutes;
        if (detail.MaxMinutes > detail.MinMinutes)
            areaMinutes = random1 % (detail.MaxMinutes - detail.MinMinutes) + detail.MinMinutes;
        var leftMinute = detail.End - detail.Start - areaMinutes;

        var minMinute = detail.Start;
        if (leftMinute > 0)
        {
            minMinute = random2 % leftMinute + detail.Start;
        }
        var maxMinute = minMinute + areaMinutes;

        if (timeSecond >= minMinute * 60 && timeSecond <= maxMinute * 60)
        {
            // in the area set status true
            load.SetStatus(true, time);
            //load.Status = true;
            //load.LastPower = CommonHelper.GetRandomNumber(load.MinPower ?? 0, load.MaxPower ?? 0);
        }
        else
        {
            load.Status = false;
        }

    }
}