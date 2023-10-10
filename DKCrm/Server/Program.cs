using DKCrm.Server.Data;
using DKCrm.Server.Services;
using DKCrm.Shared.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionStringProduct = builder.Configuration.GetConnectionString("ProductContextConnection") ??
                              throw new InvalidOperationException("Connection string 'ProductContextConnection' not found.");
var connectionStringUser = builder.Configuration.GetConnectionString("UserContextConnection") ??
                              throw new InvalidOperationException("Connection string 'UserContextConnection' not found.");

builder.Services.AddDbContext<ApplicationDBContext>(options =>
    options.UseLazyLoadingProxies().UseNpgsql(connectionStringProduct));
builder.Services.AddDbContext<UserDbContext>(options =>
    options.UseNpgsql(connectionStringUser));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<UserDbContext>();
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = false;
    options.Events.OnRedirectToLogin = context =>
    {
        context.Response.StatusCode = 401;
        return Task.CompletedTask;
    };
});

builder.Services.AddSignalR();
builder.Services.AddControllers().AddNewtonsoftJson(opt=>opt.SerializerSettings.ReferenceLoopHandling= Newtonsoft.Json.ReferenceLoopHandling.Ignore);

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");
app.MapHub<SignalRHub>("/signalRHub");

app.Run();
