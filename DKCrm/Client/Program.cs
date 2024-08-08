using DKCrm.Client;
using DKCrm.Client.Services.Auth;
using DKCrm.Client.Services.BrandService;
using DKCrm.Client.Services.CategoryService;
using DKCrm.Client.Services.Chat;
using DKCrm.Client.Services.CompanyServices;
using DKCrm.Client.Services.ConfirmationAction;
using DKCrm.Client.Services.CurrencyDictionaryService;
using DKCrm.Client.Services.CurrencyService;
using DKCrm.Client.Services.DocumentService;
using DKCrm.Client.Services.FnsRequesting;
using DKCrm.Client.Services.InternalCompanyDataService;
using DKCrm.Client.Services.OrderServices;
using DKCrm.Client.Services.ProductServices;
using DKCrm.Client.Services.UserService;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor;
using MudBlazor.Services;


var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddOptions();
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<CustomStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(s => s.GetRequiredService<CustomStateProvider>());
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserManagerCustom, UserManagerCustom>();
builder.Services.AddScoped<IProductManager, ProductManager>();
builder.Services.AddScoped<IBrandManager, BrandManager>();
builder.Services.AddScoped<ICategoryManager, CategoryManager>();
builder.Services.AddTransient<IChatManager, ChatManager>();
builder.Services.AddTransient<ICompanyManager, CompanyManager>();
builder.Services.AddTransient<ICompanyTypeManager, CompanyTypeManager>();
builder.Services.AddTransient<ICompanyTagsManager, CompanyTagsManager>();
builder.Services.AddTransient<IRequestingFromFnsService, RequestingFromFnsService>();
builder.Services.AddTransient<IEmployeeManager, EmployeeManager>();
builder.Services.AddTransient<IStorageManager, StorageManager>();
builder.Services.AddTransient<ICategoryOptionsManager, CategoryOptionsManager>();

builder.Services.AddTransient<IExportedOrderManager, ExportedOrderManager>();
builder.Services.AddTransient<IExportedProductManager, ExportedProductManager>();
builder.Services.AddTransient<IExportedOrderStatusManager, ExportedOrderStatusManager>();
builder.Services.AddTransient<IImportedOrderManager, ImportedOrderManager>();
builder.Services.AddTransient<IImportedProductManager, ImportedProductManager>();
builder.Services.AddTransient<IImportedOrderStatusManager, ImportedOrderStatusManager>();
builder.Services.AddTransient<IInternalCompanyDataManager, InternalCompanyDataManager>();
builder.Services.AddTransient<ICurrencyManager, CurrencyManager>();
builder.Services.AddTransient<ICommentOrderManager, CommentOrderManager>();
builder.Services.AddTransient<IApplicationOrderingManager, ApplicationOrderingManager>();
builder.Services.AddTransient<IMissingProductConverter, MissingProductConverter>();

builder.Services.AddTransient<IConfirmationActionService, ConfirmationActionService>();
builder.Services.AddTransient<IDocumentManager, DocumentManager>();
builder.Services.AddTransient<ICurrencyDictionaryManager, CurrencyDictionaryManager>();


builder.Services.AddMudServices(c => { c.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomRight; });
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddBlazorBootstrap();
await builder.Build().RunAsync();
