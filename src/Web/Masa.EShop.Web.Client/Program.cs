var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMasaBlazor(new Masa.Blazor.MasaBlazorOptions()
{
    Theme = new ThemeOptions()
    {
        Primary = "#7367f0"
    }
}).AddI18nForServer("Resources");

//Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddScoped<AuthenticationStateProvider, SimulateAuthStateProvider>();

builder.Services.Configure<Settings>(builder.Configuration);

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddTransient<HttpClientAuthorizationDelegatingHandler>();

builder.Services.AddCaller();

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

builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
