var builder = WebApplication.CreateBuilder(args);

var app = builder.Services
    .AddEndpointsApiExplorer() //fixed aggregatewxception
    .AddSwaggerGen(options =>
    {
        options.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "MASA EShop - Basket HTTP API",
            Version = "v1",
            Description = "The Basket Service HTTP API"
        });
    })
    .AddScoped<IBasketRepository, BasketRepository>()
    .AddMasaDbContext<IntegrationEventLogContext>(options => options.UseSqlServer("Data Source=masa.eshop.services.eshop.database;uid=sa;pwd=P@ssw0rd;database=order"))
    .AddDaprEventBus<IntegrationEventLogService>(options =>
    {
        options.Assemblies = new Assembly[] { typeof(UserCheckoutAcceptedIntegrationEvent).Assembly, Assembly.GetExecutingAssembly() };
    })
    .AddServices(builder);

app.UseRouting();
app.UseCloudEvents();
app.UseEndpoints(endpoints =>
{
    endpoints.MapSubscribeHandler();
});

app.UseSwagger().UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "MASA EShop Service HTTP API v1");
});
app.Run();

