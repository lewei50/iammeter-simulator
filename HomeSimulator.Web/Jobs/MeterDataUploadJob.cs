namespace HomeSimulator.Web.Jobs;
using Quartz;
using HomeSimulator.Services;
using HomeSimulator.Web.Models;
using HomeSimulator.Web.Models.DTO;
using Newtonsoft.Json;
using System.Text;
public class MeterDataUploadJob : IJob
{
    private readonly MeterService _meterService;
    public MeterDataUploadJob(MeterService meterService)
    {
        _meterService = meterService;
    }
    public async Task Execute(IJobExecutionContext context)
    {

        var rt = _meterService.GetMeterDataFromDB();
        if (rt.SN != "NeedToBeSet")
            await UploadToCloud(rt);
    }

    //const string CloudUrlRoot = "http://localhost:27106";
    const string CloudUrlRoot = "https://www.iammeter.com";
    public static async Task<Result?> UploadToCloud(MeterUploadDTO data)
    {
        try
        {
            System.Net.Http.HttpClient hc = new System.Net.Http.HttpClient();
            var res = await hc.PostAsync($"{CloudUrlRoot}/api/v1/sensor/UploadSensor", new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "text/plain"));
            var rdata = await res.Content.ReadAsStringAsync();
            if (string.IsNullOrEmpty(rdata) == false)
            {
                return JsonConvert.DeserializeObject<Result>(rdata);
            }
            else
            {
                return new Result { Message = "Invalid data." };
            }
        }
        catch (Exception ex)
        {
            return new Result { Message = ex.Message };
        }

    }

}
