namespace HomeSimulator.Web.Helpers;
using System.Reflection;
using System.ComponentModel;
using System.Linq;
using HomeSimulator.Web.Models;
using HomeSimulator.Web.Models.DTO;
using Newtonsoft.Json;
public class IammeterLocalHelper
{
    public static async Task<Result<MeterDataDTO>> GetCurrentData(string ip)
    {
        var rt = new Result<MeterDataDTO>();
        try
        {
            System.Net.Http.HttpClient hc = new System.Net.Http.HttpClient();
            
            hc.DefaultRequestHeaders.Add("Authorization","Basic YWRtaW46YWRtaW4=");
            var res = await hc.GetStringAsync($"http://{ip}/monitorjson");
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