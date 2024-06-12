using DKCrm.Server.Data;
using DKCrm.Server.Interfaces;
using DKCrm.Server.Interfaces.CompanyInterfaces;
using DKCrm.Server.Interfaces.OrderInterfaces;
using DKCrm.Server.Interfaces.ProductInterfaces;
using DKCrm.Server.Services;
using DKCrm.Server.Services.CompanyServices;
using DKCrm.Server.Services.OrderServices;
using DKCrm.Server.Services.ProductServices;
using DKCrm.Shared.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

var connectionStringProduct = builder.Configuration.GetConnectionString("ProductContextConnection") ??
                              throw new InvalidOperationException("Connection string 'ProductContextConnection' not found.");
var connectionStringUser = builder.Configuration.GetConnectionString("UserContextConnection") ??
                              throw new InvalidOperationException("Connection string 'UserContextConnection' not found.");

builder.Services.AddTransient<IAuthService, AuthService>();

builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IChatService, ChatService>();
builder.Services.AddTransient<IInternalCompanyDataService, InternalCompanyDataService>();
builder.Services.AddTransient<IStorageService, StorageService>();

builder.Services.AddTransient<IProductService, ProductService>();
builder.Services.AddTransient<IBrandService, BrandService>();
builder.Services.AddTransient<ICategoryService, CategoryService>();
builder.Services.AddTransient<ICategoryOptionsService, CategoryOptionsService>();

builder.Services.AddTransient<ICompanyService, CompanyService>();
builder.Services.AddTransient<ICompanyTagsService, CompanyTagsService>();
builder.Services.AddTransient<ICompanyTypeService, CompanyTypeService>();

builder.Services.AddTransient<IExportedProductService, ExportedProductService>();
builder.Services.AddTransient<IImportedProductService, ImportedProductService>();
builder.Services.AddTransient<IExportedOrderService, ExportedOrderService>();
builder.Services.AddTransient<IImportedOrderService, ImportedOrderService>();
builder.Services.AddTransient<IExportedOrderStatusServices, ExportedOrderStatusService>();
builder.Services.AddTransient<IImportedOrderStatusService, ImportedOrderStatusService>();
builder.Services.AddTransient<IApplicationOrderingService, ApplicationOrderingService>();
builder.Services.AddTransient<IOrderCommentsService, OrderCommentsService>();
builder.Services.AddTransient<IDocumentService, DocumentService>();
builder.Services.AddDbContext<ApplicationDBContext>(options =>
    options.UseNpgsql(connectionStringProduct).AddInterceptors(new SoftDeleteInterceptor()));
builder.Services.AddDbContext<UserDbContext>(options =>
    options.UseNpgsql(connectionStringUser).AddInterceptors(new SoftDeleteInterceptor()));


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

app.UseStaticFiles();
app.UseStaticFiles(new StaticFileOptions()
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"StaticFiles")),
    RequestPath = new PathString("/StaticFiles")
});

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");
app.MapHub<SignalRHub>("/signalRHub");

app.Run();
