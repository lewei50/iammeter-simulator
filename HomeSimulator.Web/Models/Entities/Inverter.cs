namespace HomeSimulator.Web.Models.Entities;
using System.ComponentModel.DataAnnotations.Schema;

[Table(name:"Inverters")]
public class Inverter
{
    public long Id { get; set; }

    public string? Name { get; set; }

    public int SourceType{get;set;}
    
    public string? SourceConfigJson{get;set;}

    // [Column(TypeName = "decimal(10,1)")]
    // public decimal? RatedPower { get; set; }

    public bool Status{get;set;}

    [Column(TypeName = "decimal(10,1)")]
    public decimal? LastPower { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? LastUpdateTime{get;set;}
}
