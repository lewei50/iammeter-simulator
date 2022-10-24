namespace HomeSimulator.Web.Models.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using HomeSimulator.Web.Models.DTO;
using HomeSimulator.Web.Helpers;
[Table(name: "Loads")]
public class Load
{
    public long Id { get; set; }

    public string? Name { get; set; }

    [Column(TypeName = "decimal(10,1)")]
    public decimal? MinPower { get; set; }

    [Column(TypeName = "decimal(10,1)")]
    public decimal? MaxPower { get; set; }

    public int RunMode { get; set; }

    public bool Status { get; set; }

    public int SetMode { get; set; }

    [Column(TypeName = "decimal(10,1)")]
    public decimal? LastPower { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? LastUpdateTime { get; set; }

    public virtual List<RunTimeDetail>? RunTimeDetails { get; set; }

    public void SetStatus(bool? status, DateTime? time = null)
    {
        if (time == null) time = DateTime.Now;
        if (status == null) status = false;
        this.Status = status.Value;
        if (Status == true)
        {
            if (SetMode == (int)SetModeEnums.Cannot)
                this.LastPower = CommonHelper.GetRandomNumber(MinPower, MaxPower);
            this.LastUpdateTime = time;
        }
    }

    public Result SetPower(decimal? v)
    {
        var rt = new Result();
        if (SetMode != (int)SetModeEnums.Configurable)
            return rt.Return("The load cannot be set");
        if (v == null)
            return rt.Return("Invalid value.");
        if (v < MinPower) 
            return rt.Return($"The value set must be greater than {MinPower}");
        if (v > MaxPower) v = MaxPower;
        LastPower = v;
        SetStatus(true);
        rt.Successful = true;
        return rt;
    }

    public Result<List<RunTimeDetailDTO>> GetValidDetails()
    {
        var result = new Result<List<RunTimeDetailDTO>>();
        if (RunMode == (int)RunModeEnums.Timing)
        {
            if (RunTimeDetails != null)
            {
                var list = RunTimeDetails.Select(o => o.GetDetailDTO()).OrderBy(o => o.Start).ToList();
                RunTimeDetailDTO? lastItem = null;
                foreach (var item in list)
                {
                    if (item.IsValid() == false)
                        return result.Return("Invalid time or minutes setting.");
                    if (lastItem != null)
                    {
                        if (item.Start < lastItem.End)
                            return result.Return("Invalid time setting.");
                    }
                    lastItem = item;
                }
                result.Data = list;
                result.Successful = true;
            }
            else
                result.Message = "Need to be set run time.";
        }
        else
            result.Successful = true;
        return result;
    }
}


