namespace HomeSimulator.Web.Models.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using HomeSimulator.Web.Models.DTO;
[Table(name: "RunTimeDetails")]
public class RunTimeDetail
{
    public long Id { get; set; }
    public long LoadId { get; set; }

    public virtual Load? Load { get; set; }
    public string? Start { get; set; }
    public string? End { get; set; }
    public int MinMinutes { get; set; }
    public int MaxMinutes { get; set; }

    public RunTimeDetailDTO GetDetailDTO()
    {
        var detail = new RunTimeDetailDTO()
        {
            Start = GetMinuteForDay(Start),
            End = GetMinuteForDay(End),
            MinMinutes = MinMinutes,
            MaxMinutes = MaxMinutes
        };

        if (detail.End <= detail.Start) detail.End = 24 * 60 + detail.End;

        return detail;
    }


    public int GetMinuteForDay(string? time)
    {
        if (string.IsNullOrEmpty(time))
            return 0;
        var times = time.Split(':');
        if (times.Length != 2)
            return 0;
        int.TryParse(times[0], out int hour);
        int.TryParse(times[1], out int min);
        return hour * 60 + min;
    }
}
