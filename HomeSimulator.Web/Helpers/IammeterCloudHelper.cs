namespace HomeSimulator.Web.Helpers;
using System.Reflection;
using System.ComponentModel;
using System.Linq;
using HomeSimulator.Web.Models;
using HomeSimulator.Web.Models.DTO;
using Newtonsoft.Json;
public class IammeterCloudHelper
{
    const string CloudUrlRoot = "https://www.iammeter.com";
    public static async Task<Result<MeterDataDTO>> GetCurrentData(string sn)
    {
        var rt = new Result<MeterDataDTO>();
        try
        {
            System.Net.Http.HttpClient hc = new System.Net.Http.HttpClient();
            var res = await hc.GetStringAsync($"{CloudUrlRoot}/api/v1/sensor/current/{sn}");
            if (string.IsNullOrEmpty(res) == false)
            {
                var data = JsonConvert.DeserializeObject<MeterDataDTO>(res);
                if (data != null)
                    return rt.Return("OK", data);
                else
                    return rt.Return("Invalid data.");

            }
            else
            {
                return rt.Return("Invalid data.");
            }
        }
        catch (Exception ex)
        {
            return rt.Return(ex.Message);
        }

    }
}