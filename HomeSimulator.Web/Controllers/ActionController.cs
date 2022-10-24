using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using HomeSimulator.Web.Models;
using HomeSimulator.Web.Models.Entities;
using HomeSimulator.Web.Models.DTO;
using HomeSimulator.Services;
using Newtonsoft.Json;
using HomeSimulator.Web.Extensions;
using Microsoft.EntityFrameworkCore;
namespace HomeSimulator.Web.Controllers;

public class ActionController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly MyContext _myContext;
    private readonly MeterService _meterService;

    public ActionController(ILogger<HomeController> logger, MyContext myContext, MeterService meterService)
    {
        _logger = logger;
        _myContext = myContext;
        _meterService = meterService;
    }

    public ActionResult Config(Config data, string password2)
    {
        var result = new Result();
        var entity = _myContext.Configs.FirstOrDefault();
        if (data.Password != password2)
            return Json(result.Return("Invalid password"));
        if (entity == null)
        {
            result.Message = "Invalid data: config";
        }
        else
        {
            //TryUpdateModelAsync(entity);
            entity.Username = data.Username;
            if (string.IsNullOrEmpty(data.Password) == false)
                entity.Password = data.Password.ToSHA256String();
            entity.SN = data.SN;

            entity.ModifyTime = DateTime.Now;
            _myContext.SaveChanges();
            result.Successful = true;
            result.Message = "Successfully saved";
        }
        return Json(result);
    }

    public ActionResult Inverter(Inverter data, bool? status, InverterSimulatorDTO dto0, InverterIAMMETERCloudDTO dto1, InverterIAMMETERLocalDTO dto2)
    {
        var result = new Result();
        var entity = _myContext.Inverters.FirstOrDefault();
        if (entity == null)
        {
            result.Message = "Invalid data: inverter";
        }
        else
        {
            var oldSourceType = entity.SourceType;

            TryUpdateModelAsync(entity);
            entity.Status = status == true;
            if (entity.SourceType == (int)SourceTypeEnums.Simulation)
            {
                if (dto0.IsValid() == false)
                    return Json(result.Return("Invalid settings."));
                entity.SourceConfigJson = JsonConvert.SerializeObject(dto0);
            }

            if (entity.SourceType == (int)SourceTypeEnums.IAMMETERCloud)
            {
                if (dto1.IsValid() == false)
                    return Json(result.Return("Invalid settings."));
                entity.SourceConfigJson = JsonConvert.SerializeObject(dto1);
            }
            if (entity.SourceType == (int)SourceTypeEnums.IAMMETERLocal)
            {
                if (dto2.IsValid() == false)
                    return Json(result.Return("Invalid settings."));
                entity.SourceConfigJson = JsonConvert.SerializeObject(dto2);
            }
            if (oldSourceType != entity.SourceType)
            {
                var meter = _myContext.Meters.First(o => o.Name == "A");
                meter.LastUpdateTime = null;
            }
            _myContext.SaveChanges();

            if (oldSourceType != entity.SourceType)
            {
                _meterService.SetMeterData();
            }

            result.Successful = true;
            result.Message = "Successfully saved";
        }
        return Json(result);
    }

    public ActionResult Load(Load data, List<RunTimeDetail> details)
    {
        var result = new Result();
        var entity = data;
        if (data.Id == 0)
        {
            _myContext.Loads.Add(entity);
        }
        else
        {
            entity = _myContext.Loads.FirstOrDefault(o => o.Id == data.Id);
            if (entity == null)
            {
                result.Message = "Invalid data: inverter";
                return Json(result);
            }
            else
            {
                TryUpdateModelAsync(entity);
            }
        }
        // runtimedetails
        if (entity.RunTimeDetails != null && entity.RunTimeDetails.Count > 0)
        {
            foreach (var item in entity.RunTimeDetails)
            {
                _myContext.RunTimeDetails.Remove(item);
            }
        }
        entity.RunTimeDetails = new List<RunTimeDetail>();
        entity.RunTimeDetails.AddRange(details);
        var rt = entity.GetValidDetails();
        if (rt.Successful == true)
        {
            _myContext.SaveChanges();
            result.Successful = true;
            result.Message = "Successfully saved";
        }
        else
        {
            result.Message = rt.Message;
        }
        return Json(result);
    }

    public ActionResult LoadDelete(long? Id)
    {
        var result = new Result();
        var entity = _myContext.Loads.FirstOrDefault(o => o.Id == Id);
        if (entity == null)
        {
            result.Message = "Invalid data: load";
        }
        else
        {
            _myContext.Loads.Remove(entity);
            _myContext.SaveChanges();
            result.Successful = true;
            result.Message = "Successfully deleted";
        }
        return Json(result);
    }

    public ActionResult LoadStatus(long? Id, bool? status)
    {
        var result = new Result();
        var entity = _myContext.Loads.FirstOrDefault(o => o.RunMode == 0 && o.Id == Id);
        if (entity == null)
        {
            result.Message = "Invalid data: load";
        }
        else
        {
            entity.SetStatus(status);
            _myContext.SaveChanges();
            _meterService.SetMeterData();
            result.Successful = true;
            result.Message = "Successfully " + (entity.Status == true ? "Opened" : "Closed");
        }
        return Json(result);
    }

    public ActionResult ClearMeter()
    {
        var result = new Result();
        var list = _myContext.Meters.ToList();
        foreach (var meter in list)
        {
            meter.Voltage = null;
            meter.Current = null;
            meter.Power = null;
            meter.Energy = null;
            meter.ReverseEnergy = null;
            meter.LastUpdateTime = null;
        }
        _myContext.SaveChanges();
        result.Successful = true;
        result.Message = "Successfully cleared.";
        return Json(result);
    }

    public ActionResult ExportLoadData()
    {
        var loads = _myContext.Loads.Include(o => o.RunTimeDetails).ToList();
        var list = new List<LoadExportDTO>();
        foreach (var load in loads)
        {
            var dto = new LoadExportDTO
            {
                Name = load.Name,
                MinPower = load.MinPower,
                MaxPower = load.MaxPower,
                RunMode = load.RunMode,
                Status = load.Status,
                SetMode = load.SetMode,
            };
            if (load.RunTimeDetails != null)
                dto.RunTimeDetails = load.RunTimeDetails.Select(o => new RunTimeDetailExportDTO
                {
                    Start = o.Start,
                    End = o.End,
                    MinMinutes = o.MinMinutes,
                    MaxMinutes = o.MaxMinutes
                }).ToList();
            list.Add(dto);
        }
        return File(System.Text.Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(list)), "text/json", "loads.json");

    }

    public ActionResult ClearLoadData()
    {
        var result = new Result();
        var list = _myContext.Loads.ToList();

        _myContext.RemoveRange(list);
        _myContext.SaveChanges();

        result.Successful = true;
        result.Message = "Successfully cleared.";
        return Json(result);
    }

    public ActionResult ImportLoadData(string jsonText)
    {

        var result = new Result();
        var list = JsonConvert.DeserializeObject<List<LoadExportDTO>>(jsonText);
        if (list != null && list.Count > 0)
        {
            foreach (var item in list)
            {
                var load = new Load()
                {
                    Name = item.Name,
                    MinPower = item.MinPower,
                    MaxPower = item.MaxPower,
                    RunMode = item.RunMode,
                    Status = item.Status,
                    SetMode = item.SetMode,
                };
                if (item.RunTimeDetails != null)
                    load.RunTimeDetails = item.RunTimeDetails.Select(o => new RunTimeDetail
                    {
                        Start = o.Start,
                        End = o.End,
                        MinMinutes = o.MinMinutes,
                        MaxMinutes = o.MaxMinutes
                    }).ToList();
                _myContext.Loads.Add(load);
            }
            _myContext.SaveChanges();
            result.Successful = true;
            result.Message = "Successfully imported.";
        }
        else
            result.Message = "Invalid file data.";

        return Json(result);
    }
}
