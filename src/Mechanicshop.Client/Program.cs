using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Mechanicshop.Client;
using MechanicShop.Client.Identity;
using Microsoft.AspNetCore.Components.Authorization;
using MechanicShop.Client.Services;
using MechanicShop.Client.Hubs;
using Blazored.LocalStorage;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.Services.AddAuthorizationCore();

builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();

builder.Services.AddScoped(
    sp => (IAccountManagement)sp.GetRequiredService<AuthenticationStateProvider>());

builder.Services.AddTransient<BearerTokenHandler>();

builder.Services.AddScoped<TimeZoneService>();

builder.Services.AddScoped<WorkOrderHubClient>();

builder.Services.AddHttpClient(
    "MechanicShopClient",
    client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
    .AddHttpMessageHandler<BearerTokenHandler>();

builder.Services.AddBlazoredLocalStorage();

builder.Services.AddScoped<ServiceApi>(); 
await builder.Build().RunAsync();
