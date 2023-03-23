using Fta.CarbonAware.Library;
using Fta.CarbonAware.Web;
using Fta.CarbonAware.Web.Interfaces;
using Fta.CarbonAware.Web.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddTransient<IEmissionsService, EmissionsService>();
builder.Services.AddCarbonAwareApiLibrary(builder.Configuration.GetValue<string>("CarbonAwareApi:BaseUrl"));
builder.Services.AddIpInfoNamedHttpClient();
builder.Services.AddCarbonAwareApiSettings(builder.Configuration);
builder.Services.AddIpInfoApiSettings(builder.Configuration);
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
