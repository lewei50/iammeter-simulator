namespace HomeSimulator.Web.Models.DTO;
public class MeterDataDTO
{
    public string? version { get; set; }
    public string? SN { get; set; }

    public string mac { get; set; }

    public decimal?[][] Datas { get; set; }
    public decimal?[] Data { get; set; }
}

