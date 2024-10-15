using System.Text.Encodings.Web;
using System.Text.Unicode;
using GoogleReCaptcha.V3;
using GoogleReCaptcha.V3.Interface;
using Imigration.DataLayer.Context;
using Imigration.Domains.ViewModels.Common;
using Imigration.IoC;
using Imigration.Web.Hubs;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

#region Services


// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSignalR();
builder.Services.AddHttpClient<ICaptchaValidator, GoogleReCaptchaValidator>();
builder.Services.Configure<ScoreManagementViewModel>(builder.Configuration.GetSection("ScoreManagment"));

#region DbContext

builder.Services.AddDbContext<ImigrationDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("ImigrationConnection"))
);
#endregion

#region Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;

}).AddCookie(options =>
{
    options.LoginPath = "/Login";
    options.LogoutPath = "/Logout";
    options.ExpireTimeSpan = TimeSpan.FromDays(30);

});
#endregion

#region Encode

builder.Services.AddSingleton<HtmlEncoder>(
    HtmlEncoder.Create(allowedRanges: new[] { UnicodeRanges.All }));

#endregion

#region Register Dependencies

DependencyContainer.RegisterDependencies(builder.Services);
#endregion


#endregion

#region MiddelWares
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
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
           name: "areas",
            pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(

name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapHub<OnlineUsersHub>("/hubs/online-users");

app.Run();
#endregion
