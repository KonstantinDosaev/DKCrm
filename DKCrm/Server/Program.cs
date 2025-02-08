using DKCrm.Server.Data;
using DKCrm.Server.Interfaces;
using DKCrm.Server.Interfaces.CompanyInterfaces;
using DKCrm.Server.Interfaces.DocumentInterfaces;
using DKCrm.Server.Interfaces.OrderInterfaces;
using DKCrm.Server.Interfaces.ProductInterfaces;
using DKCrm.Server.Services;
using DKCrm.Server.Services.CompanyServices;
using DKCrm.Server.Services.DocumentServices;
using DKCrm.Server.Services.OrderServices;
using DKCrm.Server.Services.ProductServices;
using DKCrm.Shared.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using System.Dynamic;
using System.Net;
using DKCrm.Server.Interfaces.ReportInterfaces;
using DKCrm.Server.Services.ReportServices;
using DKCrm.Shared.Constants;
using Microsoft.AspNetCore.ResponseCompression;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSignalR();
builder.Services.AddResponseCompression(opts =>
{
    opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
        ["application/octet-stream"]);
});
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
var connectionStringProduct = builder.Configuration.GetConnectionString("ProductContextConnection") ??
                              throw new InvalidOperationException("Connection string 'ProductContextConnection' not found.");
var connectionStringUser = builder.Configuration.GetConnectionString("UserContextConnection") ??
                              throw new InvalidOperationException("Connection string 'UserContextConnection' not found.");
var pathToStaticFiles = builder.Configuration[$"{DirectoryType.PrivateFolder}"];
var pathToPublicFiles = builder.Configuration[$"{DirectoryType.PublicFolder}"];
if (pathToStaticFiles == null)
{
    var appSettingsPath = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
    var json = File.ReadAllText(appSettingsPath);

    var jsonSettings = new JsonSerializerSettings();
    jsonSettings.Converters.Add(new ExpandoObjectConverter());
    jsonSettings.Converters.Add(new StringEnumConverter());

    dynamic config = JsonConvert.DeserializeObject<ExpandoObject>(json, jsonSettings) ?? throw new InvalidOperationException();
    var defaultStaticFilesPath = Path.Combine(Directory.GetCurrentDirectory());
    var expando = config as IDictionary<string, object>;
    expando?.Add($"{DirectoryType.PrivateFolder}", defaultStaticFilesPath);
    var newJson = JsonConvert.SerializeObject(config, Formatting.Indented, jsonSettings);
    File.WriteAllText(appSettingsPath, newJson);
    pathToStaticFiles = defaultStaticFilesPath;
}

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


builder.Services.AddControllers()
    .AddNewtonsoftJson(opt => opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);
builder.Services.AddControllersWithViews();

builder.Services.AddTransient<IAuthService, AuthService>();

builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IAccessRestrictionService, AccessRestrictionService>();
builder.Services.AddTransient<IChatService, ChatService>();
builder.Services.AddTransient<IInternalCompanyDataService, InternalCompanyDataService>();
builder.Services.AddTransient<IStorageService, StorageService>();

builder.Services.AddTransient<IProductService, ProductService>();
builder.Services.AddTransient<IBrandService, BrandService>();
builder.Services.AddTransient<ICategoryService, CategoryService>();
builder.Services.AddTransient<ICategoryOptionsService, CategoryOptionsService>();

builder.Services.AddTransient<ICompanyService, CompanyService>();
builder.Services.AddTransient<IEmployeeService, EmployeeService>();
builder.Services.AddTransient<ICompanyTagsService, CompanyTagsService>();
builder.Services.AddTransient<ICompanyTypeService, CompanyTypeService>();
builder.Services.AddTransient<IFileService, FileService>();
builder.Services.AddTransient<IExportedProductService, ExportedProductService>();
builder.Services.AddTransient<IImportedProductService, ImportedProductService>();
builder.Services.AddTransient<IExportedOrderService, ExportedOrderService>();
builder.Services.AddTransient<IImportedOrderService, ImportedOrderService>();
builder.Services.AddTransient<IExportedOrderStatusServices, ExportedOrderStatusService>();
builder.Services.AddTransient<IImportedOrderStatusService, ImportedOrderStatusService>();
builder.Services.AddTransient<IApplicationOrderingService, ApplicationOrderingService>();
builder.Services.AddTransient<IOrderCommentsService, OrderCommentsService>();
builder.Services.AddTransient<IDocumentService, DocumentService>();
builder.Services.AddTransient<IPriceToStringConverter, PriceToStringConverter>();
builder.Services.AddTransient<IInfoSetFromDocumentToOrderService, InfoSetFromDocumentToOrderService>();
builder.Services.AddTransient<PaymentInvoicePdfGenerator>();
builder.Services.AddTransient<OrderSpecificationPdfGenerator>();
builder.Services.AddTransient<CommercialOfferPdfGenerator>();
builder.Services.AddTransient<ICurrencyDictionaryService, CurrencyDictionaryService>();
builder.Services.AddTransient<IImageService, ImageService>();
builder.Services.AddTransient<IEmailSender, EmailSender>();
builder.Services.AddTransient<ICompanyCommentsService, CompanyCommentsService>();
builder.Services.AddTransient<IReportService, ReportService>();


builder.Services.AddRazorPages();

var app = builder.Build();
app.UseResponseCompression();
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseStaticFiles();
if (pathToPublicFiles != null)
    app.UseStaticFiles(new StaticFileOptions()
    {
        FileProvider = new PhysicalFileProvider(Path.Combine(pathToPublicFiles)),
        // RequestPath = new PathString("/StaticFiles")
    });

app.UseStaticFiles(new StaticFileOptions()
{
    FileProvider = new PhysicalFileProvider(Path.Combine(pathToStaticFiles)),
    OnPrepareResponse = (context) =>
    {
        if (context.Context.User.Identity is { IsAuthenticated: false })
        {
            context.Context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            context.Context.Response.ContentLength = 0;
            context.Context.Response.Body = Stream.Null;
            context.Context.Response.Headers.Append("Cache-Control", "no-store");
        }
    }
});
app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");
app.MapHub<SignalRHub>("/signalRHub");
app.MapHub<OrderCommentHub>("/orderCommentHub");
app.MapHub<CompanyCommentHub>("/companyCommentHub");
app.Run();
