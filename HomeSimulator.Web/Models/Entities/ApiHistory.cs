namespace HomeSimulator.Web.Models.Entities;
using System.ComponentModel.DataAnnotations.Schema;

///基本配置
[Table(name:"ApiHistory")]
public class ApiHistory
{
    public long Id { get; set; }
    
    public string? Name { get; set; }

    public string? Data { get; set; }

    public bool Result { get; set; }

    public string? Message { get; set; }

    public string? IPAddress { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? AddTime{get;set;}
}

