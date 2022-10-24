namespace HomeSimulator.Web.Models.DTO;
public class RunTimeDetailDTO
{
    public int Start { get; set; }
    public int End { get; set; }
    public int MinMinutes { get; set; }
    public int MaxMinutes { get; set; }

    public bool IsValid()
    {
        var r = false;
        if (End > Start && MaxMinutes >= MinMinutes && MaxMinutes <= (End - Start))
            r = true;
        return r;
    }
}

