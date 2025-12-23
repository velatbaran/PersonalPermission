using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using NToastNotify;
using PersonalPermission.Data;
using PersonalPermission.Service.Concrete;
using PersonalPermission.Service.IService;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddSession(options =>
{
    options.Cookie.Name = "PersonalPermission.Session";
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.IdleTimeout = TimeSpan.FromDays(1);
    options.IOTimeout = TimeSpan.FromMinutes(10);
}); //session kullan

builder.Services.AddDbContext<DatabaseContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DbConnection"));
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(x =>
{
    //x.LoginPath = "/Account/Login";
    x.LoginPath = "/giris";
    x.LogoutPath = "/cikis";
    //x.AccessDeniedPath = "/AccessDenied";
    x.AccessDeniedPath = "/erisimengellendi";
    x.Cookie.Name = "PersonalPermission.Account";
    x.Cookie.MaxAge = TimeSpan.FromDays(7);
    x.Cookie.IsEssential = true; // cookie ayarlarýný kalýcý kýlma
});

builder.Services.AddAuthorization(x =>
{
    x.AddPolicy("AdminPolicy", policy => policy.RequireClaim(ClaimTypes.Role, "Admin"));
    x.AddPolicy("UserPolicy", policy => policy.RequireClaim(ClaimTypes.Role, "Admin", "User"));
}); // Authentication 

//// Register MailHelper with configuration from appsettings.json
//builder.Services.AddSingleton<MailHelper>(provider =>
//{
//    var configuration = provider.GetRequiredService<IConfiguration>();
//    var smtpSettings = configuration.GetSection("SmtpSettings");
//    return new MailHelper(
//        smtpSettings["Server"],
//        int.Parse(smtpSettings["Port"]),
//        smtpSettings["Username"],
//        smtpSettings["Password"],
//        bool.Parse(smtpSettings["EnableSsl"])
//    );
//});

builder.Services.AddScoped(typeof(IService<>), typeof(Service<>));

builder.Services.AddScoped<ICalculatingPermision, CalculatingPermision>();
builder.Services.AddHostedService<PermissionTimerBackgorundService>();
builder.Services.AddScoped<ICalculatingServiceTime, CalculatingServiceTime>();
builder.Services.AddHostedService<ServiceTimerBackgroundService>();

builder.Services.AddControllersWithViews()
    .AddNToastNotifyToastr(new ToastrOptions()
    {
        PositionClass = ToastPositions.TopRight,
        TimeOut = 3000,
        ProgressBar = true
    })
    .AddRazorRuntimeCompilation();


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
app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapGet("/", context =>
{
    context.Response.Redirect("/giris");
    return Task.CompletedTask;
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
