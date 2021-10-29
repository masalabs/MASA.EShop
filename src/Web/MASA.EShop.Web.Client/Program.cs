var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMasaBlazor(new MASA.Blazor.MasaBlazorOptions()
{
    Theme = new ThemeOptions()
    {
        Primary = "#7367f0"
    }
});

//Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddScoped<AuthenticationStateProvider, SimulateAuthStateProvider>();

builder.Services.Configure<Settings>(builder.Configuration);

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddTransient<HttpClientAuthorizationDelegatingHandler>();

builder.Services.AddHttpClient<ICatalogService, CatalogService>().SetHandlerLifetime(TimeSpan.FromMinutes(5));
builder.Services.AddHttpClient<IBasketService, BasketService>().AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>();
builder.Services.AddHttpClient<IOrderService, OrderService>().AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>();

// Add Authentication services          
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddCookie(setup => setup.ExpireTimeSpan = TimeSpan.FromMinutes(60));

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

#region I18n

LocaleProvider.SetLocale("zh-CN");
var langfileNames = new string[] { "en-US.json", "zh-CN.json" };
langfileNames.ForEach(langFileName =>
{
    var path = Path.Combine(app.Environment.ContentRootPath, "Resources", langFileName);
    var json = File.ReadAllText(path);
    I18n.AddLang(Path.GetFileNameWithoutExtension(path), JsonSerializer.Deserialize<Dictionary<string, string>>(json));
});

#endregion 

app.Run();
