global using Microsoft.AspNetCore.Components.Authorization;
global using InventoryApp.Shared;
global using InventoryApp.Shared.Models;
global using InventoryApp.Shared.Dtos.CategoryDtos;
global using InventoryApp.Shared.Dtos.CustomerDtos;
global using InventoryApp.Shared.Dtos.EmployeeDtos;
global using InventoryApp.Shared.Dtos.ProductDtos;
global using InventoryApp.Shared.Dtos.ProviderDtos;
global using InventoryApp.Shared.Dtos.PurchaseDtos;
global using InventoryApp.Shared.Dtos.PurchaseDetailDtos;
global using InventoryApp.Shared.Dtos.SupplyDtos;
global using InventoryApp.Shared.Dtos.SupplyDetailDtos;
global using System.Net.Http.Json;

using InventoryApp.Client.Services;
using InventoryApp.Client.Services.Impl;
using InventoryApp.Client;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Blazored.LocalStorage;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
builder.Services.AddAuthorizationCore();

builder.Services.AddScoped<IAuthService, AuthService>();

await builder.Build().RunAsync();
