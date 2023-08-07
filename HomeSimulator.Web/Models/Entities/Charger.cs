namespace HomeSimulator.Web.Models.Entities;
using System.ComponentModel.DataAnnotations.Schema;

[Table(name:"Chargers")]
public class Charger
{
    public int Id { get; set; }

    [Column(TypeName = "decimal(13,3)")]
    public decimal? MaxPower { get; set; } = 70000;

    [Column(TypeName = "decimal(13,3)")]
    public decimal? MaxEnergy { get; set; } = 70;



    public string? OCPPServer { get; set; }
    public string? ChargePointId { get; set; }

    public string? IdTag { get; set; }

    [Column(TypeName = "decimal(13,3)")]
    public decimal? LimitPower { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? LimitPowerExpiredTime { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? LastUpdateTime { get; set; }

    [Column(TypeName = "decimal(13,3)")]
    public decimal? Voltage { get; set; }

    [Column(TypeName = "decimal(13,3)")]
    public decimal? Current { get; set; }

    [Column(TypeName = "decimal(13,3)")]
    public decimal? Power { get; set; }

    /// <summary>
    ///  默认为0,之后不断累加
    /// </summary>
    [Column(TypeName = "decimal(13,3)")]
    public decimal? Energy { get; set; }

    /// <summary>
    /// 每次开始充电就随机一个soc 20-70吧
    /// </summary>
    [Column(TypeName = "decimal(13,3)")]
    public decimal? SOC { get; set; }

    public int TransactionId { get; set; }

    // wh
    public int? MeterStart { get; set; }
    public DateTimeOffset? MeterStartTime { get; set; }

    // wh
    public int? MeterStop { get; set; }
    public DateTimeOffset? MeterStopTime { get; set; }

    /// <summary>
    /// 如果有开始电量,没有结束电量,则为充电中
    /// </summary>
    public bool IsCharging
    {
        get
        {
            return MeterStart != null && MeterStop == null;
        }
    }

}