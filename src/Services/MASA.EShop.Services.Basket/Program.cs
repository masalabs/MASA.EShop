var builder = WebApplication.CreateBuilder(args);

var app = builder.Services
    .AddLazyWebApplication(builder)
    .AddEndpointsApiExplorer() //fixed aggregatewxception
    .AddSwaggerGen(options =>
    {
        options.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "MASA eShop - Basket HTTP API",
            Version = "v1",
            Description = "The Basket Service HTTP API"
        });
    }).AddSwaggerGenNewtonsoftSupport()
    .AddScoped<IBasketRepository, BasketRepository>()
    .AddDaprEventBus<IntegrationEventLogService>(options =>
    {
      options.UseEventBus(builder.Services, AppDomain.CurrentDomain.GetAssemblies())
             .UseUow<IntegrationEventLogContext>(builder.Services, 
             dbOptions => dbOptions.UseSqlServer("Data Source=localhost;Initial Catalog=IntegrationEventLog;User ID=sa;Password=sa"));
    })
    .AddServices();

app.UseSwagger().UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "MASA eShop Service HTTP API v1");
});
app.Run();

