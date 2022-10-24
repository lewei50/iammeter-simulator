namespace HomeSimulator.Web.Models.Entities;
using System.ComponentModel.DataAnnotations.Schema;

///基本配置
[Table(name:"Configs")]
public class Config
{
    public long Id { get; set; }
    public string? SN { get; set; }

    public string? Username{get;set;}

    public string? Password{get;set;}

    public string? AccessToken { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ModifyTime{get;set;}
}

