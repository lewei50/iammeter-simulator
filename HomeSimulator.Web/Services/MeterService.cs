namespace HomeSimulator.Services;
using HomeSimulator.Web.Models;
using HomeSimulator.Web.Models.DTO;
using HomeSimulator.Web.Models.Entities;
using HomeSimulator.Web.Extensions;
public class MeterService
{
    private readonly MyContext _myContext;

    private readonly InverterService _inverterService;
    private readonly LoadService _loadService;
    public MeterService(MyContext myContext, InverterService inverterService, LoadService loadService)
    {
        _myContext = myContext;
        _inverterService = inverterService;
        _loadService = loadService;
    }


    public void SetMeterData(DateTime? time = null)
    {
        if (time == null)
            time = DateTime.Now;
        _inverterService.SetInverterMeterData(time);
        _loadService.SetLoadMeterData(time);
    }
    public MeterUploadDTO GetMeterDataFromDB()
    {
        var rt = new MeterUploadDTO();
        var config = _myContext.Configs.OrderBy(o => o.Id).First();
        var a = _myContext.Meters.OrderBy(o => o.Id).First(o => o.Name == "A");
        var b = _myContext.Meters.OrderBy(o => o.Id).First(o => o.Name == "B");
        var c = _myContext.Meters.OrderBy(o => o.Id).First(o => o.Name == "C");
        rt.version = "homesimulator";
        rt.SN = config.SN;
        rt.mac=$"B0F1923A38F1";
        var six = 49.99m;
        var seven = 0.94m;
        rt.Datas = new decimal?[][]{
                new decimal?[]{a.Voltage.ToDecimalNum(1),a.Current.ToDecimalNum(1),a.Power.ToDecimalNum(1),a.Energy.ToDecimalNum(),a.ReverseEnergy.ToDecimalNum(),six,seven},
                new decimal?[]{b.Voltage.ToDecimalNum(1),b.Current.ToDecimalNum(1),b.Power.ToDecimalNum(1),b.Energy.ToDecimalNum(),b.ReverseEnergy.ToDecimalNum(),six,seven},
                new decimal?[]{c.Voltage.ToDecimalNum(1),c.Current.ToDecimalNum(1),c.Power.ToDecimalNum(1),c.Energy.ToDecimalNum(),c.ReverseEnergy.ToDecimalNum(),six,seven},
                
        };
        return rt;

    }
}