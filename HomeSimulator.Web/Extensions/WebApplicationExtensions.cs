using HomeSimulator.Web.Models;
namespace HomeSimulator.Web.Extensions;
using Microsoft.EntityFrameworkCore;
using HomeSimulator.Services;

public static class WebApplicationExtensions
{


    public static void UpdateDatabase(this IApplicationBuilder app)
    {
        using (var serviceScope = app.ApplicationServices
            .GetRequiredService<IServiceScopeFactory>()
            .CreateScope())
        {
            using (var context = serviceScope.ServiceProvider.GetService<MyContext>())
            {
                if (context != null)
                    context.Database.Migrate();
            }
        }
    }

    public static void UseBasicAuthentication(this IApplicationBuilder app)
        {
            app.UseMiddleware<BasicAuthenticationMiddleware>();
        }

}
