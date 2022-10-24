using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using HomeSimulator.Web.Models;
using HomeSimulator.Services;
using HomeSimulator.Web.Models.Entities;
using System.Text;
namespace HomeSimulator.Web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly MyContext _myContext;
    private readonly InverterService _inverterService;
    private readonly LoadService _loadService;

    public HomeController(ILogger<HomeController> logger, MyContext myContext, InverterService inverterService, LoadService loadService)
    {
        _logger = logger;
        _myContext = myContext;
        _inverterService = inverterService;
        _loadService = loadService;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult IndexPartial()
    {
        return View();
    }

    public IActionResult Setting()
    {
        return View();
    }

    public IActionResult InverterSourceTypePartial(int sourceType)
    {
        var inverter = _myContext.Inverters.First();
        inverter.SourceType = sourceType;
        return View(inverter);
    }

    public IActionResult ApiHistory()
    {
        return View();
    }

    public IActionResult LoadListPartial()
    {
        var list = _myContext.Loads.ToList();
        return View(list);
    }

    public IActionResult LoadEditPartial(long? Id)
    {
        Load load = new Load() { };
        if (Id > 0)
        {
            load = _myContext.Loads.First(o => o.Id == Id);
        }
        return View(load);
    }


    public IActionResult test()
    {
        StringBuilder sb = new StringBuilder();
        var time = DateTime.Now.Date;
        var suntime = DateTime.Parse("2022-06-30");
        for (var i = 0; i <= 60 * 24; i++)
        {
            var now = time.AddMinutes(i);
            var dt = _inverterService.SetInverterMeterData(now);
            var dt2 = _loadService.SetLoadMeterData(now);
            var now2 = suntime.AddMinutes(i);
            //sb.AppendLine($"{now},{(5000* _inverterService.GetPowerForATime(now)).ToString()},{(5000* _inverterService.GetPowerForATime(now2)).ToString()}" );

            sb.AppendLine($"{now},{dt.Power},{dt.Energy},{dt2.Power},{dt2.Energy},{dt2.ReverseEnergy}");
        }
        return Content(sb.ToString());
    }



    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
