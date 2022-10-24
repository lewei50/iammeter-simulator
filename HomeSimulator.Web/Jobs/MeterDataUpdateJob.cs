namespace HomeSimulator.Web.Jobs;
using Quartz;
using HomeSimulator.Services;
public class MeterDataUpdateJob : IJob
{
    private readonly MeterService _meterService;
    public MeterDataUpdateJob(MeterService meterService)
    {
        _meterService = meterService;
    }
    public async Task Execute(IJobExecutionContext context)
    {
        await Task.Run(() =>
        {
            _meterService.SetMeterData();
        });
    }

}
