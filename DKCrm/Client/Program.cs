using DKCrm.Client;
using DKCrm.Client.Services.Auth;
using DKCrm.Client.Services.BrandService;
using DKCrm.Client.Services.CategoryService;
using DKCrm.Client.Services.Chat;
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

builder.Services.AddMudServices(c => { c.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomRight; });
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

await builder.Build().RunAsync();
