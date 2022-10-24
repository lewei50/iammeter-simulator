namespace HomeSimulator.Web.Models.DTO;
using HomeSimulator.Web.Models;
public class MeterDTO
{
    public decimal? Voltage { get; set; }

    public decimal? Current { get; set; }

    public decimal? Power { get; set; }

    public decimal? Energy { get; set; }

    public decimal? ReverseEnergy { get; set; }

    public DateTime? LastUpdateTime { get; set; }
}
