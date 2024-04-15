using AspNetCoreHero.ToastNotification;
using AspNetCoreHero.ToastNotification.Abstractions;
using AspNetCoreHero.ToastNotification.Extensions;
using AspNetCoreHero.ToastNotification.Notyf;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Core.Types;
using RestaurantRaterBooking.Services;
using System.Configuration;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using TTCK_DVKYTHUAT.Data;
using TTCK_DVKYTHUAT.Models;


        var builder = WebApplication.CreateBuilder(args);
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'AppContextConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));
// Add services to the container.
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddControllersWithViews();
builder.Services.AddSession(options =>
{
    options.Cookie.HttpOnly = true;
    //options.Cookie.Name = ".AdventureWorks.Session";
    //options.IdleTimeout = TimeSpan.FromSeconds(10);
    options.Cookie.IsEssential = true;
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(p =>
    {
        p.LoginPath = "/accounts/login";
        p.AccessDeniedPath = "/";
    });

//builder.Services.AddNotyf(config => { config.DurationInSeconds = 10; config.IsDismissable = true; config.Position = NotyfPosition.BottomRight; });
builder.Services.AddNotyf(config => { config.DurationInSeconds = 10; config.IsDismissable = true; config.Position = NotyfPosition.TopRight; });
var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }
//  void ConfigureServices(IServiceCollection services) => services.AddControllersWithViews().AddRazorRuntimeCompilation();

void ConfigureServices(IServiceCollection services)
{
    //var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'AppContextConnection' not found.");
    //builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));
    //services.AddSingleton<HtmlEncoder>(HtmlEncoder.Create(allowedRanges: new[] { UnicodeRanges.All }));
    //builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
    
    services.AddSession();
    services.AddControllersWithViews().AddRazorRuntimeCompilation();
    services.AddScoped<INotyfService, NotyfService>();
}



app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseNotyf();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseSession();

app.UseEndpoints(endpoints =>
{


    endpoints.MapControllerRoute(
      name: "areas",
      pattern: "{area:exists}/{controller=AdminAccounts}/{action=Index}/{id?}"
    );


});

app.MapControllerRoute(
  name: "default",
  pattern: "{controller=Home}/{action=Index}/{id?}");


app.Run();
   