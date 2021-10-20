using MASA.EShop.Web.Client;
using MASA.EShop.Web.Client.Data.Basket;
using MASA.EShop.Web.Client.Data.Catalog;
using MASA.EShop.Web.Client.Data.Ordering;
using MASA.EShop.Web.Client.Infrastructure;
using Microsoft.AspNetCore.Components.Authorization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMasaBlazor();

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddScoped<AuthenticationStateProvider, SimulateAuthStateProvider>();

builder.Services.Configure<Settings>(builder.Configuration);

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddTransient<HttpClientAuthorizationDelegatingHandler>();

builder.Services.AddHttpClient<ICatalogService, CatalogService>()
    .SetHandlerLifetime(TimeSpan.FromMinutes(5));
builder.Services.AddHttpClient<IBasketService, BasketService>();
//.AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>();
builder.Services.AddHttpClient<IOrderService, OrderService>();
//.AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
