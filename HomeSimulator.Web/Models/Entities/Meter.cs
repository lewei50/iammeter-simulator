namespace HomeSimulator.Web.Models.Entities;
using System.ComponentModel.DataAnnotations.Schema;

[Table(name:"Meters")]
public class Meter
{
    public long Id { get; set; }

    public string? Name { get; set; }


    [Column(TypeName = "decimal(10,1)")]
    public decimal? Voltage { get; set; }

    [Column(TypeName = "decimal(10,1)")]
    public decimal? Current { get; set; }

    [Column(TypeName = "decimal(10,1)")]
    public decimal? Power { get; set; }
    
    [Column(TypeName = "decimal(14,6)")]
    public decimal? Energy { get; set; }

    [Column(TypeName = "decimal(14,6)")]
    public decimal? ReverseEnergy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? LastUpdateTime{get;set;}
}