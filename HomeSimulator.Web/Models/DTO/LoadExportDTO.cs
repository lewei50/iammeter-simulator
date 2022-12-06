namespace HomeSimulator.Web.Models.DTO;
using HomeSimulator.Web.Models;
using HomeSimulator.Web.Helpers;

public class LoadExportDTO
{
    public string? Name { get; set; }

    public decimal? MinPower { get; set; }

    public decimal? MaxPower { get; set; }

    public int RunMode { get; set; }

    public bool Status { get; set; }

    public int SetMode { get; set; }

    public bool ToMeter { get; set; }


    public List<RunTimeDetailExportDTO>? RunTimeDetails { get; set; }

}
public class RunTimeDetailExportDTO
{
    public string? Start { get; set; }
    public string? End { get; set; }
    public int MinMinutes { get; set; }
    public int MaxMinutes { get; set; }
}


