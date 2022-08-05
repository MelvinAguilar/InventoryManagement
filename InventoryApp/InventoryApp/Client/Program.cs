global using Microsoft.AspNetCore.Components.Authorization;
global using InventoryApp.Shared;

//using InventoryApp.Client.Services;
//using InventoryApp.Client.Services.Impl;
using InventoryApp.Client;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
builder.Services.AddAuthorizationCore();

//builder.Services.AddScoped<IAuthService, AuthService>();

await builder.Build().RunAsync();
