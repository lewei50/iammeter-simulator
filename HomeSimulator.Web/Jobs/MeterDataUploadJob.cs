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
    private readonly ILogger<MeterDataUploadJob> logger;

    public MeterDataUploadJob(MeterService meterService, ILogger<MeterDataUploadJob> _logger)
    {
        _meterService = meterService;
        logger = _logger;
    }
    public async Task Execute(IJobExecutionContext context)
    {

        var rt = _meterService.GetMeterDataFromDB();
        if (rt.SN != "NeedToBeSet")
            await UploadToCloud(rt);
    }

    //const string CloudUrlRoot = "http://localhost:5050";
    //const string CloudUrlRoot = "http://localhost:27106";
    const string CloudUrlRoot = "https://www.iammeter.com";
    public async Task<Result?> UploadToCloud(MeterUploadDTO data)
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
            logger.LogError(ex, "上传到iammeter出错");
            return new Result { Message = ex.Message };
        }

    }

}
