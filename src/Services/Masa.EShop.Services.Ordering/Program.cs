var builder = WebApplication.CreateBuilder(args);

builder.Services.AddActors(options =>
{
    options.Actors.RegisterActor<OrderingProcessActor>();
});

//JsonOptions
builder.Services.Configure<JsonOptions>(options =>
{
    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    options.JsonSerializerOptions.WriteIndented = true;
});

//Add SignalR 
builder.Services.AddSignalR().AddHubOptions<NotificationsHub>(options =>
{
    options.EnableDetailedErrors = true;
});

var app = builder.Services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen(options =>
    {
        options.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "Masa EShop - Ordering HTTP API",
            Version = "v1",
            Description = "The Ordering Service HTTP API"
        });
    })
    .AddMasaDbContext<OrderingContext>(dbOptions => dbOptions.UseSqlServer("Data Source=masa.eshop.services.eshop.database;uid=sa;pwd=P@ssw0rd;database=order"))
    .AddScoped<IOrderRepository, OrderRepository>()
    .AddIntegrationEventBus<IntegrationEventLogService>(options=>options.UseDapr().UseEventLog<OrderingContext>())
    .AddResponseCompression(opts => 
    {
        opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
            new[] { "application/octet-stream" });
    })
    .AddServices(builder);

app.UseResponseCompression();

app.UseSwagger().UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Masa EShop Service HTTP API v1");
});

app.MigrateDbContext<OrderingContext>((context, services) =>
{
    if (!context.CardTypes.Any())
    {
        IEnumerable<CardType> GetPredefinedCardTypes()
        {
            return new List<CardType>()
            {
                CardType.Amex,
                CardType.Visa,
                CardType.MasterCard
            };
        }

        context.CardTypes.AddRange(GetPredefinedCardTypes());

        context.SaveChanges();
    }
});

app.UseRouting();
app.UseCloudEvents();
app.UseEndpoints(endpoint =>
{
    endpoint.MapSubscribeHandler();
    endpoint.MapActorsHandlers();
    endpoint.MapHub<NotificationsHub>("/hub/notificationhub",
                    options => options.Transports =
                        HttpTransportType.WebSockets |
                        HttpTransportType.LongPolling);
});
app.Run();
