var builder = WebApplication.CreateBuilder(args);

var app = builder.Services
    .AddFluentValidation(options =>
    {
        options.RegisterValidatorsFromAssemblyContaining<CatalogSettings>();
    })
    .AddEndpointsApiExplorer()
    .AddSwaggerGen(options =>
    {
        options.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "Masa EShop - Catalog HTTP API",
            Version = "v1",
            Description = "The Catalog Service HTTP API"
        });
    })
    .AddScoped<ICatalogItemRepository, CatalogItemRepository>()
    .AddScoped<ICatalogTypeRepository, CatalogTypeRepository>()
    .AddScoped<ICatalogBrandRepository, CatalogBrandRepository>()
    .AddMasaDbContext<CatalogDbContext>(dbOptions => dbOptions.UseSqlServer("server=masa.eshop.services.eshop.database;uid=sa;pwd=P@ssw0rd;database=catalog"))
    .AddIntegrationEventBus<IntegrationEventLogService>(options =>
    {
        options.UseDapr()
               .UseEventLog<CatalogDbContext>()
               .UseEventBus(eventBusBuilder => eventBusBuilder.UseMiddleware(typeof(ValidatorMiddleware<>)))
               .UseUoW<CatalogDbContext>();
    })
    .AddServices(builder);

app.MigrateDbContext<CatalogDbContext>((context, services) =>
{
    var env = services.GetService<IWebHostEnvironment>()!;
    var settings = services.GetService<IOptions<CatalogSettings>>()!;
    var logger = services.GetService<ILogger<CatalogContextSeed>>()!;

    new CatalogContextSeed()
        .SeedAsync(context, env, settings, logger)
        .Wait();
});

app.UseSwagger().UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Masa EShop Service HTTP API v1");
});
app.UseRouting();
app.UseCloudEvents();
app.UseEndpoints(endpoint =>
{
    endpoint.MapSubscribeHandler();
});

app.Run();


