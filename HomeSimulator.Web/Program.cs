using HomeSimulator.Web.Extensions;
using HomeSimulator.Web.Models;
using Microsoft.EntityFrameworkCore;
using HomeSimulator.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

builder.Services.AddMyService();

var app = builder.Build();

app.UseWhen(
    predicate: x => true,// x.Request.Path.StartsWithSegments(new PathString("/")),
    configuration: appBuilder => { appBuilder.UseBasicAuthentication(); }
);

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    //app.UseHsts();
}


app.UpdateDatabase();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

