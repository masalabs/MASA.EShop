var builder = WebApplication.CreateBuilder(args);

var app = builder.Services
    .AddFluentValidation(options =>
    {
        options.RegisterValidatorsFromAssemblyContaining<Program>();
    })
    .AddEndpointsApiExplorer()
    .AddSwaggerGen(options =>
    {
        options.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "Masa EShop - Payment HTTP API",
            Version = "v1",
            Description = "The Payment Service HTTP API"
        });
    })
    .AddMasaDbContext<PaymentDbContext>(dbOptions => dbOptions.UseSqlServer("server=masa.eshop.services.eshop.database;uid=sa;pwd=P@ssw0rd;database=payment"))
    .AddDomainEventBus(options =>
    {
        options.UseIntegrationEventBus(dispatcherOptions => dispatcherOptions.UseDapr().UseEventLog<PaymentDbContext>())
               .UseEventBus(eventBuilder => eventBuilder.UseMiddleware(typeof(ValidatorMiddleware<>)))
               .UseUoW<PaymentDbContext>()
               .UseRepository<PaymentDbContext>();
    })
    .AddServices(builder);

app.MigrateDbContext<PaymentDbContext>((context, services) =>
{
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

public partial class Program
{
    public static string Namespace = typeof(IntegrationEventService).Namespace!;
    public static string AppName = Namespace.Substring(Namespace.LastIndexOf('.', Namespace.LastIndexOf('.') - 1) + 1);
}
