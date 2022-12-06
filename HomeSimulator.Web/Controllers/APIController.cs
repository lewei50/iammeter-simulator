using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using HomeSimulator.Web.Models;
using HomeSimulator.Web.Models.Entities;
using HomeSimulator.Services;
using HomeSimulator.Web.Extensions;

namespace HomeSimulator.Web.Controllers;


public class APIController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly MyContext _myContext;

    private readonly MeterService _meterService;

    public APIController(ILogger<HomeController> logger, MyContext myContext, MeterService meterService)
    {
        _logger = logger;
        _myContext = myContext;
        _meterService = meterService;
    }

    public IActionResult SetLoadPower(decimal? v)
    {
        var result = new Result();
        var load = _myContext.Loads.OrderBy(o => o.Id).FirstOrDefault(o => o.SetMode == 1);
        if (load == null)
        {
            result.Message = "No configurable loads.";
        }
        else
        {
            result = load.SetPower(v);
        }
        // apihistory
        var apihistory = new ApiHistory()
        {
            Name = nameof(SetLoadPower),
            Data = $"{v}",
            Result = result.Successful,
            Message = result.Message,
            AddTime = DateTime.Now,
            IPAddress = HttpContext.GetUserIp()
        };
        _myContext.ApiHistory.Add(apihistory);
        _myContext.SaveChanges();
        _meterService.SetMeterData();
        return Json(result);
    }

    public IActionResult GetSetPower()
    {
        var result = new Result<decimal>();
        var load = _myContext.Loads.OrderBy(o => o.Id).FirstOrDefault(o => o.SetMode == 1);
        if (load == null)
        {
            result.Message = "No configurable loads.";
        }
        else
        {
            result.Successful = true;
            if (load.Status == true)
                result.Data = load.LastPower ?? 0;
            else
                result.Data = 0;
        }
        return Json(result);
    }
}
