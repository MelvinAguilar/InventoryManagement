global using InventoryApp.Shared;
global using InventoryApp.Shared.Dtos.CategoryDtos;
global using InventoryApp.Shared.Dtos.CustomerDtos;
global using InventoryApp.Shared.Dtos.EmployeeDtos;
global using InventoryApp.Shared.Dtos.ProductDtos;
global using InventoryApp.Shared.Dtos.ProviderDtos;
global using InventoryApp.Shared.Dtos.PurchaseDtos;
global using InventoryApp.Shared.Dtos.SupplyDtos;
global using InventoryApp.Shared.Dtos.PurchaseDetailDtos;
global using InventoryApp.Shared.Dtos.SupplyDetailDtos;
global using InventoryApp.Shared.Models;
global using InventoryApp.Shared.Models.Data;
global using InventoryApp.Server.Services;
using InventoryApp.Server.Services.Impl;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using InventoryApp.Server.Repository.impl;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<inventory_managementContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddControllersWithViews();
builder.Services.AddAutoMapper(typeof(Program).Assembly);
builder.Services.AddRazorPages();
// Avoid self referencing loop for the entities.
builder.Services.AddControllers().AddJsonOptions(x =>
                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IProviderService, ProviderService>();
builder.Services.AddScoped<IPurchaseService, PurchaseService>();
builder.Services.AddScoped<ISupplyService, SupplyService>();
builder.Services.AddScoped<IAuthRepository, AuthRepository>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8
                .GetBytes(builder.Configuration.GetSection("AppSettings:Token").Value))
        };
    });
builder.Services.AddHttpContextAccessor();

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

app.UseAuthentication();
app.UseRouting();
app.UseAuthorization();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();




app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
