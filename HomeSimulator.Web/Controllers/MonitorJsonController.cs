using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using HomeSimulator.Web.Models;
using HomeSimulator.Web.Models.Entities;
using HomeSimulator.Services;
namespace HomeSimulator.Web.Controllers;

public class MonitorJsonController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly MeterService _meterService;

    public MonitorJsonController(ILogger<HomeController> logger, MeterService meterService)
    {
        _logger = logger;
        _meterService = meterService;
    }

    public IActionResult Index()
    {
        var dt = _meterService.GetMeterDataFromDB();
        return Content(Newtonsoft.Json.JsonConvert.SerializeObject(dt), "text/json");
    }

}
