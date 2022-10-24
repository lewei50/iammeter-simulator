namespace HomeSimulator.Web.Models.DTO;
using HomeSimulator.Web.Models;
using HomeSimulator.Web.Helpers;
public class InverterSimulatorDTO
{
    public decimal RatedPower { get; set; } = 4500;

    public LocationTypeEnums LocationType { get; set; } = LocationTypeEnums.Northern;

    public double MinHour { get; set; } = 10;
    public double MaxHour { get; set; } = 16;

    public bool IsValid()
    {
        return MaxHour > MinHour && MinHour > 0 && MaxHour < 24;
    }
}

public class InverterIAMMETERCloudDTO
{
    public string SN { get; set; } = "";

    public int DataIndex { get; set; }

    public bool IsValid()
    {
        // get iammeter api test
        var rt = IammeterCloudHelper.GetCurrentData(SN).Result;
        if (rt.Successful)
        {
            if (DataIndex > 0 && rt.Data != null)
            {
                return rt.Data.Datas != null && rt.Data.Datas.Length > DataIndex;
            }
            else
                return true;
        }
        else
            return false;
    }
}

public class InverterIAMMETERLocalDTO
{
    public string IPAddress { get; set; } = "";

    public int DataIndex { get; set; }

    public bool IsValid()
    {
        // get iammeter api test
        var rt = IammeterLocalHelper.GetCurrentData(IPAddress).Result;
        if (rt.Successful)
        {
            if (DataIndex > 0 && rt.Data != null)
            {
                return rt.Data.Datas != null && rt.Data.Datas.Length > DataIndex;
            }
            else
                return true;
        }
        else
            return false;
    }
}