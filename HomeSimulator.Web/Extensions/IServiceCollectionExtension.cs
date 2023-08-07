using HomeSimulator.Web.Models;
namespace HomeSimulator.Web.Extensions;
using Microsoft.EntityFrameworkCore;
using HomeSimulator.Services;
using HomeSimulator.Web.Jobs;
using Quartz;
public static class IServiceCollectionExtensions
{
    public static void AddMyService(this IServiceCollection services)
    {

        services.AddDbContext<MyContext>(options =>
            {
                options.UseSqlite("Data Source=data/data.db",
                    o =>
                    o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)
                        .MigrationsAssembly("HomeSimulator.Web")
                    );
            });
        // mine
        services.AddScoped<ConfigService>();
        services.AddScoped<MeterService>();
        services.AddScoped<LoadService>();
        services.AddScoped<InverterService>();
        services.AddSingleton<OCPPSocketBackgroundService>();

        // basic auth
        services.AddAuthentication(BasicAuthenticationScheme.DefaultScheme)
                        .AddScheme<BasicAuthenticationOption, BasicAuthenticationHandler>(BasicAuthenticationScheme.DefaultScheme, null);



        services.AddQuartz(q =>
            {
                q.UseMicrosoftDependencyInjectionScopedJobFactory();

                q.ScheduleJob<MeterDataUpdateJob>(trigger => trigger
                .WithIdentity("MeterDataUpdateJob")
                .StartAt(DateBuilder.EvenSecondDate(DateTimeOffset.UtcNow.AddSeconds(1)))
                .WithDailyTimeIntervalSchedule(x => x.WithInterval(6, IntervalUnit.Second)));

                q.ScheduleJob<MeterDataUploadJob>(trigger => trigger
                .WithIdentity("MeterDataUploadJob")
                .StartAt(DateBuilder.EvenSecondDate(DateTimeOffset.UtcNow.AddSeconds(10)))
                .WithDailyTimeIntervalSchedule(x => x.WithInterval(1, IntervalUnit.Minute)));
            });
        services.AddQuartzServer(options =>
        {
            // when shutting down we want jobs to complete gracefully
            options.WaitForJobsToComplete = true;

        });

    }


}
