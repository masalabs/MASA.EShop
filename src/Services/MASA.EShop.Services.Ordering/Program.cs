var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddDapr();

builder.Services.AddActors(options =>
{
    options.Actors.RegisterActor<OrderingProcessActor>();
});

var app = builder.Services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen(options =>
    {
        options.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "MASA EShop - Ordering HTTP API",
            Version = "v1",
            Description = "The Ordering Service HTTP API"
        });
    })
    .AddScoped<IOrderRepository, OrderRepository>()
    .AddDaprEventBus<IntegrationEventLogService>(options =>
    {
        options.UseEventBus(AppDomain.CurrentDomain.GetAssemblies())
               .UseUoW<OrderingContext>(dbOptions => dbOptions.UseSqlServer("Data Source=masa.eshop.services.eshop.database;uid=sa;pwd=P@ssw0rd;database=order"))
               .UseEventLog<OrderingContext>();
    })
    .AddServices(builder);

app.UseSwagger().UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "MASA EShop Service HTTP API v1");
    //c.IndexStream = () => System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("MASA.EShop.Services.Ordering.index.html");
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
});
app.Run();
